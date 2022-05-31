using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventScript : MonoBehaviour
{
    public GameObject meleeWeapon;

    public GameObject meleeHandle;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void ReturnSwordToHandle() {
        meleeWeapon.transform.SetParent(meleeHandle.transform,false);

    }
}
