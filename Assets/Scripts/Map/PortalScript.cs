using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalScript : MonoBehaviour
{
    [SerializeField]
    private GameObject connectedPortal;
    [SerializeField]
    private bool canEnter = true;

    void OnTriggerEnter2D(Collider2D col) {
        if(col.name == "Player" && canEnter) {
            col.transform.position = connectedPortal.transform.position;
        }
    }
}
