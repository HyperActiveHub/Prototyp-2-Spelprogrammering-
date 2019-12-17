using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformBehaviour : MonoBehaviour
{
    Camera cam;
    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        if(transform.position.y < cam.ScreenToWorldPoint(Vector2.zero).y)
        {
            Destroy(gameObject);
        }
    }
}
