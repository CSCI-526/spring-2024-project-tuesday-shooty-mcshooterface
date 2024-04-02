using ScriptableObjectArchitecture;
using Scripts.Player;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverheadDisplay : MonoBehaviour
{
    [Header("References")]
    [SerializeField] ScoreTunableObject _scoreTunable;
    [SerializeField] OverheadDisplayUI _killStreakObject;
    [SerializeField] OverheadDisplayUI _healingObject;

    [Header("Killstreak Texts")]
    [SerializeField] string killstreakSubstitute;
    [SerializeField] string bonusScoreSubstitute;
    [SerializeField] List<KillStreakTextDisplay> killStreakTexts;

    [Header("Healing Texts")]
    [SerializeField] string healingText;

    [Serializable]
    struct KillStreakTextDisplay
    {
        public Vector2Int range;
        public string text;
    }

    int previousKillstreakValue = 0;
    int previousHealthValue = 0;
    Queue<(string, OverheadDisplayUI)> killstreakQueue = new Queue<(string, OverheadDisplayUI)>();
    OverheadDisplayUI currOverheadUI;

    private void Start()
    {
        previousKillstreakValue = 0;
        previousHealthValue = PlayerCharacterController.Instance.HealthComponent.CurrentHealth;
    }

    private void Update()
    {
        if (previousKillstreakValue != _scoreTunable.CurrKillStreak.Value)
        {
            if (_scoreTunable.CurrKillStreak != 0)
            {
                killstreakQueue.Enqueue((CreateKillstreakText(), _killStreakObject));
            }
            previousKillstreakValue = _scoreTunable.CurrKillStreak.Value;
        }

        if (previousHealthValue != PlayerCharacterController.Instance.HealthComponent.CurrentHealth)
        {
            if (PlayerCharacterController.Instance.HealthComponent.CurrentHealth > previousHealthValue)
            {
                killstreakQueue.Enqueue((CreateHealingText(), _healingObject));
            }
            
            previousHealthValue = PlayerCharacterController.Instance.HealthComponent.CurrentHealth;
        }

        if (currOverheadUI == null && killstreakQueue.Count > 0)
        {
            currOverheadUI = CreateOverhead(killstreakQueue.Peek().Item1, killstreakQueue.Peek().Item2);
            killstreakQueue.Dequeue();
        }
    }

    public OverheadDisplayUI CreateOverhead(string text, OverheadDisplayUI uiObject)
    {
        OverheadDisplayUI ui = Instantiate(uiObject, transform);
        ui.Construct(text);

        return ui;
    }

    private string CreateKillstreakText()
    {
        string output = "";
        foreach (KillStreakTextDisplay kstd in killStreakTexts)
        {
            if (_scoreTunable.CurrKillStreak >= kstd.range.x && _scoreTunable.CurrKillStreak <= kstd.range.y)
            {
                output = kstd.text;

                output = output.Replace(killstreakSubstitute, _scoreTunable.CurrKillStreak.ToString());
                output = output.Replace(bonusScoreSubstitute, _scoreTunable.CalculateKillStreakBonus().ToString());

                return output;
            }
        }

        return output;
    }

    private string CreateHealingText()
    {
        return healingText;
    }
}
