using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPatern : MonoBehaviour
{
    public int numberOfSpawnPositions = 0;

    private void Awake()
    {
        foreach (Transform child in transform)
        {
            numberOfSpawnPositions++;
        }

    }
    // Start is called before the first frame update
    void Start()
    {
       
      }

    // Update is called once per frame
    void Update()
    {
        
    }
}
