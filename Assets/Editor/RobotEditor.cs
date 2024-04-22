using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(Robot))]
public class RobotEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Robot robot = (Robot)target;
        if (GUILayout.Button("Draw All Connections"))
        {
            robot.DrawAllConnections();
        }
    }
}

