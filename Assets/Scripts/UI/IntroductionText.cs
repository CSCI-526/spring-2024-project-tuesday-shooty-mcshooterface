using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class IntroductionText : MonoBehaviour {
    [SerializeField] private CanvasGroup hud;
    [SerializeField] private TextMeshProUGUI text;

    [SerializeField] private List<string> wordsToShow;
    [SerializeField] private float timeBetweenText;
    
    // Start is called before the first frame update
    void Start() {
        hud.alpha = 0;
        text.text = "";
        StartCoroutine(ShowIntroText());
    }

    private IEnumerator ShowIntroText() {
        hud.alpha = 0;
        foreach (var word in wordsToShow) {
            text.text = word;

            yield return new WaitForSeconds(timeBetweenText);
        }
        text.text = "";

        // fade in hud 
        hud.alpha = 1;
    }
}
