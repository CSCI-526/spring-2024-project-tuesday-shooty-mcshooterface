using Scripts.Game;
using Scripts.Player;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathManager : MonoBehaviour
{
    [SerializeField] DeathState _deathState;

    public delegate void DeathEvent();
    public DeathEvent OnSceneDeath;
    public DeathEvent OnSceneContinue;

    private enum DeathState
    {
        Alive,
        Dying,
        TryAgain,
    }

    private void Start()
    {
        PlayerCharacterController.Instance.HealthComponent.OnDeath += OnPlayerDeath;
        _deathState = DeathState.Alive;
    }

    private void Update()
    {
        if (DeathState.Alive == _deathState && Input.GetKeyDown(KeyCode.R))
        {
            ReloadGame();
        }
        else if (DeathState.TryAgain == _deathState && (Input.GetMouseButtonDown(0)))
        {
            ReloadGame();
            _deathState = DeathState.Alive;
            Time.timeScale = 1;
        }
    }
    private void OnPlayerDeath(in DamageInfo damage)
    {
        EndGame();
    }

    public void EndGame()
    {
        _deathState = DeathState.Dying;
        //Time.timeScale = 0;
        StartCoroutine(EndGameRoutine());
    }

    private IEnumerator EndGameRoutine()
    {
        OnSceneDeath?.Invoke();
        List<KeyValue<string, long>> damageDealtPerEnemyType = (
            from kvp in GameManager.Instance.BulletQueueManager.DamageDealtPerEnemyType
            select new KeyValue<string, long>() { Key = kvp.Key, Value = kvp.Value }
        ).ToList();
        List<KeyValue<string, long>> ammoCollection = (
            from kvp in GameManager.Instance.BulletQueueManager.AmmoCollections
            select new KeyValue<string, long>() { Key = kvp.Key, Value = kvp.Value }
        ).ToList();
        List<KeyValue<string, long>> ammoDamageDealt = (
            from kvp in GameManager.Instance.BulletQueueManager.AmmoDamageDealt
            select new KeyValue<string, long>() { Key = kvp.Key, Value = kvp.Value }
        ).ToList();

        yield return GameManager.Instance.AnalyticsManager.LogRun(
            new RunData
            {
                SurvivalTimeSeconds = (int)Time.timeSinceLevelLoad,
                AmmoCollections = ammoCollection,
                DamageDealtPerAmmo = ammoDamageDealt,
                DamageDealtPerEnemyType = damageDealtPerEnemyType,
            }
        );
        OnSceneContinue?.Invoke();
        _deathState = DeathState.TryAgain;
    }

    private void ReloadGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


}
