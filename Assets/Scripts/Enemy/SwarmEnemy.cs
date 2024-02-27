using Scripts.Game;
using System.Collections;
using System.Collections.Generic;
using Scripts.Player;
using UnityEngine;

public class SwarmEnemy : BaseEnemy
{
    [Header("SwarmEnemy Variables")]
    [SerializeField] private float _speed = 1f;

    SwarmEnemyParent _parent;

    protected override IEnumerator SelfDestruct()
    {
        yield return new WaitForEndOfFrame();
        _parent.RemoveSwarmEnemy(this);
        Destroy(gameObject);
    }

    public void Construct(SwarmEnemyParent parent)
    {
        _parent = parent;
    }

    void Update() {
        var player = PlayerCharacterController.Instance;
        if (player == null)
        {
            return;
        }

        Vector3 toVector = player.transform.position - transform.position;
        if (toVector.sqrMagnitude > 1.5)
        {
            RigidbodyComponent.velocity = toVector.normalized * _speed;
        }
        else
        {
            RigidbodyComponent.velocity = Vector3.zero;
        }
    }
}
