using Scripts.Game;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DeathPanelUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] GameObject _deathPanel;
    [SerializeField] Animator _animator;
    [SerializeField] TextMeshProUGUI _timeText;
    [SerializeField] GameObject _tryAgain;

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

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = true;
    }

    public void OnDeath()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        _animator.SetBool("Open", true);
        _animator.SetBool("TryAgain", false);

        _timeText.text = "Time: " + Mathf.Floor(time).ToString();
    }

    public void OnContinue()
    {
        _animator.SetBool("Open", false);
        _animator.SetBool("TryAgain", true);
    }

    public void OpenTryAgain()
    {
        _tryAgain.SetActive(true);
    }

    public void OpenDeathPanel()
    {
        _deathPanel.SetActive(true);
    }
}
