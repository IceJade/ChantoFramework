﻿// <auto-generated>
//  You can expand your configuration logic here.
// </auto-generated>
using FlatBuffers;
using LS.Table;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Chanto
{
    public class Sprites_path_with_folderTable : SpritesTable
    {
        private int _index = 0;

        public Sprites_path_with_folderTable(int index)
        {
            this._index = index;
        }

        #region Framework Method

        protected override string GetTableFileName()
        {
            return $"sprites_path_with_folder_{this._index}";
        }

        protected override string GetDataFileName()
        {
            return $"sprites_path_with_folder_{this._index}.bytes";
        }

        protected override string GetIndexFileName()
        {
            return $"sprites_path_with_folder_{this._index}_ids";
        }

        #endregion Framework Method
    }
}