using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapFollowPlayer : MonoBehaviour
{
    public Transform player;

    Vector3 vector3;
    private void Start()
    {
        vector3 = new Vector3(0, 0, -30);
    }
    // Update is called once per frame
    void Update()
    {
        transform.position = player.position + vector3;
    }
}
