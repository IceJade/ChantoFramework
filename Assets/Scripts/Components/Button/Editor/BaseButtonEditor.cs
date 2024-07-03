using Chanto;

namespace UnityEditor.UI
{
    [CustomEditor(typeof(BaseButton))]
    public class BaseButtonEditor : SelectableEditor
    {
        protected SerializedProperty hintTypeProperty;

        protected override void OnEnable()
        {
            base.OnEnable();

            hintTypeProperty = serializedObject.FindProperty(nameof(BaseButton.m_hintType));
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            EditorGUILayout.PropertyField(hintTypeProperty);
            serializedObject.ApplyModifiedProperties();
        }
    }
}
