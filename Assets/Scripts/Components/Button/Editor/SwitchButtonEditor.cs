using Chanto;

namespace UnityEditor.UI
{
    [CustomEditor(typeof(SwitchButton), true)]
    [CanEditMultipleObjects]
    public class SwitchButtonEditor : BaseButtonEditor
    {
        SerializedProperty IsOnProperty;
        SerializedProperty CheckedNodeProperty;
        SerializedProperty UncheckedNodeProperty;
        SerializedProperty NodeNameProperty;
        SerializedProperty CheckLangIdProperty;
        SerializedProperty UncheckLangIdProperty;
        SerializedProperty OnClickProperty;

        protected override void OnEnable()
        {
            base.OnEnable();

            IsOnProperty = serializedObject.FindProperty("IsOn");
            CheckedNodeProperty = serializedObject.FindProperty("CheckedNode");
            UncheckedNodeProperty = serializedObject.FindProperty("UncheckNode");
            NodeNameProperty = serializedObject.FindProperty("NodeName");
            CheckLangIdProperty = serializedObject.FindProperty("CheckLangId");
            UncheckLangIdProperty = serializedObject.FindProperty("UncheckLangId");
            OnClickProperty = serializedObject.FindProperty("m_OnClick");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EditorGUILayout.Space();

            serializedObject.Update();

            EditorGUILayout.LabelField("是否默认选中");
            EditorGUILayout.PropertyField(IsOnProperty);

            EditorGUILayout.LabelField("选中状态");
            EditorGUILayout.PropertyField(CheckedNodeProperty);

            EditorGUILayout.LabelField("未选中状态");
            EditorGUILayout.PropertyField(UncheckedNodeProperty);

            EditorGUILayout.LabelField("按钮名称");
            EditorGUILayout.PropertyField(NodeNameProperty);

            EditorGUILayout.LabelField("选中状态时显示的文本");
            EditorGUILayout.PropertyField(CheckLangIdProperty);

            EditorGUILayout.LabelField("未选中状态时显示的文本");
            EditorGUILayout.PropertyField(UncheckLangIdProperty);

            EditorGUILayout.PropertyField(OnClickProperty);

            serializedObject.ApplyModifiedProperties();
        }
    }
}
