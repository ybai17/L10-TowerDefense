using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BaseBehavior : MonoBehaviour
{
    int maxHealth;
    public int health = 100;
    public Slider healthSlider;

    public ParticleSystem baseHitVFX;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        maxHealth = health;

        if (healthSlider)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = health;
        }
    }

    void TakeDamage(int damage)
    {
        health -= damage;

        if (healthSlider)
            healthSlider.value = health;

        if (health <= 0)
        {
            health = 0;
            Debug.Log("Game Over");

            GameLost();
        }
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            EnemyAI enemyAI = other.gameObject.GetComponent<EnemyAI>();
            if (enemyAI)
            {
                int damage = enemyAI.GetBaseDamageValue();
                TakeDamage(damage);

                if (baseHitVFX)
                    baseHitVFX.Play();

                Debug.Log("ENEMY took " + damage + " damage");
            }

            Destroy(other.gameObject);

        }
    }

    void GameLost()
    {
        //if we want to keep the singletons for the TowerBuilder and MoneyManager (AKA not call destroy or reset them)
        //we can write Reset() functions for them that reset their values, and then we can call them here
        //
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
