using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockCharacter : MonoBehaviour
{
    public Collider2D blockerCollider;

    public Collider2D characterCollider;
    // Start is called before the first frame update
    void Start()
    {
        Physics2D.IgnoreCollision(characterCollider,blockerCollider , true);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
