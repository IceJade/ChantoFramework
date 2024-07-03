using UnityEditor;
using UnityEditor.UI;

namespace Chanto
{
    [CustomEditor(typeof(GroupButton), true)]
    [CanEditMultipleObjects]
    public class GroupButtonEditor : BaseButtonEditor
    {
        SerializedProperty m_GroupIdProperty;
        SerializedProperty m_FrameIntervalProperty;
        SerializedProperty OnClickProperty;

        protected override void OnEnable()
        {
            base.OnEnable();

            m_GroupIdProperty = serializedObject.FindProperty("GroupId");
            m_FrameIntervalProperty = serializedObject.FindProperty("FrameInterval");
            OnClickProperty = serializedObject.FindProperty("m_OnClick");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EditorGUILayout.Space();

            serializedObject.Update();
            EditorGUILayout.LabelField("组ID", "建议设置为UI表里的行号+数字,防止组ID重复");
            EditorGUILayout.PropertyField(m_GroupIdProperty);
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("帧数", "指定帧数以内同一组内只有一个按钮点击事件生效");
            EditorGUILayout.PropertyField(m_FrameIntervalProperty);
            EditorGUILayout.PropertyField(OnClickProperty);
            serializedObject.ApplyModifiedProperties();
        }
    }
}
