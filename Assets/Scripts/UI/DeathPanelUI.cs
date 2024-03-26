using Scripts.Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathPanelUI : MonoBehaviour
{
    [SerializeField] Animator _animator;

    private void Start()
    {
        GameManager.Instance.DeathManager.OnSceneDeath += OnDeath;
        GameManager.Instance.DeathManager.OnSceneContinue += OnContinue;
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
    }

    public void OnContinue()
    {
        _animator.SetBool("Open", false);
        _animator.SetBool("TryAgain", true);
    }
}
