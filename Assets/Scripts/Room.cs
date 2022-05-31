using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public MonsterSpawner monsterspawner;
    public bool isBotOpening = false;
    public bool isTopOpening = false;
    public bool isLeftOpening = false;
    public bool isRightOpening = false;
    public GameObject botEntrance;
    public GameObject topEntrance;
    public GameObject leftEntrance;
    public GameObject rightEntrance;
    private bool battleGoingOn = false;
    public bool alreadyVisited = false;
    public Room botRoom = null;
    public Room topRoom = null;
    public Room rightRoom = null;
    public Room leftRoom = null;
    [SerializeField] private int depthInGraph = 0;
    public int numberOfOpenings= 0;
    public int x=0;
    public int y=0;
    //var graphToScan = AstarPath.active.data.gridGraph;
    //AstarPath.active.Scan(graphToScan);
    // Start is called before the first frame update


    public void Start()
    {

        initExits();
        
       
    }
    
    // Update is called once per frame
    void Update()
    {    
        
        
    }


    protected void initExits()
    {
        botEntrance.transform.GetChild(0).gameObject.SetActive(!isBotOpening);
        topEntrance.transform.GetChild(0).gameObject.SetActive(!isTopOpening);
        rightEntrance.transform.GetChild(0).gameObject.SetActive(!isRightOpening);
        leftEntrance.transform.GetChild(0).gameObject.SetActive(!isLeftOpening);
        botEntrance.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.SetActive(!isBotOpening);
        botEntrance.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject.SetActive(isBotOpening);
        topEntrance.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.SetActive(!isTopOpening);
        topEntrance.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject.SetActive(isTopOpening);
        rightEntrance.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.SetActive(!isRightOpening);
        rightEntrance.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject.SetActive(isRightOpening);
        leftEntrance.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.SetActive(!isLeftOpening);
        leftEntrance.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject.SetActive(isLeftOpening);
    }

    public virtual Room OnEnterRoom()
    {
        if (!alreadyVisited)
        {
            SetLockDoors(true);
            monsterspawner.spawnRandomMob(this);
            alreadyVisited = true;
        }
        return this;
    }


    public virtual void endBattle()
    {
        SetLockDoors(false);
    }
    
    protected void SetLockDoors(bool lockDoors)
    {
        if (isBotOpening)
        {
            botEntrance.transform.GetChild(0).gameObject.SetActive(lockDoors);
        } 
        if (isTopOpening)
        {  
            topEntrance.transform.GetChild(0).gameObject.SetActive(lockDoors);
        }
        if (isLeftOpening)
        {
            leftEntrance.transform.GetChild(0).gameObject.SetActive(lockDoors);
        }
        if (isRightOpening)
        {
            rightEntrance.transform.GetChild(0).gameObject.SetActive(lockDoors);
        }
    }

    public void computerNumberOfOpenings()
    {
        if (isBotOpening) numberOfOpenings++;
        if (isTopOpening) numberOfOpenings++;
        if (isRightOpening) numberOfOpenings++;
        if (isLeftOpening) numberOfOpenings++;
    }
    
    private int GetNumberOfOpenings()
    {
        return numberOfOpenings;
    }

    public void setGraphDepth(int depth)
    {
        depthInGraph = depth;
    }
    
    public int getDepthInGraph()
    {
        return depthInGraph;
    }

    public Room ChangeRoom(Direction direction)
    {
        switch (direction)
        {
            case Direction.Bot: return botRoom.OnEnterRoom();
                
            case Direction.Top:
                return topRoom.OnEnterRoom();
                
            case Direction.Right:
                return rightRoom.OnEnterRoom();
                
            case Direction.Left:
                return  leftRoom.OnEnterRoom();
               
        }
        return null;
      
    }


    public void DestroyMonsters()
    {
        //monsterspawner.DestroyMonsters();
    }
}
