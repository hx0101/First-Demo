using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapCheck : MonoBehaviour
{
    float timer;
    public void OnTriggerStay2D(Collider2D other)
    {
        PlayerFSM fsm = other.gameObject.GetComponent<PlayerFSM>();
        if (fsm != null && timer <= 0)
        {
            fsm.ChangeHealth();
            timer = 1f;
        }
    }

    private void Start()
    {
        timer = 0f;
    }

    private void FixedUpdate()
    {
        if (timer > 0)
        {
            timer -= Time.fixedDeltaTime;
        }
        else
        {
            return;
        }
    }
}
