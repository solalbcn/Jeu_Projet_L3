using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : Entity
{
    private bool isChangingRoom = false;
    private bool canMove = true;
    private Room currentRoom = null;
    private int xOffSet = 28;
    private int yOffSet = 20;
    private int xMovementOnRoomChange = 50 ;
    private int yMovementOnRoomChange = 50 ;
    private GameObject destination;
    private Camera cameraCurrent;
    private Animator fadeAnimator = null;
    public bool isFadeInFinished=false;
    public bool isFadeOutFinished = false;
    public bool automaticMovement = false;
    public AudioSource audioSource;
    public HealthBarScript healthBar;
    public Canvas deathCanv;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        cameraCurrent = Camera.main;
        if(cameraCurrent != null) fadeAnimator = cameraCurrent.GetComponentInChildren<Animator>();
        
        healthBar.SetMaxHealth(health);
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        if(canMove)
            animator.SetFloat("speed", movement.sqrMagnitude);
        else
            animator.SetFloat("speed", 0);
    }

    private void FixedUpdate()
    {
        if (rb != null)
        {
            if (!isKnockbacked && canMove && !automaticMovement)
                rb.velocity = (movement.normalized * (movementSpeed * Time.fixedDeltaTime));
            else if (!automaticMovement)
                rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, 0f, Time.fixedDeltaTime), Mathf.Lerp(rb.velocity.y, 0f, Time.fixedDeltaTime));

        }
            
            
    }

    private void PlayerDamagedByMonsterCollision(Collision2D col)
    {

        Damage dmg = new Damage
        {
            
            damageAmount = 1,
            origin = col.transform.position,
            pushForce = col.gameObject.GetComponent<Monster>().knockbackStrength * Mathf.Clamp(col.relativeVelocity.magnitude*0.2f,1f,3f),
            knockback = true
        };
        TakeDamage(dmg);
    }
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.transform.CompareTag("Monster"))
        {
            PlayerDamagedByMonsterCollision(col);
        }
    }
    
    void OnCollisionStay2D(Collision2D col)
    {
        if (col.transform.CompareTag("Monster"))
        {
            PlayerDamagedByMonsterCollision(col);
        }
    }

    protected override void TakeDamage(Damage dmg)
    {
        base.TakeDamage(dmg);
        audioSource.Play();
        animator.SetTrigger("take_damage");
        healthBar.TakeAHit(health);        
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("TRIGGER " + col);
        if (!isChangingRoom && (col.CompareTag("BotEntrance")|| col.CompareTag("TopEntrance")|| col.CompareTag("LeftEntrance") || col.CompareTag("RightEntrance")))
        {
            //col.GetComponentInParent<Room>().ChangeRoom(col);
            StartCoroutine(changeRoom(col));
        }
    }
    
    IEnumerator changeRoom(Collider2D col)
    {
        rb.velocity = Vector2.zero;
        canMove = false;
        isFadeInFinished = false;
        fadeAnimator.SetTrigger("fadeIn");
        isChangingRoom = true;
        yield return new WaitUntil(() => isFadeInFinished == true);
        switch (col.gameObject.tag)
        {
            case "BotEntrance" :
                if (cameraCurrent != null) cameraCurrent.transform.position = new Vector3(cameraCurrent.transform.position.x , cameraCurrent.transform.position.y- yOffSet, cameraCurrent.transform.position.z);
                destination = cameraCurrent.transform.Find("Top").gameObject;
                this.transform.position = new Vector2(destination.transform.position.x, destination.transform.position.y);
                currentRoom = col.GetComponentInParent<Room>().ChangeRoom(Direction.Bot);
                break;
            case "TopEntrance" :
                if (cameraCurrent != null) cameraCurrent.transform.position = new Vector3(cameraCurrent.transform.position.x , cameraCurrent.transform.position.y +yOffSet,cameraCurrent.transform.position.z);
                destination = cameraCurrent.transform.Find("Bot").gameObject;
                this.transform.position = new Vector2(destination.transform.position.x, destination.transform.position.y);
                currentRoom = col.GetComponentInParent<Room>().ChangeRoom(Direction.Top);
                break;
            case "RightEntrance" : 
                if (cameraCurrent != null) cameraCurrent.transform.position = new Vector3(cameraCurrent.transform.position.x +xOffSet, cameraCurrent.transform.position.y,cameraCurrent.transform.position.z );
                destination = cameraCurrent.transform.Find("Left").gameObject;
                this.transform.position = new Vector2(destination.transform.position.x, destination.transform.position.y);
                currentRoom = col.GetComponentInParent<Room>().ChangeRoom(Direction.Right);
                break;
            case "LeftEntrance" :
                if (cameraCurrent != null) cameraCurrent.transform.position = new Vector3(cameraCurrent.transform.position.x - xOffSet, cameraCurrent.transform.position.y,cameraCurrent.transform.position.z);
                destination = cameraCurrent.transform.Find("Right").gameObject;
                this.transform.position = new Vector2(destination.transform.position.x, destination.transform.position.y);
                currentRoom = col.GetComponentInParent<Room>().ChangeRoom(Direction.Left);
                break;
        }
        isFadeOutFinished = false;

        var graphToScan = AstarPath.active.data.gridGraph;
        graphToScan.center = cameraCurrent.transform.position;
        AstarPath.active.Scan(graphToScan);
        fadeAnimator.SetTrigger("fadeOut");
        yield return new WaitUntil(() => isFadeOutFinished == true);
        canMove = true;
        //automaticMovement = true;
        //yield return new WaitUntil(() => automaticMovement == false);
        Debug.Log("transition finie");
        isChangingRoom = false;
        switch (col.gameObject.tag)
        {
            case "BotEntrance" : rb.velocity = new Vector2(-xMovementOnRoomChange,0 );
                break;
            case "TopEntrance" : rb.velocity = new Vector2(xMovementOnRoomChange,0 );
                break;
            case "RightEntrance" : rb.velocity = new Vector2(0,yMovementOnRoomChange );
                break;
            case "LeftEntrance" : rb.velocity = new Vector2(0,-yMovementOnRoomChange );
                break;
        }
    } 

    protected override void Death()
    {
        audioSource.Play();
       // cameraCurrent.GetComponent<AudioListener>().enabled = false;
       
        animator.SetBool("death",true);
        animator.SetTrigger("deathtrigger");
        rb.isKinematic = true;
        Destroy(GetComponent<CapsuleCollider2D>());
    }

    protected  void AfterDeathAnimation()
    {
        GameManager.Instance.RestartGame();
    }
}

