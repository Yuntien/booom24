using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance; // Singleton instance

    public GameObject screen; // The panel named "screen"
    public TextMeshProUGUI anomalyModuleText; // The UI Text element to display anomaly module info
    public TextMeshProUGUI taskText; // The UI Text element to display task info
    public GameObject checkCompleteUI; // The UI for "check complete"

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

    public void UpdateAnomalyModuleText(string moduleName, int anomalyValue)
    {
        if (!screen.activeSelf)
            screen.SetActive(true);

        anomalyModuleText.text = $"问题模块: {moduleName}\n异常值: {anomalyValue}";
        taskText.text = $"找到异常源: {0} / {anomalyValue}\n";
    }

    public void UpdateTaskText(string[] foundAnomalySources, int totalAnomalyValue)
    {
        taskText.text = $"找到异常源: {foundAnomalySources.Length} / {totalAnomalyValue}\n";

        foreach (string source in foundAnomalySources)
        {
            taskText.text += $"异常源: {source}已找到\n";
        }

        if (foundAnomalySources.Length == totalAnomalyValue)
        {
            checkCompleteUI.SetActive(true);
        }
    }
}
