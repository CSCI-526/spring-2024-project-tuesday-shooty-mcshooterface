using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KillstreakSlider : MonoBehaviour
{
    public Slider killStreakSlider;
    public TMP_Text killStreakText; 
    public float sliderDecayTime = 10f;

    private int currentKillStreak = 0;
    private float timeSinceLastKill;

    void Start()
    {
        killStreakSlider.maxValue = sliderDecayTime;
        killStreakSlider.value = sliderDecayTime;
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
    }

    public void OnKill() 
    {
        currentKillStreak++;
        killStreakText.text = currentKillStreak + " kills";
        timeSinceLastKill = 0; 
        killStreakSlider.value = sliderDecayTime; 
    }

    private void ResetKillStreak() 
    {
        currentKillStreak = 0;
        killStreakText.text = currentKillStreak + " kills";
        killStreakSlider.value = sliderDecayTime;
    }
}
