using System.Collections;
using UnityEngine;

public class Shield : MonoBehaviour {

    public int shieldAmount = 50;
    public float rotationSpeed = 50f;
    public float respawnTime = 90f;
    public GameObject shield;

    private Collider myCollider;

    private void Start() {
        myCollider = GetComponent<Collider>();

        if (shield == null && transform.childCount > 0) {

            shield = transform.GetChild(0).gameObject;
        }
    }

    private void Update() {

        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter(Collider other) {

        if (other.CompareTag("Player")) {

            PlayerHealth playerHealth = other.GetComponentInChildren<PlayerHealth>();

            if (playerHealth != null) {

                playerHealth.activateShield(shieldAmount);
                Debug.Log("Escudo coletado!");

                StartCoroutine(disableShortly());
            }
        }
    }

    private IEnumerator disableShortly() { 
    
        if (shield != null) shield.SetActive(false);
        myCollider.enabled = false;

        yield return new WaitForSeconds(respawnTime);

        if (shield != null) shield.SetActive(true);
        myCollider.enabled = true;

        Debug.Log("Escudo respawnado!");
    }
}
