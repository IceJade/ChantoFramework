using UnityEngine;

namespace EnhancedUI.EnhancedScroller
{
    public class UIEnhancedScrollerItem : EnhancedScrollerCellView
    {
        private RectTransform _rt;

        public float SizeY{
            get{
                return Rt.sizeDelta.y;
            }
        }

        public float SizeX{
            get{
                return Rt.sizeDelta.x;
            }
        }

        public RectTransform Rt{
            get{
                if(_rt == null) _rt = transform.GetComponent<RectTransform>();
                return _rt;
            }
        }
        
    }
}

