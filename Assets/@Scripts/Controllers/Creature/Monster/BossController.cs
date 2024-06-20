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
        AttackMonster();

        if (Target != null)
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

    public override bool Init()
    {
        base.Init();

        return true;
    }
    public override void SetInfo(int templateID)
    {
        _rigid = GetComponent<Rigidbody>();
        _rangeWeapon =GetComponent<RangeWeaponController>();
        _animator = GetComponentInChildren<Animator>();
        _meshrenderers = GetComponentsInChildren<MeshRenderer>();
        MonsterName = MonsterName.Boss;
        ObjectType = ObjectType.BossMonster;
        CreatureState = CreatureState.Idle;
    }

    private void FixedUpdate()
    {
        MonsterAI();

    }

    #region RandomSkill
    Coroutine _coRandomSkill;
    float _randomSkill;
    IEnumerator CoRandomSkill()
    {
        while (true)
        {
            yield return new WaitForSeconds(3.0f);
            //_randomSkill = Random.Range(0,3);
            _randomSkill = 2;
            switch (_randomSkill)
            {
                case 0:
                    CoTanutMonster();

                    break;
                case 1:
                    CoShotMonster();
                    _rangeWeapon.Use(this, _missilePos1.transform.position, transform.forward, this.transform.rotation, "Missile_Boss");
                    yield return new WaitForSeconds(0.5f);
                    _rangeWeapon.Use(this, _missilePos2.transform.position, transform.forward, this.transform.rotation, "Missile_Boss");
                    break;
                case 2:
                    CoBigShotMonster();
                    _rangeWeapon.Use(this, _rockPos.transform.position, transform.forward, this.transform.rotation, "Rock_Boss");
                    yield return new WaitForSeconds(5.0f);

                    break;
            }
        }
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

    #region WaitAnimation
    Coroutine _coWaitAnimation;
    IEnumerator CoWaitAnimation(float delay)
    {
        yield return new WaitForSeconds(delay);
        CreatureState = CreatureState.Idle;
    }
    void SetAnimationDelay(float delay)
    {
        if (_coWaitAnimation != null)
            _coWaitAnimation = null;
        _coWaitAnimation = StartCoroutine(CoWaitAnimation(delay));
    }
    #endregion
}
