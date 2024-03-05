using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
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
        for (var w = 0; w < wordsToShow.Count; w++) {
            var word = wordsToShow[w];
            text.text = word;

            if (w == wordsToShow.Count - 1) {
                hud.DOFade(1, 2*timeBetweenText);
            }
            yield return new WaitForSeconds(timeBetweenText);
        }

        text.text = "";

        // wait for hud to fade in
        yield return new WaitForSeconds(timeBetweenText);
    }
}
