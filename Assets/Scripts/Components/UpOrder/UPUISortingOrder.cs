using Framework.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Chanto
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Canvas))]
    [ExecuteInEditMode]
    public class UPUISortingOrder : MonoBehaviour, IForceUpdateOrder
    {
        /// <summary>
        /// 相对于当前UI实际运行时的 Group 的层级提高的层级
        /// </summary>
        [Header("相对于UI根节点提升的层距")]
        [SerializeField]
        private int upCount;

        /// <summary>
        /// 该UI下面的元素是否需要UI交互
        /// </summary>
        [Header("是否需要UI交互")]
        [SerializeField]
        private bool needGraphicRaycaster;

        private bool haveUpdateSortingOrder = false;

        public void UpdateSortingOrder(int baseSortingOrder, string sortingLayerName)
        {
            var canvas = gameObject.GetOrAddComponent<Canvas>();
            canvas.overrideSorting = true;
            canvas.sortingOrder = baseSortingOrder + upCount;
            canvas.sortingLayerName = sortingLayerName;

            if (needGraphicRaycaster && null == gameObject.GetComponent<GraphicRaycaster>())
                gameObject.AddComponent<GraphicRaycaster>();

            this.haveUpdateSortingOrder = true;
        }

        private void Start()
        {
            if (this.haveUpdateSortingOrder)
                return;

            UpdateSortingOrderAndLayer();
        }

        private int SortingOrder { set; get; }
        private string _sortingLayer = string.Empty;

        private void UpdateSortingOrderAndLayer()
        {
#if UNITY_EDITOR
            if (upCount > UIFormLogic.DepthFactor)
            {
                Log.Warning($"层级提升超标,可能覆盖上层UI! name:{this.gameObject.name}");
            }
#endif

            var (order, layer) = this.GetBaseSortingOrderAndLayer();
            var tSortingOrder = order + upCount;

            //说明层级没变
            if (SortingOrder == tSortingOrder
                && string.CompareOrdinal(_sortingLayer, layer) == 0)
                return;

            var canvas = gameObject.GetOrAddComponent<Canvas>();
            canvas.overrideSorting = true;

            _sortingLayer = layer;
            SortingOrder = tSortingOrder;
            canvas.sortingOrder = SortingOrder;
            canvas.sortingLayerName = layer;

            if (!needGraphicRaycaster)
                return;
            if (!gameObject.GetComponent<GraphicRaycaster>())
                gameObject.AddComponent<GraphicRaycaster>();
        }
    }
}