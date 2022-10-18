using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCheck : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D other)
    {
        PlayerFSM playFSM = other.gameObject.GetComponent<PlayerFSM>();
        if (playFSM != null)
        {
            playFSM.ChangeHealth();
        }
    }

}
