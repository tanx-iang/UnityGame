using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionNode : Node
{
    private readonly System.Func<NodeState> _action;

    public ActionNode(System.Func<NodeState> action)
    {
        _action = action;
    }

    public override NodeState Tick()
    {
        return state = _action();
    }
}
