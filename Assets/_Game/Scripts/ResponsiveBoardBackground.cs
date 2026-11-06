using UnityEngine;

namespace StorageAuction3
{
    [ExecuteAlways]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(RectTransform), typeof(UnityEngine.UI.Image))]
    public sealed class ResponsiveBoardBackground : MonoBehaviour
    {
        [SerializeField, Range(0.05f, 1f)] private float boardNormalizedWidth = 0.65f;
        [SerializeField, Range(0.05f, 1f)] private float boardNormalizedHeight = 0.5f;
        [SerializeField] private Vector2 boardNormalizedCenter = new Vector2(0.5f, 0.526f);
        [SerializeField, Range(0.1f, 1f)] private float boardViewportWidth = 0.96f;
        [SerializeField] private Vector2 boardViewportCenter = new Vector2(0.5f, 0.6f);
        [SerializeField] private RectTransform collectionsRoot;
        [SerializeField] private float collectionsAspect = 1080f / 570f;
        [SerializeField] private float collectionsGap = 18f;
        [SerializeField] private float collectionsBottomPadding = 24f;
        [SerializeField] private float collectionsSidePadding = 12f;

        private bool applying;
        private bool layoutDirty = true;
        private Vector2 lastParentSize;
        private Sprite lastSprite;

        private void OnEnable() => layoutDirty = true;
        private void OnValidate() => layoutDirty = true;
        private void OnTransformParentChanged() => layoutDirty = true;
        private void OnRectTransformDimensionsChange()
        {
            if (!applying) layoutDirty = true;
        }

        private void LateUpdate()
        {
            var parent = transform.parent as RectTransform;
            var image = GetComponent<UnityEngine.UI.Image>();
            if (parent != null && image != null &&
                (layoutDirty || parent.rect.size != lastParentSize || image.sprite != lastSprite))
                ApplyLayout();
            ApplyCollectionsLayout();
        }

        private void ApplyLayout()
        {
            if (applying) return;
            var rect = transform as RectTransform;
            var parent = transform.parent as RectTransform;
            var image = GetComponent<UnityEngine.UI.Image>();
            if (rect == null || parent == null || image == null || image.sprite == null) return;

            var viewport = parent.rect.size;
            var artwork = image.sprite.rect.size;
            if (viewport.x <= 0f || viewport.y <= 0f || artwork.x <= 0f || artwork.y <= 0f) return;

            // Keep the board large while using the surrounding artwork to cover the viewport.
            var coverScale = Mathf.Max(viewport.x / artwork.x, viewport.y / artwork.y);
            var boardScale = viewport.x * Mathf.Clamp01(boardViewportWidth) /
                             (artwork.x * Mathf.Clamp(boardNormalizedWidth, 0.05f, 1f));
            if (collectionsRoot != null)
                boardScale = Mathf.Min(boardScale, viewport.y * 0.64f /
                    (artwork.y * Mathf.Clamp(boardNormalizedHeight, 0.05f, 1f)));
            var size = artwork * Mathf.Max(coverScale, boardScale);
            var position = Vector2.Scale(boardViewportCenter - Vector2.one * 0.5f, viewport) -
                           Vector2.Scale(boardNormalizedCenter - Vector2.one * 0.5f, size);
            var overflow = Vector2.Max(Vector2.zero, (size - viewport) * 0.5f);
            var boardHalfSize = new Vector2(boardNormalizedWidth, boardNormalizedHeight) * 0.5f;
            var margin = viewport * 0.01f;
            var minPosition = -viewport * 0.5f + margin -
                              Vector2.Scale(boardNormalizedCenter - boardHalfSize - Vector2.one * 0.5f, size);
            var maxPosition = viewport * 0.5f - margin -
                              Vector2.Scale(boardNormalizedCenter + boardHalfSize - Vector2.one * 0.5f, size);
            position.x = ClampCoveredPosition(position.x, overflow.x, minPosition.x, maxPosition.x);
            position.y = ClampCoveredPosition(position.y, overflow.y, minPosition.y, maxPosition.y);

            applying = true;
            try
            {
                rect.anchorMin = rect.anchorMax = Vector2.one * 0.5f;
                rect.pivot = Vector2.one * 0.5f;
                rect.localScale = Vector3.one;
                rect.sizeDelta = size;
                rect.anchoredPosition = position;
                lastParentSize = viewport;
                lastSprite = image.sprite;
                layoutDirty = false;
            }
            finally
            {
                applying = false;
            }
        }

        private void ApplyCollectionsLayout()
        {
            if (collectionsRoot == null || collectionsAspect <= 0f) return;
            var parent = collectionsRoot.parent as RectTransform;
            var art = transform as RectTransform;
            if (parent == null || art == null || parent.rect.width <= 0f) return;

            var boardFoot = art.TransformPoint(new Vector3(0f, (0.286f - 0.5f) * art.rect.height, 0f));
            var top = Mathf.Min(parent.rect.yMax, parent.InverseTransformPoint(boardFoot).y - collectionsGap);
            var bottom = parent.rect.yMin + collectionsBottomPadding;
            var availableHeight = Mathf.Max(0f, top - bottom);
            var width = Mathf.Min(Mathf.Max(0f, parent.rect.width - collectionsSidePadding * 2f),
                                 availableHeight * collectionsAspect);

            var anchor = new Vector2(0.5f, 0f);
            var pivot = new Vector2(0.5f, 1f);
            var size = new Vector2(width, width / collectionsAspect);
            var position = new Vector2(0f, top - parent.rect.yMin);
            if (collectionsRoot.anchorMin != anchor) collectionsRoot.anchorMin = anchor;
            if (collectionsRoot.anchorMax != anchor) collectionsRoot.anchorMax = anchor;
            if (collectionsRoot.pivot != pivot) collectionsRoot.pivot = pivot;
            if (collectionsRoot.localScale != Vector3.one) collectionsRoot.localScale = Vector3.one;
            if ((collectionsRoot.sizeDelta - size).sqrMagnitude > 0.0001f) collectionsRoot.sizeDelta = size;
            if ((collectionsRoot.anchoredPosition - position).sqrMagnitude > 0.0001f) collectionsRoot.anchoredPosition = position;
        }

        private static float ClampCoveredPosition(float value, float overflow, float boardMin, float boardMax)
        {
            var minimum = Mathf.Max(-overflow, boardMin);
            var maximum = Mathf.Min(overflow, boardMax);
            return minimum <= maximum ? Mathf.Clamp(value, minimum, maximum) : Mathf.Clamp(value, -overflow, overflow);
        }
    }
}
