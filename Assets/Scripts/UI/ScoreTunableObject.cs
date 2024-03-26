using ScriptableObjectArchitecture;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ScoreTunableObject", order = 1)]
public class ScoreTunableObject : ScriptableObject
{
    [Header("Tunables")]
    public int survivalBonusPerSec;
    public int killBonus;
    public int consecutiveKillStreakBonus;
    public float killstreakDuration;
    public int oneshotBonus;

    [Header("Int References")]
    public IntVariable Score;
    public IntVariable CurrKillStreak;


    public int CalculateKillStreakBonus()
    {
        return killBonus + ((CurrKillStreak - 1) * consecutiveKillStreakBonus);
    }
}
