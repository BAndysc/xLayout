using System;
using System.Collections.Generic;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace xLayout
{
    [ExecuteInEditMode]
    #if UNITY_2018_3_OR_NEWER
    [ExecuteAlways]
    #endif
    [Serializable]
    public class ExternalLayouter : MonoBehaviour
    {
        [SerializeField] private string layoutPath;

        [SerializeField] private DateTime lastReload;

        public HashSet<string> ReferencedResources;
        
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
        
            xLayouter.BuildLayoutFromXML(gameObject, path, out ReferencedResources);
        }
    }

    #if UNITY_EDITOR
    [CustomEditor(typeof(ExternalLayouter))]
    public class ExternalLayouterEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            var target = this.target as ExternalLayouter;
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
    
            EditorGUILayout.LabelField($"Referenced resources: {target.ReferencedResources.Count}");

            EditorGUI.indentLevel++;

            int i = 1;
            foreach (var resource in target.ReferencedResources)
                EditorGUILayout.LabelField($"{i++}. {resource}");
            
            EditorGUI.indentLevel--;
            
            if (GUILayout.Button("Force reload"))
            {
                target.Reload();
            }
            
            serializedObject.ApplyModifiedProperties();
        }
    }
    #endif
}