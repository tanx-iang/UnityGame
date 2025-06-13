using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(menuName = "BT/Selector")]
public class SelectorAsset : NodeAsset
{
    private void OnEnable() => type = AssetNodeType.Selector;
    public override Node Build(AIController ctx)
    {
        var list = children.Select(c => c.Build(ctx)).ToList();
        return new SelectorNode(list);
    }
}
