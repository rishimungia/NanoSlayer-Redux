using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject[] enemies;

    public void SpawnEnemy()
    {
        int selectedEnemyType = Random.Range(0, enemies.Length);

        Instantiate(enemies[selectedEnemyType], transform.position, Quaternion.identity);
    }
}
