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

        anomalyModuleText.text = $"Problem Module: {moduleName}\nAnomaly Value: {anomalyValue}";
        taskText.text = $"Found Anomaly Sources: {0} / {anomalyValue}\n";
    }

    public void UpdateTaskText(string[] foundAnomalySources, int totalAnomalyValue)
    {
        taskText.text = $"Found Anomaly Sources: {foundAnomalySources.Length} / {totalAnomalyValue}\n";

        foreach (string source in foundAnomalySources)
        {
            taskText.text += $"Anomaly Source: {source} Found\n";
        }

        if (foundAnomalySources.Length == totalAnomalyValue)
        {
            checkCompleteUI.SetActive(true);
        }
    }
}
