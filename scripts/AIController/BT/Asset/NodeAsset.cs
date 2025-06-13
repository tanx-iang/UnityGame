using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AssetNodeType { Sequence, Selector, Condition, Action }

public abstract class NodeAsset : ScriptableObject
{
    public AssetNodeType type;
    public List<NodeAsset> children;
    
    public abstract Node Build(AIController context);
}
