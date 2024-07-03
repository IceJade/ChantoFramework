using Framework.Editor;
using UnityEditor;

namespace Framework.UI.Editor
{
    [CustomEditor(typeof(UIComponent))]
    internal sealed class UIComponentInspector : FrameworkInspector
    {
        private SerializedProperty m_InstanceAutoReleaseInterval = null;
        private SerializedProperty m_InstanceCapacity = null;
        private SerializedProperty m_InstanceExpireTime = null;
        private SerializedProperty m_InstancePriority = null;
        private SerializedProperty m_InstanceRoot = null;
        private SerializedProperty m_UICamera = null;
        private SerializedProperty m_ChangeSceneMask = null;
        private SerializedProperty m_Loading = null;
        private SerializedProperty m_GrayMat = null;
        private SerializedProperty m_UIGroups = null;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            UIComponent t = (UIComponent)target;

            EditorGUILayout.PropertyField(m_GrayMat);

            EditorGUI.BeginDisabledGroup(EditorApplication.isPlayingOrWillChangePlaymode);
            {
                EditorGUILayout.PropertyField(m_InstanceRoot);
                EditorGUILayout.PropertyField(m_ChangeSceneMask);

                //UIComponent中没有m_Loading,在选中UI节点的会报空，先加判空处理
                if (m_Loading != null)
                    EditorGUILayout.PropertyField(m_Loading);

                EditorGUILayout.PropertyField(m_UICamera);
            }
            EditorGUI.EndDisabledGroup();

            float instanceAutoReleaseInterval = EditorGUILayout.DelayedFloatField("Instance Auto Release Interval", m_InstanceAutoReleaseInterval.floatValue);
            if (instanceAutoReleaseInterval != m_InstanceAutoReleaseInterval.floatValue)
            {
                if (EditorApplication.isPlaying)
                {
                    t.InstanceAutoReleaseInterval = instanceAutoReleaseInterval;
                }
                else
                {
                    m_InstanceAutoReleaseInterval.floatValue = instanceAutoReleaseInterval;
                }
            }

            int instanceCapacity = EditorGUILayout.DelayedIntField("Instance Capacity", m_InstanceCapacity.intValue);
            if (instanceCapacity != m_InstanceCapacity.intValue)
            {
                if (EditorApplication.isPlaying)
                {
                    t.InstanceCapacity = instanceCapacity;
                }
                else
                {
                    m_InstanceCapacity.intValue = instanceCapacity;
                }
            }

            float instanceExpireTime = EditorGUILayout.DelayedFloatField("Instance Expire Time", m_InstanceExpireTime.floatValue);
            if (instanceExpireTime != m_InstanceExpireTime.floatValue)
            {
                if (EditorApplication.isPlaying)
                {
                    t.InstanceExpireTime = instanceExpireTime;
                }
                else
                {
                    m_InstanceExpireTime.floatValue = instanceExpireTime;
                }
            }

            int instancePriority = EditorGUILayout.DelayedIntField("Instance Priority", m_InstancePriority.intValue);
            if (instancePriority != m_InstancePriority.intValue)
            {
                if (EditorApplication.isPlaying)
                {
                    t.InstancePriority = instancePriority;
                }
                else
                {
                    m_InstancePriority.intValue = instancePriority;
                }
            }

            EditorGUILayout.PropertyField(m_UIGroups, true);

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
            m_InstanceAutoReleaseInterval = serializedObject.FindProperty("m_InstanceAutoReleaseInterval");
            m_InstanceCapacity = serializedObject.FindProperty("m_InstanceCapacity");
            m_InstanceExpireTime = serializedObject.FindProperty("m_InstanceExpireTime");
            m_InstancePriority = serializedObject.FindProperty("m_InstancePriority");
            m_InstanceRoot = serializedObject.FindProperty("m_InstanceRoot");
            m_ChangeSceneMask = serializedObject.FindProperty("ChangeSceneMask");
            m_Loading = serializedObject.FindProperty("Loading");
            m_UICamera = serializedObject.FindProperty("m_UICamera");
            m_GrayMat = serializedObject.FindProperty("m_GrayMaterial");
            m_UIGroups = serializedObject.FindProperty("m_UIGroups");

            RefreshTypeNames();
        }

        private void RefreshTypeNames()
        {
            serializedObject.ApplyModifiedProperties();
        }
    }
}
