using System.Collections;
using ScriptableObjectArchitecture;
using Scripts.Game;
using Scripts.Player;
using UnityEngine;

[Enemy]
public class OgreEnemy : BaseEnemy
{
    [SerializeField]
    private GameObjectCollection enemyCollection;

    [SerializeField]
    private GameObjectCollection meleeEnemyCollection;

    [Header("Ogre Enemy Vars")]
    [SerializeField]
    private float _knockbackForce = 100f;

    [SerializeField]
    private float _knockbackStunDuration = 1.5f;

    private bool _isStunned = false;

    // anim
    private Animator monsterAnimator;
    public int Wave { get; set; }

    protected override void Start()
    {
        base.Start();
        enemyCollection.Add(gameObject);
        meleeEnemyCollection.Add(gameObject);

        // get anim
        Transform monsterTransform = transform.Find("monster_orc");
        if (monsterTransform != null)
        {
            monsterAnimator = monsterTransform.GetComponent<Animator>();
        }
        else
        {
            Debug.LogError("No Animator for ORGE!");
        }
    }

    private void OnDestroy()
    {
        enemyCollection.Remove(gameObject);
        meleeEnemyCollection.Remove(gameObject);
    }

    void Update()
    {
        var player = PlayerCharacterController.Instance;
        if (player == null)
        {
            return;
        }

        Vector3 toVector_raw = player.transform.position - transform.position;
        Vector3 toVector = new Vector3(toVector_raw.x, 0, toVector_raw.z);
        transform.rotation = Quaternion.LookRotation(toVector.normalized);

        if (!_isStunned)
        {
            if (toVector.sqrMagnitude > 1 + _collider.radius * 2)
            {
                float mult = 2.0f / (1.0f + Mathf.Exp(-0.5f * Wave)) - 0.572f;
                RigidbodyComponent.velocity = toVector.normalized * _enemyStatTunable.OgreSpeed;
            }
            else
            {
                Attack(player.gameObject);
            }
        }
    }

    private void Attack(GameObject entity)
    {
        // anim
        monsterAnimator.Play("Atk");

        Vector3 toVector = entity.transform.position - transform.position;

        DamageInfo d = new DamageInfo(1, BulletColor.Red, GetType().Name);
        entity.GetComponent<HealthComponent>().TakeDamage(d);

        RigidbodyComponent.velocity = Vector3.zero;
        RigidbodyComponent.AddForce(
            (-toVector.normalized + Vector3.up).normalized * _knockbackForce
        );
        StartCoroutine(Stun());
    }

    private IEnumerator Stun()
    {
        _isStunned = true;
        yield return new WaitForSeconds(_knockbackStunDuration);
        _isStunned = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            Debug.DrawRay(contact.point, contact.normal, Color.white);
        }

        // TODO:Contact
    }
}
