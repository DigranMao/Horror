using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSCharacterController : MonoBehaviour
{
    [SerializeField] private CharacterController characterController;
    [Header("Movement Speed Parameters")]
    [SerializeField] private float walkingSpeed = 4;
    [SerializeField] private float runningSpeed = 8;
    private float walkRunLerp;
    [SerializeField] private float walkToRunChangeSpeed = 0.5f;
    [SerializeField] private float movementSpeed = 1;
    [Header("Camera Parameters")]
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private float mouseSensitivity = 2;

    [Header("Jump Parameters")]
    [SerializeField] private float jumpForce;
    [SerializeField] private float gravity = 9.81f;

    private float verticalVelocity;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        HandleMouseLook();
        HandleGravity();
        TryJump();
        TryRun();
        HandleMovement(); // Теперь HandleMovement в Update
    }

    void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        float newAngleX = cameraTransform.rotation.eulerAngles.x - mouseY * mouseSensitivity;
        if (newAngleX > 180)
        {
            newAngleX = newAngleX - 360;
        }
        newAngleX = Mathf.Clamp(newAngleX, -80, 80);
        cameraTransform.rotation = Quaternion.Euler(newAngleX, cameraTransform.rotation.eulerAngles.y, cameraTransform.rotation.eulerAngles.z);
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + mouseX * mouseSensitivity, transform.rotation.eulerAngles.z);
    }

    void HandleGravity()
    {
        if (!characterController.isGrounded)
        {
            verticalVelocity -= gravity * Time.deltaTime;
        }
        else
        {
            verticalVelocity = -gravity * Time.deltaTime; // Сброс вертикальной скорости, если на земле
        }
    }

    void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 cameraForwardDir = cameraTransform.forward;
        cameraForwardDir.y = 0;
        Vector3 cameraRightDir = cameraTransform.right;
        cameraRightDir.y = 0;

        Vector3 movementDir = cameraForwardDir.normalized * vertical + cameraRightDir.normalized * horizontal;
        movementDir = Vector3.ClampMagnitude(movementDir, 1) * movementSpeed;

        if (characterController.isGrounded)
        {
            movementDir.y = 0; // Keep the character on the same level
            if (Input.GetButtonDown("Jump"))
            {
                verticalVelocity = jumpForce;
            }
        }

        movementDir.y = verticalVelocity;
        characterController.Move(movementDir * Time.deltaTime);
    }

    private void TryJump()
    {
        if (Input.GetButtonDown("Jump") && characterController.isGrounded)
        {
            verticalVelocity = jumpForce;
        }
    }

    private void TryRun()
    {
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W))
        {
            if (walkRunLerp < 1)
                walkRunLerp += Time.deltaTime / walkToRunChangeSpeed;
        }
        else
        {
            if (walkRunLerp > 0)
                walkRunLerp -= Time.deltaTime / walkToRunChangeSpeed;
        }

        movementSpeed = Mathf.Lerp(walkingSpeed, runningSpeed, walkRunLerp);
    }
}
