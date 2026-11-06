using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace AuctionMerge
{
    [DisallowMultipleComponent]
    public sealed class MergeGameController : MonoBehaviour
    {
        [Serializable] public class Chain
        {
            public string name;
            public Sprite[] levels = new Sprite[3];
        }
        [Serializable] public class Cell
        {
            public RectTransform rect;
            public UnityEngine.UI.Image icon;
            public int initialChain = -1;
            public int initialLevel;
            public int lockCollection = -1;
        }
        [Serializable] public class Collection
        {
            public RectTransform dropArea;
            public GameObject[] itemLayers;
            public GameObject closedCompartment;
            public GameObject openCompartment;
            public UnityEngine.UI.Image key;
            public UnityEngine.UI.Image[] dots;
            public Sprite completeDot;
            public Sprite openLock;
            public Color accent = Color.white;
        }

        [SerializeField] public Canvas canvas;
        [SerializeField] public Cell[] cells;
        [SerializeField] public Chain[] chains;
        [SerializeField] public Collection[] collections;
        [SerializeField] public Sprite tutorialHandSprite;
        [SerializeField] public TMPro.TMP_FontAsset endgameFont;
        [SerializeField, Min(0.1f)] public float flightDuration = 0.65f;
        [SerializeField, Min(0.01f)] public float scatterStagger = 0.065f;
        [SerializeField, Min(0f)] public float hintDelay = 1.3f;
        [SerializeField] public UnityEvent onGameCompleted = new UnityEvent();

        public MergeGameState State { get; private set; }
        public bool IsBusy { get; private set; }
        public bool IsDragging => dragging != null;
        public string CurrentHint { get; private set; } = "";
        public int HintSource { get; private set; } = -1;
        public int HintTarget { get; private set; } = -1;
        readonly System.Random random = new System.Random();
        RectTransform effects;
        UnityEngine.UI.Image hand;
        UnityEngine.UI.Image ghost;
        UnityEngine.UI.Image dragOriginal;
        MergeDragSource dragging;
        Color originalColor;
        float lastInput;
        float hintCycleStart;
        string hintIdentity;
        Vector2 dragOffset;
        readonly Vector3[] corners = new Vector3[4];
        Camera EventCamera => canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera;

        void Awake()
        {
            // Mobile players default to 30 FPS unless a target is set explicitly.
            Application.targetFrameRate = 60;
            QualitySettings.vSyncCount = 0;

            if (canvas == null || cells == null || cells.Length != 20 || chains == null || chains.Length != 9 || collections == null || collections.Length != 3)
            { Debug.LogError("Merge game is not configured.", this); enabled = false; return; }
            var initial = new MergeGameState.BoardItem[cells.Length];
            for (int i = 0; i < cells.Length; i++)
            {
                var cell = cells[i];
                if (cell.lockCollection >= 0) initial[i] = MergeGameState.BoardItem.Padlock(cell.lockCollection);
                else if (cell.initialLevel > 0) initial[i] = MergeGameState.BoardItem.Card(cell.initialChain, cell.initialLevel);
                BindDrag(cell.icon, i, false);
            }
            State = new MergeGameState(initial);
            for (int i = 0; i < collections.Length; i++) BindDrag(collections[i].key, i, true);
            effects = NewRect("GameplayEffects", canvas.transform);
            effects.anchorMin = Vector2.zero; effects.anchorMax = Vector2.one;
            effects.offsetMin = effects.offsetMax = Vector2.zero;
            hand = NewImage("TutorialHand_White", tutorialHandSprite);
            hand.rectTransform.sizeDelta = new Vector2(95, 115);
            hand.rectTransform.pivot = new Vector2(0.16f, 0.89f);
            hand.gameObject.SetActive(false);
            lastInput = Time.unscaledTime - hintDelay + 0.35f;
            // No initial Refresh: all authored sprites, layers and positions stay exactly as saved.
        }

        void BindDrag(UnityEngine.UI.Image image, int index, bool key)
        {
            image.raycastTarget = true;
            var source = image.GetComponent<MergeDragSource>();
            if (source == null) source = image.gameObject.AddComponent<MergeDragSource>();
            source.Bind(this, index, key);
        }

        public void NotifyInput()
        {
            lastInput = Time.unscaledTime;
            if (hand != null) hand.gameObject.SetActive(false);
            hintIdentity = null;
        }

        public void BeginDrag(MergeDragSource source, PointerEventData data)
        {
            if (data.button != PointerEventData.InputButton.Left || IsBusy || dragging != null || State == null || State.Finished) return;
            if (source.IsKey ? !State.KeyAvailable(source.Index) : !State[source.Index].IsCard) return;
            NotifyInput();
            dragging = source;
            dragOriginal = source.GetComponent<UnityEngine.UI.Image>();
            originalColor = dragOriginal.color;
            ghost = CopyImage("DraggedItem", dragOriginal);
            var start = LocalCenter(dragOriginal.rectTransform);
            ghost.rectTransform.anchoredPosition = start;
            dragOffset = start - LocalPointer(data.position);
            dragOriginal.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0);
        }

        public void Drag(MergeDragSource source, PointerEventData data)
        {
            if (dragging == source && ghost != null) ghost.rectTransform.anchoredPosition = LocalPointer(data.position) + dragOffset;
        }

        public void EndDrag(MergeDragSource source, PointerEventData data)
        {
            if (dragging != source) return;
            var visual = ghost;
            dragging = null; ghost = null;
            dragOriginal.color = originalColor;
            dragOriginal = null;
            NotifyInput();
            if (source.IsKey)
            {
                int target = HitCell(data.position);
                if (State.Unlock(source.Index, target, random, out int[] spawned))
                { StartCoroutine(UnlockSequence(source.Index, target, spawned, visual)); return; }
            }
            else
            {
                for (int c = 0; c < collections.Length; c++)
                    if (Contains(collections[c].dropArea, data.position) && State.Deliver(source.Index, c))
                    {
                        RefreshCell(source.Index);
                        StartCoroutine(DeliverySequence(c, visual));
                        return;
                    }
                int target = HitCell(data.position);
                if (State.MoveOrMerge(source.Index, target, out bool merged))
                {
                    RefreshCell(source.Index); RefreshCell(target);
                    StartCoroutine(BoardDropSequence(target, merged, visual));
                    return;
                }
            }
            StartCoroutine(ReturnSequence(source, visual));
        }

        public void CancelDrag(MergeDragSource source)
        {
            if (dragging != source) return;
            if (dragOriginal != null) dragOriginal.color = originalColor;
            if (ghost != null) Destroy(ghost.gameObject);
            dragging = null; dragOriginal = null; ghost = null;
            NotifyInput();
        }

        void OnApplicationFocus(bool focused) { if (!focused && dragging != null) CancelDrag(dragging); }
        void OnDisable() { if (dragging != null) CancelDrag(dragging); }

        int HitCell(Vector2 point)
        {
            for (int i = 0; i < cells.Length; i++) if (Contains(cells[i].rect, point)) return i;
            return -1;
        }
        bool Contains(RectTransform rect, Vector2 point) => rect.gameObject.activeInHierarchy && RectTransformUtility.RectangleContainsScreenPoint(rect, point, EventCamera);
        Vector2 LocalPointer(Vector2 screen)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(effects, screen, EventCamera, out Vector2 local);
            return local;
        }
        Vector2 LocalCenter(RectTransform rect) => effects.InverseTransformPoint(rect.TransformPoint(rect.rect.center));
        Vector2 LocalSize(RectTransform rect)
        {
            rect.GetWorldCorners(corners);
            return new Vector2(Vector3.Distance(effects.InverseTransformPoint(corners[0]), effects.InverseTransformPoint(corners[3])),
                Vector3.Distance(effects.InverseTransformPoint(corners[0]), effects.InverseTransformPoint(corners[1])));
        }
        static RectTransform NewRect(string objectName, Transform parent)
        {
            var rt = new GameObject(objectName, typeof(RectTransform)).GetComponent<RectTransform>();
            rt.SetParent(parent, false);
            rt.anchorMin = rt.anchorMax = rt.pivot = new Vector2(0.5f, 0.5f);
            return rt;
        }
        UnityEngine.UI.Image NewImage(string objectName, Sprite sprite)
        {
            var rt = NewRect(objectName, effects);
            var img = rt.gameObject.AddComponent<UnityEngine.UI.Image>();
            img.sprite = sprite; img.preserveAspect = true; img.raycastTarget = false;
            return img;
        }
        UnityEngine.UI.Image CopyImage(string objectName, UnityEngine.UI.Image source)
        {
            var img = NewImage(objectName, source.sprite);
            img.color = source.color; img.material = source.material;
            img.rectTransform.sizeDelta = LocalSize(source.rectTransform);
            return img;
        }
        void RefreshCell(int index)
        {
            var item = State[index];
            var image = cells[index].icon;
            if (item.IsCard) image.sprite = chains[item.Chain].levels[item.Level - 1];
            image.gameObject.SetActive(!item.IsEmpty);
        }

        IEnumerator ReturnSequence(MergeDragSource source, UnityEngine.UI.Image visual)
        {
            IsBusy = true;
            var original = source.GetComponent<UnityEngine.UI.Image>();
            Color tint = original.color;
            original.color = new Color(tint.r, tint.g, tint.b, 0);
            yield return Fly(visual, LocalCenter(original.rectTransform), 0.2f, 20);
            original.color = tint;
            Destroy(visual.gameObject);
            IsBusy = false;
        }

        IEnumerator BoardDropSequence(int target, bool merged, UnityEngine.UI.Image visual)
        {
            IsBusy = true;
            Destroy(visual.gameObject);
            if (merged) Burst(LocalCenter(cells[target].icon.rectTransform), collections[State.Stage].accent, 10);
            yield return Pulse(cells[target].icon.rectTransform, merged ? 1.24f : 1.08f, 0.26f);
            IsBusy = false;
        }

        IEnumerator DeliverySequence(int collection, UnityEngine.UI.Image visual)
        {
            IsBusy = true;
            var view = collections[collection];
            yield return Fly(visual, LocalCenter(view.dropArea), 0.32f, 45);
            Destroy(visual.gameObject);
            for (int i = 0; i < 3; i++)
            {
                if (!State.Delivered(collection * 3 + i)) continue;
                if (view.itemLayers[i] != null) view.itemLayers[i].SetActive(collection == 2);
            }
            Burst(LocalCenter(view.dropArea), view.accent, 22);
            for (int i = 0; i < 3; i++) if (State.Delivered(collection * 3 + i)) view.dots[i].sprite = view.completeDot;
            yield return Pulse(view.dropArea, 1.04f, 0.26f);
            if (State.Complete(collection))
            {
                view.closedCompartment.SetActive(false);
                view.openCompartment.SetActive(true);
                view.key.gameObject.SetActive(true);
                Burst(LocalCenter(view.key.rectTransform), view.accent, 16);
                yield return Pulse(view.key.rectTransform, 1.18f, 0.3f);
            }
            IsBusy = false;
            NotifyInput();
        }

        IEnumerator UnlockSequence(int collection, int target, int[] spawned, UnityEngine.UI.Image visual)
        {
            IsBusy = true;
            collections[collection].key.gameObject.SetActive(false);
            yield return Fly(visual, LocalCenter(cells[target].icon.rectTransform), 0.3f, 35);
            Destroy(visual.gameObject);
            cells[target].icon.sprite = collections[collection].openLock;
            Burst(LocalCenter(cells[target].icon.rectTransform), collections[collection].accent, 28);
            yield return Pulse(cells[target].icon.rectTransform, 1.25f, 0.4f);
            if (State.Finished)
            {
                yield return new WaitForSecondsRealtime(0.45f);
                ShowEndgame();
                onGameCompleted.Invoke();
                IsBusy = false;
                yield break;
            }
            // Blue reward explicitly originates at Cell_16; subsequent rewards originate at their lock.
            Vector2 origin = LocalCenter(cells[collection == 0 ? 15 : target].rect);
            Burst(origin, collections[collection].accent, 24);
            for (int i = 0; i < spawned.Length; i++)
            {
                int destination = spawned[i];
                StartCoroutine(ScatterItem(destination, origin, i));
                yield return new WaitForSecondsRealtime(scatterStagger);
            }
            yield return new WaitForSecondsRealtime(flightDuration + 0.28f);
            IsBusy = false;
        }

        IEnumerator ScatterItem(int cell, Vector2 origin, int order)
        {
            var item = State[cell];
            var visual = NewImage("RewardFlight", chains[item.Chain].levels[0]);
            visual.rectTransform.sizeDelta = LocalSize(cells[cell].icon.rectTransform);
            visual.rectTransform.anchoredPosition = origin;
            yield return Fly(visual, LocalCenter(cells[cell].icon.rectTransform), flightDuration, 140 + order % 4 * 32, true);
            Destroy(visual.gameObject);
            RefreshCell(cell);
            yield return Pulse(cells[cell].icon.rectTransform, 1.12f, 0.2f);
        }

        IEnumerator Fly(UnityEngine.UI.Image image, Vector2 target, float duration, float arc, bool spin = false)
        {
            var rect = image.rectTransform;
            Vector2 start = rect.anchoredPosition;
            for (float t = 0; t < duration; t += Time.unscaledDeltaTime)
            {
                float p = Mathf.Clamp01(t / duration), eased = Mathf.SmoothStep(0, 1, p);
                rect.anchoredPosition = Vector2.Lerp(start, target, eased) + Vector2.up * (Mathf.Sin(p * Mathf.PI) * arc);
                rect.localScale = Vector3.one * (spin ? Mathf.Lerp(0.35f, 1f, Mathf.Min(p * 3, 1)) : 1f);
                if (spin) rect.localRotation = Quaternion.Euler(0, 0, Mathf.Sin(p * Mathf.PI) * 18);
                yield return null;
            }
            rect.anchoredPosition = target;
            rect.localScale = Vector3.one; rect.localRotation = Quaternion.identity;
        }
        IEnumerator Pulse(RectTransform rect, float amount, float duration)
        {
            Vector3 initial = rect.localScale;
            for (float t = 0; t < duration; t += Time.unscaledDeltaTime)
            {
                rect.localScale = initial * (1 + Mathf.Sin(t / duration * Mathf.PI) * (amount - 1));
                yield return null;
            }
            rect.localScale = initial;
        }

        void Burst(Vector2 origin, Color color, int count)
        {
            for (int i = 0; i < count; i++) StartCoroutine(Spark(origin, color, i, count));
        }
        IEnumerator Spark(Vector2 origin, Color color, int index, int count)
        {
            var img = NewImage("Spark", null);
            var rt = img.rectTransform;
            rt.sizeDelta = new Vector2(4, 11);
            float angle = index * Mathf.PI * 2 / count;
            Vector2 direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
            float distance = 65 + (index % 4) * 20;
            for (float t = 0; t < 0.65f; t += Time.unscaledDeltaTime)
            {
                float p = t / 0.65f;
                rt.anchoredPosition = origin + direction * distance * (1 - (1 - p) * (1 - p)) + Vector2.down * (p * p * 55);
                rt.localRotation = Quaternion.Euler(0, 0, index * 37 + p * 160);
                img.color = new Color(color.r, color.g, color.b, 1 - p);
                yield return null;
            }
            Destroy(img.gameObject);
        }

        void Update()
        {
            if (State == null || hand == null) return;
            if (State.Stage != 0 || IsBusy || dragging != null || Time.unscaledTime - lastInput < hintDelay)
            { hand.gameObject.SetActive(false); CurrentHint = ""; return; }
            RectTransform from = null, to = null;
            HintSource = HintTarget = -1;
            string identity = "";
            if (State.KeyAvailable(0))
            {
                from = collections[0].key.rectTransform;
                for (int i = 0; i < cells.Length; i++) if (State[i].Lock == 1) { to = cells[i].icon.rectTransform; HintTarget = i; }
                identity = "Key_Blue > Lock_Blue";
            }
            else
            {
                for (int i = 0; i < cells.Length; i++) if (State[i].IsCard && State[i].Chain == 0 && State[i].Level == 3)
                { from = cells[i].icon.rectTransform; to = collections[0].dropArea; HintSource = i; identity = "TopHat > Sketeton_Variations"; break; }
                if (from == null)
                    for (int i = 0; i < cells.Length && from == null; i++)
                        for (int j = i + 1; j < cells.Length; j++) if (State.CanMerge(i, j))
                        { from = cells[i].icon.rectTransform; to = cells[j].icon.rectTransform; HintSource = i; HintTarget = j; identity = "Merge " + i + " > " + j; break; }
            }
            if (from == null || to == null) { hand.gameObject.SetActive(false); return; }
            if (hintIdentity != identity) { hintIdentity = identity; hintCycleStart = Time.unscaledTime; }
            CurrentHint = identity;
            float phase = (Time.unscaledTime - hintCycleStart) % 2.5f;
            float travel = Mathf.SmoothStep(0, 1, Mathf.Clamp01((phase - 0.35f) / 1.3f));
            hand.gameObject.SetActive(true);
            hand.transform.SetAsLastSibling();
            hand.rectTransform.anchoredPosition = Vector2.Lerp(LocalCenter(from), LocalCenter(to), travel);
            float alpha = Mathf.Min(Mathf.Clamp01(phase / 0.15f), Mathf.Clamp01((2.4f - phase) / 0.3f));
            hand.color = new Color(1, 1, 1, alpha);
            hand.rectTransform.localScale = Vector3.one * (phase < 0.35f ? 1 - 0.12f * Mathf.Sin(phase / 0.35f * Mathf.PI) : 1);
        }

        void ShowEndgame()
        {
            hand.gameObject.SetActive(false);
            var shade = NewImage("Endgame", null);
            shade.color = new Color(0.035f, 0.025f, 0.09f, 0.88f);
            shade.raycastTarget = true;
            shade.rectTransform.anchorMin = Vector2.zero; shade.rectTransform.anchorMax = Vector2.one;
            shade.rectTransform.offsetMin = shade.rectTransform.offsetMax = Vector2.zero;
            var title = NewRect("Title", shade.transform);
            title.sizeDelta = new Vector2(800, 160); title.anchoredPosition = new Vector2(0, 160);
            var text = title.gameObject.AddComponent<TMPro.TextMeshProUGUI>();
            text.font = endgameFont; text.text = "ALL COLLECTIONS\nCOMPLETE!";
            text.fontSize = 64; text.fontStyle = TMPro.FontStyles.Bold; text.alignment = TMPro.TextAlignmentOptions.Center;
            text.color = new Color(1, 0.84f, 0.4f); text.raycastTarget = false;
            for (int i = 0; i < 3; i++)
            {
                var icon = NewImage("CompletedLock", collections[i].openLock);
                icon.rectTransform.SetParent(shade.transform, false);
                icon.rectTransform.sizeDelta = new Vector2(155, 155);
                icon.rectTransform.anchoredPosition = new Vector2((i - 1) * 185, -40);
                Burst(new Vector2((i - 1) * 185, -40), collections[i].accent, 25);
            }
        }
    }
}
