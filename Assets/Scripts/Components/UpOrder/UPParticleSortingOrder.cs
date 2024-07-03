using UnityEngine;

namespace Chanto
{
    [RequireComponent (typeof (ParticleSystem))]
    [ExecuteInEditMode]
    public class UPParticleSortingOrder : MonoBehaviour, IForceUpdateOrder
    {
        [Header ("相对于UI根节点提升的层距")] [SerializeField]
        private int upCount;

        private bool haveUpdateSortingOrder = false;

        public void UpdateSortingOrder(int baseSortingOrder, string sortingLayerName)
        {
            var particle = GetComponent<ParticleSystem>();
            if (null == particle)
                return;

            var pRenderer = particle.GetComponent<Renderer>();
            pRenderer.sortingOrder = baseSortingOrder + upCount;
            pRenderer.sortingLayerName = sortingLayerName;

            this.haveUpdateSortingOrder = true;
        }

        private void Start()
        {
            if (this.haveUpdateSortingOrder)
                return;

            UpdateSortingOrderAndLayer();
        }

        private void UpdateSortingOrderAndLayer()
        {
            var (order, layer) = this.GetBaseSortingOrderAndLayer();

            var tOrder = order + upCount;

            if (SortingOrder == tOrder
                && string.CompareOrdinal(_sortingLayer, layer) == 0)
                return;

            var particle = GetComponent<ParticleSystem>();

            if (particle == null)
                return;

            var pRenderer = particle.GetComponent<Renderer>();
            SortingOrder = tOrder;
            _sortingLayer = layer;
            pRenderer.sortingOrder = SortingOrder;
            pRenderer.sortingLayerName = layer;
        }

        private int    SortingOrder { set; get; }
        private string _sortingLayer = string.Empty;

#if UNITY_EDITOR

        /// <summary>
        /// 便于编辑模式时调试
        /// </summary>
        private void Update ()
        {
            if (Application.isPlaying)
                return;

            if (SortingOrder == upCount)
                return;

            var particle = GetComponent<ParticleSystem> ();

            if (particle == null)
                return;

            var pRenderer = particle.GetComponent<Renderer> ();
            SortingOrder           = upCount;
            pRenderer.sortingOrder = SortingOrder;
        }

#endif
    }
}