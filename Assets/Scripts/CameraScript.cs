using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public Transform follow;
    [Tooltip("Should the camera only follow the object if it is moving upward?")]
    public bool onlyFollowUp;
    public float offset;
    public float camSpeed = 10;
    void Start()
    {

    }


    void Update()
    {
        Vector3 target = new Vector3(transform.position.x, follow.position.y + offset, transform.position.z);

        if (onlyFollowUp)
        {
            if (target.y > transform.position.y)
            {
                Move(target);
            }
        }
        else
            Move(target);
    }

    void Move(Vector3 target)
    {
        transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * camSpeed);
    }
}
