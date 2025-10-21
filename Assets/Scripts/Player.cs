using UnityEngine;

[RequireComponent(typeof(CharacterController))]

public class Player : MonoBehaviour
{
    CharacterController characterController;

    Vector3 moveDirection = Vector3.zero;

    float rotationX = 0;

    public InventoryObject inventory;
    public Camera playerCamera;
    public float walkSpeed = 5;
    public float runspeed = 10;
    public float jumpHeight = 7;
    public float gravity = 9.8f;
    public float lookSpeed = 2;
    public float lookXLimit = 45;
    public bool canMove = true;


    void Start()
    {
        characterController = GetComponent<CharacterController>();
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        UnityEngine.Cursor.visible = false;

    }

    void Update()
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        if (Time.timeScale > 0)
        {
            float cursorSpeedX = canMove ? (isRunning ? runspeed : walkSpeed) * Input.GetAxis("Vertical") : 0;
            float cursorSpeedY = canMove ? (isRunning ? runspeed : walkSpeed) * Input.GetAxis("Horizontal") : 0;
            float moveDirectionY = moveDirection.y;
            moveDirection = (forward * cursorSpeedX) + (right * cursorSpeedY);

            if (Input.GetButton("Jump") && canMove && characterController.isGrounded)
            {
                moveDirection.y = jumpHeight;
            }
            else
            {
                moveDirection.y = moveDirectionY;
            }

            if (!characterController.isGrounded)
            {
                moveDirection.y -= gravity * Time.deltaTime;
            }


            characterController.Move(moveDirection * Time.deltaTime);

            if (canMove)
            {
                rotationX -= Input.GetAxis("Mouse Y") * lookSpeed;
                rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
                playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
                transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var item = other.GetComponent<Item>();
        if(item)
        {
            inventory.AddItem(item.item, 1);
            Destroy(other.gameObject);
        }
    }
}
