using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordWeapon : MonoBehaviour
{
    public int damage = 1;
    public float pushForce = 2.0f;
    
    public float cooldown = 1.5f;
    public float cooldownBeetwenSwing = 0.5f;
    public Transform attackPoint;
    public float attackRadius;
    public Animator swing;
    public LayerMask whatIsEnemies;
    private float lastSwing; 
    public Animator slash;
    public GameObject rotationSwordParent;
    public GameObject sword;

    public AudioSource swordImpact;
    public AudioSource swordNoImpact;
    private PolygonCollider2D attackCollider;



    Transform t;
    private int touche = 0;
    private void Start()
    {
        t = transform;
        attackCollider = GetComponent<PolygonCollider2D>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {

            sword.transform.SetParent(rotationSwordParent.transform,false);
            if(Time.time-lastSwing>cooldownBeetwenSwing)
            {
                    lastSwing = Time.time;
                    Swing();
            }

        }
    }

    private void Swing()
    {
        if ((transform.eulerAngles.z < 270) && (transform.eulerAngles.z > 90))
        {
            swing.SetTrigger("Attack_Inverted");
        }
        else
        {
            swing.SetTrigger("Attack");
        }
        swordNoImpact.Play();
        slash.SetTrigger("Slash");
        
    }
    private void EnableCollider()
    {
        attackCollider.enabled = true;
    }
    private void DisableCollider()
    {
        attackCollider.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Monster"))
        {
            Debug.Log("ennemy touché : " + touche++);
            Damage dmg = new Damage
            {
                damageAmount = damage,
                origin = transform.position,
                pushForce = pushForce,
                knockback = true
            };
            collision.SendMessage("TakeDamage", dmg);
            swordImpact.Play();
        } 
    }
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
    }
    
}
