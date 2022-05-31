using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoTowardPlayer : Node
{
    private Rigidbody2D _rb;
    public Transform _target;

    public GoTowardPlayer(Transform target, Rigidbody2D rb)
    {
        _target = target;
        _rb = rb;
    }

    public override NodeState Evaluate()
    {

        Vector2 direction = ((Vector2)_target.position - _rb.position).normalized;
        Vector2 force = direction * (MinotorIA.speed * Time.fixedDeltaTime);

        _rb.AddForce(force);

        state = NodeState.RUNNING;
        return state;
    }
}
