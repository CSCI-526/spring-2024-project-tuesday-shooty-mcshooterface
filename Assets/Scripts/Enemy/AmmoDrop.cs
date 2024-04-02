using Scripts.Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoDrop : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField] int _amount = 0;
    [SerializeField] BulletColor _color;
    [SerializeField] private Prompt completeOnPickup;

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
                Destroy(gameObject);
            }
        }
    }
}
