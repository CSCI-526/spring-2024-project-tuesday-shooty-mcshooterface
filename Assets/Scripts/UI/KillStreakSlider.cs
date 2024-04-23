using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ScriptableObjectArchitecture;

public class KillstreakSlider : MonoBehaviour
{
    [Header("References")]
    [SerializeField] ScoreTunableObject _scoreTunable;

    public Slider killStreakSlider;
    public TMP_Text killStreakText; 
    private float sliderDecayTime;

    private int currentKillStreak = 0;
    private float timeSinceLastKill;

    void Start()
    {

        if (_scoreTunable != null)
        {
            sliderDecayTime = _scoreTunable.killstreakDuration;
            Debug.Log("killstreakDuration = " + sliderDecayTime);

            killStreakSlider.maxValue = sliderDecayTime;
            killStreakSlider.value = 0;
            timeSinceLastKill = sliderDecayTime + 1;
        }
        else
        {
            Debug.LogError("ScoreTunableObject is not assigned!");
        }
    }

    void Update()
    {
        if (timeSinceLastKill < sliderDecayTime)
        {
            timeSinceLastKill += Time.deltaTime;
            killStreakSlider.value = sliderDecayTime - timeSinceLastKill;
        }
        else
        {
            ResetKillStreak();
        }

        if (currentKillStreak != _scoreTunable.CurrKillStreak.Value)
        {
            currentKillStreak = _scoreTunable.CurrKillStreak.Value;
            killStreakText.text = currentKillStreak + " kills"; // Ensure killstreak text is always up to date
        }
    }

    public void OnKill() 
    {
        /*currentKillStreak++;
        killStreakText.text = currentKillStreak + " kills";
        timeSinceLastKill = 0;
        killStreakSlider.value = sliderDecayTime;*/

        //_scoreTunable.CurrKillStreak.Value++; // Increment the kill streak in ScoreTunableObject
        /*killStreakText.text = _scoreTunable.CurrKillStreak.Value + " kills"; // Update the text display*/
        timeSinceLastKill = 0;
        killStreakSlider.value = sliderDecayTime;
    }

    private void ResetKillStreak()
    {
        /* currentKillStreak = 0;
         killStreakText.text = currentKillStreak + " kills";
         killStreakSlider.value = sliderDecayTime;*/

        _scoreTunable.CurrKillStreak.Value = 0; // Reset the kill streak in ScoreTunableObject
        currentKillStreak = 0;
        killStreakText.text = "0 kills"; // Reset the text display
        killStreakSlider.value = 0;
    }
}
