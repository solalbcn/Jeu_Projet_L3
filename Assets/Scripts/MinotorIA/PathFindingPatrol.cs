using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using Pathfinding;
public class PathFindingPatrol : Node
{
    private Path path;
    private int currentWaypoint = 0;
    private bool reachedEndOfPath;
    private Seeker _seeker;
    private Rigidbody2D _rb;
    public float nextWayPointDistance = 3f;
    public Transform _target;


    public PathFindingPatrol(Seeker seeker, Transform target, Rigidbody2D rb)
    {
        _seeker = seeker;
        _target = target;
        _rb = rb;
        seeker.StartPath(rb.position, target.position, OnPathComplete);
    }
    // Start is called before the first frame update
    public override NodeState Evaluate()
    {
        UpdatePath();
        if (path == null)
        {
            return NodeState.RUNNING;
        }

        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return NodeState.RUNNING;
        }
        else
        {
            reachedEndOfPath = false;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - _rb.position).normalized;
        Vector2 force = direction * (MinotorIA.speed * Time.fixedDeltaTime);

        _rb.AddForce(force);

        if ((_rb.velocity.magnitude <= 0.1))
        {
            AstarPath.active.Scan();

        }

        float distance = Vector2.Distance(_rb.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWayPointDistance)
        {
            currentWaypoint++;
        }

        state = NodeState.RUNNING;
        return state;
    }

    IEnumerator UpdatePath()
    {
        if (_seeker.IsDone())
        {
            _seeker.StartPath(_rb.position, _target.position, OnPathComplete);
        }
        yield return new WaitForSeconds(0.5f);
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }



}
