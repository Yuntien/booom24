using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Conversa.Runtime;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(menuName = "Game Scene/TalkSceneSO")]
public class TalkSceneSO : ScriptableObject
{
    public AssetReference fixSceneReference;

    public List<string> deepFixPartName;

    public DeepRepairRuleType deepFixRule;

    public Conversation conversation;

    public TalkSceneSO nextScene;

    public bool isEndOfDay;
}
