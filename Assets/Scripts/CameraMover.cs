using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMover : MonoBehaviour
{
    private int speed = 1000;

    private void Update()
    {
        if (Input.GetMouseButton(1))
        {
            float mouseX = Input.GetAxisRaw("Mouse X");
            transform.eulerAngles += new Vector3(0, mouseX, 0) * Time.deltaTime * speed;
        }
    }
}
