using UnityEngine;

public struct DamageInfo
{
    public float amount;
    public WeaponStatsSO.WeaponElementType type;
    private float totalDamage;

    public DamageInfo(float totalDamage) : this()
    {
        this.totalDamage = totalDamage;
    }
}