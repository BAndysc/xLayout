using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace xLayout
{
    [ExecuteInEditMode]
    [ExecuteAlways]
    [Serializable]
    public class ExternalLayouter : MonoBehaviour
    {
        [SerializeField] private string layoutPath;

        [SerializeField] private DateTime lastReload;

        void Update()
        {
            if (Application.isPlaying)
                return;
        
            var path = Application.dataPath + "/" + layoutPath;
            if (File.Exists(path))
            {
                var lastWrite = File.GetLastWriteTimeUtc(path);
                if (lastReload < lastWrite)
                {
                    lastReload = lastWrite;
                    Reload();
                }
            }
        }
    
        // not fired?
        //    private void OnApplicationFocus(bool hasFocus)
        //    {
        //        if (hasFocus)
        //        {
        //        }
        //    }

        public void Reload()
        {
            string path = Application.dataPath + "/" + layoutPath;

            if (!File.Exists(path))
            {
                Debug.LogError($"File {path} doesn't exist! Cannot build layout.");
                return;
            }
        
            xLayouter.BuildLayoutFromXML(gameObject, path);
        }
    }


    [CustomEditor(typeof(ExternalLayouter))]
    public class ExternalLayouterEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            var layoutPath = serializedObject.FindProperty("layoutPath");

            if (string.IsNullOrEmpty(layoutPath.stringValue))
            {
                EditorGUILayout.HelpBox("Put layout file name with extension relative to Assets/ folder", MessageType.Info);
            }
            else if (!File.Exists(Application.dataPath + "/" + layoutPath.stringValue))
            {
                EditorGUILayout.HelpBox("File " + layoutPath.stringValue + " doesn't exists!", MessageType.Error);
            }
            
            EditorGUILayout.PropertyField(layoutPath);

            if (GUILayout.Button("Force reload"))
            {
                (target as ExternalLayouter).Reload();
            }
            
            serializedObject.ApplyModifiedProperties();
        }
    }
}