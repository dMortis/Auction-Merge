using UnityEngine;
using UnityEngine.EventSystems;

namespace StorageAuction3
{
    [DisallowMultipleComponent]
    public sealed class MergeDragSource : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler
    {
        public MergeGameController Owner { get; private set; }
        public int Index { get; private set; }
        public bool IsKey { get; private set; }
        public void Bind(MergeGameController owner, int index, bool key)
        { Owner = owner; Index = index; IsKey = key; }
        public void OnPointerDown(PointerEventData data) { if (Owner != null) Owner.NotifyInput(); }
        public void OnBeginDrag(PointerEventData data) { if (Owner != null) Owner.BeginDrag(this, data); }
        public void OnDrag(PointerEventData data) { if (Owner != null) Owner.Drag(this, data); }
        public void OnEndDrag(PointerEventData data) { if (Owner != null) Owner.EndDrag(this, data); }
        void OnDisable() { if (Owner != null) Owner.CancelDrag(this); }
    }
}
