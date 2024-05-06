using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance; // Singleton instance

    public GameObject shortscreen; // The panel named "screen"
    public GameObject longScreen; // The panel named "screen"
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
    longScreen.gameObject.SetActive(false);
    shortscreen.GetComponent<Image>().color=new Color(0.27f,0.27f,0.27f,1);
    longScreen.GetComponent<Image>().color=new Color(0.27f,0.27f,0.27f,1);
    anomalyModuleText.text="";
    checking.text = "";
    log.text = "";
}
public void UpdateModuleCheckText(string moduleName)
{


    checking.text = $"checking: {moduleName}\nfinding connection...";
    
     
}
public void GetMemeryFormMoudule()
{

    checking.text = $"正在从模块拷贝记忆到回收站\n正在从模块删除记忆";

}
public void SendMemeryToMoudle()
{

    checking.text = $"正在从回收站拷贝记忆到模块\n正在从回收站删除记忆";

}
public void UpdateAnomalyCalculationText(string moduleName, int inAnomalySum, int outAnomalySum, int finalAnomalyValue, bool inAnomaly, bool outAnomaly, bool hasNotifiedAnomaly,int anomalyValue,string errortext,string warningtext)
{
    //string infoText = $"in模块异常值={inAnomalySum}\nout模块异常值={outAnomalySum}\n流入异常值={finalAnomalyValue}\n当前异常值={finalAnomalyValue - anomalyValue}";

    if (finalAnomalyValue > 0)
    {
        shortscreen.GetComponent<Image>().color=Color.red;
        log.text=null;
        log.text=$"\n当前模块（{moduleName}）受到排异反应影响异常，需要寻找排异源头";

        if(errortext!="")
        {
            longScreen.gameObject.SetActive(true);
            longScreen.GetComponent<Image>().color=Color.red;
            anomalyModuleText.text = $"Error message: {errortext}";

        }

        //infoText += $"\n当前模块（{moduleName}）存在流入异常，需要寻找排异源头";
        //log.text=null;
        //log.text = $"问题模块: {moduleName}\n异常阈值: {anomalyValue}\n流入异常值: {finalAnomalyValue}";
    }
    else if (!inAnomaly && outAnomaly)
    {
        shortscreen.GetComponent<Image>().color=Color.red;
        log.text=null;
        log.text=$"\n该模块为排异源头,需要去除";
        if(errortext!="")
        {
            longScreen.gameObject.SetActive(true);
            longScreen.GetComponent<Image>().color=Color.red;
            anomalyModuleText.text = $"Error message: {errortext}";
        }

        //log.text=null;
        //infoText += $"\n找到排异源头";
        //log.text += $"\n问题源头: {moduleName}\n流出异常值: {outAnomalySum}";
    }
    else
    {
        //shortscreen.GetComponent<Image>().color=Color.yellow;
        log.text=null;
        log.text= $"\n该模块正常";
        if(warningtext!="")
        {
            longScreen.gameObject.SetActive(true);
             longScreen.GetComponent<Image>().color=Color.yellow;
             anomalyModuleText.text = $"Warning message: {warningtext}";
        }

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
        //Debug.Log("111222");

        //anomalyModuleText.text = $"问题模块: {moduleName}";
        //taskText.text = $"找到异常源: {0} / {finalAnomalyValue}\n";
    }

    public void UpdateTaskText(string[] foundAnomalySources, int totalAnomalyValue)
    {
        //taskText.text = $"找到异常源: {foundAnomalySources.Length} / {totalAnomalyValue}\n";

        //foreach (string source in foundAnomalySources)
        //{
            //taskText.text += $"异常源: {source}已找到\n";
        //}

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
