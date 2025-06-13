using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionNode : Node
{
    private readonly System.Func<bool> _condition;

    public ConditionNode(System.Func<bool> condition)
    {
        _condition = condition;
    }

    public override NodeState Tick()
    {
        return state = (_condition() ? NodeState.Success : NodeState.Failure);
    }
}
