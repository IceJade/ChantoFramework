using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using EditorUtility = UnityEditor.EditorUtility;
using UnityEditor;

[CustomEditor(typeof(SkinnedMeshRenderer), true)]
public class SkinnedMeshRendererInspector : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        OnGUISortingLayer();
    }

    private void OnGUISortingLayer()
    {
        GUILayout.Space(6);

        var rend = (SkinnedMeshRenderer)target;

        GUILayout.BeginVertical("box");

        var layerNames = SortingLayer.layers.Select(x => x.name).ToList();
        EditorGUI.BeginChangeCheck();
        var selectedIndex = EditorGUILayout.Popup("SortingLayerName", layerNames.IndexOf(rend.sortingLayerName), layerNames.ToArray());
        if (EditorGUI.EndChangeCheck())
        {
            rend.sortingLayerName = SortingLayer.layers[selectedIndex].name;
            EditorUtility.SetDirty(rend.gameObject);
        }

        EditorGUI.BeginChangeCheck();
        var sortingOrder = EditorGUILayout.IntField("SortingOrder", rend.sortingOrder);
        if (EditorGUI.EndChangeCheck())
        {
            rend.sortingOrder = sortingOrder;
            EditorUtility.SetDirty(rend.gameObject);
        }

        var mats = rend.sharedMaterials;
        foreach (var mat in mats)
            DrawMaterialRenderQueue(mat);

        GUILayout.EndVertical();
    }
    private void DrawMaterialRenderQueue(Material mat)
    {
        GUILayout.BeginHorizontal();
        EditorGUILayout.ObjectField(mat, typeof(Material), false);
        EditorGUI.BeginChangeCheck();
        var val = EditorGUILayout.IntField("RenderQueue", mat.renderQueue);
        if (EditorGUI.EndChangeCheck())
        {
            mat.renderQueue = val;
            EditorUtility.SetDirty(mat);
        }
        GUILayout.EndHorizontal();
    }
}
