using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    public Slider slider;
    private PlayerFSM playerFsm;

    // Start is called before the first frame update
    void Start()
    {
        playerFsm = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerFSM>();
    }

    // Update is called once per frame
    void Update()
    {
        slider.value = playerFsm.health / 100;
    }
}
