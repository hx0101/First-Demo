using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    public Transform cam;
    public Transform play;
    public float moveRate;
    private float startPoinX, startPoinY;
    public bool lockY;

    // Start is called before the first frame update
    void Start()
    {
        startPoinX = transform.position.x;
        startPoinY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (lockY)
        {
            transform.position = new Vector2(startPoinX + cam.position.x * moveRate, transform.position.y);
        }
        else
        {
            transform.position = new Vector2(startPoinX + cam.position.x * moveRate, play.position.y - 0.1f);
        }
    }
}
