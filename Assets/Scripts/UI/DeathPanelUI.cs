using Scripts.Game;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using ScriptableObjectArchitecture;

public class DeathPanelUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] GameObject _deathPanel;
    [SerializeField] Animator _animator;
    [SerializeField] TextMeshProUGUI _timeText;
    [SerializeField] GameObject _tryAgain;
    [SerializeField] IntVariable _timeNumber;

    [Header("Debug")]
    [SerializeField] float time;

    private void Start()
    {
        GameManager.Instance.DeathManager.OnSceneDeath += OnDeath;
        GameManager.Instance.DeathManager.OnSceneContinue += OnContinue;

        _animator.updateMode = AnimatorUpdateMode.UnscaledTime;

        time = 0;
    }

    private void Update()
    {
        time += Time.deltaTime;
        _timeNumber.Value = Mathf.FloorToInt(time);
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

        Time.timeScale = 0;

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
