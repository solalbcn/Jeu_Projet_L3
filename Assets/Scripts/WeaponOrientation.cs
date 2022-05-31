using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponOrientation : MonoBehaviour
{
    public float speed = 1000f;
    public Animator anim;
    public GameObject swordHandle;

    // Update is called once per frame
    void Update()
    {
        Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        
        if ((transform.eulerAngles.z < 270) && (transform.eulerAngles.z > 90) && (swordHandle.transform.localScale.x > 0))
        {
            swordHandle.transform.localScale *= new Vector2(-1, 1);
        }
        else if (((transform.eulerAngles.z > 270) || (transform.eulerAngles.z < 90)) && (swordHandle.transform.localScale.x < 0))
        {
            swordHandle.transform.localScale *= new Vector2(-1, 1);
        }

        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, speed * Time.deltaTime);

        anim.SetFloat("rotation", transform.eulerAngles.z);
    }
}
