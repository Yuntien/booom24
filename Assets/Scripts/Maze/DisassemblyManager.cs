using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
//using Unity.VisualScripting;
using DG.Tweening;

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

    public GameObject subSprite;
    public GameObject moduleSprite;

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

    private void OnEnable()
    {
        OnDisassemblyEnd.AddListener(ConversationController.Instance.ContinueConversation);
    }

    private void OnDisable()
    {
        OnDisassemblyEnd.RemoveAllListeners();
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
       //维修开始，触发这里

    public void StartRepairMode(bool isSuboutside)
    {
        isSub=isSuboutside;
        DragDrop2D line=FindObjectOfType<DragDrop2D>();
        line?.DisableDrag();
        Debug.Log("Repair");
        Initialize(); 
        isInRepairMode = true;
        // Instantiate the screwdriverPrefab
        screwdriver=Instantiate(screwdriverPrefab);
        if(isSub)
        {
            DeepRepairManager.Instance.maze.OnSubmoduleClicked += HandleSubmoduleClicked;
            //ConversationController.Instance.ContinueConversation();

        }
        else
        {
            Robot.Instance.OnModuleClicked += HandleModuleClicked;
        }
        
    }
    //维修时点击子模块，触发这里
    private void HandleModuleClicked(Module module)
{
        if (isSub)
        {
            return;
        }
            //ConversationController.Instance.ContinueChoice(module.Name);
        
        
        if (module != null)
    {
        if (module.isRemovable)
        {
            currentModule = module;
            StartDisassemblyMode(module.showName);
        }
        else
        {
            Debug.Log("Module is not removable.");
        }
    }
}
 //维修时点击模块，触发这里
    private void HandleSubmoduleClicked(Submodule submodule)
    {
        if(!isSub)
        {
            return;
        }
        //

        if (submodule != null)
                {
                    if (submodule.isRemovable)
                    {
                         currentSubmodule = submodule; 
                        //ConversationController.Instance.ContinueChoice(submodule.name);
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

    // 所有螺丝都已经被拧掉
    if (screwsRemovedCount >= screws.Count)
    {  
        Transform target;
        // 获取 AudioSource 组件
        //AudioSource audioSource = GetComponent<AudioSource>();
        if (isSub)
        {
            target = subSprite.transform;
            subModuleVisual.GetComponentInChildren<TextMeshPro>().text="";
        }
        else
        {
            target = moduleSprite.transform;
            moduleVisual.GetComponentInChildren<TextMeshPro>().text="";
        }
        // 创建一个新的序列
        Sequence sequence = DOTween.Sequence();
        sequence.AppendInterval(1f);

        // 向序列中添加一个旋转动画
        sequence.Append(target.DORotate(new Vector3(0, 0, 45), 0.5f));

        // 向序列中添加一个移动动画
        sequence.Append(target.DOMoveY(-15f, 1.2f).SetRelative().SetEase(Ease.OutQuad));

        // 向序列中添加一个延迟
        sequence.AppendInterval(1.5f);

        // 向序列中添加一个回调来播放音频剪辑
        //sequence.AppendCallback(() => audioSource.Play());

        // 向序列中添加一个延迟
        //sequence.AppendInterval(audioSource.clip.length);

        // 向序列中添加一个回调来销毁模块或子模块
        sequence.AppendCallback(() =>
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
        });

        // 开始序列
        sequence.Play();
    }
}
    public void DestroyScrewdriver()
    {
        if (screwdriver != null) 
            Destroy(screwdriver);
    }
}
