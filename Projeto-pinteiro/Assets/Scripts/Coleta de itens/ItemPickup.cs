using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public string itemName = "Item";  // Nome interno do item (nao aparece na UI)
    public Sprite itemIcon;          // icone que sera mostrado no slot

    private void OnTriggerEnter(Collider other)
    {
        // Verifica se o jogador entrou em contato com o item
        if (other.CompareTag("Player"))
        {
            // Procura o inventario na cena
            InventoryUIManager inventory = FindObjectOfType<InventoryUIManager>();
            if (inventory != null)
            {
                inventory.AddItem(itemName, itemIcon); // Adiciona o item ao inventario
                Destroy(gameObject); // Remove o objeto da cena
            }
        }
    }
}
