using UnityEngine;

public class WeaponVisualManager : MonoBehaviour
{
    private PlayerNovo playerController;

    public Transform itemSlot; 
    public GameObject defaultWeaponPrefab;
    public GameObject secondWeaponPrefab; 

    private GameObject defaultWeaponInstance;
    private GameObject secondWeaponInstance;

    private bool hasSecondWeapon = false;

    void Start()
    {
        playerController = FindObjectOfType<PlayerNovo>();

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
            secondWeaponInstance.SetActive(false); // S� ativa ao pressionar T
        }
    }

    public void EquipWeaponByIndex(int index)
    {
        playerController?.SetWeaponIndex(index);

        if (index == 0) // Slot 0 = arma colet�vel
        {
            if (hasSecondWeapon)
            {
                defaultWeaponInstance.SetActive(false);
                secondWeaponInstance.SetActive(true);
            }
        }
        else if (index == 1) // Slot 1 = arma padr�o
        {
            defaultWeaponInstance.SetActive(true);
            if (hasSecondWeapon)
                secondWeaponInstance.SetActive(false);
        }
    }
}
