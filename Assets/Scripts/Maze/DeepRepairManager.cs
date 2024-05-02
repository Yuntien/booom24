using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class DeepRepairManager : MonoBehaviour
{
    // Start is called before the first frame update
    public List<string> logs = new List<string> { "愤怒", "伤心", "害羞" };
    private int currentLogIndex = 0;
    public int GetCurrentLogIndex()
{
    return currentLogIndex;
    
}
public List<string> submoduleNames = new List<string>();
public int submoduleCount = 0;


    private MazeGenerator maze;
    private GameObject player;
    public GameObject playerPrefab;
     private IDeepRepairRule currentRule;


    
void OnEnable()
{
    MazeGenerator.OnPlayerReachTarget += HandlePlayerReachTarget;
}

void OnDisable()
{
    MazeGenerator.OnPlayerReachTarget -= HandlePlayerReachTarget;
}

private void Start() {
    startReapir(new Rule1(),logs);
}
public void startReapir(IDeepRepairRule rule,List<string> names)
{
    SetRule(rule);
    submoduleNames=names;
    submoduleCount=names.Count;
    maze=GetComponent<MazeGenerator>();
    maze.GenerateMaze(10,10);  
}
public void IncrementLogIndex()
{
    currentLogIndex++;
}
public void SetRule(IDeepRepairRule rule)
{
    currentRule = rule;
}void HandlePlayerReachTarget(MazeGenerator.Cell targetCell)
{
    
   currentRule.HandlePlayerReachTarget(targetCell, this);
    if (currentRule.CheckIfRepairFinished(this))
    {
        FinishDeepReapir();
    }
}

public void FinishDeepReapir()
    { 
            Debug.Log("Find target");
            //maze.DeleteMaze();

    }   

}
