using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashAttack : Node
{
    // Start is called before the first frame update

    private Rigidbody2D _rb;
    private Transform _target;
    private Animator _animator;
    private bool targetSet = false;
    private bool attackExecuted = false;
    private float timerBeforeLaunch = 0f;
    private float timerAfterAttack = 0f;
    Vector2 attackForceRush;
    public DashAttack(Transform target, Rigidbody2D rb, Animator animator)
    {
        _target = target;
        _rb = rb;
        _animator = animator;
    }

    public override NodeState Evaluate()
    {
        timerBeforeLaunch -= Time.fixedDeltaTime;
        timerAfterAttack -= Time.fixedDeltaTime;
        if (!targetSet)
        {
            targetSet = true;
            _animator.SetTrigger("prepare_dash"); 
            _rb.velocity = Vector2.zero;
            Vector2 attackDirection = ((Vector2)_target.position - _rb.position).normalized;
            attackForceRush = attackDirection * (MinotorIA.attackDashingSpeed * Time.fixedDeltaTime);
            timerBeforeLaunch = MinotorIA.attackDashingWait;
        }
        else if(targetSet && (timerBeforeLaunch <= 0 ))
        {
            _animator.SetTrigger("launch_dash");
            _rb.velocity = (attackForceRush);
            _rb.velocity = Vector2.zero;
            _animator.SetTrigger("ending_dash");
            timerAfterAttack = MinotorIA.attackDashingWait;
        } else if (attackExecuted && (timerAfterAttack <= 0))
        {
            targetSet = false;
            return NodeState.SUCCESS;
        }
        state = NodeState.RUNNING;
        return state;
    }

}
