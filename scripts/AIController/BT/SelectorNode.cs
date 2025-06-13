using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectorNode : Node
{
    private readonly List<Node> _children;
    private int _currentIndex = 0;

    public SelectorNode(List<Node> children)
    {
        _children = children;
    }

    public override NodeState Tick()
    {
        for (; _currentIndex < _children.Count; )
        {
            var childState = _children[_currentIndex].Tick();
            if (childState == NodeState.Running)
                return state = NodeState.Running;
            if (childState == NodeState.Success)
            {
                Reset();
                return state = NodeState.Success;
            }
            _currentIndex++;
        }
        Reset();
        return state = NodeState.Failure;
    }

    public override void Reset()
    {
        _currentIndex = 0;
        foreach (var c in _children)
            c.Reset();
    }
}
