using UnityEngine;
using System.Collections.Generic;

public class Page : MonoBehaviour
{
    public List<Entry> entries;

    public void SetupPage(ManualManager manualManager, EyeTransitionManager eyeTransitionManager)
    {
        foreach (var entry in entries)
        {
            entry.SetupButton(manualManager, eyeTransitionManager);
        }
    }
}
