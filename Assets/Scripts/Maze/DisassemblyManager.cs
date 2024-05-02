using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events; 

public class DisassemblyManager : MonoBehaviour
{
    public static DisassemblyManager Instance { get; private set; }
    public GameObject screwdriverPrefab;
    public GameObject subModuleVisual;
    public GameObject moduleVisual;
    public UnityEvent OnDisassemblyEnd;  // Add this line
    private Screw[] screws;

    private Submodule currentSubmodule; 


    private int screwsRemovedCount = 0;
      private bool isInRepairMode = false;
    //private bool isInDisassemblyMode = false;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start() {
        Initialize();
    }
    private void Initialize()
    {
        currentSubmodule = null; 
        isInRepairMode = false;
        // Set visual related GameObjects to false
        subModuleVisual.SetActive(false);
        moduleVisual.SetActive(false);

        // Find all screws and enable them
        screws = GetComponentsInChildren<Screw>(true); // passing true to include inactive children
        foreach (Screw screw in screws)
        {
            screw.gameObject.SetActive(true);
        }

        screwsRemovedCount = 0;
    }
    public void StartRepairMode()
    {
        Initialize();
        isInRepairMode = true;
        // Instantiate the screwdriverPrefab
        Instantiate(screwdriverPrefab);

        DeepRepairManager.Instance.maze.OnSubmoduleClicked += HandleSubmoduleClicked;
    }
    private void HandleSubmoduleClicked(Submodule submodule)
    {

        if (submodule != null)
                {
                    if (submodule.isRemovable)
                    {
                         currentSubmodule = submodule; 
                        StartDisassemblyMode(submodule.name);
                    }
                    else
                    {
                        Debug.Log("Submodule is not removable.");
                    }
                }
    }
    public void StartDisassemblyMode(string moduleName)
    {
            subModuleVisual.SetActive(true);
            //moduleVisual.SetActive(false);
            subModuleVisual.GetComponentInChildren<TextMeshPro>().text = moduleName;
            screws = subModuleVisual.GetComponentsInChildren<Screw>();
        
         foreach (Screw screw in screws)
        {
            screw.OnScrewRemoved.AddListener(HandleScrewRemoved);
        }
    }
     private void HandleScrewRemoved()
    {
        screwsRemovedCount++;

        // If all screws have been removed
        if (screwsRemovedCount >= screws.Length)
        {
            // Disable the visual GameObjects
            subModuleVisual.SetActive(false);
            //moduleVisual.SetActive(false);
            if (currentSubmodule != null)
            {
                DeepRepairManager.Instance.RemoveSubmodule(currentSubmodule);
                currentSubmodule = null; 
            }
            OnDisassemblyEnd?.Invoke();
        }
    }
}
