using UnityEngine;

public class PropSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject[] props;

    public bool SpawnProps(LayerMask overlapMask, float overlapRadius)
    {
        bool canSpawn = !Physics2D.OverlapCircle(transform.position, overlapRadius, overlapMask);

        if(canSpawn) {
            int selectedEnemyType = Random.Range(0, props.Length);
            Instantiate(props[selectedEnemyType], transform.position, Quaternion.identity);
            return true;
        }
        return false;
    }
}
