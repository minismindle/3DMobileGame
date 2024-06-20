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
                _animator.SetBool("IsWalk", false);
                break;
            case Define.CreatureState.Moving:
                _animator.SetBool("IsWalk", true);
                break;
            case Define.CreatureState.Attack:
                _animator.SetTrigger("DoAttack");
                break;
            case Define.CreatureState.Dead:
                break;
        }
    }
    public override void MonsterAI()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player == null)
            return;

        float dist = (player.transform.position - this.transform.position).magnitude;

        if (dist <= AttackRange)
        {
            AttackMonster();
        }
        else if(dist <= ScanRange)
        {
            Target = player;
            MoveMonster();
        }
        else if(dist > ScanRange)
        {
            IdleMonster();
        }

        if (Target != null)
            TurnMonster(Target.transform.position);
    }
    public override void IdleMonster()
    {
        CreatureState = CreatureState.Idle;
        _nav.avoidancePriority = 51;
    }
    public override void MoveMonster()
    {
        CreatureState = CreatureState.Moving;
        StopAttack(this, 10);
        _nav.avoidancePriority = 51;
        _nav.SetDestination(Target.transform.position);
    }
    public override void AttackMonster()
    {
        StartAttack(this, 10);
        _nav.avoidancePriority = 50;
        _nav.SetDestination(transform.position);
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
        _collider = GetComponentInChildren<Collider>();
        _meshrenderers = GetComponentsInChildren<MeshRenderer>();
        ScanRange = 20f;
        AttackRange = 2f;
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
        CreatureState = CreatureState.Idle;
        CreatureState = CreatureState.Attack;
        yield return new WaitForSeconds(0.3f);
        Target.gameObject.GetComponent<PlayerController>().OnDotDamage(attacker, damage);
        yield return new WaitForSeconds(1f);
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
        if (_coAttack == null)
            return;
        StopCoroutine(_coAttack);
        _coAttack = null;
    }
    #endregion
}
