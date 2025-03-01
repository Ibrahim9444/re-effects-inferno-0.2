using System.Collections.Generic;
using UnityEngine;

public class ModManger : MonoBehaviour
{
    public List<ModSO> activeMods = new List<ModSO>();
    private WeaponController weaponController;

    void Start()
    {
        weaponController = GetComponent<WeaponController>();  // Get the WeaponController on the same object
    }

    // Add a mod to the weapon
    public void AddMod(ModSO mod)
    {
        if (!activeMods.Contains(mod)) // Check if the mod isn't already added
        {
            activeMods.Add(mod);
            weaponController.ApplyMods();  // Apply all active mods to the weapon
        }
    }

    public void RemoveMod(ModSO mod)
    {
        if (activeMods.Contains(mod))
        {
            activeMods.Remove(mod);
            weaponController.ApplyMods();  // Reapply the mods after removal
        }
    }


    // Apply mod effects to the weapon
    public void ApplyModEffects(ModSO mod)
    {
        int level = mod.currentLevel;

        if (mod.modType == ModType.Elemental)
        {
            weaponController.ApplyElementalModEffects(mod, level);
        }
        else if (mod.modType == ModType.Basic)
        {
            weaponController.ApplyBasicModEffects(mod, level);
        }
        else if (mod.modType == ModType.Hybrid)
        {
            weaponController.ApplyHybridModEffects(mod, level);
        }
    }

    // Remove mod effects from the weapon
    public void RemoveModEffects(ModSO mod)
    {
        int level = mod.currentLevel;

        if (mod.modType == ModType.Elemental)
        {
            weaponController.RemoveElementalModEffects(mod, level);
        }
        else if (mod.modType == ModType.Basic)
        {
            weaponController.RemoveBasicModEffects(mod, level);
        }
        else if (mod.modType == ModType.Hybrid)
        {
            weaponController.RemoveHybridModEffects(mod, level);
        }
    }


}
