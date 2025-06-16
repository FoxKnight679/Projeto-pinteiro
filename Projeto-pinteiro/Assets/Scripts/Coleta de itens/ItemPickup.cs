using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public GameObject itemPrefab;
    public string itemName = "Item";
    public Sprite itemIcon;

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {

            InventoryUIManager inventory = FindFirstObjectByType<InventoryUIManager>();
            if (inventory != null)
            {
                inventory.collectedItemPrefab = itemPrefab;
                inventory.AddItem(itemName, itemIcon); 
                Destroy(gameObject);
            }
            else
            {
                Debug.LogWarning("Inventário não encontrado na cena!");
            }
        }
    }
}
