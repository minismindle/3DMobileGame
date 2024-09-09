using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static Define;
using static UnityEngine.GraphicsBuffer;
enum BossSkillType
{
    Tanut,
    SKill1,
    Skill2,
}
public class BossController : MonsterController
{
    public event Action OnBossDead;

    [SerializeField]
    Transform _missilePos1;
    [SerializeField]
    Transform _missilePos2;
    [SerializeField]
    Transform _rockPos;
    public override void UpdateAnimation()
    {
        switch (CreatureState)
        {
            case CreatureState.Idle:
                _animator.CrossFade("Idle", 0.1f);
                break;
            case CreatureState.Tanut:
                _animator.SetTrigger("DoTanut");
                break;
            case CreatureState.Skill1:
                _animator.SetTrigger("DoShot");
                break;
            case CreatureState.Skill2:
                _animator.SetTrigger("DoBigShot");
                break;
            case CreatureState.Dead:
                _animator.SetTrigger("DoDie");
                OnBossDead?.Invoke();   
                break;
        }
    }
    public override void MonsterAI()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player == null)
            return;
        Target = player;
        AttackMonster();

        if (Target != null || CreatureState == CreatureState.Idle)
            TurnMonster(Target.transform.position);
    }
    public override void IdleMonster()
    {
        CreatureState = CreatureState.Idle;
    }
    public override void AttackMonster()
    {
        StartRandomSkill(); 
    }
    public override void TurnMonster(Vector3 dir)
    {
        base.TurnMonster(dir);
    }
    public override void FreezeVelocity()
    {
        base.FreezeVelocity();
    }
    public override bool Init()
    {
        base.Init();
        SetInfo(13000);
        return true;
    }
    public override void SetInfo(int templateID)
    {
        base.SetInfo(templateID);
        Managers.Data.MonsterDataDic.TryGetValue(templateID, out var data); 
        MaxHP = data.MaxHP;
        HP = MaxHP;
        ManualWeapon = GetComponent<ManualWeaponController>();
        AttackRange = data.AttackRange;
        MonsterName = MonsterName.Boss;
        ObjectType = ObjectType.BossMonster;    
    }
    protected override void SubScribe()
    {
        InvokeMonsterData();
    }
    public override void OnDamaged(BaseController attacker, int damage)
    {
        if (CreatureState == CreatureState.Dead)
            return;

        base.OnDamaged(attacker, damage);
    }
    protected override void OnDead()
    {
        StopRandomSkill();
        _nav.SetDestination(transform.position);
        base.OnDead();
        OnBossDead?.Invoke();
    }
    private void FixedUpdate()
    {
        if (CreatureState == CreatureState.Dead)
            return;

        MonsterAI();
        FreezeVelocity();
    }

    #region RandomSkill
    Coroutine _coRandomSkill;
    int _randomSkill;
    IEnumerator CoRandomSkill()
    {
        while (true)
        {
            yield return new WaitForSeconds(3.0f);
            CreatureState = CreatureState.Idle;
            SetRandomSKill();
            switch (_randomSkill)
            {
                case (int)BossSkillType.Tanut:
                    CreatureState = CreatureState.Tanut;
                    _nav.SetDestination(Target.transform.position);
                    yield return new WaitForSeconds(2f);
                    _nav.SetDestination(transform.position);
                    break;
                case (int)BossSkillType.SKill1:
                    CreatureState = CreatureState.Skill1;
                    ManualWeapon.Use(this, _missilePos1.transform.position, transform.forward, this.transform.rotation, "Missile_Boss");
                    yield return new WaitForSeconds(0.5f);
                    ManualWeapon.Use(this, _missilePos2.transform.position, transform.forward, this.transform.rotation, "Missile_Boss");
                    break;
                case (int)BossSkillType.Skill2:
                    CreatureState = CreatureState.Skill2;
                    ManualWeapon.Use(this, _rockPos.transform.position, transform.forward, this.transform.rotation, "Rock_Boss");
                    yield return new WaitForSeconds(3.0f);
                    break;
            }
        }
    }
    void SetRandomSKill()
    {
        float dist = (Target.transform.position - this.transform.position).magnitude;

        if (dist > AttackRange)
        {
            _randomSkill = (int)BossSkillType.Tanut;
        }
        else if(dist <= AttackRange)
        {
            _randomSkill = UnityEngine.Random.Range((int)BossSkillType.SKill1, (int)BossSkillType.Skill2 + 1);
        }
    }
    void StartRandomSkill()
    {
        if (_coRandomSkill != null)
            return;
        _coRandomSkill = StartCoroutine(CoRandomSkill());
    }
    void StopRandomSkill()
    {
        StopCoroutine(_coRandomSkill);
        _coRandomSkill = null;
    }
    #endregion
}
