using Chanto;
using LS;
using UnityEngine;

namespace UnityEditor.UI
{
    [CustomEditor(typeof(EffectButton))]
    [CanEditMultipleObjects]
    public class EffectButtonEditor : CDButtonEditor
    {
        protected SerializedProperty EffectProperty;

        protected override void OnEnable()
        {
            base.OnEnable();

            EffectProperty = serializedObject.FindProperty(nameof(EffectButton.Effect));
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            EditorGUILayout.PropertyField(EffectProperty, new GUIContent("点击时播放特效"));

            serializedObject.ApplyModifiedProperties();
        }
    }
}
