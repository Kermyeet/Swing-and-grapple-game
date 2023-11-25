using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Transform player;

    //surprisingly simple and good way to keep camera on head position without rotating a bunch of other things.
    void Update() {
        transform.position = player.transform.position;
    }
}
