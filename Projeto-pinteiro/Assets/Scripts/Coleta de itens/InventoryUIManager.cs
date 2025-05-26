using UnityEngine;
using UnityEngine.UI;

public class InventoryUIManager : MonoBehaviour
{
    public Image[] slotImages;      // Fundo do slot (para destaque)
    public Image[] slotIcons;       // Ícones dos itens
    public Color normalColor = Color.white;
    public Color highlightColor = Color.yellow;

    private int selectedIndex = 0;                  // Começa selecionado no slot da arma padrão
    private string[] items = new string[2];         // [0] = item coletável, [1] = arma padrão

    void Start()
    {
        // Slot 1 (índice 1) = arma padrão: sempre visível
        if (slotImages[1] != null) slotImages[1].enabled = true;
        if (slotIcons[1] != null)
        {
            slotIcons[1].enabled = true;
            items[1] = "Arma padrão";  // Nome fictício ou real
        }

        // Slot 0 (índice 0) = item coletável: começa escondido
        if (slotImages[0] != null) slotImages[0].enabled = false;
        if (slotIcons[0] != null) slotIcons[0].enabled = false;

        UpdateSlotHighlight();
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.T))
        {
            if (!string.IsNullOrEmpty(items[0]))
            {
                selectedIndex = 1;
                UpdateSlotHighlight();
            }
        }

        // Slot 1 (arma padrão) sempre pode ser selecionado
        if (Input.GetKeyDown(KeyCode.R))
        {
            selectedIndex = 0;
            UpdateSlotHighlight();
        }
    }

    void UpdateSlotHighlight()
    {
        for (int i = 0; i < slotImages.Length; i++)
        {
            if (slotImages[i] != null)
                slotImages[i].color = (i == selectedIndex) ? highlightColor : normalColor;
        }
    }

    public void AddItem(string itemName, Sprite itemIcon)
    {
        // Adiciona apenas no slot 0 (item coletável)
        int i = 0;

        if (string.IsNullOrEmpty(items[i]))
        {
            items[i] = itemName;

            if (slotIcons[i] != null)
            {
                slotIcons[i].sprite = itemIcon;
                slotIcons[i].enabled = true;
            }

            if (slotImages[i] != null)
                slotImages[i].enabled = true;

            UpdateSlotHighlight();
        }
        else
        {
            Debug.Log("Slot de item já ocupado!");
        }
    }
}
