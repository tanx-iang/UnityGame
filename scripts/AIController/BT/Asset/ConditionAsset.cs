using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;


[CreateAssetMenu(menuName = "BT/Condition")]
public class ConditionAsset : NodeAsset
{
    public string conditionKey; 
    private void OnEnable() => type = AssetNodeType.Condition;
    public override Node Build(AIController ctx)
    {
        Func<bool> cond = () => ctx.EvaluateCondition(conditionKey);
        return new ConditionNode(cond);
    }
}
