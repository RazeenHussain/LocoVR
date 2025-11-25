using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCapsule : MonoBehaviour
{
    private float speed = 0.2f;

    void Update()
    {
        float y = Mathf.PingPong(Time.time * speed, 0.2f)+0.8f;
        transform.position = new Vector3(transform.position.x, y, transform.position.z);
    }
}
