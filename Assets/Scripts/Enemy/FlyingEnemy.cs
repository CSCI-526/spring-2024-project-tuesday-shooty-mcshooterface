using System.Collections;
using System.Collections.Generic;
using ScriptableObjectArchitecture;
using Scripts.Player;
using TMPro;
using UnityEngine;

public class FlyingEnemy : BaseEnemy
{
    [SerializeField] private GameObjectCollection enemyCollection;
    [SerializeField] private GameObjectCollection flyingEnemyCollection;
    
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
    //
    private float _max_speed;
    private float dedgeInterval = 2.0f;
    private float dedgeTimer = 0f;
    // sight detect
    private bool _insight = true;
    private float _fov = 90;

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
        flyingEnemyCollection.Add(gameObject);
        transform.position = new Vector3(transform.position.x, 5, transform.position.z);
        _curState = FlyingState.Chase;
        _plTf = PlayerCharacterController.Instance.transform;
        StartCoroutine(SetAttack());

        //
        _max_speed = _enemyStatTunable.FlyingSpeed * 1.0f;
    }
    
    private void OnDestroy() {
        enemyCollection.Remove(gameObject);
        flyingEnemyCollection.Remove(gameObject);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_plTf == null) return;

        _toPlayer3 = _plTf.position - transform.position;
        _toPlayer = new Vector3(_toPlayer3.x, 0, _toPlayer3.z);


        transform.rotation = Quaternion.LookRotation(_toPlayer.normalized, transform.up);
        // dodge
        if (_curState == FlyingState.Strafe)
        {
            if (dedgeTimer <= 0f)
            {
                Dodge();
                dedgeTimer = dedgeInterval;
            }
            else
            {
                dedgeTimer -= Time.fixedDeltaTime;
            }
        }
        else
        {
            dedgeTimer = 0;
        }

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
                if (this._indodge)
                {
                    _max_speed = 1.5f * _enemyStatTunable.FlyingSpeed;
                }
                else 
                {
                    _max_speed = 0.7f * _enemyStatTunable.FlyingSpeed;
                }
                ToStrafe();
                break;
        }

        CheckState();
        if (RigidbodyComponent.velocity.magnitude > _max_speed)
        {
            RigidbodyComponent.velocity = RigidbodyComponent.velocity.normalized * _max_speed;
        }

        float angle = Vector3.Angle(_plTf.forward, -_toPlayer3);
        _insight = angle < (_fov / 2) ? true : false;
    }


    private IEnumerator SetAttack()
    {
        while (true)
        {
            if (!_insight)
            {
                Debug.Log("Flying: Attack canceled!");
            }
            else 
            {
                StartCoroutine(ChangeColor());
                yield return new WaitForSeconds(attackDuration);
                toAttack();
            }
            yield return new WaitForSeconds(attackCooldown);
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
        RigidbodyComponent.AddForce(_toPlayer.normalized * 12.0f, ForceMode.Force);
    }

    private void ToFlee()
    {
        RigidbodyComponent.AddForce(-_toPlayer.normalized * 12.0f, ForceMode.Force);
    }

    private void ToStrafe()
    {
        int directionFactor = Random.Range(0, 2) * 2 - 1;
        Vector3 rawStrafeDirection = Vector3.Cross(_toPlayer, _upVector).normalized * directionFactor;
        RigidbodyComponent.AddForce(rawStrafeDirection * 10.0f, ForceMode.Force);

    }

    private bool _indodge = false;
    private void Dodge() 
    {
        int directionFactor = Random.Range(0, 2) * 2 - 1;
        Vector3 rawStrafeDirection = Vector3.Cross(_toPlayer, _upVector).normalized * directionFactor;
        RigidbodyComponent.AddForce(rawStrafeDirection * 12.0f, ForceMode.Impulse);

        // anim_indicator
        if (!_indodge) 
        {
            StartCoroutine(IndicateDodge());
        }
    }

    // currently rotating along x 180 degree, cant be 360
    private IEnumerator IndicateDodge()
    {
        this._indodge = true;
        float rotationDuration = 0.9f;
        Quaternion startRotation = transform.rotation;
        Quaternion endRotation = startRotation * Quaternion.Euler(180.0f, 0, 0);
        float timeElapsed = 0.0f;

        while (timeElapsed < rotationDuration)
        {
            transform.rotation = Quaternion.Slerp(startRotation, endRotation, timeElapsed / rotationDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        transform.rotation = endRotation; 
        this._indodge = false;
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
