using Chanto;
using UnityEngine;

namespace UnityEditor.UI
{
    [CustomEditor(typeof(UISequenceFrameAnimation))]
    [CanEditMultipleObjects]
    public class UISequenceFrameAnimationEditor : Editor
    {
        protected SerializedProperty IsLoopProperty;
        protected SerializedProperty AutoPlayProperty;
        protected SerializedProperty ImageFormatProperty;
        protected SerializedProperty StartIndexProperty;
        protected SerializedProperty EndIndexProperty;
        protected SerializedProperty IntervalProperty;

        protected void OnEnable()
        {
            IsLoopProperty = serializedObject.FindProperty(nameof(UISequenceFrameAnimation.IsLoop));
            AutoPlayProperty = serializedObject.FindProperty(nameof(UISequenceFrameAnimation.AutoPlay));
            ImageFormatProperty = serializedObject.FindProperty(nameof(UISequenceFrameAnimation.ImageFormat));
            StartIndexProperty = serializedObject.FindProperty(nameof(UISequenceFrameAnimation.StartIndex));
            EndIndexProperty = serializedObject.FindProperty(nameof(UISequenceFrameAnimation.EndIndex));
            IntervalProperty = serializedObject.FindProperty(nameof(UISequenceFrameAnimation.Interval));
        }

        public override void OnInspectorGUI()
        {
            //base.OnInspectorGUI();

            serializedObject.Update();

            EditorGUILayout.PropertyField(IsLoopProperty, new GUIContent("是否循环播放"));
            EditorGUILayout.PropertyField(AutoPlayProperty, new GUIContent("是否自动播放"));
            EditorGUILayout.PropertyField(ImageFormatProperty, new GUIContent("图片名称格式"));
            EditorGUILayout.PropertyField(StartIndexProperty, new GUIContent("起始索引"));
            EditorGUILayout.PropertyField(EndIndexProperty, new GUIContent("结束索引"));
            EditorGUILayout.PropertyField(IntervalProperty, new GUIContent("间隔时间(秒)"));

            serializedObject.ApplyModifiedProperties();
        }
    }
}
