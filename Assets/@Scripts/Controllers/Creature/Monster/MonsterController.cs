using System;
using Data;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using Sequence = DG.Tweening.Sequence;
using UnityEngine.AI;
using UnityEngine.AdaptivePerformance.VisualScripting;
using static Define;

public class MonsterController : CreatureController
{
    public GameObject _attackPos;
    protected Vector3 _destPos;
    public MonsterName MonsterName { get; set; }
    public virtual float ScanRange { get; set; } 
    public virtual float AttackRange { get; set; } 
    public GameObject Target { get; set; }

    protected MonsterName _monsterName;
    public event Action<MonsterController> MonsterInfoUpdate;
    public override void UpdateAnimation()
    {
        
    }
    protected override void OnDead()
    {
        if (CreatureState == Define.CreatureState.Dead)
            return;
        OnDeadState();
    }
    public virtual void  MonsterAI(){}
    public virtual void IdleMonster(){}
    public virtual void MoveMonster(){}
    public virtual void AttackMonster(){ }
    public virtual void WaitMonster(){}
    public virtual void TurnMonster(Vector3 dir)
    {
        transform.LookAt(dir);
    }
    public virtual void FreezeVelocity()
    {
        _rigid.velocity = Vector3.zero;
        _rigid.angularVelocity = Vector3.zero;
    }
    public override bool Init()
    {
        base.Init();
        return true;
    }
    public override void SetInfo(int templateID)
    {
        base.SetInfo(templateID);
        _rigid = GetComponent<Rigidbody>();
        _nav = GetComponent<NavMeshAgent>();
        _animator = GetComponentInChildren<Animator>();
        _collider = GetComponentInChildren<Collider>();
        _meshrenderers = GetComponentsInChildren<MeshRenderer>();
        ObjectType = ObjectType.Monster;
        CreatureState = CreatureState.Idle;
    }
    public void InvokeMonsterData()
    {
        if (this.IsValid() && gameObject.IsValid() && ObjectType != Define.ObjectType.Monster)
        {
            MonsterInfoUpdate?.Invoke(this);
        }
    }
    #region DotDamage
    Coroutine _coDotDamage;
    IEnumerator CoStartDotDamage(BaseController attacker, int damage)
    {
        OnDamaged(attacker, damage);
        ChangeColor(Color.red);
        yield return new WaitForSeconds(0.3f);
        ChangeColor(Color.white);
    }
    public void OnDotDamage(BaseController attacker, int damage)
    {
        if (_coDotDamage != null)
            StopCoroutine(_coDotDamage);
        _coDotDamage = StartCoroutine(CoStartDotDamage(attacker, damage));
    }
    void ChangeColor(Color color)
    {
        foreach (MeshRenderer meshrenderer in _meshrenderers)
        {
            Material material = meshrenderer.material;
            material.color = color;
        }
    }
    #endregion

    #region Dead
    Coroutine _coDead;
    IEnumerator CoDead()
    {
        ChangeColor(Color.black);
        CreatureState = Define.CreatureState.Dead;
        yield return new WaitForSeconds(3.0f);
        _coDead = null;
        Managers.Object.Despawn(this);
        Managers.Game.KillCount++;
        if (ObjectType == Define.ObjectType.BossMonster) 
        {
            Managers.UI.ShowPopup<UI_GameResultPopup>(); 
        }
    }
    void OnDeadState()
    {
        if (_coDead != null)
            return;
        _coDead = StartCoroutine(CoDead()); 
    }
    #endregion

}


