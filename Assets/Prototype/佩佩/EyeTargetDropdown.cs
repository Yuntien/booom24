using System.Collections.Generic;
using UnityEngine;
using TMPro; // 添加这个命名空间来使用TextMesh Pro的组件

public class EyeTargetDropdown : MonoBehaviour
{
    public EyeManager eyeManager;
    private TMP_Dropdown dropdown; // 修改类型为TMP_Dropdown

    private void Start()
    {
        dropdown = GetComponent<TMP_Dropdown>(); // 修改GetComponent调用来获取TMP_Dropdown
        if (dropdown == null)
        {
            Debug.LogError("TMP_Dropdown component not found on the GameObject.");
            return;
        }

        if (eyeManager == null)
        {
            Debug.LogError("EyeManager reference not set in the inspector.");
            return;
        }

        dropdown.ClearOptions();

        List<string> options = new List<string>();
        foreach (EyeTarget target in eyeManager.Targets)
        {
            options.Add(target.name);
        }

        dropdown.AddOptions(options);

        // 设置默认选项为第一项
        dropdown.value = 0;
        dropdown.RefreshShownValue(); // 确保TextMesh Pro下拉菜单显示更新

        // 添加监听器，当选项改变时调用 SetTarget 方法
        dropdown.onValueChanged.AddListener(delegate { OnDropdownChanged(dropdown.value); });
    }

    private void OnDropdownChanged(int index)
    {
        eyeManager.SetTarget(eyeManager.Targets[index]);
    }
}
