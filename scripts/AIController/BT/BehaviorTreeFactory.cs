using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BehaviorTreeFactory
{
    public static Node Create(BehaviorTreeAsset asset, AIController ctx)
    {
        return asset.root.Build(ctx);
    }
}
