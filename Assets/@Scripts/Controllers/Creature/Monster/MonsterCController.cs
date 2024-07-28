using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static Define;

public class MonsterCController : MonsterController
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
                _animator.SetTrigger("DoDie");
                break;
        }
    }
    public override void MonsterAI()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player == null)
            return;

        float dist = (player.transform.position - this.transform.position).magnitude;

        if (dist <= ScanRange)
        {
            Target = player;
            if (dist <= AttackRange)
            {
                AttackMonster();
            }
            else
            {
                MoveMonster();
            }
        }
        else if (dist > ScanRange)
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
        StopAttack();
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
        if (CreatureState == CreatureState.Dead)
            return;

        MonsterAI();
        FreezeVelocity();
    }
    public override bool Init()
    {
        base.Init();
        SetInfo(3);
        return true;
    }
    public override void SetInfo(int templateID)
    {
        MakeDead = false;
        Hp = 30;
        _rigid = GetComponent<Rigidbody>();
        _nav = GetComponent<NavMeshAgent>();
        _manualWeapon = GetComponent<ManualWeaponController>();
        _animator = GetComponentInChildren<Animator>();
        _collider = GetComponentInChildren<Collider>();
        _meshrenderers = GetComponentsInChildren<MeshRenderer>();
        ScanRange = 25f;
        AttackRange = 15f;
        MonsterName = MonsterName.MonsterC;
        ObjectType = ObjectType.Monster;
        CreatureState = CreatureState.Idle;
    }
    public override void OnDamaged(BaseController attacker, int damage)
    {
        if (CreatureState == CreatureState.Dead)
            return;

        base.OnDamaged(attacker, damage);
    }
    protected override void OnDead()
    {
        StopAttack();
        _nav.SetDestination(transform.position);
        base.OnDead();
    }
    #region Attack
    Coroutine _coAttack;
    IEnumerator CoMonsterC(BaseController attacker, int damage)
    {
        CreatureState = CreatureState.Idle;
        CreatureState = CreatureState.Attack;
        yield return new WaitForSeconds(0.5f);
        _manualWeapon.Use(this, _attackPos.transform.position, transform.forward,transform.rotation.normalized, "Missile");
        yield return new WaitForSeconds(3f);
        _coAttack = null;
    }
    void StartAttack(BaseController attacker, int damage)
    {
        if (_coAttack != null)
            return;
        _coAttack = StartCoroutine(CoMonsterC(attacker, damage));
    }
    void StopAttack()
    {
        if (_coAttack == null)
            return;
        StopCoroutine(_coAttack);
        _coAttack = null;
    }
    #endregion
}
