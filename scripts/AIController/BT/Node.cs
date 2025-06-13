using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NodeState { Success, Failure, Running }

public abstract class Node
{
    protected NodeState state;
    public NodeState State => state;
    public abstract NodeState Tick();
    public virtual void Reset(){}
}

