using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSController : MonoBehaviour
{
    [SerializeField] private Rigidbody rig;
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
    [SerializeField] private float groundCheckDistance = 1f;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        rig.maxAngularVelocity = 0;
    }
    private void Update()
    {
        TryJump();
        TryRun();
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
    // Update is called once per frame
    void FixedUpdate()
    {
        Debug.DrawLine(transform.position, transform.position + Vector3.down * groundCheckDistance, Color.red);
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        

        Vector3 cameraForwardDir = cameraTransform.forward;
        cameraForwardDir.y = 0;
        Vector3 cameraRightDir = cameraTransform.right;
        cameraRightDir.y = 0;

        Vector3 movementDir = cameraForwardDir.normalized * vertical + cameraRightDir.normalized * horizontal;
        movementDir = Vector3.ClampMagnitude(movementDir,1) * movementSpeed;
        rig.velocity = new Vector3(movementDir.x, rig.velocity.y, movementDir.z);

        rig.angularVelocity = new Vector3(0,0,0);

        //if(cameraTransform.rotation.eulerAngles.x - mouseY > 280 || cameraTransform.rotation.eulerAngles.x - mouseY < 80)
        
    }
    private void TryJump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, LayerMask.GetMask("Default")))
            {
                rig.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
        }
    }
    private void TryRun()
    {
        if(Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W))
        {
            if (walkRunLerp < 1)
                walkRunLerp += Time.deltaTime/ walkToRunChangeSpeed;
        }
        else
        {
            if(walkRunLerp > 0)
                walkRunLerp -= Time.deltaTime/ walkToRunChangeSpeed;
        }

        movementSpeed = Mathf.Lerp(walkingSpeed, runningSpeed, walkRunLerp);
    }
}
