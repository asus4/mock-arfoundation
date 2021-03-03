using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace MockARFoundation
{
    [CustomEditor(typeof(MockARFoundationSetting))]
    public class MockARFoundationSettingEditor : Editor
    {
        private SerializedProperty source;
        private SerializedProperty preferredCamera;
        private SerializedProperty preferredResolution;
        private SerializedProperty videoPath;
        private MockARFoundationSetting _target;

        private void OnEnable()
        {
            _target = target as MockARFoundationSetting;
            source = serializedObject.FindProperty("source");
            preferredCamera = serializedObject.FindProperty("preferredCamera");
            preferredResolution = serializedObject.FindProperty("preferredResolution");
            videoPath = serializedObject.FindProperty("videoPath");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(source);

            if (_target.CurrentSource == MockARFoundationSetting.Source.WebCamera)
            {
                EditorGUILayout.PropertyField(preferredCamera);
                EditorGUILayout.PropertyField(preferredResolution);
            }
            else
            {
                EditorGUILayout.PropertyField(videoPath);
                if (GUILayout.Button("Load Video"))
                {
                    string path = EditorUtility.OpenFilePanelWithFilters("Selecte the mock video", "", new string[] { "Vidoe files", "mp4,mov", "All files", "*" });
                    if (!string.IsNullOrWhiteSpace(path))
                    {
                        videoPath.stringValue = path;
                    }
                }
            }

            serializedObject.ApplyModifiedProperties();
        }

    }
}
