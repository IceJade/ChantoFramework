namespace UnityEditor.UI
{
    [CustomEditor(typeof(IMImage), true)]
    [CanEditMultipleObjects]
    public class IMImageEditor : ImageEditor
    {
        protected override void OnEnable()
        {
            base.OnEnable();
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
        }
    }
}