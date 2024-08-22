using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : ItemController
{
    public Define.WeaponType WeaponType;
    public Define.WeaponName WeaponName;
    public BoxCollider _collider;
    public TrailRenderer _trailRenderer;
    public CreatureController _owner;
    public GameObject _weapon;
    public bool _equip;
    public virtual int Damage {  get; set; }
    public virtual float CoolTime { get; set; } 
    public virtual string ProjectileName {  get; set; }  
    public override bool Init()
    {
        base.Init();
        return true;
    }
    public void Clear()
    {
        _weapon = null; 
        itemData = null;    
        _equip = false;
    }
}
