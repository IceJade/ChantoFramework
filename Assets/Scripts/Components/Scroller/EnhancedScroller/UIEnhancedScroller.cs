using EnhancedUI.EnhancedScroller;
using UnityEngine;
using XLua;

[RequireComponent(typeof(EnhancedScroller))]
public class UIEnhancedScroller : MonoBehaviour, IEnhancedScrollerDelegate
{

    [CSharpCallLua] public delegate void luaOnGetItemByIndex(EnhancedScrollerCellView cellView, int dataIndex);

    private LuaFunction m_luaOnGetItemFunc = null;
    private LuaTable m_luaListener = null;
    private bool isInit = false;
    private int maxDataCount = 0;
    private EnhancedScroller _scroller;
    private luaOnGetItemByIndex mOnGetItemByIndex;
    
    [SerializeField]private float _cellViewSize = 0;
    [SerializeField]private EnhancedScrollerCellView _Cell;

    public EnhancedScroller getEnhancedScroller
    {
        get
        {
            if (_scroller == null)
            {
                _scroller = transform.GetComponent<EnhancedScroller>();
            }
            return _scroller;
        }
    }

    private float cellViewSize
    {
        get
        {
            return _cellViewSize;
        }
        set
        {
            _cellViewSize = value;
        }
    }

    public void InitListView(int dataCount, luaOnGetItemByIndex onGetItemByIndex)
    {
        mOnGetItemByIndex = onGetItemByIndex;
        LuaTable m_luaListener = null;
        getEnhancedScroller.Delegate = this;
        if (isInit == false)
        {
            isInit = true;
            if (_Cell != null)
            {
                var rt = _Cell.transform.GetComponent<RectTransform>();
                if (getEnhancedScroller.scrollDirection == EnhancedScroller.ScrollDirectionEnum.Vertical)
                {
                    cellViewSize = rt.sizeDelta.y;
                }
                else
                {
                    cellViewSize = rt.sizeDelta.x;
                }
            }
        }
        _SetListItemCount(dataCount);
    }

    public void InitListView(int dataCount, LuaFunction onGetItemByIndex, LuaTable luaListener)
    {
        m_luaOnGetItemFunc = onGetItemByIndex;
        m_luaListener = luaListener;
        getEnhancedScroller.Delegate = this;
        if (isInit == false)
        {
            isInit = true;
            if (_Cell != null)
            {
                var rt = _Cell.transform.GetComponent<RectTransform>();
                if (getEnhancedScroller.scrollDirection == EnhancedScroller.ScrollDirectionEnum.Vertical)
                {
                    cellViewSize = rt.sizeDelta.y;
                }
                else
                {
                    cellViewSize = rt.sizeDelta.x;
                }
            }
        }
        _SetListItemCount(dataCount);
    }
    
    public void InitListView(int dataCount,float cellSize, luaOnGetItemByIndex onGetItemByIndex)
    {        
        mOnGetItemByIndex = onGetItemByIndex;
        getEnhancedScroller.Delegate = this;
        cellViewSize = cellSize;
        if (isInit == false)
        {
            isInit = true;
        }
        _SetListItemCount(dataCount);
    }

    private void _SetListItemCount(int num)
    {
        _Cell.gameObject.SetActive(false);
        getEnhancedScroller.ClearAll();
        maxDataCount = num;
        getEnhancedScroller.ReloadData();
    }
    
    public void SetListItemCount(int num)
    {
        _SetListItemCount(num);
    }
    
    public void ClearAll()
    {
        getEnhancedScroller.ClearAll();
        m_luaOnGetItemFunc = null;
        mOnGetItemByIndex = null;
    }

    public int GetNumberOfCells(EnhancedScroller scroller)
    {
        return maxDataCount;
    }

    public float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
    {
        return _cellViewSize;
    }

    public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
    {
        EnhancedScrollerCellView cellView;
        cellView = scroller.GetCellView(_Cell);
        cellView.gameObject.SetActive(true);

        if (mOnGetItemByIndex != null)
        {
            mOnGetItemByIndex(cellView, dataIndex);
        }

        if(m_luaOnGetItemFunc != null && m_luaListener != null)
        {
            var a = m_luaOnGetItemFunc.Call(m_luaListener, cellView, dataIndex);
        }
        return cellView;
    }

}
