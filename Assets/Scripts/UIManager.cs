using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager instance; // Singleton instance

    public GameObject screen; // The panel named "screen"
    public TextMeshProUGUI checking; // The UI Text element to display anomaly module info
    public TextMeshProUGUI log; // The UI Text element to display task info
    public TextMeshProUGUI anomalyModuleText;
    public TextMeshProUGUI taskText;


    private void Awake()
    {
        // Singleton pattern
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ClearText()
{
    checking.text = "";
    log.text = "";
}
public void UpdateModuleCheckText(string moduleName)
{
    if (!screen.activeSelf)
            screen.SetActive(true);

    checking.text = $"checking: {moduleName}\nfinding connection...";
    
     
}
public void UpdateAnomalyCalculationText(string moduleName, int inAnomalySum, int outAnomalySum, int finalAnomalyValue, bool inAnomaly, bool outAnomaly, bool hasNotifiedAnomaly,int anomalyValue)
{
    //string infoText = $"in模块异常值={inAnomalySum}\nout模块异常值={outAnomalySum}\n流入异常值={finalAnomalyValue}\n当前异常值={finalAnomalyValue - anomalyValue}";

    if (finalAnomalyValue > 0)
    {
        log.text=null;
        log.text=$"\n当前模块（{moduleName}）input异常，need finding problem source";

        //infoText += $"\n当前模块（{moduleName}）存在流入异常，需要寻找排异源头";
        //log.text=null;
        //log.text = $"问题模块: {moduleName}\n异常阈值: {anomalyValue}\n流入异常值: {finalAnomalyValue}";
    }
    else if (!inAnomaly && outAnomaly)
    {
        log.text=null;
        log.text=$"\nfind problem source";

        //log.text=null;
        //infoText += $"\n找到排异源头";
        //log.text += $"\n问题源头: {moduleName}\n流出异常值: {outAnomalySum}";
    }
    else
    {
        log.text=null;
        log.text= $"\n该模块不是问题模块或problem source";
        //log.text=null;
        //infoText += $"\n该模块正常";
    }

    //taskText.text += $"\n{infoText}";
}
public void UpdateConnectionInfoText(string startModuleName, string endModuleName, string direction, string status)
{
    log.text += $"\n{status}";
}
    public void UpdateAnomalyModuleText(string moduleName, int finalAnomalyValue,int anomalyValue)
    {
        Debug.Log("111222");

        anomalyModuleText.text = $"问题模块: {moduleName}\n异常值: {finalAnomalyValue}\nyuzhi: {anomalyValue}";
        taskText.text = $"找到异常源: {0} / {finalAnomalyValue}\n";
    }

    public void UpdateTaskText(string[] foundAnomalySources, int totalAnomalyValue)
    {
        taskText.text = $"找到异常源: {foundAnomalySources.Length} / {totalAnomalyValue}\n";

        foreach (string source in foundAnomalySources)
        {
            taskText.text += $"异常源: {source}已找到\n";
        }

        /*if (foundAnomalySources.Length == totalAnomalyValue)
        {
            DisassemblyManager.Instance.StartRepairMode(false);
            //checkCompleteUI.SetActive(true);
        }*/
    }

    public void ContinueTalk()
    {
        
        GameManager.Instance.ContinueTalk();
    }
}
