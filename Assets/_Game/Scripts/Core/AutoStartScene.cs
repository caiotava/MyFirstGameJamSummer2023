using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoStartScene : MonoBehaviour
{
    [SerializeField] private SpawnManager spawnManager;
    
    // Start is called before the first frame update
    void Start()
    {
        spawnManager.StartSpawn();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
