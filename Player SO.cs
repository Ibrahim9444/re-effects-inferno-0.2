
using UnityEngine;



[CreateAssetMenu(fileName = "NewPlayer", menuName = "ScriptableObjects/New Player")]
public class PlayerSO : ScriptableObject
{
    [Header("Player Stats")]
    public string playerName;
    public float playerHealth;
    public float PlayerArmor;


   

    [Header("Extra abilities ")] // not by default but can be added depending on player level
    public float rangeAffinity; // boost indicator range
    public bool canHealAllies; // can heal allies
}

// rebuild check point