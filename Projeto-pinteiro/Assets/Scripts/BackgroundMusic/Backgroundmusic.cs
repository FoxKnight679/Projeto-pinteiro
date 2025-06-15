using UnityEngine;

public class Backgroundmusic : MonoBehaviour {

    private static Backgroundmusic instance;

    private void Awake() {

        if (instance == null) {

            instance = this;
            DontDestroyOnLoad(gameObject);
        } else { 
        
            Destroy(gameObject);
        }
    }
}
