using Scripts.Game;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DeathPanelUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Animator _animator;
    [SerializeField] TextMeshProUGUI _timeText;

    [Header("Debug")]
    [SerializeField] float time;

    private void Start()
    {
        GameManager.Instance.DeathManager.OnSceneDeath += OnDeath;
        GameManager.Instance.DeathManager.OnSceneContinue += OnContinue;

        time = 0;
    }

    private void Update()
    {
        time += Time.deltaTime;
    }

    private void OnDestroy()
    {
        GameManager.Instance.DeathManager.OnSceneDeath -= OnDeath;
        GameManager.Instance.DeathManager.OnSceneContinue -= OnContinue;
    }

    public void OnDeath()
    {
        _animator.SetBool("Open", true);
        _animator.SetBool("TryAgain", false);

        _timeText.text = "Time: " + Mathf.Floor(time).ToString();
    }

    public void OnContinue()
    {
        _animator.SetBool("Open", false);
        _animator.SetBool("TryAgain", true);
    }
}
