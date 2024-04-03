using Scripts.Game;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoDrop : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField] int _amount = 0;
    [SerializeField] BulletColor _color;
    [SerializeField] private Prompt completeOnPickup;
    [SerializeField] float _lifespan;
    [SerializeField] float _offsetLifespan;
    [SerializeField] int _minActiveAmmo;

    static Dictionary<BulletColor, int> _ammoDictionary;

    private void Awake()
    {
        if (_ammoDictionary == null)
        {
            _ammoDictionary = new Dictionary<BulletColor, int>();
            foreach (BulletColor color in Enum.GetValues(typeof(BulletColor)))
            {
                _ammoDictionary.Add(color, 0);
            }
        }

        _ammoDictionary[_color]++;
        StartCoroutine(Despawn());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other != null && (other.gameObject.layer == LayerMask.NameToLayer("Player")))
        {
            if (TutorialPromptManager.Instance && completeOnPickup) {
                TutorialPromptManager.Instance.TryCompletePrompt(completeOnPickup);
            }
            if (!GameManager.Instance.BulletQueueManager.IsMaxAmmo())
            {
                for (int i = 0; i < _amount; i++)
                {
                    GameManager.Instance.BulletQueueManager.ObtainBullet(_color);
                }
                SelfDestruct();
            }
        }
    }

    private IEnumerator Despawn()
    {
        float elapsed = _lifespan + _ammoDictionary[_color] * _offsetLifespan;
        while (elapsed > 0)
        {
            if (_ammoDictionary[_color] > _minActiveAmmo)
            {
                elapsed -= Time.deltaTime;
            }
            yield return null;
        }
        SelfDestruct();
    }

    private void SelfDestruct()
    {
        _ammoDictionary[_color]--;
        Destroy(gameObject);
    }
}
