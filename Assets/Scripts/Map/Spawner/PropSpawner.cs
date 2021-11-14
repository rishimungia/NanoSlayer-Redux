using UnityEngine;

public class PropSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject[] props;

    public void SpawnProps()
    {
        int selectedEnemyType = Random.Range(0, props.Length);

        Instantiate(props[selectedEnemyType], transform.position, Quaternion.identity);
    }
}
