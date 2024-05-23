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
    public GameObject _AttackPos;
    protected Vector3 _destPos;
    public MonsterName MonsterName { get; set; }
    public float ScanRange { get; set; } = 20f;
    public float AttackRange { get; set; } = 2f;
    public GameObject Target { get; set; }

    protected Define.MonsterName _monsterName;
    public event Action<MonsterController> MonsterInfoUpdate;
    public override void UpdateAnimation()
    {
        
    }
    public virtual void  MonsterAI()
    {
       
    }
    public virtual void IdleMonster()
    {

    }
    public virtual void MoveMonster()
    {
        
    }
    public virtual void AttackMonster()
    {
        
    }

    public virtual void TurnMonster(Vector3 dir)
    {
    }
    public virtual void FreezeVelocity()
    {

    }
    public override bool Init()
    {
        base.Init();
        SetInfo(0);
        return true;
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

}


