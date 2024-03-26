using ScriptableObjectArchitecture;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillstreakDisplay : MonoBehaviour
{
    [Header("References")]
    [SerializeField] ScoreTunableObject _scoreTunable;
    [SerializeField] KillstreakDisplayUI _killStreakObject;

    [Header("Killstreak Texts")]
    [SerializeField] string killstreakSubstitute;
    [SerializeField] string bonusScoreSubstitute;
    [SerializeField] List<KillStreakTextDisplay> killStreakTexts;

    [Serializable]
    struct KillStreakTextDisplay
    {
        public Vector2Int range;
        public string text;
    }

    int previousValue = 0;

    private void Start()
    {
        previousValue = 0;
    }

    private void Update()
    {
        if (previousValue != _scoreTunable.CurrKillStreak.Value)
        {
            if (_scoreTunable.CurrKillStreak != 0)
            {
                CreateKillstreak();
            }
            previousValue = _scoreTunable.CurrKillStreak.Value;
        }
    }

    public void CreateKillstreak()
    {
        KillstreakDisplayUI ui = Instantiate(_killStreakObject, transform);
        ui.Construct(CreateKillstreakText());
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
}
