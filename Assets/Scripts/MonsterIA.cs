using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class MonsterIA : MonoBehaviour
{
    public Transform target;

    public float speed = 200f;

    public float nextWayPointDistance = 3f;

    private Path path;
    private int currentWaypoint = 0;
    private bool reachedEndOfPath;

    private Seeker seeker;
    private Rigidbody2D rb;
    public float attackRange ;
    public float attackDashingSpeed;
    public float attackCoolDown;
    private float timerAttackCoolDown = 0f;
    private Animator animator;
    public float monsterAttackRadius;
    public int monsterDamage;
    public float monsterPushForce;
    public float attackDuration = 1f;
    public float dashDuration = 0.3f;
    private bool dashingAttackExecuting = false;


    private void Awake()
    {
        target = GameObject.Find("Player").transform;
    }

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        InvokeRepeating("UpdatePath",0f,.5f);
        seeker.StartPath(rb.position, target.position, OnPathComplete);

    }

    void UpdatePath()
    {
        if (seeker.IsDone())
        {
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        }
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        timerAttackCoolDown -= Time.fixedDeltaTime;
        if ((Vector2.Distance(target.position, rb.position) <= attackRange) && (timerAttackCoolDown <=0) && !dashingAttackExecuting )
        {
            StartCoroutine(dashAttack());
            
        }
        else if(!dashingAttackExecuting)
        {
            if (path == null)
            {
                return;
            }

            if (currentWaypoint >= path.vectorPath.Count)
            {
                reachedEndOfPath = true;
                return;
            }
            else
            {
                reachedEndOfPath = false;
            }

            Vector2 direction = ((Vector2) path.vectorPath[currentWaypoint] - rb.position).normalized;
            Vector2 force = direction * (speed * Time.fixedDeltaTime);

        
            rb.AddForce(force);
        
            if ((rb.velocity.magnitude <= 0.1)  )
            {
                AstarPath.active.Scan();
           
            }

            float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

            if (distance < nextWayPointDistance)
            {
                currentWaypoint++;
            }

            MonsterSpriteOrientation(force.x);


        }
        
    }
    IEnumerator dashAttack()
    {
        float previousSpriteSize;
        dashingAttackExecuting = true;
        animator.SetTrigger("Monster_Attack");
        rb.velocity = Vector2.zero;
        Vector2 attackDirection = ((Vector2)target.position - rb.position).normalized;
        Vector2 attackForceRush = attackDirection * (attackDashingSpeed * Time.fixedDeltaTime);
        yield return new WaitForSeconds(attackDuration);
        animator.SetTrigger("Attack_ready");
        rb.velocity = (attackForceRush);
        MonsterSpriteOrientation(attackDirection.x);
        yield return new WaitForSeconds(dashDuration);
        rb.velocity = Vector2.zero;
        dashingAttackExecuting = false;
        animator.SetTrigger("Attack_end");
        timerAttackCoolDown = attackCoolDown;
    }

    void MonsterSpriteOrientation(float desiredDirection)
    {
        if (desiredDirection <= 0.01f)
        {
            this.transform.localScale = new Vector3(-5f, 5f, 1f);

        } else if (desiredDirection >= 0.01f)
        {
            this.transform.localScale = new Vector3(5f, 5f, 1f);
        }
        
    }

  
}
