using UnityEngine;

[RequireComponent(typeof(CharacterController))]

public class Player : MonoBehaviour
{
    CharacterController characterController;

    Vector3 moveDirection = Vector3.zero;

    float rotationX = 0;

    public InventoryObject inventory;
    public Camera playerCamera;
    public Transform holdPoint;
    public float walkSpeed = 5;
    public float runspeed = 10;
    public float jumpHeight = 7;
    public float gravity = 9.8f;
    public float lookSpeed = 2;
    public float lookXLimit = 45;
    public bool canMove = true;

    [Header("Harvesting Information")]
    public float harvestRange = 1f;
    public float hitCooldown = 0.5f;
    public KeyCode harvestKey = KeyCode.Mouse0;

    private ItemObject currentHotbarItem;
    private GameObject currentHeldItem;
    private float nextHitTime = 0;


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

        if (Input.GetKeyDown(harvestKey) && Time.time >= nextHitTime)
        {
            nextHitTime = Time.time + hitCooldown;
            TryHarvest();
        }
    }

    public void SetCurrentHotbarItem(int slotIndex)
    {
        if (inventory == null || slotIndex < 0 || slotIndex >= inventory.Container.Count)
        {
            currentHotbarItem = null;
            UpdateHeldItem();
            return;
        }

        currentHotbarItem = inventory.Container[slotIndex].item;
        UpdateHeldItem();
    }

    public void TryHarvest()
    {
        ToolObject currentTool = currentHotbarItem as ToolObject;

        if (currentTool == null) return;

        float activeRange = harvestRange;

        Ray ray = new(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, activeRange))
        {
            Harvestable harvestable = hit.collider.GetComponent<Harvestable>();

            if (harvestable != null)
            {
                harvestable.Harvest(this, currentTool);
            }
        }
    }

    public void UpdateHeldItem()
    {
        // Remove previous item
        if (currentHeldItem != null)
        {
            Destroy(currentHeldItem);
            currentHeldItem = null;
        }

        if (currentHotbarItem == null) return;

        GameObject prefabToHold = null;

        if (currentHotbarItem is ToolObject tool)
        {
            prefabToHold = tool.prefab;
        }

        if (prefabToHold != null)
        {
            currentHeldItem = Instantiate(prefabToHold, holdPoint);
            currentHeldItem.transform.localPosition = Vector3.zero;
            currentHeldItem.transform.localRotation = Quaternion.identity;

            if (currentHotbarItem is ToolObject t)
            {
                currentHeldItem.transform.localPosition += t.holdPositionOffset;
                currentHeldItem.transform.localRotation *= Quaternion.Euler(t.holdRotationOffset);
            }
        }
    }
}
