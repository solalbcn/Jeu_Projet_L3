using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public int health;
    public float movementSpeed = 1f;
    public Rigidbody2D rb;
    public Animator animator;
    protected Vector2 movement;
    public float speed;
    public float knockbackStrength;
    public float knockbackTime;
    protected float invulnerabilityFrame;
    public float timeBetweenInvulnerabilityFrame;
    protected bool isCoroutineExecuting = false;
    public GameObject player;
    protected bool collided;
    protected bool isKnockbacked;


    // Start is called before the first frame update
    protected virtual void Start()
    {
        
        
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (invulnerabilityFrame > 0)
        {
            invulnerabilityFrame -= Time.deltaTime;
        }
        
    }
    
    protected virtual void TakeDamage(Damage damage)
    {
        if (!(invulnerabilityFrame <= 0)) return;
        health -= damage.damageAmount;
        Debug.Log(health);
        Vector2 direction = transform.position- damage.origin;
        if (damage.knockback)
        {
            rb.velocity=direction.normalized * damage.pushForce;
            StartCoroutine(KnockBack(knockbackTime));
        }
           
        if (health <= 0)
        {
            Death();
            Debug.Log("DEAD");
        }
        invulnerabilityFrame = timeBetweenInvulnerabilityFrame;

    }
    
   protected IEnumerator KnockBack(float time)
    {
        if (isCoroutineExecuting)
            yield break;
 
        isCoroutineExecuting = true;
        isKnockbacked = true;
 
        yield return new WaitForSeconds(time);

        rb.velocity = new Vector2(0, 0);
 
        isCoroutineExecuting = false;
        isKnockbacked = false;
    }
    
    
    virtual protected void Death()
    {
        
    }
    
}
