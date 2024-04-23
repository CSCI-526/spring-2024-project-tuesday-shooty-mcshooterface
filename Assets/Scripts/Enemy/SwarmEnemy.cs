using System.Collections;
using Scripts.Player;
using UnityEngine;

public class SwarmEnemy : BaseEnemy
{
    SwarmEnemyParent _parent;

    public enum SwarmState
    {
        Chase,
        Attack,
        Wander,
        Flee
    }

    private SwarmState _curState;
    private Transform _plTf; // PlayerTransfrom

    public float closeRange = 10.0f;
    public float farRange = 12.0f;

    private Animator monsterAnimator;

    protected override IEnumerator SelfDestruct()
    {
        yield return new WaitForEndOfFrame();
        _parent.RemoveSwarmEnemy(this);
        Destroy(gameObject);
    }

    public void Construct(SwarmEnemyParent parent)
    {
        _curState = SwarmState.Chase;
        _parent = parent;
        _plTf = PlayerCharacterController.Instance.transform;
        //RigidbodyComponent.AddForce(Vector3.down * 20.0f, ForceMode.Impulse);

        // get anim
        Transform monsterTransform = transform.Find("Slime_Red");
        if (monsterTransform != null)
        {
            monsterAnimator = monsterTransform.GetComponent<Animator>();
        }
        else
        {
            Debug.LogError("No Animator found for Slime!");
        }

        if (monsterAnimator != null)
        {
            monsterAnimator.Play("slime_move");
        }
        else
        {
            Debug.LogError("No Animator constructed for Slime!");
        }
    }

    private void Update() { }

    Vector3 _distance;

    void FixedUpdate()
    {
        if (_plTf == null)
        {
            return;
        }
        Vector3 ds = _plTf.position - transform.position;
        _distance = new Vector3(ds.x, 0, ds.z);

        transform.rotation = Quaternion.LookRotation(_distance.normalized);

        switch (_curState)
        {
            case SwarmState.Chase:
                ToChase();
                break;
            case SwarmState.Attack:

                break;
            case SwarmState.Wander:
                ToWander();
                break;
            case SwarmState.Flee:
                ToFlee();
                break;
        }

        CheckState();
        if (RigidbodyComponent.velocity.magnitude > _enemyStatTunable.SwarmSpeed)
        {
            RigidbodyComponent.velocity =
                RigidbodyComponent.velocity.normalized * _enemyStatTunable.SwarmSpeed;
        }
    }

    void ToChase()
    {
        RigidbodyComponent.velocity = _distance.normalized * _enemyStatTunable.SwarmSpeed;
    }

    void ToFlee()
    {
        /*
        Vector3 nm = _parent.SwarmCenter - _plTf.position;
        Vector3 mid = Vector3.Project(_distance, nm); // mid = - mid
        Vector3 dir = (_distance + 2 * mid).normalized;
        RigidbodyComponent.velocity = - dir * _enemyStatTunable.SwarmSpeed;
        */
        Vector3 nm = _parent.SwarmCenter - _plTf.position;
        float mag = Mathf.Abs(Vector3.Dot(_distance, nm) / nm.magnitude);
        Vector3 dir = _distance + nm.normalized * 2 * mag;
        RigidbodyComponent.velocity = _enemyStatTunable.SwarmSpeed * dir.normalized;
    }

    void ToWander()
    {
        //RigidbodyComponent.velocity = _parent.wanderDir * _enemyStatTunable.SwarmSpeed;
        RigidbodyComponent.AddForce(_parent.wanderDir * 10.0f, ForceMode.Force);
        RigidbodyComponent.AddForce(
            (_parent.SwarmCenter - transform.position) * 5.0f,
            ForceMode.Force
        );
    }

    void CheckState()
    {
        if (_distance.magnitude > farRange)
        {
            _curState = SwarmState.Chase;
        }
        else if (_distance.magnitude <= farRange && _distance.magnitude > closeRange)
        {
            _curState = SwarmState.Wander;
        }
        else
        {
            _curState = SwarmState.Flee;
        }
    }

    public void IndicateAttack()
    {
        if (_parent == null)
        {
            Debug.Log("null parent");
            return;
        }
        else if (RigidbodyComponent == null)
        {
            Debug.Log("null RB");
            return;
        }

        RigidbodyComponent.AddForce(
            (_parent.SwarmCenter - transform.position) * 7.0f,
            ForceMode.Impulse
        );
        StartCoroutine(ChangeColor(Color.magenta, _parent.attackIndicate / 2));
    }

    // Debug attack indicator
    IEnumerator ChangeColor(Color targetColor, float duration)
    {
        Color startColor = GetComponent<Renderer>().material.color;
        float startTime = Time.time;

        while (Time.time - startTime < duration)
        {
            float t = (Time.time - startTime) / duration;

            GetComponent<Renderer>().material.color = Color.Lerp(startColor, targetColor, t);

            yield return null;
        }

        yield return new WaitForSeconds(duration);

        // anim
        this.monsterAnimator.Play("slime_attack");

        GetComponent<Renderer>().material.color = startColor;
    }
}
