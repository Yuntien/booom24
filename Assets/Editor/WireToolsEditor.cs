using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(WireTools))]
public class WireToolsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        WireTools wireToolsScript = (WireTools)target;
        if(GUILayout.Button("Draw Wire"))
        {
            wireToolsScript.DrawWire();
        }
    }
}