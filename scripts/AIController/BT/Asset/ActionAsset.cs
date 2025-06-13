using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

[CreateAssetMenu(menuName = "BT/Action")]
public class ActionAsset : NodeAsset
{
    public string actionKey;  
    private void OnEnable() => type = AssetNodeType.Action;
    public override Node Build(AIController ctx)
    {
        Func<NodeState> act = () => ctx.ExecuteAction(actionKey);
        return new ActionNode(act);
    }
}
