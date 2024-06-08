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
    public Transform _RockPos;
   
    public override void UpdateAnimation()
    {
        switch (CreatureState)
        {
            case Define.CreatureState.Idle:
                _animator.CrossFade("Idle", 0.1f);
                break;
            case Define.CreatureState.Tanut:
                _animator.SetTrigger("DoTanut");
                break;
            case Define.CreatureState.Skill1:
                _animator.SetTrigger("DoShot");
                break;
            case Define.CreatureState.Skill2:
                _animator.SetTrigger("DoBigShot");
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
        AttackMonster();
    }
    public override void IdleMonster()
    {
        CreatureState = Define.CreatureState.Idle;
    }
    public override void AttackMonster()
    {
        StartRandomSkill(); 

    }
    void TanutMonster()
    {

    }
    void ShotMonster()
    {
        CreatureState = Define.CreatureState.Skill1;
    }
    void BigShotMonster()
    {

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
        MonsterName = Define.MonsterName.Boss;
        ObjectType = Define.ObjectType.BossMonster;
        CreatureState = Define.CreatureState.Idle;
    }

    private void FixedUpdate()
    {
        MonsterAI();
    }

    #region RandomSkill
    Coroutine _coRandomSkill;
    IEnumerator CoRandomSkill()
    {
        yield return new WaitForSeconds(3.0f);

        switch(CreatureState)
        {
            case CreatureState.Tanut:
                break;
            case CreatureState.Skill1:
                break;
            case CreatureState.Skill2: 
                break;   
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

    #region WaitAnimation
    Coroutine _coWaitAnimation;
    IEnumerator CoWaitAnimation(float delay)
    {
        yield return new WaitForSeconds(delay);
        CreatureState = Define.CreatureState.Idle;
    }
    void SetAnimationDelay(float delay)
    {
        if (_coWaitAnimation != null)
            _coWaitAnimation = null;
        _coWaitAnimation = StartCoroutine(CoWaitAnimation(delay));
    }
    #endregion
}
