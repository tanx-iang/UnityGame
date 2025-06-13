using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(menuName = "BT/Sequence")]
public class SequenceAsset : NodeAsset
{
    private void OnEnable() => type = AssetNodeType.Sequence;
    public override Node Build(AIController ctx)
    {
        var list = children.Select(c => c.Build(ctx)).ToList();
        return new SequenceNode(list);
    }
}
