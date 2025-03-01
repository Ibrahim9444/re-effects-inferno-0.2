using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    public float projectileSpeed;
    public float lifeTime;
    private float currentLifeTime;
    private float damage;
    private WeaponStatsSO.WeaponElementType weaponElement;
    private float fireDamage;
    private float toxicDamage;

    void Start()
    {
        currentLifeTime = lifeTime;
    }

    void Update()
    {
        currentLifeTime -= Time.deltaTime;
        transform.Translate(Vector3.forward * Time.deltaTime * projectileSpeed);

        if (currentLifeTime <= 0)
        {
            destroyProjectile();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Projectile hit enemy!");
            EnemyController enemyController = other.GetComponent<EnemyController>();
            enemyController.TakeDamage(damage);

            WeaponController weaponController = GetComponentInParent<WeaponController>();
            if (weaponController != null)
            {
                float triggerChance = weaponController.currentElementTriggerChance;

                if (Random.value <= triggerChance)
                {
                    WeaponController.ActiveElementCombination activeCombination = weaponController.GetActiveElementCombination();

                    Debug.Log($"Active Combination: {activeCombination}, Triggered!");

                    switch (activeCombination)
                    {
                        case WeaponController.ActiveElementCombination.FireFire:
                            Debug.Log("Applying FireFire effect!");
                            ApplyInfernoPlague(enemyController);
                            break;
                        case WeaponController.ActiveElementCombination.FireToxic:
                            Debug.Log("Applying FireToxic effect!");
                            ApplyInfernoPlague(enemyController);
                            break;
                        case WeaponController.ActiveElementCombination.None:
                            Debug.Log("No elemental effect to apply.");
                            break;
                        default:
                            Debug.Log("Unknown elemental combination.");
                            break;
                    }
                }
                else
                {
                    Debug.Log("Elemental effect trigger failed.");
                }
            }
            else
            {
                Debug.LogWarning("WeaponController not found in parent.");
            }

            destroyProjectile();
        }
    }
    public void SetDamage(float damage)
    {
        this.damage = damage;
    }

    public void SetEffect(WeaponStatsSO.WeaponElementType weaponElement)
    {
        this.weaponElement = weaponElement;
    }

    public void SetElementalDamage(float fireDamage, float toxicDamage)
    {
        this.fireDamage = fireDamage;
        this.toxicDamage = toxicDamage;
    }

    // ProjectileController.cs
    private void ApplyInfernoPlague(EnemyController enemy)
    {
        Debug.Log("ApplyInfernoPlague called!");
        Debug.Log($"Fire Damage: {fireDamage}, Toxic Damage: {toxicDamage}");
        InfernoPlagueEffect effect = enemy.gameObject.AddComponent<InfernoPlagueEffect>();
        effect.ApplyEffect(enemy, fireDamage, toxicDamage);
    }

    void destroyProjectile()
    {
        Destroy(gameObject);
    }
}