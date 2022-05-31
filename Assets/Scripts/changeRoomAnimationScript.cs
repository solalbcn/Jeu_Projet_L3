using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class changeRoomAnimationScript : MonoBehaviour
{
    public Player player = null;
    // Start is called before the first frame update
    void Start()
    {
        //player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void afterFadeIn()
    {
        player.isFadeInFinished = true;       
    }
    public void afterFadeOut()
    {
        player.isFadeOutFinished = true;
    }
}
