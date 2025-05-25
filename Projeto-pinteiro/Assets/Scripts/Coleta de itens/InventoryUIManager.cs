using UnityEngine;
using UnityEngine.UI;

public class InventoryUIManager : MonoBehaviour
{
    public Image[] slotImages;      // Fundo do slot, muda de cor quando selecionado
    public Image[] slotIcons;       // Imagem do item visivel no slot
    public Color normalColor = Color.white;
    public Color highlightColor = Color.yellow;

    private int selectedIndex = 0;           // Slot atualmente selecionado
    private string[] items = new string[2];  // Armazena internamente os itens coletados

    void Start()
    {
        UpdateSlotHighlight(); // Marca o primeiro slot ao iniciar
    }

    void Update()
    {
        // Selecao dos slots com as teclas R e T
        if (Input.GetKeyDown(KeyCode.T)) { selectedIndex = 0; UpdateSlotHighlight(); }
        if (Input.GetKeyDown(KeyCode.R)) { selectedIndex = 1; UpdateSlotHighlight(); }
    }

    void UpdateSlotHighlight()
    {
        for (int i = 0; i < slotImages.Length; i++)
        {
            slotImages[i].color = (i == selectedIndex) ? highlightColor : normalColor;
        }
    }

    // Adiciona item ao inventario e atualiza o icone no slot correspondente
    public void AddItem(string itemName, Sprite itemIcon)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (string.IsNullOrEmpty(items[i]))
            {
                items[i] = itemName;

                if (slotIcons[i] != null)
                {
                    slotIcons[i].sprite = itemIcon;
                    slotIcons[i].enabled = true;
                }

                break;
            }
        }
    }
}
