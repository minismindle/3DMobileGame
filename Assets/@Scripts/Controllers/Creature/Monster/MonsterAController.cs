using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static Define;

public class MonsterAController : MonsterController
{
    public override void UpdateAnimation()
    {
        switch (CreatureState)
        {
            case Define.CreatureState.Idle:
                _animator.CrossFade("Idle", 0.1f);
                break;
            case Define.CreatureState.Moving:
                _animator.CrossFade("Walk", 0.1f);
                break;
            case Define.CreatureState.Attack:
                _animator.CrossFade("Attack", 0.1f, -1, 0);
                break;
            case Define.CreatureState.Dead:
                break;
        }
    }

    public override void MonsterAI()
    {
        switch (CreatureState)
        {
            case Define.CreatureState.Idle:
                IdleMonster();
                break;
            case Define.CreatureState.Moving:
                MoveMonster();
                break;
            case Define.CreatureState.Attack:
                AttackMonster();
                break;
        }
        if (Target != null)
            TurnMonster(Target.transform.position);
    }

    public override void IdleMonster()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
            return;

        float dist = (player.transform.position - this.transform.position).magnitude;

        if (dist <= ScanRange)
        {
            Target = player;

            CreatureState = CreatureState.Moving;
            return;
        }
    }

    public override void MoveMonster()
    {
        if (Target != null)
        {
            _nav.avoidancePriority = 51;

            _destPos = Target.transform.position;
            float dist = (_destPos - this.transform.position).magnitude;
            if (dist <= AttackRange)

            {
                _nav.avoidancePriority = 50;
                _nav.SetDestination(transform.position);
                CreatureState = Define.CreatureState.Attack;
                return;
            }
        }

        Vector3 _dir = _destPos - transform.position;

        if (_dir.magnitude < 0.1f)
        {
            CreatureState = Define.CreatureState.Idle;
        }
        else if (_dir.magnitude <= ScanRange)

        {
            _nav.SetDestination(_destPos);
        }
        else
        {
            _nav.SetDestination(transform.position);
            CreatureState = Define.CreatureState.Idle;
            Target = null;

        }
    }

    public override void AttackMonster()
    {
        if (Target != null)

        {

            StartAttack(this, 10);

            float dist = (Target.transform.position - transform.position).magnitude;

            if (dist <= AttackRange)

            {
                if (CreatureState != CreatureState.Attack)
                    CreatureState = Define.CreatureState.Attack;
            }
            else
            {
                CreatureState = Define.CreatureState.Moving;
                StopAttack(this, 10);
            }
        }
        else
        {
            CreatureState = Define.CreatureState.Idle;
            StopAttack(this, 10);
        }
    }
    public override void TurnMonster(Vector3 dir)
    {
        base.TurnMonster(dir);
    }
    public override void FreezeVelocity()
    {
        base.FreezeVelocity();
    }
    void FixedUpdate()
    {
        MonsterAI();
        FreezeVelocity();
    }
    public override bool Init()
    {
        base.Init();
        SetInfo(0);
        return true;
    }
    public override void SetInfo(int templateID)
    {
        MakeDead = false;
        Hp = 100;
        _rigid = GetComponent<Rigidbody>();
        _nav = GetComponent<NavMeshAgent>();
        _animator = GetComponentInChildren<Animator>();
        _meshrenderers = GetComponentsInChildren<MeshRenderer>();
        MonsterName = MonsterName.MonsterA;
        ObjectType = ObjectType.Monster;
        CreatureState = CreatureState.Idle;
    }
    public override void OnDamaged(BaseController attacker, int damage)
    {
        if (CreatureState == Define.CreatureState.Dead)
            return;

        base.OnDamaged(attacker, 10);
    }
    #region Attack
    Coroutine _coAttack;
    IEnumerator CoMonsterA(BaseController attacker, int damage)
    {
        yield return new WaitForSeconds(0.5f);
        Target.gameObject.GetComponent<PlayerController>().OnDotDamage(attacker, damage);
        yield return new WaitForSeconds(0.5f);
        _coAttack = null;
    }
    void StartAttack(BaseController attacker, int damage)
    {
        if (_coAttack != null)
            return;
        _coAttack = StartCoroutine(CoMonsterA(attacker, damage));
    }
    void StopAttack(BaseController attacker, int damage)
    {
        StopCoroutine(_coAttack);
        _coAttack = null;
    }
    #endregion
}
