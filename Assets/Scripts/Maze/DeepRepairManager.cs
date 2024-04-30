using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeepRepairManager : MonoBehaviour
{
    // Start is called before the first frame update
    private List<string> logs = new List<string> { "愤怒", "伤心", "害羞" };
    private int currentLogIndex = 0;
void OnEnable()
{
    MazeGenerator.OnPlayerReachTarget += HandlePlayerReachTarget;
}

void OnDisable()
{
    MazeGenerator.OnPlayerReachTarget -= HandlePlayerReachTarget;
}
void HandlePlayerReachTarget(MazeGenerator.Cell targetCell)
{
    // Log the message
    LogNextMessage();

    // Get the TextMeshPro component from the child gameobject
    var textMesh = targetCell.cellObject.GetComponentInChildren<TMPro.TextMeshPro>();

    textMesh.enabled=true;

    if (textMesh != null)
    {
        // Set the text to the current log message
        textMesh.text = logs[currentLogIndex - 1];
    }
}
public void LogNextMessage()
    {
        if (currentLogIndex < logs.Count)
        {
            Debug.Log(logs[currentLogIndex]);
            currentLogIndex++;
        }
    }

}
