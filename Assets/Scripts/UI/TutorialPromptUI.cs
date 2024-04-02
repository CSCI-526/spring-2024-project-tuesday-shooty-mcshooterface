using System;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class TutorialPromptUI : MonoBehaviour {
    // components
    public CanvasGroup promptCanvasGroup;
    public TextMeshProUGUI text;
    
    // public constants
    public float fadeInTime;
    public float fadeAwayTime;

    // private state
    private Tween _fadingTween;
    
    // Start is called before the first frame update
    void Start() {
        // set text blank
        promptCanvasGroup.alpha = 0;
        text.text = "";
        
        UpdateActivePrompt();
        if(TutorialPromptManager.Instance) TutorialPromptManager.Instance.onActivePromptChange.AddListener(UpdateActivePrompt);
    }

    private void OnEnable() {
        if(TutorialPromptManager.Instance) TutorialPromptManager.Instance.onActivePromptChange.AddListener(UpdateActivePrompt);
    }

    private void OnDisable() {
        if(TutorialPromptManager.Instance) TutorialPromptManager.Instance.onActivePromptChange.RemoveListener(UpdateActivePrompt);
    }

    private void UpdateActivePrompt() {
        if (TutorialPromptManager.Instance.ActivePrompt == null)
        {
            _fadingTween?.Kill();
            _fadingTween = promptCanvasGroup.DOFade(0, fadeAwayTime).SetUpdate(true);
        }
        else {
            text.text = TutorialPromptManager.Instance.ActivePrompt.promptText;
            
            _fadingTween?.Kill();
            _fadingTween = promptCanvasGroup.DOFade(1, fadeInTime).SetUpdate(true);
        }
    }
}