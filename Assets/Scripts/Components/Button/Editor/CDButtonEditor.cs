using Chanto;
using UnityEngine;

namespace UnityEditor.UI
{
    [CustomEditor(typeof(CDButton))]
    [CanEditMultipleObjects]
    public class CDButtonEditor : BaseButtonEditor
    {
        protected SerializedProperty ClickCDProperty;
        protected SerializedProperty OnClickProperty;

        protected override void OnEnable()
        {
            base.OnEnable();

            ClickCDProperty = serializedObject.FindProperty(nameof(CDButton.ClickCD));
            OnClickProperty = serializedObject.FindProperty("m_OnClick");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            EditorGUILayout.PropertyField(ClickCDProperty, new GUIContent("两次有效点击的间隔时间(秒)"));
            EditorGUILayout.PropertyField(OnClickProperty);
            serializedObject.ApplyModifiedProperties();
        }
    }
}
