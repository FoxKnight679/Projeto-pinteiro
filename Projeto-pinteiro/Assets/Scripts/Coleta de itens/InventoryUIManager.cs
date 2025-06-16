using UnityEngine;
using UnityEngine.UI;


public class InventoryUIManager : MonoBehaviour
{
    public Image[] slotImages;      // Fundo do slot 
    public Image[] slotIcons;       // �cones dos itens
    public Color normalColor = Color.white;
    public Color highlightColor = Color.yellow;

    public GameObject collectedItemPrefab;
    private WeaponVisualManager weaponVisualManager;


    private int selectedIndex = 1;                  // Come�a selecionado no slot da arma padr�o
    private string[] items = new string[2];       

    void Start()
    {
        weaponVisualManager = FindObjectOfType<WeaponVisualManager>();

       
        if (slotImages[1] != null) slotImages[1].enabled = true;
        if (slotIcons[1] != null)
        {
            slotIcons[1].enabled = true;
            items[1] = "Arma padr�o"; 
        }

   
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
                selectedIndex = 0;
                UpdateSlotHighlight();
                weaponVisualManager?.EquipWeaponByIndex(0);
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            selectedIndex = 1;
            UpdateSlotHighlight();
            weaponVisualManager?.EquipWeaponByIndex(1);
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
        weaponVisualManager?.OnItemCollected(collectedItemPrefab);
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
            Debug.Log("Slot de item j� ocupado!");
        }
    }
}
