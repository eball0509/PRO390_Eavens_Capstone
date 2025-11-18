using UnityEngine;

public class AnimalHealth : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;

    public AnimalHarvestable harvestable;

    private AnimalAI ai;
    private Collider animalCollider;

    private void Start()
    {
        currentHealth = maxHealth;
        ai = GetComponent<AnimalAI>();
        animalCollider = GetComponent<Collider>();

        if (harvestable != null)
        {
            harvestable.enabled = false;
        }
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        ai.enabled = false;
        harvestable.enabled = true;
        animalCollider.isTrigger = true;
    }
}
