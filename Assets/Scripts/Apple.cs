using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apple : MonoBehaviour
{
    public LayerMask player;

    void Start()
    {

    }

    void Update()
    {
        if (Physics2D.OverlapCircle(transform.position, 0.02f, player))
        {
            Debug.Log("按下F键拾取苹果");

            if (Input.GetKeyDown("f"))
            {
                gameObject.SetActive(false);
            }
        }
    }

}
