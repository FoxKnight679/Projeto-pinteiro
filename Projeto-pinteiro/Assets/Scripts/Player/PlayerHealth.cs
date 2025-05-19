using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;
    private bool isDead = false;

    private PlayerMovement movement; // referência ao script de movimento
    private Animator animator;

    void Start()
    {
        currentHealth = maxHealth;
        movement = GetComponent<PlayerMovement>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (isDead) return;

        // Simula dano com a tecla H
        if (Input.GetKeyDown(KeyCode.H))
        {
            TakeDamage(25);
        }

        // Simula cura com a tecla J
        if (Input.GetKeyDown(KeyCode.J))
        {
            Heal(15);
        }
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        Debug.Log("Tomou dano. Vida atual: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        Debug.Log("Curado. Vida atual: " + currentHealth);
    }

    void Die()
    {
        isDead = true;
        Debug.Log("Morto");

        if (animator != null)
            animator.SetInteger("transition", 3); // Morte

        if (movement != null)
            movement.enabled = false;

        CharacterController cc = GetComponent<CharacterController>();
        if (cc != null)
            cc.enabled = false; // impede colisão que levanta o corpo

        // Se estiver usando Rigidbody:
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
        }
    }

}
