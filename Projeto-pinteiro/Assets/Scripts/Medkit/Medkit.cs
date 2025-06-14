using System.Collections;
using UnityEngine;

public class Medkit : MonoBehaviour {
    
    public float rotationSpeed = 50f;
    public float respawnTime = 5f;
    public GameObject medkit;

    private Collider myCollider;

    private void Start() { 
    
        myCollider = GetComponent<Collider>();

        if (medkit == null) { 
        
            medkit = transform.GetChild(0).gameObject;
        
        }
    }

    private void Update() { 
    
        transform.Rotate(Vector3.up *  rotationSpeed * Time.deltaTime, Space.World);
    
    }

    private void OnTriggerEnter(Collider other) {

        if (other.CompareTag("Player")) {

            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();

            if (playerHealth != null) {

                playerHealth.restoreHealht();
                Debug.Log("Iniciando respawn");
                StartCoroutine(disableShortly());

            }
        }
    }

    private IEnumerator disableShortly() { 
        
        medkit.SetActive(false);
        myCollider.enabled = false;

        yield return new WaitForSeconds(respawnTime);

        medkit.SetActive(true);
        myCollider.enabled = true;

    }
}
