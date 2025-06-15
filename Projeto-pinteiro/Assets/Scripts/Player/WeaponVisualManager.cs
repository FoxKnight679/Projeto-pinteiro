using UnityEngine;

public class WeaponVisualManager : MonoBehaviour
{
    public Transform itemSlot; // Arraste o ItemSlot (na mão direita)
    public GameObject defaultWeaponPrefab;
    public GameObject secondWeaponPrefab; // Prefab que será coletado

    private PlayerNovo playerController;

    private GameObject defaultWeaponInstance;
    private GameObject secondWeaponInstance;

    private bool hasSecondWeapon = false;

    void Start()
    {
        playerController = FindObjectOfType<PlayerNovo>();

        // Instancia a arma padrão
        if (defaultWeaponPrefab != null)
        {
            defaultWeaponInstance = Instantiate(defaultWeaponPrefab, itemSlot);
            defaultWeaponInstance.transform.localPosition = Vector3.zero;
            defaultWeaponInstance.transform.localRotation = Quaternion.identity;
        }
    }

    public void OnItemCollected(GameObject collectedPrefab)
    {
        if (!hasSecondWeapon && collectedPrefab != null)
        {
            hasSecondWeapon = true;
            secondWeaponInstance = Instantiate(collectedPrefab, itemSlot);
            secondWeaponInstance.transform.localPosition = Vector3.zero;
            secondWeaponInstance.transform.localRotation = Quaternion.identity;
            secondWeaponInstance.SetActive(false); // Só ativa ao pressionar T
        }
    }

    public void EquipWeaponByIndex(int index)
    {
        playerController?.SetWeaponIndex(index);

        if (index == 0) // Slot 0 = arma coletável
        {
            if (hasSecondWeapon)
            {
                defaultWeaponInstance.SetActive(false);
                secondWeaponInstance.SetActive(true);
            }
        }
        else if (index == 1) // Slot 1 = arma padrão
        {
            defaultWeaponInstance.SetActive(true);
            if (hasSecondWeapon)
                secondWeaponInstance.SetActive(false);
        }
    }
}
