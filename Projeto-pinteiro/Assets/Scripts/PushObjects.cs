using UnityEngine;

public class PushObjects : MonoBehaviour {
    
    public float pushForce = 5f;

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody rigidbody = hit.collider.attachedRigidbody;

        if (rigidbody != null && !rigidbody.isKinematic)
        {
            Vector3 pushDirection = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);

            rigidbody.AddForce(pushDirection * pushForce, ForceMode.Impulse);
        }
    }
}
