using UnityEngine;

public class AutoStartScene : MonoBehaviour
{
    [SerializeField] private SpawnManager spawnManager;

    // Start is called before the first frame update
    private void Start()
    {
        spawnManager.StartSpawn();
    }
}