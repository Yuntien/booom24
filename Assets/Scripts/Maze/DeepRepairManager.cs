using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeepRepairManager : MonoBehaviour
{
    // Start is called before the first frame update
    public List<string> logs = new List<string> { "愤怒", "伤心", "害羞" };
    private int currentLogIndex = 0;
    private MazeGenerator maze;
    private GameObject player;
    public GameObject playerPrefab;


    
void OnEnable()
{
    MazeGenerator.OnPlayerReachTarget += HandlePlayerReachTarget;
}

void OnDisable()
{
    MazeGenerator.OnPlayerReachTarget -= HandlePlayerReachTarget;
}

private void Start() {
    startReapir();
}
public void startReapir()
{
    maze=GetComponent<MazeGenerator>();
    maze.GenerateMaze(10,10);

    
}

void HandlePlayerReachTarget(MazeGenerator.Cell targetCell)
{
    
    // Check if the target cell has a submodule
    if (targetCell.submodule != null)
    {
       
        // Get the TextMeshPro component from the submodule
        var textMesh = targetCell.submodule.screenText;

        // If textMesh is not null and not already enabled
        if (!targetCell.submodule.isDefined)
        {
            textMesh.text = logs[currentLogIndex];
            targetCell.submodule.isDefined=true;
            currentLogIndex++;
            if (currentLogIndex == logs.Count)
            {
                targetCell.submodule.SetRemovable(true);
                FinishDeepReapir();
            }
        }
    }
}


public void FinishDeepReapir()
    { 
            Debug.Log("Find target");
            maze.DeleteMaze();

    }
    

}
