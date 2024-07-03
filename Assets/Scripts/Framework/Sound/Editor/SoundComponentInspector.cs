using Framework.Sound;
using UnityEditor;

namespace Framework.Editor
{
    [CustomEditor(typeof(SoundComponent))]
    internal sealed class SoundComponentInspector : FrameworkInspector
    {
        private SerializedProperty m_InstanceRoot = null;
        private SerializedProperty m_AudioMixer = null;
        private SerializedProperty m_SoundGroups = null;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            SoundComponent t = (SoundComponent)target;

            EditorGUI.BeginDisabledGroup(EditorApplication.isPlayingOrWillChangePlaymode);
            {
                EditorGUILayout.PropertyField(m_InstanceRoot);
                EditorGUILayout.PropertyField(m_AudioMixer);
                EditorGUILayout.PropertyField(m_SoundGroups, true);
            }
            EditorGUI.EndDisabledGroup();

            if (EditorApplication.isPlaying && PrefabUtility.GetPrefabType(t.gameObject) != PrefabType.Prefab)
            {
                EditorGUILayout.LabelField("Sound Group Count", t.SoundGroupCount.ToString());
            }

            serializedObject.ApplyModifiedProperties();

            Repaint();
        }

        protected override void OnCompileComplete()
        {
            base.OnCompileComplete();

            RefreshTypeNames();
        }

        private void OnEnable()
        {
            m_InstanceRoot = serializedObject.FindProperty("m_InstanceRoot");
            m_AudioMixer = serializedObject.FindProperty("m_AudioMixer");
            m_SoundGroups = serializedObject.FindProperty("m_SoundGroups");

            RefreshTypeNames();
        }

        private void RefreshTypeNames()
        {
            serializedObject.ApplyModifiedProperties();
        }
    }
}
