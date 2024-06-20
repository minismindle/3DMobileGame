using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CreatureController : BaseController
{
    protected float _speed = 10.0f;

    public bool MakeDead = false;
    [SerializeField]
    protected Animator _animator;
    protected Rigidbody _rigid;
    protected Material _material;
    protected NavMeshAgent _nav;
    public MeleeWeaponController _meleeWeapon;
    public RangeWeaponController _rangeWeapon;
    protected MeshRenderer[] _meshrenderers;
    public Collider _collider;
    
    Define.CreatureState _creatureState = Define.CreatureState.Idle;
    public Vector3 CenterPosition
    {
        get
        {
            return new Vector3(transform.position.x,transform.position.y - 0.3f);
        }
    }
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

    public Data.CreatureData CreatureData;
    public virtual int templateID { get;set; } 
    public virtual string type { get; set; }    
    public virtual string prefab { get; set; }
    public virtual int level { get; set; }
    public virtual int attack { get; set; } 
    public virtual float speed { get; set; }
    public virtual int maxHp { get; set;}
    public virtual int maxExp { get; set;}
    public int Hp { get; set; }
    public virtual int Gold {  get; set; }  
    public virtual int Ammo { get; set; }
    public virtual int Bomb {  get; set; }
    public virtual int Potion { get;set; }
    public virtual float AttackCoolTime {  get; set; }

    public override bool Init()
	{
		base.Init();

        return true;
	}
    public bool IsMonster()
    {
        switch(ObjectType) 
        {
            case Define.ObjectType.Monster:
            case Define.ObjectType.BossMonster:
                return true;
            default:
                return false;
        }
    }
	public virtual void OnDamaged(BaseController attacker, int damage = 0)
	{
		if (Hp <= 0)
			return;

		Hp -= damage;
        Debug.Log($"{attacker}");
        Debug.Log($"{Hp}");
        //Managers.Object.ShowDamageFont(CenterPosition,damage,transform,isCritical : false);
		if (Hp <= 0)
		{
			Hp = 0;
		}
    }

	protected virtual void OnDead()
	{

	}
    
}
