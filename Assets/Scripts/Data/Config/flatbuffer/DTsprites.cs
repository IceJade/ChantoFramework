// <auto-generated>
//  automatically generated by the FlatBuffers compiler, do not modify
// </auto-generated>

namespace LS.Table
{

using global::System;
using global::System.Collections.Generic;
using global::FlatBuffers;

public struct DRsprites : IFlatbufferObject
{
  private Table __p;
  public ByteBuffer ByteBuffer { get { return __p.bb; } }
  public static void ValidateVersion() { FlatBufferConstants.FLATBUFFERS_1_12_0(); }
  public static DRsprites GetRootAsDRsprites(ByteBuffer _bb) { return GetRootAsDRsprites(_bb, new DRsprites()); }
  public static DRsprites GetRootAsDRsprites(ByteBuffer _bb, DRsprites obj) { return (obj.__assign(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public void __init(int _i, ByteBuffer _bb) { __p = new Table(_i, _bb); }
  public DRsprites __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }

  public string Id { get { int o = __p.__offset(4); return o != 0 ? __p.__string(o + __p.bb_pos) : null; } }
#if ENABLE_SPAN_T
  public Span<byte> GetIdBytes() { return __p.__vector_as_span<byte>(4, 1); }
#else
  public ArraySegment<byte>? GetIdBytes() { return __p.__vector_as_arraysegment(4); }
#endif
  public byte[] GetIdArray() { return __p.__vector_as_array<byte>(4); }
  public int LoadType { get { int o = __p.__offset(6); return o != 0 ? __p.bb.GetInt(o + __p.bb_pos) : (int)0; } }
  public string AtlasOrBundleName { get { int o = __p.__offset(8); return o != 0 ? __p.__string(o + __p.bb_pos) : null; } }
#if ENABLE_SPAN_T
  public Span<byte> GetAtlasOrBundleNameBytes() { return __p.__vector_as_span<byte>(8, 1); }
#else
  public ArraySegment<byte>? GetAtlasOrBundleNameBytes() { return __p.__vector_as_arraysegment(8); }
#endif
  public byte[] GetAtlasOrBundleNameArray() { return __p.__vector_as_array<byte>(8); }

  public static Offset<LS.Table.DRsprites> CreateDRsprites(FlatBufferBuilder builder,
      StringOffset idOffset = default(StringOffset),
      int LoadType = 0,
      StringOffset AtlasOrBundleNameOffset = default(StringOffset)) {
    builder.StartTable(3);
    DRsprites.AddAtlasOrBundleName(builder, AtlasOrBundleNameOffset);
    DRsprites.AddLoadType(builder, LoadType);
    DRsprites.AddId(builder, idOffset);
    return DRsprites.EndDRsprites(builder);
  }

  public static void StartDRsprites(FlatBufferBuilder builder) { builder.StartTable(3); }
  public static void AddId(FlatBufferBuilder builder, StringOffset idOffset) { builder.AddOffset(0, idOffset.Value, 0); }
  public static void AddLoadType(FlatBufferBuilder builder, int LoadType) { builder.AddInt(1, LoadType, 0); }
  public static void AddAtlasOrBundleName(FlatBufferBuilder builder, StringOffset AtlasOrBundleNameOffset) { builder.AddOffset(2, AtlasOrBundleNameOffset.Value, 0); }
  public static Offset<LS.Table.DRsprites> EndDRsprites(FlatBufferBuilder builder) {
    int o = builder.EndTable();
    return new Offset<LS.Table.DRsprites>(o);
  }
};

public struct Table_sprites : IFlatbufferObject
{
  private Table __p;
  public ByteBuffer ByteBuffer { get { return __p.bb; } }
  public static void ValidateVersion() { FlatBufferConstants.FLATBUFFERS_1_12_0(); }
  public static Table_sprites GetRootAsTable_sprites(ByteBuffer _bb) { return GetRootAsTable_sprites(_bb, new Table_sprites()); }
  public static Table_sprites GetRootAsTable_sprites(ByteBuffer _bb, Table_sprites obj) { return (obj.__assign(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public void __init(int _i, ByteBuffer _bb) { __p = new Table(_i, _bb); }
  public Table_sprites __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }

  public LS.Table.DRsprites? Data(int j) { int o = __p.__offset(4); return o != 0 ? (LS.Table.DRsprites?)(new LS.Table.DRsprites()).__assign(__p.__indirect(__p.__vector(o) + j * 4), __p.bb) : null; }
  public int DataLength { get { int o = __p.__offset(4); return o != 0 ? __p.__vector_len(o) : 0; } }

  public static Offset<LS.Table.Table_sprites> CreateTable_sprites(FlatBufferBuilder builder,
      VectorOffset dataOffset = default(VectorOffset)) {
    builder.StartTable(1);
    Table_sprites.AddData(builder, dataOffset);
    return Table_sprites.EndTable_sprites(builder);
  }

  public static void StartTable_sprites(FlatBufferBuilder builder) { builder.StartTable(1); }
  public static void AddData(FlatBufferBuilder builder, VectorOffset dataOffset) { builder.AddOffset(0, dataOffset.Value, 0); }
  public static VectorOffset CreateDataVector(FlatBufferBuilder builder, Offset<LS.Table.DRsprites>[] data) { builder.StartVector(4, data.Length, 4); for (int i = data.Length - 1; i >= 0; i--) builder.AddOffset(data[i].Value); return builder.EndVector(); }
  public static VectorOffset CreateDataVectorBlock(FlatBufferBuilder builder, Offset<LS.Table.DRsprites>[] data) { builder.StartVector(4, data.Length, 4); builder.Add(data); return builder.EndVector(); }
  public static void StartDataVector(FlatBufferBuilder builder, int numElems) { builder.StartVector(4, numElems, 4); }
  public static Offset<LS.Table.Table_sprites> EndTable_sprites(FlatBufferBuilder builder) {
    int o = builder.EndTable();
    return new Offset<LS.Table.Table_sprites>(o);
  }
  public static void FinishTable_spritesBuffer(FlatBufferBuilder builder, Offset<LS.Table.Table_sprites> offset) { builder.Finish(offset.Value); }
  public static void FinishSizePrefixedTable_spritesBuffer(FlatBufferBuilder builder, Offset<LS.Table.Table_sprites> offset) { builder.FinishSizePrefixed(offset.Value); }
};


}