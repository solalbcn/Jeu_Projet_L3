using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckEnemyInDashRange : Node
{
    private Rigidbody2D _rb;
    private Transform _target;
    private float timerBetweenAttack = 0f;
    private float valuetimerBetweenAttack = 10f;

    public CheckEnemyInDashRange(Transform target, Rigidbody2D rb)
    {
        _target = target;
        _rb = rb;
    }

    // Start is called before the first frame update
    public override NodeState Evaluate()
    {
        timerBetweenAttack -= Time.fixedDeltaTime ;
        if ((Vector2.Distance(_target.position, _rb.position) <= MinotorIA.attackRange * 4) && timerBetweenAttack <=0)
        {
            timerBetweenAttack = valuetimerBetweenAttack;
            return NodeState.SUCCESS;
        }

        return NodeState.FAILURE;  
    }
    }
