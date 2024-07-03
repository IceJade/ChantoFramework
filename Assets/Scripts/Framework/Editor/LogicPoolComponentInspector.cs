using Framework.Pool;
using UnityEditor;

namespace Framework.Editor
{
    [CustomEditor(typeof(LogicPoolComponent))]
    internal sealed class LogicPoolComponentInspector : FrameworkInspector
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (!EditorApplication.isPlaying)
            {
                EditorGUILayout.HelpBox("Available during runtime only.", MessageType.Info);
                return;
            }

            LogicPoolComponent t = (LogicPoolComponent)target;

            if (PrefabUtility.GetPrefabAssetType(t.gameObject) == PrefabAssetType.NotAPrefab)
            {
                EditorGUILayout.LabelField("Logic Pool Count", LogicPoolManager.Instance.GetPoolsCount().ToString());

                var logicPools = LogicPoolManager.Instance.GetAllPools();
                foreach (var logicPool in logicPools)
                {
                    DrawReferencePoolInfo(logicPool.Key.ToString(), logicPool.Value.Count, LogicPoolManager.Instance.GetCapacity(logicPool.Key));
                }
            }

            Repaint();
        }

        private void OnEnable()
        {

        }

        private void DrawReferencePoolInfo(string typeName, int count, int capacity)
        {
            EditorGUILayout.LabelField(typeName, count.ToString() + "/" + capacity.ToString());
        }
    }
}
