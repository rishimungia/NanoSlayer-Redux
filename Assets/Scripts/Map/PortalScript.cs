using System.Collections;
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
    [SerializeField]
    private float teleportCooldown = 0.5f;

    void OnTriggerEnter2D(Collider2D col) {
        if(canEnter && PlayerMovement.canTeleport && col.name == "Player") {
            StartCoroutine(Teleport(col));
        }
    }

    IEnumerator Teleport(Collider2D player) {
        bool right = connectedPortal.GetComponent<PortalScript>().spawnRight;
        player.transform.position = new Vector2(connectedPortal.transform.position.x + (right ? spawnDistance : -1 * spawnDistance), connectedPortal.transform.position.y);
        
        SoundManager.PlaySound(SoundManager.FXSounds.Teleport);
        PlayerMovement.canTeleport = false;

        yield return new WaitForSeconds(teleportCooldown);
        PlayerMovement.canTeleport = true;
    }
}
