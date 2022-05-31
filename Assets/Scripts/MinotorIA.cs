using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using BehaviorTree;


public class MinotorIA : BehaviorTree.Tree
{
    public static float attackDashingSpeed = 2000f;
    public static float attackDashingWait = 10f;
    public float attackCoolDown;
    
    public float tripleCoolDown;
    public float trippleAttackSpeed;

    public Transform target;

    public static float speed = 200f;
    private Seeker seeker;

    private Rigidbody2D rb;
    private float timerAttackCoolDown = 0f;
    private float timerTripleAttackCoolDown = 0f;
    private Animator animator;

    public static float attackRange = 3f ;
    public int monsterDamage;
    public float monsterPushForce;
    public float attackDuration = 1f;
    public float dashDuration = 0.3f;
    private bool paternExecuting = false;

    private bool tripleAttackAnimationEnded = false;
    // Start is called before the first frame update
  
    private void Awake()
    {
        animator = GetComponent<Animator>();
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        target = GameObject.Find("Player").transform;
    }
  
    protected override Node SetupTree()
    {

        /*Node root = new Selector(new List<Node>
        {
             new Sequence(new List<Node>
            {
                new CheckEnemyInDashRange(target,rb),
                new DashAttack(target,rb,animator),
            }),
             new GoTowardPlayer(target,rb),

        }); */
      
        return null;
    }

    void FixedUpdate()
    {
        Debug.Log(paternExecuting);
        timerAttackCoolDown -= Time.deltaTime;
        timerTripleAttackCoolDown -= Time.deltaTime;

        animator.SetFloat("speed", rb.velocity.magnitude);
        computePlayerAngle();
        MinotorPaternEvaluation();
    }

    public void MinotorPaternEvaluation()
    {
        timerAttackCoolDown -= Time.fixedDeltaTime;

        if ((Vector2.Distance(target.position, rb.position) <= attackRange) && (timerAttackCoolDown <= 0) && !paternExecuting && (timerTripleAttackCoolDown <= 0))
        {
            Debug.Log("triplelauncher");
            StartCoroutine(tripleAttack());

        } else if (!paternExecuting && (Vector2.Distance(target.position, rb.position) <= attackRange *4 ) && (timerAttackCoolDown <= 0))
        {
            StartCoroutine(dashAttack());
        }
        else 
        {
            PathFindingFunction();
        }

    }

    void PathFindingFunction()
    {
        Vector2 direction = ((Vector2)target.position - rb.position).normalized;
        Vector2 force = direction * (MinotorIA.speed * Time.fixedDeltaTime);
        rb.AddForce(force);
    }
    
   
    // Update is called once per frame



    IEnumerator dashAttack()
    {
        paternExecuting = true;
        animator.SetTrigger("prepare_dash");
        rb.velocity = Vector2.zero;
        Vector2 attackDirection = ((Vector2)target.position - rb.position).normalized;
        Vector2 attackForceRush = attackDirection * (attackDashingSpeed * Time.fixedDeltaTime);
        yield return new WaitForSeconds(attackDuration);
        animator.SetTrigger("launch_dash");
        rb.velocity = (attackForceRush);
        yield return new WaitForSeconds(dashDuration);
        rb.velocity = Vector2.zero;
        paternExecuting = false;
        animator.SetTrigger("ending_dash");
        timerAttackCoolDown = attackCoolDown;
    }


    IEnumerator tripleAttack()
    {
        paternExecuting = true;
        animator.SetTrigger("Swing_attack");
        Vector2 attackDirection = ((Vector2)target.position - rb.position).normalized;
        Vector2 attackForceRush = attackDirection * (trippleAttackSpeed * Time.fixedDeltaTime);
        rb.velocity = (attackForceRush);
        timerTripleAttackCoolDown = tripleCoolDown;
        yield return new WaitForSeconds(0.5f);
        rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(0.5f);
        paternExecuting = false;
    }

    void computePlayerAngle()
    {
        Vector2 direction = target.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        if (angle < 0) angle += 360f;
        animator.SetFloat("rotation", angle);
    }

    public void endTrippleAttack()
    {
        //animator.SetTrigger("End_attack");
        tripleAttackAnimationEnded = true;
    }

}
