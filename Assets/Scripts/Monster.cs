using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Monster : Entity
{
    private float dazedTime;
    public float pushForce = 2.0f;
    public float startDazedTime;
    public Transform target;
    public MonsterSpawner associatedSpawner;
    public AudioSource audioSource;
    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();


    }
    // Update is called once per frame
     protected override void Update()
    {
        base.Update();
        if (dazedTime <= 0)
        {
            movementSpeed = 5;
        }
        else
        {
            movementSpeed = 0;
            dazedTime -= Time.deltaTime;
        }
        
        
    }


    protected override void TakeDamage(Damage damage)
    {
        base.TakeDamage(damage);
    }

    private void FixedUpdate()
    {
      /*  if(!isKnockbacked)
            rb.velocity =(Vector2)(target.transform.position - transform.position).normalized * speed * Time.deltaTime;
        else
            rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, 0f, Time.fixedDeltaTime), Mathf.Lerp(rb.velocity.y, 0f, Time.fixedDeltaTime));  
       */
    }

    void OnCollisionEnter2D(Collision2D col)
    {

    }


    protected override void Death()
    {
        Debug.Log("MonsterDeath");
        animator.SetTrigger("death");
        Destroy(gameObject);
        gameObject.GetComponent<Collider2D>().enabled = false;
        if(associatedSpawner != null) associatedSpawner.OneMonsterDead(this);
         
    }

    public void setSpawner(MonsterSpawner spawn)
    {
        
    }


    public void waitDeathAnimation()
    {
       
    }
 

    
 

    
}
