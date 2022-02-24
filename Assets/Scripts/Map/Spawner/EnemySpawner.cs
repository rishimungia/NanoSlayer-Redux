using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject[] enemies;

    public bool SpawnEnemy(LayerMask overlapMask, float overlapRadius)
    {
        bool canSpawn = !Physics2D.OverlapCircle(transform.position, overlapRadius, overlapMask);

        if(canSpawn) {
            int selectedEnemyType = Random.Range(0, enemies.Length);
            Instantiate(enemies[selectedEnemyType], transform.position, Quaternion.identity);
            return true;
        }
        return false;
    }
}
