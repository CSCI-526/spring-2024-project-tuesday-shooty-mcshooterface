using ScriptableObjectArchitecture;
using Scripts.Game;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] ScoreTunableObject _scoreTunable;

    [Header("Debug")]
    [SerializeField] float killStreakTimer = 0;

    private void Start()
    {
        _scoreTunable.Score.Value = 0;
        _scoreTunable.CurrKillStreak.Value = 0;
        StartCoroutine(GiveSurvivalBonus());
    }

    private void Update()
    {
        if (killStreakTimer > 0)
        {
            killStreakTimer -= Time.deltaTime;
            if (killStreakTimer <= 0)
            {
                _scoreTunable.CurrKillStreak.Value = 0;
            }
        }
    }

    public void AddScore(int val)
    {
        _scoreTunable.Score.Value += val;
    }

    private IEnumerator GiveSurvivalBonus()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            AddScore(_scoreTunable.survivalBonusPerSec);
        }
    }

    public void GiveEnemyBonus()
    {
        _scoreTunable.CurrKillStreak.Value++;

        int bonus = _scoreTunable.CalculateKillStreakBonus();

        AddScore(bonus);

        killStreakTimer = _scoreTunable.killstreakDuration;
    }
}
