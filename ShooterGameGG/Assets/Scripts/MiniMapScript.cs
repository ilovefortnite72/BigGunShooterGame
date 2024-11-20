using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapScript : MonoBehaviour
{
    public Transform player;

    

    void LateUpdate()
    {
        //camera follows player position
        Vector3 newPosition = player.position;
        newPosition.z = transform.position.z;
        transform.position = newPosition;

    }
}
