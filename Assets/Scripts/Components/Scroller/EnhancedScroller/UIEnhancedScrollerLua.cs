using System.Collections.Generic;
using EnhancedUI.EnhancedScroller;
using UnityEngine;
using System;
using XLua;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Sirenix.OdinInspector;

//[RequireComponent(typeof(EnhancedScroller))]
public partial class UIEnhancedScrollerLua : EnhancedScroller, IEnhancedScrollerDelegate, IEndDragHandler, IBeginDragHandler
{
    public class Param
    {
        public int Count;
        public LuaTable listener;
        public LuaFunction onViewUpdate;
        public LuaFunction onViewRecycle;
        public LuaFunction onViewSize;
        public List<int> dataArray;
        public List<string> itemArry;
    }
    [SerializeField] private List<UIEnhancedScrollerItem> cellArray;
    [ReadOnly][SerializeField] private List<int> dataArray = new List<int>();  //存放UIEnhancedScrollerItem的索引值，对应cellArray的index
    [ReadOnly][SerializeField] private int mDataCount = 0;

    private Param mParam;
    private EnhancedScroller _enhancedScroller;
    private LuaFunction luaDataItemUpdateAc, luaViewItemSize, luaCellViewRecycle;
    private LuaTable mLuaListener;


    #region IEnhancedScrollerDelegate接口实现

    public float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
    {
        return getCellSize(dataIndex);
    }

    public int GetNumberOfCells(EnhancedScroller scroller)
    {
        if (dataArray.Count > 0)
            return dataArray.Count;
        return mDataCount;
    }

    private float getCellSize(int dataIndex)
    {
        if(isPageMode)
        {
            pageHeight = transform.GetComponent<RectTransform>().rect.height;
            return pageHeight;
        }
        float size = 0;
        if (luaViewItemSize != null)
        {
            var data = luaViewItemSize.Call(mLuaListener, dataIndex);
            if (float.TryParse(data[0].ToString(), out size))
            {
                return size;
            }
        }

        var cell = _getCell(dataIndex);
        size = scrollDirection == EnhancedScroller.ScrollDirectionEnum.Vertical ? cell.SizeY : cell.SizeX;
        return size;
    }

    private UIEnhancedScrollerItem _getCell(int dataIndex)
    {
        var item = cellArray[0];
        if (dataArray.Count > 0 && dataIndex >= 0 && dataIndex < dataArray.Count)
            item = cellArray[dataArray[dataIndex]];
        return item;
    }

    public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
    {
        var item = _getCell(dataIndex);

        var cellView = scroller.GetCellView(item);
        cellView.gameObject.SetActive(true);

        if (luaDataItemUpdateAc != null)
        {
            luaDataItemUpdateAc.Call(mLuaListener, cellView.gameObject, dataIndex);
        }

        return cellView;
    }

    #endregion


    private void _initScroller()
    {
        for (int i = 0; i < cellArray.Count; i++)
        {
            cellArray[i].gameObject.SetActive(false);
        }
        Delegate = this;
        ReloadData();
        if (isPageMode)
            InitPageView();
        cellViewVisibilityChanged = onViewVisibilityChanged;
    }

    /// <summary>
    /// 设置列表数据
    /// </summary>
    /// <param name="count">总数</param>
    /// <param name="datas">列表对应的索引数组</param>
    /// <param name="_updateFunc">更新函数</param>
    /// <param name="listener"></param>
    public void SetEnScrollerData(int count, List<int> datas, LuaFunction _updateFunc, LuaFunction _getItemSize, LuaTable listener)
    {
        //Debug.LogError("SetEnScrollerData");
        ClearAll();
        luaDataItemUpdateAc = _updateFunc;
        luaViewItemSize = _getItemSize;
        mLuaListener = listener;
        mDataCount = count;
        SetDataArray(datas);
        _initScroller();
    }

    /// <summary>
    /// 设置列表数据
    /// </summary>
    /// <param name="Count">总数</param>
    /// <param name="_updateFunc">更新函数</param>
    /// <param name="listener"></param>
    public void SetEnScrollerData(int Count, LuaFunction _updateFunc, LuaTable listener)
    {
        ClearAll();
        luaDataItemUpdateAc = _updateFunc;
        mLuaListener = listener;
        mDataCount = Count;
        _initScroller();
    }

    ///设置数据列表，对应的列表显示顺序
    public void SetDataArray(List<int> cellDatas)
    {
        dataArray = new List<int>();
        if (cellDatas != null)
        {
            dataArray = cellDatas;
            mDataCount = dataArray.Count;
        }
        //Debug.LogError($"数据个数 = {mDataCount}");
    }

    ///设置列表的数量，需在dataArray为空的情况下,否则以dataArray.Count为准
    public void SetDataCount(int cnt)
    {
        mDataCount = cnt;
    }

    public void SetCellUpdateFunc(LuaFunction _recycleDel, LuaTable listener)
    {
        luaCellViewRecycle = _recycleDel;
    }

    private void onViewVisibilityChanged(EnhancedScrollerCellView view)
    {
        if(luaCellViewRecycle != null && view.active == false) luaCellViewRecycle.Call(mLuaListener, view.gameObject);
    }

    private void _load(string cellstr, Action<UIEnhancedScrollerItem> complete)
    {
        ResourceHelper.LoadAsset<GameObject>(cellstr, (key, asset, error) =>
        {
            if (!string.IsNullOrEmpty(error))
            {
                Log.Error($"UIEnhancedScrollerLua load error path = {cellstr}");
            }
            else
            {
                GameObject obj = UnityEngine.Object.Instantiate(asset as GameObject);
                var cell = obj.GetComponent<UIEnhancedScrollerItem>();
                if (cell == null)
                {
                    Log.Error($"UIEnhancedScrollerLua GetComponent<UIEnhancedScrollerItem> error path = {cellstr}");
                    return;
                }
                if (complete != null)
                {
                    complete(cell);
                }
            }
        });
    }

    public Dictionary<string,GameObject> _loadedDict = new Dictionary<string,GameObject>();

    public void loadAndInitScroller(Param _paras)
    {
        mParam = _paras;
        if(mParam == null)
            return;

        ClearAll();
        luaDataItemUpdateAc = mParam.onViewUpdate;
        luaCellViewRecycle = mParam.onViewRecycle;
        luaViewItemSize = mParam.onViewSize;
        mLuaListener = mParam.listener;
        mDataCount = mParam.Count;
        if(mParam.dataArray != null)
            dataArray = mParam.dataArray;
        if(mParam.itemArry != null && mParam.itemArry.Count > 0)
        {
            var itemArry = mParam.itemArry;
            int cnt = itemArry.Count;
            int _successLoad = 0;
            for (int i = 0; i < itemArry.Count; i++)
            {
                int index = i;
                string _assetPath = itemArry[i];
                _load(_assetPath, (cell) =>
                {
                    cell.transform.SetParent(transform);
                    cell.gameObject.SetActive(false);
                    _loadedDict.Add(_assetPath, cell.gameObject);
                    _successLoad++;
                    cellArray.Insert(index, cell);
                    if (_successLoad >= cnt)
                    {
                        _initScroller();
                    }
                });
            }
        }
        else
        {
            _initScroller();
        }
 
    }

    public override void ClearAll()
    {
        base.ClearAll();
        mDataCount = 0;
        if(_loadedDict.Count > 0)
        {
            var list = new List<string>(_loadedDict.Keys);
            for (int i = list.Count - 1; i >= 0; i--)
            {
                ResourceHelper.UnloadAssetWithPath<GameObject>(list[i]);
                GameObject.Destroy(_loadedDict[list[i]].gameObject);
            }
            _loadedDict.Clear();
        }
    }
}

partial class UIEnhancedScrollerLua
{
    ///
    /// 目前只支持相同大小的Cell
    ///
    [SerializeField] private bool isPageMode = false;
    [ShowIf("isPageMode")][SerializeField] private float smoothDuration = 0.2f;//滑动时间
    [ShowIf("isPageMode")][SerializeField] private float _lastJustPers = 0.1f;

    private bool _auto;//是否执行滑动
    private float pageHeight = 0;//单个页面高度
    private int _index = 0;//页面下标
    private float smoothTimer = 0;//滑动时间
    private float dragHeight = 0.0f;//开始拖动高度
    private float smoothHeight = 0.0f;//开始滑动的高度
    private int _childCount = 0;

    private RectTransform content
    {
        get
        {
            return scrollRect.content;
        }
    }

    private ScrollRect scrollRect;

    public void InitPageView()
    {
        _index = 0;
        isPageMode = true;
        _childCount = mDataCount;
        scrollRect = transform.GetComponent<ScrollRect>();
        scrollRect.movementType = ScrollRect.MovementType.Unrestricted;
        pageHeight = transform.GetComponent<RectTransform>().sizeDelta.y;
        scrollRect.inertia = false;
        content.anchoredPosition = Vector2.zero;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (isPageMode)
            AutoBone();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (isPageMode)
            dragHeight = content.anchoredPosition.y;
    }
    protected void LateUpdate()
    {
        if (isPageMode)
            AutoMove();
    }
    private void AutoMove()
    {
        if (_auto)
        {
            if (pageHeight == 0)
            {
                _auto = false;
                return;
            }
            float target = pageHeight * _index;

            if (smoothTimer < smoothDuration)
            {
                float t = smoothTimer / smoothDuration;
                content.anchoredPosition = Vector2.up * Mathf.LerpUnclamped(smoothHeight, target, t);
                smoothTimer += Time.deltaTime;
                if (1.0f - (t) < _lastJustPers)
                {
                    content.anchoredPosition = Vector2.up * target;
                    _auto = false;
                    smoothTimer = 0;
                }
            }
            else
            {
                content.anchoredPosition = Vector2.up * target;
                _auto = false;
                smoothTimer = 0;
            }


        }
    }

    private void AutoBone()
    {
        if (_auto)
            return;

        if (_childCount != mDataCount || _childCount == 0)
        {
            Refresh();
        }
        if (_childCount <= 0) return;
        if (content.anchoredPosition.y > dragHeight)
        {
            _index++;
            _index = _index >= mDataCount ? mDataCount - 1 : _index;

        }
        else
        {
            _index--;
            _index = _index < 0 ? 0 : _index;
        }

        smoothHeight = content.anchoredPosition.y;
        _auto = true;
    }

    private void Refresh()
    {
        if (_childCount <= 0) return;
        if (mDataCount == 1)
        {
            scrollRect.movementType = ScrollRect.MovementType.Clamped;
        }
        else
        {
            scrollRect.movementType = ScrollRect.MovementType.Elastic;
        }
    }

}
