using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class MonsterSpawner : MonoBehaviour
{
    private Random random = new Random();

    public List<SpawnPatern> listePaternes;
    public Monster kobold;
    public Monster slime;

    protected List<Monster> instaciatedMonsters;
    protected bool battleGoingOn = false;
    protected bool monsterSpawned = false;
    protected bool endBattleSignalSent = false;
    protected Room associatedRoom;


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

    public void spawnRandomMob(Room callerRoom)
    {
        instaciatedMonsters = new List<Monster>();
        endBattleSignalSent = false;
        Debug.Log("BONJOUR : " + instaciatedMonsters.Count);
        associatedRoom = callerRoom;
        monsterSpawned = true;
        SpawnPatern selectedSpawnPatern = Instantiate(listePaternes[(random.Next(0, listePaternes.Count))], associatedRoom.transform.position, Quaternion.identity);

        for(int i = 0; i< selectedSpawnPatern.numberOfSpawnPositions; i++)
        {
             Monster instanciatedMonster = Instantiate(GetRandomMob(i, selectedSpawnPatern.numberOfSpawnPositions), selectedSpawnPatern.transform.GetChild(i).position, Quaternion.identity);
             instanciatedMonster.associatedSpawner = this;
             instaciatedMonsters.Add(instanciatedMonster);
            
        }
    }

    public Monster GetRandomMob(int currentI, int maxI)
    {
        if(maxI == 2)
        {
            return kobold;
        } 
        else 
        {
            if((currentI % 3) ==  1)
            {
                return kobold;
            } else
            {
                return slime;
            }

        }
        
    }

    public void OneMonsterDead(Monster monster)
    {
        Debug.Log("ONE DEAD");
        instaciatedMonsters.Remove(monster);
        Debug.Log("monsterSpawned" + monsterSpawned );
        Debug.Log("endBattleSignal" + !endBattleSignalSent);
        Debug.Log("instaciatedMonsters.Count   " + instaciatedMonsters.Count);

        if (monsterSpawned && !endBattleSignalSent && (instaciatedMonsters.Count == 0))
        {
            Debug.Log("IIIIIIN");
            associatedRoom.endBattle();
            endBattleSignalSent = true;
        }
    }

    public void DestroyMonsters()
    {
        foreach(Monster monster in instaciatedMonsters)
        {
            Destroy(monster);
        }

    }


}
