// EnemyController.cs
using UnityEngine;
using System.Collections;
using static EnemyStatsSO;

public class EnemyController : MonoBehaviour
{
    public EnemyStatsSO enemyStats;
    public EnemyType enemyType;

    [Header("Enemy Stats")]
    private string enemyCurrentName;
    [HideInInspector] public float enemyCurrentHealth;
    [HideInInspector] public float enemyCurrentArmor;
    private float enemyCurrentMovementSpeed;
    private float enemyCurrentBaseDamage;

    [Header("Enemy Elemental Stats")]
    private float acidResistance;
    private float fireResistance;
    private float electricResistance;
    private float toxicResistance;

    private float debugTimer = 0f;
    private float debugInterval = 5f; // Log every 5 seconds

    private int infernoTriggers = 0;
    private float currentHealingReduction = 0f;

    void Start()
    {
        References();
        enemyCurrentHealth = enemyStats.Health;
        enemyCurrentArmor = enemyStats.Armor;
        Debug.Log("Enemy Type: " + enemyStats.enemyType);

        if (enemyStats.healingGen)
        {
            StartCoroutine(HealRegeneration());
        }

        if (enemyStats.armorGen)
        {
            StartCoroutine(ArmorRegeneration());
        }
    }

    void Update()
    {
        debugTimer += Time.deltaTime;
        if (debugTimer >= debugInterval)
        {
            Debug.Log($"Current health: {enemyCurrentHealth}, Current armor: {enemyCurrentArmor}, Healing Reduction: {currentHealingReduction}");
            debugTimer -= debugInterval;
        }
    }

    public void TakeDamage(float damage)
    {
        Debug.Log($"Enemy {enemyCurrentName} taking {damage} damage");
        if (enemyCurrentArmor > 0)
        {
            float damageToArmor = Mathf.Min(damage, enemyCurrentArmor);
            enemyCurrentArmor -= damageToArmor;
            damage -= damageToArmor;
            Debug.Log($"Damage absorbed by armor: {damageToArmor}. Remaining Armor: {enemyCurrentArmor}");
        }

        if (damage > 0)
        {
            enemyCurrentHealth -= damage;
            Debug.Log($"Damage taken: {damage}. Remaining Health: {enemyCurrentHealth}");
        }

        if (enemyCurrentHealth <= 0)
        {
            Debug.Log("Enemy destroyed.");
            Destroy(gameObject);
        }
    }

    void References()
    {
        enemyCurrentName = enemyStats.enemyName;
        enemyCurrentHealth = enemyStats.Health;
        enemyCurrentArmor = enemyStats.Armor;
        enemyCurrentMovementSpeed = enemyStats.MovementSpeed;
        enemyCurrentBaseDamage = enemyStats.BaseDamage;
    }

    public void SetHealingReduction(float reduction)
    {
        infernoTriggers++;
        currentHealingReduction = Mathf.Min(100f, currentHealingReduction + reduction); // Stack reduction, max 100%
        Debug.Log($"{enemyCurrentName} healing reduction set to: {currentHealingReduction}%");
    }

    public void ResetHealingReduction()
    {
        // Do not reset the reduction, keep the stacks
        Debug.Log($"{enemyCurrentName} healing reduction effect ended, current reduction: {currentHealingReduction}%");
    }

    private IEnumerator HealRegeneration()
    {
        while (true)
        {
            if (enemyCurrentHealth < enemyStats.Health)
            {
                float healAmount = enemyStats.Health * (enemyStats.healingRegenPercentage / 100f);
                healAmount *= (1f - (currentHealingReduction / 100f)); // Apply healing reduction
                healAmount = Mathf.Max(0, healAmount); // Prevent negative healing
                enemyCurrentHealth += healAmount;
                enemyCurrentHealth = Mathf.Min(enemyCurrentHealth, enemyStats.Health);
                Debug.Log($"{enemyCurrentName} healing: +{healAmount}. Current Health: {enemyCurrentHealth}");
            }
            yield return new WaitForSeconds(1f);
        }
    }

    private IEnumerator ArmorRegeneration()
    {
        while (true)
        {
            if (enemyCurrentArmor < enemyStats.Armor)
            {
                float armorRegenAmount = enemyStats.Armor * (enemyStats.armorRegenPercentage / 100f);
                enemyCurrentArmor += armorRegenAmount;
                enemyCurrentArmor = Mathf.Min(enemyCurrentArmor, enemyStats.Armor);
                Debug.Log($"{enemyCurrentName} regenerating armor: +{armorRegenAmount}. Current Armor: {enemyCurrentArmor}");
            }
            yield return new WaitForSeconds(1f);
        }
    }
}