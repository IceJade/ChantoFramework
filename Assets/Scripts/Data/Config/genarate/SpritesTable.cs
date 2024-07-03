﻿// <auto-generated>
//  automatically generated by table tool, do not modify
// </auto-generated>
using System.IO;
using FlatBuffers;
using LS.Table;
using System.Collections.Generic;

namespace Chanto
{
    #region Table
    
	public class SpritesTable : StringIdConfig
    {
        private Table_sprites table = default(Table_sprites);

        private Dictionary<string, SpritesDataRow> data_row = new Dictionary<string, SpritesDataRow>(128);
        
        public static string g_TableFileName = "sprites";

        public virtual bool HasSprite(string spriteName)
        {
            if (this.GetDataCount() <= 0)
                return false;

            if (null == this.row_index || this.row_index.Count <= 0)
                return false;

            var hashCode = spriteName.GetHashCode();
            return this.row_index.ContainsKey(hashCode);
        }

        #region Data Method

        public override bool GetBoolValue(string id, string column, bool defaultValue = false)
        {
            bool result = defaultValue;

            var datarow = this.GetDataRow(id);
            if (null == datarow)
                return result;

            switch (column)
            {

                default: { Log.ErrorFormat("Table_sprites.GetBoolValue => data type is not match, or not find [id:{0}, column:{1}]", id, column); break; }
            }

            return result;
        }
        
        public override short GetShortValue(string id, string column, short defaultValue = 0)
        {
            short result = defaultValue;

            var datarow = this.GetDataRow(id);
            if (null == datarow)
                return result;

            switch (column)
            {

                default: { Log.ErrorFormat("Table_sprites.GetShortValue => data type is not match, or not find [id:{0}, column:{1}]", id, column); break; }
            }

            return result;
        }
        
        public override int GetIntValue(string id, string column, int defaultValue = 0)
        {
            int result = defaultValue;

            var datarow = this.GetDataRow(id);
            if (null == datarow)
                return result;

            switch (column)
            {
                case "LoadType": { result = datarow.LoadType; break; }

                default: { Log.ErrorFormat("Table_sprites.GetIntValue => data type is not match, or not find [id:{0}, column:{1}]", id, column); break; }
            }

            return result;
        }
        
        public override float GetFloatValue(string id, string column, float defaultValue = 0.0f)
        {
            float result = defaultValue;

            var datarow = this.GetDataRow(id);
            if (null == datarow)
                return result;

            switch (column)
            {
                case "LoadType": { result = datarow.LoadType; break; }

                default: { Log.ErrorFormat("Table_sprites.GetFloatValue => data type is not match, or not find [id:{0}, column:{1}]", id, column); break; }
            }

            return result;
        }

        public override string GetStringValue(string id, string column, string defaultValue = "")
        {
            string result = defaultValue;

            var datarow = this.GetDataRow(id);
            if (null == datarow)
                return result;

            switch (column)
            {
                case "id": { result = datarow.Id; break; }
                case "LoadType": { result = datarow.LoadType.ToString(); break; }
                case "AtlasOrBundleName": { result = datarow.AtlasOrBundleName; break; }

                default: { Log.ErrorFormat("Table_sprites.GetStringValue => data type is not match, or not find [id:{0}, column:{1}]", id, column); break; }
            }

            return result;
        }

        public override bool[] GetBoolArray(string id, string column)
        {
            bool[] result = null;

            var datarow = this.GetDataRow(id);
            if (null == datarow)
                return result;

            switch (column)
            {

                default: { Log.ErrorFormat("Table_sprites.GetBoolArray => data type is not match, or not find [id:{0}, column:{1}]", id, column); break; }
            }

            return result;
        }
        
        public override short[] GetShortArray(string id, string column)
        {
            short[] result = null;

            var datarow = this.GetDataRow(id);
            if (null == datarow)
                return result;

            switch (column)
            {

                default: { Log.ErrorFormat("Table_sprites.GetShortArray => data type is not match, or not find [id:{0}, column:{1}]", id, column); break; }
            }

            return result;
        }
        
        public override int[] GetIntArray(string id, string column)
        {
            int[] result = null;

            var datarow = this.GetDataRow(id);
            if (null == datarow)
                return result;

            switch (column)
            {

                default: { Log.ErrorFormat("Table_sprites.GetIntArray => data type is not match, or not find [id:{0}, column:{1}]", id, column); break; }
            }

            return result;
        }
        
        public override float[] GetFloatArray(string id, string column)
        {
            float[] result = null;

            var datarow = this.GetDataRow(id);
            if (null == datarow)
                return result;

            switch (column)
            {

                default: { Log.ErrorFormat("Table_sprites.GetFloatArray => data type is not match, or not find [id:{0}, column:{1}]", id, column); break; }
            }

            return result;
        }

        public override string[] GetStringArray(string id, string column)
        {
            string[] result = null;

            var datarow = this.GetDataRow(id);
            if (null == datarow)
                return result;

            switch (column)
            {

                default: { Log.ErrorFormat("Table_sprites.GetStringArray => data type is not match, or not find [id:{0}, column:{1}]", id, column); break; }
            }

            return result;
        }
        
        public override string GetStringArrayItem(string id, string column, int index, string defaultValue = "")
        {
            string result = defaultValue;

            int length = this.GetStringArrayLength(id, column);
            if (index < 0 || index >= length)
            {
                Log.ErrorFormat("Table_sprites.GetStringArrayItem => index out of array length({0}), [id:{1}, column:{2}, index:{3}]", length, id, column, index);
                return result;
            }
            
            var datarow = this.GetDataRow(id);
            if (null == datarow)
                return result;

            switch (column)
            {

                default: { Log.ErrorFormat("Table_sprites.GetStringArrayItem => data type is not match, or not find [id:{0}, column:{1}]", id, column); break; }
            }

            return result;
        }
        
        public override int GetStringArrayLength(string id, string column)
        {
            int result = 0;

            var datarow = this.GetDataRow(id);
            if (null == datarow)
                return result;

            switch (column)
            {

                default: { Log.ErrorFormat("Table_sprites.GetStringArrayLength => data type is not match, or not find [id:{0}, column:{1}]", id, column); break; }
            }

            return result;
        }
        
        public override Dictionary<int, int> GetDictionaryII(string id, string column) 
        {
            Dictionary<int, int> result = null;

            var datarow = this.GetDataRow(id);
            if (null == datarow)
                return result;

            switch (column)
            {

                default: { Log.ErrorFormat("Table_sprites.GetDictionaryII => data type is not match, or not find [id:{0}, column:{1}]", id, column); break; }
            }

            return result;
        }

        public override Dictionary<int, string> GetDictionaryIS(string id, string column) 
        {
            Dictionary<int, string> result = null;

            var datarow = this.GetDataRow(id);
            if (null == datarow)
                return result;

            switch (column)
            {

                default: { Log.ErrorFormat("Table_sprites.GetDictionaryIS => data type is not match, or not find [id:{0}, column:{1}]", id, column); break; }
            }

            return result;
        }

        public override Dictionary<string, int> GetDictionarySI(string id, string column) 
        {
            Dictionary<string, int> result = null;

            var datarow = this.GetDataRow(id);
            if (null == datarow)
                return result;

            switch (column)
            {

                default: { Log.ErrorFormat("Table_sprites.GetDictionarySI => data type is not match, or not find [id:{0}, column:{1}]", id, column); break; }
            }

            return result;
        }

        public override Dictionary<string, string> GetDictionarySS(string id, string column) 
        {
            Dictionary<string, string> result = null;

            var datarow = this.GetDataRow(id);
            if (null == datarow)
                return result;

            switch (column)
            {

                default: { Log.ErrorFormat("Table_sprites.GetDictionarySS => data type is not match, or not find [id:{0}, column:{1}]", id, column); break; }
            }

            return result;
        }

        #endregion Data Method

        #region DataRow Method

        /// <summary>
        /// 获得数据行的数量
        /// </summary>
        /// <returns></returns>
        public override int GetDataCount() 
        {
            Table_sprites table = this.GetTable();
            if (null == table.ByteBuffer)
                return 0;

            return table.DataLength; 
        }
        
        /// <summary>
        /// 获得行数据
        /// </summary>
        /// <param name="id">字段ID的值</param>
        /// <returns>行数据</returns>
        public SpritesDataRow GetDataRow(string id)
        {
            if (data_row.ContainsKey(id))
                return data_row[id];

            Table_sprites table = this.GetTable();
            if (null == table.ByteBuffer)
                return null;
                
            if (data_row.Count >= table.DataLength)
                return null;
            
            int hashcode = id.GetHashCode();
            if (null != row_index && row_index.ContainsKey(hashcode))
            {
                DRsprites? data = table.Data(row_index[hashcode]);
                if (data.HasValue && data.Value.Id == id)
                {
                    SpritesDataRow datarow = new SpritesDataRow(data.Value, row_index[hashcode]);
                    if(!data_row.ContainsKey(datarow.Id))
                        data_row.Add(datarow.Id, datarow);

                    return datarow;
                }
            }

            if (current_row_index >= table.DataLength)
                return null;

            int start = current_row_index;
            for (int i = start; i < table.DataLength; i++)
            {
                DRsprites? data = table.Data(i);
                if (data.HasValue)
                {
                    SpritesDataRow datarow = new SpritesDataRow(data.Value, i);
                    if(!data_row.ContainsKey(datarow.Id))
                        data_row.Add(datarow.Id, datarow);

                    current_row_index = i;
                    
                    if (datarow.Id == id)
                        return datarow;
                }
            }

            return null;
        }

        /// <summary>
        /// 通过索引获取行数据
        /// </summary>
        /// <param name="index">索引,即行号,从0开始</param>
        /// <returns></returns>
        public SpritesDataRow GetDataRowByIndex(int index)
        {
            Table_sprites table = this.GetTable();
            if (null == table.ByteBuffer)
                return null;

            if (index >= table.DataLength)
                return null;

            DRsprites? data = table.Data(index);
            if (data.HasValue)
            {
                if (!data_row.ContainsKey(data.Value.Id))
                {
                    SpritesDataRow datarow = new SpritesDataRow(data.Value, index);
                    data_row.Add(datarow.Id, datarow);
                }

                return data_row[data.Value.Id];
            }

            return null;
        }
        
        /// <summary>
        /// 获得所有行数据
        /// </summary>
        /// <returns>所有行数据</returns>
        public Dictionary<string, SpritesDataRow> GetAllData()
        {
            this.LoadTable();

            Table_sprites table = this.GetTable();
            if (null == table.ByteBuffer)
                return null;
                
            int dataCount = table.DataLength;
            if (data_row.Count < dataCount)
            {
                for (int i = 0; i < table.DataLength; i++)
                {
                    DRsprites? data = table.Data(i);
                    if (data.HasValue && !data_row.ContainsKey(data.Value.Id))
                    {
                        SpritesDataRow datarow = new SpritesDataRow(data.Value, i);
                        data_row.Add(data.Value.Id, datarow);
                    }
                }
            }

            return data_row;
        }

        public override BaseDataRow GetTableRow(string id) 
        { 
            return this.GetDataRow(id); 
        }

        public override BaseDataRow GetTableRowByIndex(int index)
        {
            return this.GetDataRowByIndex(index);
        }

        #endregion DataRow Method
                
        #region Framework Method

        protected override void InitTable(ByteBuffer byteBuffer)
        {
            table = Table_sprites.GetRootAsTable_sprites(byteBuffer);
            
            this.load_state = E_LoadState.Loaded;
        }

        public override void ResetTable()
        {
            base.ResetTable();

            this.data_row.Clear();
        }
        
        private Table_sprites GetTable()
        {
            LoadTable();

            return table;
        }

        protected override string GetTableFileName()
        {
            return g_TableFileName;
        }

        protected override string GetDataFileName()
        {
            return "sprites.bytes";
        }

        protected override string GetIndexFileName()
        {
            return "sprites_ids";
        }
        
        #endregion Framework Method
    }

    #endregion Table

    #region DataRow

    public sealed class SpritesDataRow : BaseDataRow
    {
        private DRsprites _datarow;

        public SpritesDataRow(DRsprites datarow, int index) : base(index)
        {
            this._datarow = datarow;
        }

        protected override LuaValue GetLuaValue(string rowId)
        {
            LuaValue luaValue = new LuaValue();
            switch (rowId)
            {
                case "id": { luaValue.SetValue(this.Id); break; }
                case "LoadType": { luaValue.SetValue(this.LoadType); break; }
                case "AtlasOrBundleName": { luaValue.SetValue(this.AtlasOrBundleName); break; }

                default:
                    break;
            }

            return luaValue;
        }

        private string _Id = null;
        public string Id { get { if (null == _Id) _Id = _datarow.Id; return _Id; } }

        public int LoadType { get { return this.GetTableInt(_datarow.LoadType); } }

        private string _AtlasOrBundleName = null;
        public string AtlasOrBundleName { get { if (null == _AtlasOrBundleName) _AtlasOrBundleName = _datarow.AtlasOrBundleName; return _AtlasOrBundleName; } }


    }

    #endregion DataRow
}