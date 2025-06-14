using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int health = 100;
    public int actualHealth;

    public void Start()
    {
        actualHealth = health;
    }

    public void TakeDamage(int amount)
    {
        actualHealth -= amount;
        Debug.Log("Dano recebido: " + amount);

        if (actualHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Jogador morreu!");
        // Aqui você pode ativar animações, tela de game over etc.
    }

    public void restoreHealht() {
        actualHealth = health;
        Debug.Log("Vida cheia.");
    }

}
