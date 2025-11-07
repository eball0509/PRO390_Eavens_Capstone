using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public ItemObject item;

    private bool canPickUp = false;
    private Player player;

    private void Start()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    private void Update()
    {
        if (canPickUp && Input.GetKeyDown(KeyCode.E))
        {
            PickUp();
        }
    }

    private void PickUp()
    {
        player.inventory.AddItem(item, 1);

        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        Player p = other.GetComponent<Player>();

        if (p != null)
        {
            player = p;
            canPickUp = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Player>() != null)
        {
            canPickUp = false;
            player = null;
        }
    }
}
