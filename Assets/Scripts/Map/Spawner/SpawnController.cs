using UnityEngine;

public class SpawnController : MonoBehaviour
{
    [SerializeField]
    private int maxEnemies;                 // max number of enemies to spawn at once
    [SerializeField]
    private int enemySpawnDelay;
    [SerializeField]
    private GameObject[] enemySpawnPoints;  // available enemy spawn points

    [SerializeField]
    private int maxProps;                   // max number of props to spawn at once
    [SerializeField]
    private int propSpawnDelay;
    [SerializeField]
    private GameObject[] propSpawnPoints;   // available prop spawn points

    private int totalEnemiesAlive;          // current enemies in scene
    private int totalPropsAvailable;        // current props in scene

    private int randomSpawnPoint;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("RandomEnemySpawner", 0f, enemySpawnDelay);
        InvokeRepeating("RandomPropSpawner", 0f, propSpawnDelay);
    }

    void FixedUpdate()
    {
        // update current number of alive enemies
        totalEnemiesAlive = GameObject.FindGameObjectsWithTag("Enemy").Length;

        // update current number of props 
        totalPropsAvailable = GameObject.FindGameObjectsWithTag("Barrel").Length;
    }

    void RandomEnemySpawner()
    {
        if (totalEnemiesAlive < maxEnemies) {
            randomSpawnPoint = Random.Range(0, enemySpawnPoints.Length);
            enemySpawnPoints[randomSpawnPoint].GetComponent<EnemySpawner>().SpawnEnemy();
        }
    }

    void RandomPropSpawner()
    {
        if(totalPropsAvailable < maxProps) {
            randomSpawnPoint = Random.Range(0, propSpawnPoints.Length);
            propSpawnPoints[randomSpawnPoint].GetComponent<PropSpawner>().SpawnProps();
        }
    }
}
