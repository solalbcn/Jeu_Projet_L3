using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoom : Room
{

    public BossSpawner bossSpawner;
    public Canvas winCanvas;
    public Animator fadeAnimator;
    // Start is called before the first frame update   


    public void Start()
    {
       if(Camera.main != null) fadeAnimator = Camera.main.GetComponentInChildren<Animator>();
    }

    public void closeOtherDoor()
    {
        if(rightRoom != null)
        {
            isLeftOpening = false;

        } else if(leftRoom != null)
        {
            isRightOpening = false;
        }

        initExits();
    }

    public override Room OnEnterRoom()
    {
        if (!alreadyVisited)
        {
            base.SetLockDoors(true);
            bossSpawner.SpawnBoss(this);
            alreadyVisited = true;
        }
        return this;
    }

    public override void endBattle()
    {
        GameManager.Instance.RestartGame();

    }
}
