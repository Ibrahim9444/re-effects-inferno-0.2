using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewMod", menuName = "ScriptableObjects/New Mod")]
public class ModSO : ScriptableObject
{
    [Header("Mod Basics")]
    public ModType modType; // Elemental, Hybrid, Basic
    public ModRarity modRarity; // Common, Rare, Legendary, etc.
    public string modName;
    public int currentLevel;
    public string modDescription;
    public bool isUpgradable;
    public int maxLevel;
    public float modWeight;  // Determines how heavy the mod is (affects max slots)

    [Header("Mod basics")]
    // weapon basic stats
    public int[] clipSizeBoostLevels;
    public float[] baseDamageBoostLevels;
    public float[] ReloadSpeedBoostLevels;
    public float[] FireRateBoostLevels;
    public float[] critChanceBoostLevels;
    public float[] critMultiplierBoostLevels;
    public float[] weaponElementalTriggerChanceBoostLevels;

    [Header("Mod Elemental ")]
    // elemental damage mods stats
    public float[] fireDamageBoostLevels;
    public float[] toxicDamageBoostLevels;
    public float[] acidDamageBoostLevels;
    public float[] electricDamageBoostLevels;
    public float[] cryoDamageBoostLevels;      
    public float[] plasmaDamageBoostLevels;    

    [Header("Mod player stats")]
    // effect player mods stats
    public float[] healthBoostLevels;
    public float[] armorBoostLevels;
    public float[] moveSpeedBoostLevels;
    // indicator range
    public float[] rangeIndicatorBoostLevels;
    // crits and elemental trigger chances

    [Header("Mod Specials")]
    [Header("Special Effects (Optional)")]
    public bool hasSpecialEffect;
    public List<SpecialModEffect> specialEffects = new List<SpecialModEffect>();  // Now supports multiple effects
}

// Enum for Mod Type
public enum ModType
{
    Elemental,
    Hybrid,
    Basic,
}

// Enum for Mod Rarity (Optional)
public enum ModRarity
{
    Common,
    Uncommon,
    Rare,
    Epic,
    Legendary
}

// Enum for Special Mod Effects (Now Supports Multiple Effects)
public enum SpecialModEffect
{
    None,
    LifeSteal,       // Converts % of damage into HP
    ShieldRegen,     // Slowly regenerates armor over time
    HealthRegen,     // Regenerates health over time
    HealAllies,      // Heals allies over time
    LastStand        // When health is low, gain a temporary shield
                     //add more if needed 
}
