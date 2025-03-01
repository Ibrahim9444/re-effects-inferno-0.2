using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static WeaponStatsSO;



public class WeaponController : MonoBehaviour
{
    [Header("Weapon Stats")]
    public WeaponStatsSO weaponStatsSO;

    private float currentBaseDamage;
    private float currentFireRate;
    private float currentReloadSpeed;
    private int currentClipSize;
    private float currentAcidDamage;
    private float currentFireDamage;
    private float currentElectricDamage;
    private float currentToxicDamage;
    private float currentCryoDamage;
    private float currentPlasmaDamage;
    public float currentElementTriggerChance { get; private set; }
    private float currentCritChance;
    private float currentCritMultiplier;
    private float currentSpreadAngle;
    private int currentBulletsPerShot;
    private int currentAmmo;
    private bool isReloading = false;
    private float nextFireTime = 0f;
    private bool statsChanged = false;
    public ModManger modManger;
    public Transform firePoint;
    public GameObject projectilePrefab;

    private EnemyStatsSO targetEnemyStats;

    public enum ActiveElementCombination
    {
        None,
        FireFire,
        FireToxic,
        // Add other combinations later...
    }

    void Start()
    {
        firePoint = transform.Find("FirePoint");
        modManger = GetComponent<ModManger>();
        WeaponReference();
        ApplyMods();
        currentAmmo = currentClipSize;
        Debug.Log("Starting Ammo: " + currentAmmo);

        // Set target enemy stats for testing (you might want to set this dynamically in your game)
        var enemyController = Object.FindFirstObjectByType<EnemyController>();  // Use the recommended method
        if (enemyController != null)
        {
            targetEnemyStats = enemyController.enemyStats;
            Debug.Log("Target enemy stats set to: " + targetEnemyStats.enemyName);
        }
        else
        {
            Debug.LogWarning("No enemy found to set target enemy stats.");
        }
    }

    void Update()
    {
        if (isReloading) return;

        if (Input.GetMouseButton(0) && Time.time >= nextFireTime && currentAmmo > 0)
        {
            Debug.Log("Attempting to shoot...");
            Shoot();
            nextFireTime = Time.time + (1f / currentFireRate);
        }

        if (currentAmmo <= 0 && !isReloading)
        {
            StartCoroutine(Reload());
        }

        if (statsChanged)
        {
            WeaponReference();
            statsChanged = false;
        }
    }

    public void OnStatsChanged()
    {
        statsChanged = true;
    }

    public void Shoot()
    {
        if (currentAmmo <= 0 || isReloading) return;

        if (targetEnemyStats == null)
        {
            Debug.LogWarning("No target enemy stats set. Cannot calculate damage.");
            return;
        }

        WeaponStats weaponStats = new WeaponStats
        {
            baseDamage = currentBaseDamage,
            acidDamage = currentAcidDamage,
            fireDamage = currentFireDamage,
            electricDamage = currentElectricDamage,
            toxicDamage = currentToxicDamage,
            critChance = currentCritChance,
            cryoDamage = currentCryoDamage,
            plasmaDamage = currentPlasmaDamage,
            critMultiplier = currentCritMultiplier,
            weaponElementType = weaponStatsSO.weaponElementType
        };

        float damage = DamageCalculator.CalculateDamage(weaponStats, targetEnemyStats);
        Debug.Log($"Shooting: Weapon Element = {weaponStatsSO.weaponElementType}, Damage = {damage}");

        if (weaponStatsSO.weaponType == WeaponType.Shotgun)
        {
            for (int i = 0; i < currentBulletsPerShot; i++)
            {
                float angleOffset = Random.Range(-currentSpreadAngle, currentSpreadAngle);
                Quaternion spreadRotation = Quaternion.Euler(0, angleOffset, 0);
                var projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation * spreadRotation);
                projectile.transform.parent = transform; // Set parent to WeaponController
                projectile.GetComponent<ProjectileController>().SetDamage(damage);
                projectile.GetComponent<ProjectileController>().SetEffect(weaponStatsSO.weaponElementType);
                projectile.GetComponent<ProjectileController>().SetElementalDamage(weaponStats.fireDamage, weaponStats.toxicDamage);
            }
            currentAmmo--;
        }
        else
        {
            var projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
            projectile.transform.parent = transform; // Set parent to WeaponController
            projectile.GetComponent<ProjectileController>().SetDamage(damage);
            projectile.GetComponent<ProjectileController>().SetEffect(weaponStatsSO.weaponElementType);
            projectile.GetComponent<ProjectileController>().SetElementalDamage(weaponStats.fireDamage, weaponStats.toxicDamage);
            currentAmmo--;
        }
        Debug.Log("Bullets left: " + currentAmmo);
    }
    //elemental effects section ...................................................................
    public float GetCurrentElementStat()
    {
        switch (weaponStatsSO.weaponElementType)
        {
            case WeaponStatsSO.WeaponElementType.Fire: return currentFireDamage;
            case WeaponStatsSO.WeaponElementType.Acid: return currentAcidDamage;
            case WeaponStatsSO.WeaponElementType.Toxic: return currentToxicDamage;
            case WeaponStatsSO.WeaponElementType.Electric: return currentElectricDamage;
            case WeaponStatsSO.WeaponElementType.Plasma: return currentPlasmaDamage;
            case WeaponStatsSO.WeaponElementType.Cryo: return currentCryoDamage;
            default: return 0f; // Or throw an exception if you prefer
        }
    }
    public float GetCurrentFireDamage()
    {
        return currentFireDamage;
    }

    public float GetCurrentToxicDamage()
    {
        return currentToxicDamage;
    }

    public ActiveElementCombination GetActiveElementCombination()
    {
        Debug.Log($"Weapon Element: {weaponStatsSO.weaponElementType}, Current Fire Damage: {currentFireDamage}, Current Toxic Damage: {currentToxicDamage}");

        if (weaponStatsSO.weaponElementType == WeaponStatsSO.WeaponElementType.Fire)
        {
            if (currentFireDamage > 0)
            {
                if (currentToxicDamage <= 0)
                {
                    Debug.Log("Returning FireFire");
                    return ActiveElementCombination.FireFire;
                }
                else
                {
                    if (currentFireDamage >= currentToxicDamage)
                    {
                        Debug.Log("Returning FireFire (Fire >= Toxic)");
                        return ActiveElementCombination.FireFire;
                    }
                    else
                    {
                        Debug.Log("Returning FireToxic (Toxic > Fire)");
                        return ActiveElementCombination.FireToxic;
                    }
                }
            }
            else if (currentToxicDamage > 0)
            {
                Debug.Log("Returning FireToxic (Toxic only, Fire Weapon)");
                return ActiveElementCombination.FireToxic;
            }
            else
            {
                Debug.Log("Returning None (Fire weapon, no stats)");
                return ActiveElementCombination.None;
            }
        }
        else if (weaponStatsSO.weaponElementType == WeaponStatsSO.WeaponElementType.Toxic) //Added Toxic Weapon logic
        {
            if (currentFireDamage > 0)
            {
                Debug.Log("Returning FireToxic (Fire stats, Toxic Weapon)");
                return ActiveElementCombination.FireToxic;
            }
            else
            {
                Debug.Log("Returning None (Toxic weapon, no fire stats)");
                return ActiveElementCombination.None;
            }
        }
        else
        {
            Debug.Log("Returning None (Not Fire or Toxic weapon)");
            return ActiveElementCombination.None;
        }
    }



    IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log("Reloading...");

        yield return new WaitForSeconds(currentReloadSpeed);

        currentAmmo = currentClipSize;
        isReloading = false;
        Debug.Log("Reloaded Ammo: " + currentAmmo);
    }

    void WeaponReference()
    {
        currentBaseDamage = weaponStatsSO.baseDamage;
        currentFireRate = weaponStatsSO.fireRate;
        currentReloadSpeed = weaponStatsSO.reloadSpeed;
        currentClipSize = weaponStatsSO.clipSize;

        currentAcidDamage = weaponStatsSO.acidDamage;
        currentFireDamage = weaponStatsSO.fireDamage;
        currentElectricDamage = weaponStatsSO.electricDamage;
        currentToxicDamage = weaponStatsSO.toxicDamage;
        currentCryoDamage = weaponStatsSO.cryoDamage;
        currentPlasmaDamage = weaponStatsSO.plasmaDamage;

        currentElementTriggerChance = weaponStatsSO.elementTriggerChance;
        currentCritChance = weaponStatsSO.critChance;
        currentCritMultiplier = weaponStatsSO.critMultiplier;
        currentSpreadAngle = weaponStatsSO.spreadAngle;
        currentBulletsPerShot = weaponStatsSO.bulletsPerShot;
    }
    
// mods section ..............................................................................................................   
    
   
    public void ApplyElementalModEffects(ModSO mod, int level)
    {
        if (mod.modType == ModType.Elemental)
        {
            currentFireDamage += mod.fireDamageBoostLevels[level];
            currentToxicDamage += mod.toxicDamageBoostLevels[level];
            currentAcidDamage += mod.acidDamageBoostLevels[level];
            currentElectricDamage += mod.electricDamageBoostLevels[level];
            currentPlasmaDamage += mod.plasmaDamageBoostLevels[level];
            currentCryoDamage += mod.cryoDamageBoostLevels[level];

        }
    }

    public void ApplyBasicModEffects(ModSO mod, int level)
    {
        if (mod.modType == ModType.Basic)
        {
            currentBaseDamage += mod.baseDamageBoostLevels[level];
            currentFireRate += mod.FireRateBoostLevels[level];
            currentClipSize += mod.clipSizeBoostLevels[level];
            currentCritChance += mod.critChanceBoostLevels[level];
            currentCritMultiplier += mod.critMultiplierBoostLevels[level];

            float reloadMultiplier = 1f - (mod.ReloadSpeedBoostLevels[level] / 100f);
            float oldReloadSpeed = currentReloadSpeed;
            currentReloadSpeed *= reloadMultiplier;
            currentReloadSpeed = Mathf.Max(currentReloadSpeed, 0.1f);

            currentElementTriggerChance += mod.weaponElementalTriggerChanceBoostLevels[level]; // Add trigger chance
            currentElementTriggerChance = Mathf.Clamp(currentElementTriggerChance, 0f, 1f); // Clamp
            Debug.Log($"Basic Mod Applied: Element Trigger Chance: {currentElementTriggerChance}"); // Debugging


            Debug.Log($"[Reload Debug] Old Reload Speed: {oldReloadSpeed} sec | Modifier: {mod.ReloadSpeedBoostLevels[level]}% | New Reload Speed: {currentReloadSpeed} sec");
            Debug.Log($"Applying Basic Mod Effects: Damage Boost: {mod.baseDamageBoostLevels[level]}, New Base Damage: {currentBaseDamage}");
            Debug.Log($"[Reload Debug] Old Reload Speed: {oldReloadSpeed} sec | Modifier: {mod.ReloadSpeedBoostLevels[level]}% | New Reload Speed: {currentReloadSpeed} sec");
        }
    }

    public void ApplyHybridModEffects(ModSO mod, int level)
    {
        if (mod.modType == ModType.Hybrid)
        {
            currentBaseDamage += mod.baseDamageBoostLevels[level];
            currentFireRate += mod.FireRateBoostLevels[level];
            currentReloadSpeed += mod.ReloadSpeedBoostLevels[level];
            currentClipSize += mod.clipSizeBoostLevels[level];
            currentFireDamage += mod.fireDamageBoostLevels[level];
            currentToxicDamage += mod.toxicDamageBoostLevels[level];
            currentAcidDamage += mod.acidDamageBoostLevels[level];
            currentElectricDamage += mod.electricDamageBoostLevels[level];
            currentPlasmaDamage += mod.plasmaDamageBoostLevels[level];
            currentCryoDamage += mod.cryoDamageBoostLevels[level];
            currentCritChance += mod.critChanceBoostLevels[level];
            currentCritMultiplier += mod.critMultiplierBoostLevels[level];

        }
    }

    public void RemoveElementalModEffects(ModSO mod, int level)
    {
        currentFireDamage -= mod.fireDamageBoostLevels[level];
        currentToxicDamage -= mod.toxicDamageBoostLevels[level];
        currentAcidDamage -= mod.acidDamageBoostLevels[level];
        currentElectricDamage -= mod.electricDamageBoostLevels[level];
        currentPlasmaDamage -= mod.plasmaDamageBoostLevels[level];
        currentCryoDamage -= mod.cryoDamageBoostLevels[level];

    }

    public void RemoveBasicModEffects(ModSO mod, int level)
    {
        currentBaseDamage -= mod.baseDamageBoostLevels[level];
        currentFireRate -= mod.FireRateBoostLevels[level];
        currentReloadSpeed -= mod.ReloadSpeedBoostLevels[level];
        currentClipSize -= mod.clipSizeBoostLevels[level];
        currentCritChance += mod.critChanceBoostLevels[level];
        currentCritMultiplier += mod.critMultiplierBoostLevels[level];

        currentElementTriggerChance -= mod.weaponElementalTriggerChanceBoostLevels[level]; // Remove trigger chance
        currentElementTriggerChance = Mathf.Clamp(currentElementTriggerChance, 0f, 1f); // Clamp
        Debug.Log($"Basic Mod Removed: Element Trigger Chance: {currentElementTriggerChance}"); // Debugging

    }

    public void RemoveHybridModEffects(ModSO mod, int level)
    {
        currentBaseDamage -= mod.baseDamageBoostLevels[level];
        currentFireRate -= mod.FireRateBoostLevels[level];
        currentReloadSpeed -= mod.ReloadSpeedBoostLevels[level];
        currentClipSize -= mod.clipSizeBoostLevels[level];
        currentFireDamage -= mod.fireDamageBoostLevels[level];
        currentToxicDamage -= mod.toxicDamageBoostLevels[level];
        currentAcidDamage -= mod.acidDamageBoostLevels[level];
        currentElectricDamage -= mod.electricDamageBoostLevels[level];
        currentCritChance += mod.critChanceBoostLevels[level];
        currentCritMultiplier -= mod.critMultiplierBoostLevels[level]; currentPlasmaDamage += mod.plasmaDamageBoostLevels[level];
        currentCryoDamage -= mod.cryoDamageBoostLevels[level];

    }

    public void ApplyMods()
    {
        foreach (var mod in modManger.activeMods)
        {
            if (mod.modType == ModType.Basic)
            {
                ApplyBasicModEffects(mod, mod.currentLevel);
            }
            else if (mod.modType == ModType.Elemental)
            {
                ApplyElementalModEffects(mod, mod.currentLevel);
            }
            else if (mod.modType == ModType.Hybrid)
            {
                ApplyHybridModEffects(mod, mod.currentLevel);
            }
        }
    }

    // Method to set target enemy stats
    public void SetTargetEnemyStats(EnemyStatsSO enemyStats)
    {
        targetEnemyStats = enemyStats;
    }
  
        // ... (Your other variables and methods)

       

    }
