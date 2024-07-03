using Chanto;

namespace UnityEditor.UI
{
    [CustomEditor(typeof(ToggleButton), true)]
    [CanEditMultipleObjects]
    public class ToggleButtonEditor : BaseButtonEditor
    {
        protected SerializedProperty StateProperty;
        protected SerializedProperty AutoNotifyProperty;
        protected SerializedProperty GroupProperty;
        protected SerializedProperty CheckedNodeProperty;
        protected SerializedProperty UncheckedNodeProperty;
        protected SerializedProperty DisableNodeProperty;
        protected SerializedProperty OnClickProperty;

        protected override void OnEnable()
        {
            base.OnEnable();

            StateProperty = serializedObject.FindProperty("state");
            AutoNotifyProperty = serializedObject.FindProperty("AutoNotify");
            GroupProperty = serializedObject.FindProperty("group");
            CheckedNodeProperty = serializedObject.FindProperty("CheckedNode");
            UncheckedNodeProperty = serializedObject.FindProperty("UncheckNode");
            DisableNodeProperty = serializedObject.FindProperty("DisableNode");
            OnClickProperty = serializedObject.FindProperty("m_OnClick");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EditorGUILayout.Space();

            serializedObject.Update();

            EditorGUILayout.LabelField("初始状态");
            EditorGUILayout.PropertyField(StateProperty);

            EditorGUILayout.LabelField("点击时是否自动通知组内按钮刷新状态");
            EditorGUILayout.LabelField("比如点击弹出确认框后再刷新状态时就不要勾选此选项");
            EditorGUILayout.LabelField("不勾选时需要调用SetState或者RefreshState来刷新状态");
            EditorGUILayout.PropertyField(AutoNotifyProperty);

            EditorGUILayout.LabelField("按钮组");
            EditorGUILayout.PropertyField(GroupProperty);

            EditorGUILayout.LabelField("选中状态");
            EditorGUILayout.PropertyField(CheckedNodeProperty);

            EditorGUILayout.LabelField("未选中状态");
            EditorGUILayout.PropertyField(UncheckedNodeProperty);

            EditorGUILayout.LabelField("不可用状态");
            EditorGUILayout.PropertyField(DisableNodeProperty);

            EditorGUILayout.LabelField("点击事件");
            EditorGUILayout.PropertyField(OnClickProperty);

            serializedObject.ApplyModifiedProperties();
        }
    }
}
