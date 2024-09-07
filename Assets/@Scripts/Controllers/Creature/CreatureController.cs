using Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CreatureController : BaseController
{
    protected float _speed = 10.0f;


    protected Animator                  _animator;
    protected Collider                  _collider;
    protected Rigidbody                 _rigid;
    protected Material                  _material;
    protected NavMeshAgent              _nav;
    protected MeshRenderer[]            _meshrenderers;

    [SerializeField]
    private int _hp = 0;
    private int _maxHp = 0;


    Define.CreatureState _creatureState = Define.CreatureState.Idle;
    public Define.Scene sceneType = Define.Scene.None;
    [SerializeField]
    MeleeWeaponController _meleeWeapon;
    [SerializeField]
    ManualWeaponController _manualWeapon;
    [SerializeField]
    AutoWeaponController _autoWeapon;
    [SerializeField]
    GrenadeController _grenade;
    [SerializeField]
    ConsumableController _consumable;  

    public virtual MeleeWeaponController MeleeWeapon { get { return _meleeWeapon; } set { _meleeWeapon = value; } }
    public virtual ManualWeaponController ManualWeapon { get { return _manualWeapon; } set { _manualWeapon = value; } }
    public virtual AutoWeaponController AutoWeapon { get { return _autoWeapon; } set { _autoWeapon = value; } }
    public virtual GrenadeController Grenade { get { return _grenade; } set { _grenade = value; } }
    public virtual ConsumableController Consumable { get { return _consumable; } set { _consumable = value; } }
    public virtual Define.CreatureState CreatureState
    {
        get { return _creatureState; }
        set
        {
            _creatureState = value;
            UpdateAnimation();
        }
    }
    public virtual void UpdateAnimation(){}
    public virtual int DataID { get;set; } 
    public virtual float Speed { get; set; }
    public virtual int Gold {  get; set; }  
    public virtual int TotalAmmo { get; set; }
    public virtual int Potion { get;set; }
    public virtual int FragGrenade {  get; set; }   
    public virtual float AttackCoolTime {  get; set; }
    public virtual int HP 
    {
        get { return _hp;  }
        set { _hp = value;} 
    }
    public virtual int MaxHP
    {
        get { return _maxHp; }
        set { _maxHp = value;  }
    }
    public override bool Init()
	{
		base.Init();

        return true;
	}
	public virtual void OnDamaged(BaseController attacker, int damage = 0)
	{
		if (HP <= 0)
			return;

        HP -= damage;
		if (HP <= 0)
		{
            HP = 0; 
            OnDead();
		}
    }

	protected virtual void OnDead()
	{

	}
    
}
