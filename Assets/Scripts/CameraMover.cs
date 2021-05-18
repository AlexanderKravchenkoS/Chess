using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMover : MonoBehaviour
{
    private float speed = 500;

    private void Update()
    {
        if (Input.GetMouseButton(1))
        {
            float mouseX = Input.GetAxisRaw("Mouse X");
            transform.localEulerAngles += new Vector3(0, mouseX, 0) * Time.deltaTime * speed;
        }
    }
}
