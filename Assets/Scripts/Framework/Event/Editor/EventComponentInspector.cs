using Framework.Event;
using Framework.Editor;
using UnityEditor;

namespace Framework.Event.Editor
{
    [CustomEditor(typeof(EventComponent))]
    internal sealed class EventComponentInspector : FrameworkInspector
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (!EditorApplication.isPlaying)
            {
                EditorGUILayout.HelpBox("Available during runtime only.", MessageType.Info);
                return;
            }

            EventComponent t = (EventComponent)target;

            if (PrefabUtility.GetPrefabType(t.gameObject) != PrefabType.Prefab)
            {
                EditorGUILayout.LabelField("Event Count", t.Count.ToString());
            }

            Repaint();
        }

        private void OnEnable()
        {

        }
    }
}
