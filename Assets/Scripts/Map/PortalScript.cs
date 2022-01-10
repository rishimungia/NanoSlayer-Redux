using UnityEngine;

public class PortalScript : MonoBehaviour
{
    [SerializeField]
    private GameObject connectedPortal;
    [SerializeField]
    public bool spawnRight;
    [SerializeField]
    private float spawnDistance;
    [SerializeField]
    private bool canEnter = true;

    void OnTriggerEnter2D(Collider2D col) {
        if(col.name == "Player" && canEnter) {
            bool right = connectedPortal.GetComponent<PortalScript>().spawnRight;
            col.transform.position = new Vector2(connectedPortal.transform.position.x + (right ? spawnDistance : -1 * spawnDistance), connectedPortal.transform.position.y);
        }
    }
}
