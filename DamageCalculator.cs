using UnityEngine;

public static class DamageCalculator
{
    public static float CalculateDamage(WeaponStats weapon, EnemyStatsSO enemy)
    {
        // Calculate base damage adjusted for enemy resistance/weakness
        float baseDamage = weapon.baseDamage * ApplyElementalAdjustment(enemy, weapon.weaponElementType);
        Debug.Log($"Base Damage: {baseDamage}");

        float totalDamage = baseDamage;

        // Only apply elemental damage if the enemy is not Nightmare
        if (enemy.enemyType != EnemyStatsSO.EnemyType.Nightmare)
        {
            // Calculate elemental damage contributions with debugging logs
            float acidDamage = weapon.acidDamage * GetElementCompatibilityMultiplier(weapon.weaponElementType, WeaponStatsSO.WeaponElementType.Acid) * ApplyElementalAdjustment(enemy, WeaponStatsSO.WeaponElementType.Acid);
            Debug.Log($"Acid Damage: {acidDamage}");

            float fireDamage = weapon.fireDamage * GetElementCompatibilityMultiplier(weapon.weaponElementType, WeaponStatsSO.WeaponElementType.Fire) * ApplyElementalAdjustment(enemy, WeaponStatsSO.WeaponElementType.Fire);
            Debug.Log($"Fire Damage: {fireDamage}");

            float electricDamage = weapon.electricDamage * GetElementCompatibilityMultiplier(weapon.weaponElementType, WeaponStatsSO.WeaponElementType.Electric) * ApplyElementalAdjustment(enemy, WeaponStatsSO.WeaponElementType.Electric);
            Debug.Log($"Electric Damage: {electricDamage}");

            float toxicDamage = weapon.toxicDamage * GetElementCompatibilityMultiplier(weapon.weaponElementType, WeaponStatsSO.WeaponElementType.Toxic) * ApplyElementalAdjustment(enemy, WeaponStatsSO.WeaponElementType.Toxic);
            Debug.Log($"Toxic Damage: {toxicDamage}");

            float cryoDamage = weapon.cryoDamage * GetElementCompatibilityMultiplier(weapon.weaponElementType, WeaponStatsSO.WeaponElementType.Cryo) * ApplyElementalAdjustment(enemy, WeaponStatsSO.WeaponElementType.Cryo);
            Debug.Log($"Cryo Damage: {cryoDamage}");

            float plasmaDamage = weapon.plasmaDamage * GetElementCompatibilityMultiplier(weapon.weaponElementType, WeaponStatsSO.WeaponElementType.Plasma) * ApplyElementalAdjustment(enemy, WeaponStatsSO.WeaponElementType.Plasma);
            Debug.Log($"Plasma Damage: {plasmaDamage}");

            // Sum total damage
            totalDamage += acidDamage + fireDamage + electricDamage + toxicDamage + cryoDamage + plasmaDamage;
            Debug.Log($"Total Damage (before crit): {totalDamage}");
        }

        // Apply critical hit multiplier
        if (UnityEngine.Random.value <= weapon.critChance / 100)
        {
            totalDamage *= weapon.critMultiplier;
            Debug.Log("Critical Hit! Damage multiplied.");
        }

        Debug.Log($"Final Total Damage: {totalDamage}");
        return totalDamage;
    }

    private static float GetElementCompatibilityMultiplier(WeaponStatsSO.WeaponElementType weaponElement, WeaponStatsSO.WeaponElementType modElement)
    {
        return weaponElement == modElement ? 1.0f : 0.8f;
    }

    private static float ApplyElementalAdjustment(EnemyStatsSO enemy, WeaponStatsSO.WeaponElementType elementType)
    {
        switch (enemy.enemyType)
        {
            case EnemyStatsSO.EnemyType.Parasite:
                return elementType == WeaponStatsSO.WeaponElementType.Fire ? 1.2f : elementType == WeaponStatsSO.WeaponElementType.Toxic ? 0.6f : 1.0f;
            case EnemyStatsSO.EnemyType.Mesh:
                return elementType == WeaponStatsSO.WeaponElementType.Electric ? 1.2f : elementType == WeaponStatsSO.WeaponElementType.Acid ? 0.6f : 1.0f;
            case EnemyStatsSO.EnemyType.Armored:
                return elementType == WeaponStatsSO.WeaponElementType.Acid ? 1.2f : elementType == WeaponStatsSO.WeaponElementType.Fire ? 0.6f : 1.0f;
            case EnemyStatsSO.EnemyType.Cybernetic:
                return elementType == WeaponStatsSO.WeaponElementType.Cryo ? 1.2f : elementType == WeaponStatsSO.WeaponElementType.Electric ? 0.6f : 1.0f;
            case EnemyStatsSO.EnemyType.Mutant:
                return elementType == WeaponStatsSO.WeaponElementType.Toxic ? 1.2f : elementType == WeaponStatsSO.WeaponElementType.Plasma ? 0.6f : 1.0f;
            case EnemyStatsSO.EnemyType.Spectral:
                return elementType == WeaponStatsSO.WeaponElementType.Plasma ? 1.2f : elementType == WeaponStatsSO.WeaponElementType.Cryo ? 0.6f : 1.0f;
            case EnemyStatsSO.EnemyType.Nightmare:
                return 1.0f; // Only takes base damage
            default:
                return 1.0f;
        }
    }
}