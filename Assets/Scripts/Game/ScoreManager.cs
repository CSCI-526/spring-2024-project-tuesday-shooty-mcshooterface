using ScriptableObjectArchitecture;
using Scripts.Game;
using Scripts.Player;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] ScoreTunableObject _scoreTunable;

    [Header("Healing")]
    [SerializeField] int _healingAmount;
    [SerializeField] int _healingThreshold;

    [Header("Debug")]
    [SerializeField] float _killStreakTimer = 0;
    [SerializeField] int _healsGiven = 0;

    private void Start()
    {
        _scoreTunable.Score.Value = 0;
        _scoreTunable.CurrKillStreak.Value = 0;
        StartCoroutine(GiveSurvivalBonus());
    }

    private void Update()
    {
        if (_killStreakTimer > 0)
        {
            _killStreakTimer -= Time.deltaTime;
            if (_killStreakTimer <= 0)
            {
                _scoreTunable.CurrKillStreak.Value = 0;
            }
        }
    }

    public void AddScore(int val)
    {
        _scoreTunable.Score.Value += val;

        if (_scoreTunable.Score.Value >= _healingThreshold * (_healsGiven + 1))
        {
            PlayerCharacterController.Instance?.HealthComponent.HealHealth(_healingAmount);
            _healsGiven++;
        }
    }

    private IEnumerator GiveSurvivalBonus()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            if (PlayerCharacterController.Instance.HealthComponent.CurrentHealth > 0) AddScore(_scoreTunable.survivalBonusPerSec);
        }
    }

    public void GiveEnemyBonus()
    {
        _scoreTunable.CurrKillStreak.Value++;

        int bonus = _scoreTunable.CalculateKillStreakBonus();

        AddScore(bonus);

        _killStreakTimer = _scoreTunable.killstreakDuration;
    }
}
