using UnityEngine;

[CreateAssetMenu(fileName = "NewSkill", menuName = "ScriptableObjects/New skill")]

public class SkillSO : ScriptableObject
{
    [Header("Skill Info")]
    public string skillName;                  // Name of the skill
    public float fireRateBonus;                    // Time between skill uses   
    public float healthBonus;                    // Time between skill uses   
    public float armorBonus;                    // Time between skill uses
    public float baseDamageBonus;                    // Time between skill uses
    public float movementSpeedBonus;                    // play movement buff
    public float attackRangeBonus;                    // range indicator buff
    public float critChanceBonus;                    // crit chance buff
    public float critDamageBonus;                   // crit damage buff
    public float fireElementalDamageBonus;                    // fire damage buff
    public float toxicDamageBonus;                    // toxic damage buff
    public float acidDamageBonus;                    // acid damage buff
    public float electricDamageBonus;                    // electric damage buff
    public float clipSizeBonus;                    // clip size buff
    public float weaponElementalTriggerChanceBonus;                    // weapon elemental trigger chance buff
    public float healthRegenBonus;                    // health regen buff
    public float bulletPenetrationBonus;                    // bullet penetration
    [Header("Allies Buffs")]
    public float allyDamageBonus;                    // ally damage buff
    public float allyHealthBonus;                    // ally health buff
    public float allyArmorBonus;                    // ally armor buff
    public float allySpeedBonus;                    // ally speed buff
    public float allyRespawnTimeBonus;                    // ally respawn time buff


}
// rebuild check point