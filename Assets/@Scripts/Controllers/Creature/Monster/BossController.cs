using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static Define;
using static UnityEngine.GraphicsBuffer;

public class BossController : MonsterController
{
    public Transform _missilePos1;
    public Transform _missilePos2;
    public Transform _rockPos;
    enum BossSkillType
    {
        Tanut,
        SKill1,
        Skill2,
    }
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

        if (Target != null)
            TurnMonster(Target.transform.position);
    }
    public override void IdleMonster()
    {
        CreatureState = CreatureState.Idle;
        _nav.avoidancePriority = 51;
    }
    public override void AttackMonster()
    {
        _nav.avoidancePriority = 50;
        StartRandomSkill(); 
    }
    public override void TurnMonster(Vector3 dir)
    {
        base.TurnMonster(dir);
    }

    public override bool Init()
    {
        base.Init();
        SetInfo(0);
        return true;
    }
    public override void SetInfo(int templateID)
    {
        Hp = 30;
        _rigid = GetComponent<Rigidbody>();
        _nav = GetComponent<NavMeshAgent>();   
        _collider = GetComponent<Collider>();
        _rangeWeapon =GetComponent<RangeWeaponController>();
        _meleeWeapon = GetComponent<MeleeWeaponController>();   
        _animator = GetComponentInChildren<Animator>();
        _meshrenderers = GetComponentsInChildren<MeshRenderer>();
        MonsterName = MonsterName.Boss;
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
        StopRandomSkill();
        base.OnDead();
    }
    private void FixedUpdate()
    {
        if (CreatureState == CreatureState.Dead)
            return;

        MonsterAI();
    }

    #region RandomSkill
    Coroutine _coRandomSkill;
    int _randomSkill;
    IEnumerator CoRandomSkill()
    {
        while (true)
        {
            yield return new WaitForSeconds(3.0f);
            SetRandomSKill();
            switch (_randomSkill)
            {
                case (int)BossSkillType.Tanut:
                    _nav.avoidancePriority = 51;
                    _nav.SetDestination(Target.transform.position);
                    CoTanutMonster();
                    yield return new WaitForSeconds(2f);
                    _nav.avoidancePriority = 50;
                    _nav.SetDestination(transform.position);
                    break;
                case (int)BossSkillType.SKill1:
                    CoShotMonster();
                    _rangeWeapon.Use(this, _missilePos1.transform.position, transform.forward, this.transform.rotation, "Missile_Boss");
                    yield return new WaitForSeconds(0.5f);
                    _rangeWeapon.Use(this, _missilePos2.transform.position, transform.forward, this.transform.rotation, "Missile_Boss");
                    break;
                case (int)BossSkillType.Skill2:
                    CoBigShotMonster();
                    _rangeWeapon.Use(this, _rockPos.transform.position, transform.forward, this.transform.rotation, "Rock_Boss");
                    yield return new WaitForSeconds(3.0f);
                    break;
            }
        }
    }
    void SetRandomSKill()
    {
        //전에 사용한 스킬은 쓸수없도록 만들지 고민중
        _randomSkill = Random.Range((int)BossSkillType.Tanut, (int)BossSkillType.Skill2 + 1);
    }
    void CoTanutMonster()
    {
        CreatureState = CreatureState.Idle;
        CreatureState = CreatureState.Tanut;
    }
    void CoShotMonster()
    {
        CreatureState = CreatureState.Idle;
        CreatureState = CreatureState.Skill1;
    }
    void CoBigShotMonster()
    {
        CreatureState = CreatureState.Idle;
        CreatureState = CreatureState.Skill2;
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
