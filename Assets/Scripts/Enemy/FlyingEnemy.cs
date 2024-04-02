using System.Collections;
using System.Collections.Generic;
using ScriptableObjectArchitecture;
using Scripts.Player;
using TMPro;
using UnityEngine;

public class FlyingEnemy : BaseEnemy
{
    [SerializeField] private GameObjectCollection enemyCollection;
    
    public enum FlyingState
    {
        Chase,
        Attack,
        Strafe,
        Flee
    }

    private FlyingState _curState;
    private Transform _plTf; // PlayerTransform
    private Vector3 _toPlayer; // vector from enemy to player (excluding y)
    private Vector3 _toPlayer3; // vector form enemy to player (including y)
    private Vector3 _upVector = new Vector3(0, 1, 0);
    private Color attackColor = Color.cyan;
    
    [SerializeField] private float closeRange = 10.0f;
    [SerializeField] private float farRange = 20.0f;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float projectileSpeed = 1.0f;
    [SerializeField] private float projectileTimeToLive = 5.0f;
    [SerializeField] private float attackDuration = 2.0f;
    [SerializeField] private float attackCooldown = 5.0f;
    
    protected override void Start()
    {
        base.Start();
        enemyCollection.Add(gameObject);
        transform.position = new Vector3(transform.position.x, 5, transform.position.z);
        _curState = FlyingState.Chase;
        _plTf = PlayerCharacterController.Instance.transform;
        StartCoroutine(SetAttack());
    }
    
    private void OnDestroy() {
        enemyCollection.Remove(gameObject);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_plTf == null) return;

        _toPlayer3 = _plTf.position - transform.position;
        _toPlayer = new Vector3(_toPlayer3.x, 0, _toPlayer3.z);

        switch (_curState)
        {
            case FlyingState.Chase:
                ToChase();
                break;
            case FlyingState.Attack:
                //IndicateAttack();
                //ToAttack();
                break;
            case FlyingState.Flee:
                ToFlee();
                break;
            case FlyingState.Strafe:
                ToStrafe();
                break;
        }

        CheckState();
        
    }

    private IEnumerator SetAttack()
    {
        while (true)
        {
            yield return new WaitForSeconds(attackCooldown);
            StartCoroutine(ChangeColor());
            yield return new WaitForSeconds(attackDuration);
            toAttack();
        }
    }

    private void CheckState()
    {
        if (_toPlayer.magnitude > farRange)
        {
            _curState = FlyingState.Chase;
        }
        else if (_toPlayer.magnitude <= farRange && _toPlayer.magnitude > closeRange)
        {
            _curState = FlyingState.Strafe;
        }
        else
        {
            _curState = FlyingState.Flee;
        }
    }
    private void ToChase()
    {
        RigidbodyComponent.velocity = _toPlayer.normalized * _enemyStatTunable.FlyingSpeed;
    }

    private void ToFlee()
    {
        RigidbodyComponent.velocity = -_toPlayer.normalized * _enemyStatTunable.FlyingSpeed;
    }

    private void ToStrafe()
    {
        
        Vector3 rawStrafeDirection = Vector3.Cross(_toPlayer, _upVector).normalized;
        RigidbodyComponent.velocity = rawStrafeDirection * _enemyStatTunable.FlyingSpeed;

    }

    private void toAttack()
    {
        
        GameObject projectile = Instantiate(projectilePrefab, transform.position, transform.rotation);
        float offset = 2.0f;
        Vector3 aim = new Vector3(_toPlayer3.x, _toPlayer3.y + offset, _toPlayer3.z);
        projectile.GetComponent<Rigidbody>().AddForce(_toPlayer3 * projectileSpeed, ForceMode.Impulse);
        Destroy(projectile, projectileTimeToLive);

    }

    private IEnumerator ChangeColor()
    {
        Color startColor = GetComponent<Renderer>().material.color;
        GetComponent<Renderer>().material.color = attackColor;
        yield return new WaitForSeconds(attackDuration);
        GetComponent<Renderer>().material.color = startColor;
    }
    
}
