using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestManager : MonoBehaviour
{
    public List<string> submoduleNames;
    public enum DeepRepairRuleType
{
    Rule1,
    Rule2
}
    public DeepRepairRuleType currentRuleType;
    private IDeepRepairRule currentRule;
    private Dictionary<DeepRepairRuleType, IDeepRepairRule> ruleInstances;

     private void Awake()
    {
        ruleInstances = new Dictionary<DeepRepairRuleType, IDeepRepairRule>
        {
            { DeepRepairRuleType.Rule1, new Rule1() },
            { DeepRepairRuleType.Rule2, new Rule2() },
            // Add more rules here as needed
        };
    }
    private void Start()
    {
        currentRule = ruleInstances[currentRuleType];
        

    }
    public void startDeepRepair()
    {
        DeepRepairManager.Instance.startReapir(currentRule,submoduleNames);
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
