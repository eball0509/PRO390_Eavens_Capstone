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

    [Header("Player Movement Settings")]
    public float walkSpeed = 5;
    public float runspeed = 10;
    public float jumpHeight = 7;
    public float gravity = 9.8f;
    public float lookSpeed = 2;
    public float lookXLimit = 45;
    public bool canMove = true;

    [Header("Harvesting Settings")]
    public float harvestRange = 1f;
    public float hitCooldown = 0.5f;
    public KeyCode harvestKey = KeyCode.Mouse0;

    [Header("Player Stats")]
    public float currentHealth = 100;
    public float maxHealth = 100;
    public float currentHunger = 100;
    public float maxHunger = 100;
    public float maxWeight = 200;
    public float currentWeight = 0;

    [Header("Hunger Settings")]
    public float hungerDecreaseRate = 2.5f;
    public float hungerTickDelay = 15f;
    public float hungerDamageRate = 5;
    private float hungerTimer = 0f;

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

            if (Input.GetKey(KeyCode.Space) && canMove && characterController.isGrounded)
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

        if (Input.GetKeyDown(KeyCode.Mouse0) && Time.time >= nextHitTime)
        {
            nextHitTime = Time.time + hitCooldown;

            if (currentHotbarItem is FoodObject fooditem)
            {
                TryEat(fooditem);
            }
            else
            {
                TryHarvest();
            }
        }

        HandleHunger();
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

            AnimalHealth animalHealth = hit.collider.GetComponent<AnimalHealth>();
            if (animalHealth != null && animalHealth.currentHealth > 0)
            {
                animalHealth.TakeDamage(currentTool.damage);
                Debug.Log($"Dealt {currentTool.damage} damage to {animalHealth.name}");
                return;
            }

            AnimalHarvestable deadAnimal = hit.collider.GetComponent<AnimalHarvestable>();
            if (deadAnimal != null)
            {
                deadAnimal.Harvest(this, currentTool);
                Debug.Log($"Harvested {deadAnimal.name}");
                return;
            }

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

    private void HandleHunger()
    {
        hungerTimer += Time.deltaTime;

        if (hungerTimer >= hungerTickDelay)
        {
            hungerTimer = 0;
            ModifyHunger(-hungerDecreaseRate);
        }

        if (currentHunger <= 0)
        {
            ModifyHealth(-hungerDamageRate * Time.deltaTime);
        }
        
    }

    public void ModifyHealth(float amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void ModifyHunger(float amount)
    {
        currentHunger = Mathf.Clamp(currentHunger + amount, 0, maxHunger);
    }

    private void Die()
    {
        canMove = false;
    }

    public void TryEat(FoodObject food)
    {
        if (currentHunger < maxHunger)
        {
            ModifyHunger(food.hungerRestore);

            if (food.healthRestore > 0)
            {
                ModifyHealth(food.healthRestore);
            }

            inventory.RemoveItem(food, 1);
        }
    }

}
