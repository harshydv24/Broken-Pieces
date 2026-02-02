using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_CameraController : MonoBehaviour
{
    [SerializeField] private float Sensitivity;
    [SerializeField] private float MaxXRotationAmount;
    [SerializeField] private float MaxYRotationAmount;

    float MouseX;
    float MouseY;
    float xRotation = 0f;
    float yRotation = 0f;

    public bool canLook = true;

    void Update()
    {
        if (!canLook || !GameManager.Instance.IsPlayingStarted()) return;

        CamInputs();
        MouseRotation();
    }

    void CamInputs()
    {
        MouseX = Input.GetAxisRaw("Mouse X") * Sensitivity * Time.deltaTime;
        MouseY = Input.GetAxisRaw("Mouse Y") * Sensitivity * Time.deltaTime;
    }

    void MouseRotation()
    {
        xRotation -= MouseY;
        yRotation += MouseX;
        xRotation = Mathf.Clamp(xRotation, -MaxXRotationAmount, MaxXRotationAmount);
        yRotation = Mathf.Clamp(yRotation, -MaxYRotationAmount, MaxYRotationAmount);

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0f);
    }
}
