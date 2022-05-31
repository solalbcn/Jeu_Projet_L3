using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class BossSpawner : MonsterSpawner
{
     
    public Monster boss;
    public SpawnPatern bossSpawPatern;

    // Start is called before the first frame update

    private void Awake()
    {
        
    }
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnBoss(Room callerRoom)
    {
        instaciatedMonsters = new List<Monster>();
        endBattleSignalSent = false;
        associatedRoom = callerRoom;
        monsterSpawned = true;
        SpawnPatern bossSpawnPaternInstantiate = Instantiate(bossSpawPatern, associatedRoom.transform.position, Quaternion.identity);
        Monster instanciatedBoss = Instantiate(boss, bossSpawnPaternInstantiate.transform.GetChild(0).position, Quaternion.identity);
        instanciatedBoss.associatedSpawner = this;
        instaciatedMonsters.Add(instanciatedBoss);
    }



}
