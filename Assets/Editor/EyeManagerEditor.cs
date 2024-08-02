using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(EyeManager))]
public class EyeManagerEditor : Editor
{
    private int selectedIndex = 0; // 用于保存下拉菜单中选定项的索引

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector(); // 绘制默认的Inspector面板

        EyeManager script = (EyeManager)target; // 获取当前编辑的EyeManager实例

        // 获取所有目标的名字
        List<string> options = new List<string>();
        foreach (var t in script.Targets)
        {
            options.Add(t.name);
        }

        // 创建一个下拉菜单让用户选择目标
        selectedIndex = EditorGUILayout.Popup("Select Target", selectedIndex, options.ToArray());

        // 如果有目标可选，并且用户选择了一个新目标
        if (options.Count > 0 && GUILayout.Button("Set As Current Target"))
        {
            // 设置选定的目标为当前目标
            script.SetTarget(script.Targets[selectedIndex]);

            // 告诉 Unity 这个对象已被修改
            EditorUtility.SetDirty(script);
        }
    }
}
