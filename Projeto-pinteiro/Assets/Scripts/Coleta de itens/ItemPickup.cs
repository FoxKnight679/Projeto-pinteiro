using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public GameObject itemPrefab;
    public string itemName = "Item";  // Nome interno do item (nao aparece na UI)
    public Sprite itemIcon;          // icone que sera mostrado no slot

    private void OnTriggerEnter(Collider other)
    {
        // Verifica se o jogador entrou em contato com o item
        if (other.CompareTag("Player"))
        {
            // Procura o inventario na cena
            InventoryUIManager inventory = FindFirstObjectByType<InventoryUIManager>();
            if (inventory != null)
            {
                inventory.collectedItemPrefab = itemPrefab;
                inventory.AddItem(itemName, itemIcon); // Adiciona o item ao inventario
                Destroy(gameObject); // Remove o objeto da cena
            }
            else
            {
                Debug.LogWarning("Inventário não encontrado na cena!");
            }
        }
    }
}
