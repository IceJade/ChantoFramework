using Chanto;

namespace UnityEditor.UI
{
    [CustomEditor(typeof(AniSwitchButton), true)]
    [CanEditMultipleObjects]
    public class AniSwitchButtonEditor : BaseButtonEditor
    {
        SerializedProperty AnimatorProperty;
        SerializedProperty IsOnProperty;
        SerializedProperty ClickCDProperty;
        SerializedProperty OnClickProperty;

        protected override void OnEnable()
        {
            base.OnEnable();

            IsOnProperty = serializedObject.FindProperty("isOn");
            AnimatorProperty = serializedObject.FindProperty("ani");
            ClickCDProperty = serializedObject.FindProperty("clickCD");
            OnClickProperty = serializedObject.FindProperty("m_OnClick");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EditorGUILayout.Space();

            serializedObject.Update();

            EditorGUILayout.LabelField("播放的动画");
            EditorGUILayout.PropertyField(AnimatorProperty);

            EditorGUILayout.LabelField("是否默认选中");
            EditorGUILayout.PropertyField(IsOnProperty);

            EditorGUILayout.LabelField("两次有效点击的间隔时间(秒)");
            EditorGUILayout.PropertyField(ClickCDProperty);

            EditorGUILayout.LabelField("按钮点击事件");
            EditorGUILayout.PropertyField(OnClickProperty);

            serializedObject.ApplyModifiedProperties();
        }
    }
}
