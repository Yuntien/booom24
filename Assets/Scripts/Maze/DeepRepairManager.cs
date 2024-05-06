using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.Events;

public class DeepRepairManager : MonoBehaviour
{
    public static DeepRepairManager Instance { get; private set; }
    // Start is called before the first frame update
    [HideInInspector]
    public List<string> submoduleNames;
    private IDeepRepairRule currentRule;
    private int currentLogIndex = 0;
    public int GetCurrentLogIndex()
    {
        return currentLogIndex;
    }
    [HideInInspector]
    public int submoduleCount = 0;

    [HideInInspector]
    public MazeGenerator maze;
    private GameObject player;
    [HideInInspector]
    public UnityEvent OnAllSubmodulesFound;
    private bool isFinished = false;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

    }
    void OnEnable()
    {
        MazeGenerator.OnPlayerReachTarget += HandlePlayerReachTarget;
    }

    void OnDisable()
    {
        MazeGenerator.OnPlayerReachTarget -= HandlePlayerReachTarget;
    }

    private void Start()
    {

    }
    public void startReapir(IDeepRepairRule rule, List<string> names)
    {
        if (maze != null)
        {
            maze.DeleteMaze();
        }
        currentLogIndex = 0;
        isFinished = false;
        SetRule(rule);
        submoduleNames = names;
        submoduleCount = names.Count;
        maze = GetComponent<MazeGenerator>();
        maze.GenerateMaze(8, 8);
    }
    public void EndDeepRepair()
    {
        if (isFinished)
        {
            if (maze != null)
                maze.DeleteMaze();

        }
        else
        {
            Debug.Log("未完成维修，不能退出");

        }

    }
    public void IncrementLogIndex()
    {
        currentLogIndex++;
    }
    public void SetRule(IDeepRepairRule rule)
    {
        currentRule = rule;
    }
    void HandlePlayerReachTarget(MazeGenerator.Cell targetCell)
    {
        currentRule.HandlePlayerReachTarget(targetCell, this);
        if (currentRule.CheckIfRepairFinished(this))
        {
            FinishDeepReapirCheck();
        }
    }
    public void FinishDeepReapirCheck()
    {
        Debug.Log("Find target");
        OnAllSubmodulesFound?.Invoke();
        maze.disablePlayer();
        // 检修完成
        ConversationController.Instance.AllFound();
        //DisassemblyManager.Instance.StartRepairMode(true);
    }

    public void RemoveSubmodule(Submodule submodule)
    {
        submodule.gameObject.SetActive(false);
        maze.SetSubmodulesRemoveable(false);
        isFinished = true;
    }
}


