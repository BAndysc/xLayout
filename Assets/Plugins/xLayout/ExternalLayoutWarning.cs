#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace xLayout
{
    public class ExternalLayoutWarning : MonoBehaviour
    {
    }

    #if UNITY_EDITOR
    [CustomEditor(typeof(ExternalLayoutWarning))]
    public class ExternalLayoutWarningEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            EditorGUILayout.HelpBox("DO NOT MODIFY THIS GAME OBJECT! It is driven by ExternalLayout (automatically builds from xml)", MessageType.Warning);
        }
    }
    #endif
}