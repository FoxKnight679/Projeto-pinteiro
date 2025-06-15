using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour {

    public int maxHealth = 100; //Vida máxima do player
    public int currentHealth; //Vida atual do player

    public int maxShield = 0; //Escudo máximo do player
    public int currentShield = 0; //Escudo atual do player

    public Image HpBar;
    public Image ShieldBar;

    private void Start() {

        currentHealth = maxHealth; //Aqui ele só iguala a vida atual a vida máxima
        currentShield = 0; //Começa sem escudo, pra evitar bugs
        UpdateUI();
    }

    public void TakeDamage(int amount) {

        Debug.Log("Dano recebido: " + amount);

        if (currentShield > 0) {

            int shieldDamage = Mathf.Min(amount, currentShield);
            currentShield -= shieldDamage;
            amount -= shieldDamage;

        }

        if (amount > 0) {

            currentHealth -= amount;
            if (currentHealth <= 0) {

                currentHealth = 0;
                Die();
            }
        }

        UpdateUI();
    }

    public void restoreHealth() {
        currentHealth = maxHealth;
        Debug.Log("Vida cheia.");
        UpdateUI();
    }

    public void activateShield(int amount) {
        maxShield = amount;
        currentShield = amount;
        Debug.Log("Escudo ativo! Valor: " + amount);
        UpdateUI();
    }
    private void Die() {
        Debug.Log("Jogador morreu!");

    }
    void UpdateUI() {

        if (HpBar != null)
            HpBar.fillAmount = (float)currentHealth / maxHealth;

        if (ShieldBar != null) {
            if (currentShield > 0 && maxShield > 0) {
                ShieldBar.gameObject.SetActive(true);
                ShieldBar.fillAmount = (float)currentShield / maxShield;
            } else {
                ShieldBar.gameObject.SetActive(false);
            }
        }
    }
}
