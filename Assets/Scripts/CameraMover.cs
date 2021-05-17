using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMover : MonoBehaviour
{
    private int speed = 500;

    private void Update()
    {
        if (Input.GetMouseButton(1))
        {
            float mouseX = Input.GetAxisRaw("Mouse X");
            float mouseY = Input.GetAxisRaw("Mouse Y");
            Mathf.Clamp(mouseY, -90, 90);
            transform.eulerAngles += new Vector3(mouseY, mouseX, 0) * Time.deltaTime * speed;
        }
    }
}
