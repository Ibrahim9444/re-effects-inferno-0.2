// InfernoPlagueEffect.cs
using UnityEngine;
using System.Collections;

public class InfernoPlagueEffect : MonoBehaviour, IElementalEffect
{
    public float duration = 5f;
    public float baseDamage = 10f;
    public float healingReductionPercentage = 25f; // 25% healing reduction per stack

    public void ApplyEffect(EnemyController enemy, float fireBonus, float toxicBonus)
    {
        if (!enemy.enemyStats.isFlesh)
        {
            Debug.Log("Inferno effect ineffective against non-flesh enemies.");
            Destroy(this);
            return;
        }

        enemy.SetHealingReduction(healingReductionPercentage); // Apply healing reduction
        StartCoroutine(TickDamage(enemy, fireBonus, toxicBonus));
    }

    private IEnumerator TickDamage(EnemyController enemy, float fireBonus, float toxicBonus)
    {
        float timer = duration; // Timer is fixed to duration
        while (timer > 0)
        {
            float damageAmount = baseDamage + ((fireBonus + toxicBonus) / 10f); // Correct damage calculation
            enemy.TakeDamage(damageAmount);
            timer -= 1f;
            yield return new WaitForSeconds(1f);
        }
        enemy.ResetHealingReduction(); // Reset healing reduction
        Destroy(this);
    }
}