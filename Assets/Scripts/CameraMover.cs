using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMover : MonoBehaviour
{
    private Vector3 startPosition = new Vector3(3.5f, 0, 3.5f);
    private Vector3 startRotation = new Vector3(0, 45, 0);
    private int speed = 100;

    private void Start()
    {
        transform.position = startPosition;
        transform.eulerAngles = startRotation;
    }

    private void Update()
    {
        if (Input.GetMouseButton(1))
        {
            if (Input.GetAxis("Mouse X") < 0)
            {
                transform.eulerAngles -= new Vector3(0, Input.GetAxisRaw("Mouse X"), 0.0f) * Time.deltaTime * speed;
            }
            else if (Input.GetAxis("Mouse X") > 0)
            {
                transform.eulerAngles -= new Vector3(0, Input.GetAxisRaw("Mouse X"), 0.0f) * Time.deltaTime * speed;
            }
        }
    }
}
