using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private GameObject player;

    private void LateUpdate()
    {
        Vector3 newPos = new Vector3(player.transform.position.x, player.transform.position.y, -10f);
        transform.position = newPos;
    }
}
