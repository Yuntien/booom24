using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events; 

public class DisassemblyManager : MonoBehaviour
{
    public static DisassemblyManager Instance { get; private set; }
    public GameObject screwdriverPrefab;
    public GameObject screwPrefab;
    public List<GameObject> SubModulescrewPositions = new List<GameObject>();
    public List<GameObject> ModulescrewPositions = new List<GameObject>();

    private GameObject screwdriver;
    public GameObject subModuleVisual;
    public GameObject moduleVisual;
    public UnityEvent OnDisassemblyEnd;  // Add this line
    private List<Screw> screws = new List<Screw>();

    private Submodule currentSubmodule; 
    private Module currentModule; 

    private bool isSub;


    private int screwsRemovedCount = 0;
      private bool isInRepairMode = false;
    //private bool isInDisassemblyMode = false;
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
    private void RemoveAllListeners()
{
    foreach (Screw screw in screws)
    {
        screw.OnScrewRemoved.RemoveAllListeners();
    }
}
    private void Start() {
        //Initialize();
    }
    private void Initialize()
    {
        
        screws.Clear();
        currentSubmodule = null; 
        isInRepairMode = false;
        // Set visual related GameObjects to false
        subModuleVisual.SetActive(false);
        moduleVisual.SetActive(false);
        if(isSub)
        {
            foreach (GameObject screwPosition in SubModulescrewPositions)
            {
            Screw s=Instantiate(screwPrefab, screwPosition.transform.position, Quaternion.identity, screwPosition.transform).GetComponent<Screw>();
            screws.Add(s);
            }

        }  
        else
        {
            foreach (GameObject screwPosition in ModulescrewPositions)
            {
            Screw s=Instantiate(screwPrefab, screwPosition.transform.position, Quaternion.identity, screwPosition.transform).GetComponent<Screw>();
            screws.Add(s);
            }

        }   
        screwsRemovedCount = 0;
    }
    public void StartRepairMode(bool isSub)
    {
        Initialize();
        isInRepairMode = true;
        // Instantiate the screwdriverPrefab
        screwdriver=Instantiate(screwdriverPrefab);
        if(isSub)
        {
            DeepRepairManager.Instance.maze.OnSubmoduleClicked += HandleSubmoduleClicked;
        }
        else
        {
            Robot.Instance.OnModuleClicked += HandleModuleClicked;
        }
        
    }
    private void HandleModuleClicked(Module module)
{
    if (module != null)
    {
        if (module.isRemovable)
        {
            currentModule = module;
            StartDisassemblyMode(module.name);
        }
        else
        {
            Debug.Log("Module is not removable.");
        }
    }
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
            RemoveAllListeners();
            if(isSub)
            {
                subModuleVisual.SetActive(true);
                subModuleVisual.GetComponentInChildren<TextMeshPro>().text = moduleName;

            }
            else
            {
                moduleVisual.SetActive(true);
                moduleVisual.GetComponentInChildren<TextMeshPro>().text = moduleName;
                Robot.Instance.OnModuleClicked -= HandleModuleClicked;
                

            }
            
            //moduleVisual.SetActive(false);
           
        
         foreach (Screw screw in screws)
        {
            screw.OnScrewRemoved.AddListener(HandleScrewRemoved);
        }
    }
     private void HandleScrewRemoved()
    {
     
    screwsRemovedCount++;
    Debug.Log("remove screw");

    // If all screws have been removed
    if (screwsRemovedCount >= screws.Count)
    {
        // Disable the visual GameObjects
        subModuleVisual.SetActive(false);
        Destroy(screwdriver);
        moduleVisual.SetActive(false);
        if (currentSubmodule != null)
        {
            DeepRepairManager.Instance.RemoveSubmodule(currentSubmodule);
            currentSubmodule = null; 
        }
        if (currentModule != null)
        {
            Robot.Instance.RemoveModule(currentModule);
            currentModule = null; 
        }
        isInRepairMode=false;
        OnDisassemblyEnd?.Invoke();
    }
    }
}
