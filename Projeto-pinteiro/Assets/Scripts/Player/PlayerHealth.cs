using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int health = 100;

    public void TakeDamage(int amount)
    {
        health -= amount;
        Debug.Log("Dano recebido: " + amount);

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Jogador morreu!");
        // Aqui voc� pode ativar anima��es, tela de game over etc.
    }
}
