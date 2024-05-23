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
            case Define.CreatureState.Moving:
                _animator.CrossFade("Walk", 0.1f);
                break;
            case Define.CreatureState.Tanut:
                _animator.CrossFade("Tanut", 0.1f, -1, 0);
                break;
            case Define.CreatureState.Skill1:
                _animator.CrossFade("Shot", 0.1f, -1, 0);
                break;
            case Define.CreatureState.Skill2:
                _animator.CrossFade("BigShot", 0.1f, -1, 0);

                break;

        }
    }
    void MonsterAI()
    {
        switch (CreatureState)
        {
            case Define.CreatureState.Idle:
                IdleMonster();
                break;
            case Define.CreatureState.Attack:
                AttackMonster();
                break;
            case Define.CreatureState.Tanut:
                TanutMonster();
                break;
            case Define.CreatureState.Skill1:
                ShotMonster();
                break;
            case Define.CreatureState.Skill2:
                BigShotMonster();
                break;
        }
        if (Target != null)
            TurnMonster(Target.transform.position);
    }
    void IdleMonster()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
            return;

        Target = player;
        CreatureState = CreatureState.Attack;
        return;
    }
    void AttackMonster()
    {
        if (Target != null)
        {

        }
        else
        {

        }
    }
    void TanutMonster()
    {

    }
    void ShotMonster()
    {

    }
    void BigShotMonster()
    {

    }
    void TurnMonster(Vector3 dir)
    {
        transform.LookAt(dir);
    }

    public override bool Init()
    {
        base.Init();

        return true;
    }
    public override void SetInfo(int templateID)
    {
        _rigid = GetComponent<Rigidbody>();
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
