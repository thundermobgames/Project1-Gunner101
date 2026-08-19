using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSController : MonoBehaviour
{
    [SerializeField] float sensitivity = 100f;
    [SerializeField] float smoothTime = 0.1f;
    [SerializeField] Transform playerBody;
    [SerializeField] Shooting weapon;
    float xRotation = 0f;
    float yRotation = 0f;
    Vector3 currentRotation;
    Vector3 currentVelocity = Vector3.zero;
    bool isMouseLookActive;

    void Start()
    {
        
    }

    void LateUpdate()
    {

        if (Input.GetMouseButton(1))
        {
            if (!isMouseLookActive) { Cursor.lockState = CursorLockMode.Locked; Cursor.visible = false; isMouseLookActive = true; }

            float mouseX = 0f;
            float mouseY = 0f;
            
            mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
            mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -30f, 30f);

            yRotation += mouseX;
            yRotation = Mathf.Clamp(yRotation, -45f, 45f);

            Vector3 targetRotation = new Vector3(xRotation, yRotation, 0f);
            currentRotation = Vector3.SmoothDamp(currentRotation, targetRotation, ref currentVelocity, smoothTime);
            playerBody.localEulerAngles = currentRotation;

            weapon.shoot(true);

        }
        else
        {
            weapon.shoot(false);
            if (isMouseLookActive) { Cursor.lockState = CursorLockMode.None; Cursor.visible = true; isMouseLookActive = false; }
        }
    }
}