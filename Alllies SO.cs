
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAlly", menuName = "ScriptableObjects/New Ally")]
public class AllySO : ScriptableObject
{
    [Header("Ally Info")]
    public string allyName;                  // Name of the Ally
    public AllyType allyType;                // Defensive, Offensive, Support
    public AllyWeaponType weaponType;        // Ranged or Melee
    public ElementType elementType;          // Elemental affinity of the ally (Fire, Acid, etc.)

    [Header("Ally Stats")]
    public float health;                     // Ally's health
    public float armor;                      // Ally's armor
    public float speed;                      // Movement speed
    public float damage;                     // Base damage dealt by the ally
    public float attackRange;                // Range of attack (for ranged allies)
    public float reAttackTime;                //time between attacks
    public float respawnTime;               // Time it takes for the ally to respawn after dying



    [Header("Abilities")]
    public string abilityName;               // Name of the Ally's special ability
    public float abilityCooldown;            // Time it takes for the ability to cooldown
    public string specialEffect;             // Special effects, like buffs, depuffs, etc.


    [Header("Behavior")]
    public bool isAggressive;                // Whether the ally automatically attacks enemies
    public bool isDefensive;                // Whether the ally defends the player
    public bool isHealing;                   // Whether the ally has healing abilities

    [Header("Level Scaling")]
    public int maxLevel = 10;                // Max level the ally can reach
    public float healthScale;                // How health scales with level
    public float damageScale;                // How damage scales with level
    public float speedScale;                 // How speed scales with level
}

public enum AllyType
{
    Defensive,
    Offensive,
    Support
}

public enum AllyWeaponType
{
    Melee,
    Ranged
}

public enum ElementType
{
    Fire,
    Acid,
    Electric,
    Toxic,
    Cryo,
    Plasma,
    None
}
// rebuild check point