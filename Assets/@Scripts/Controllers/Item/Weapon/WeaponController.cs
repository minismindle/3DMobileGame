using Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : ItemController
{
    public Define.WeaponType        WeaponType;
    public Define.WeaponName        WeaponName;
    protected BoxCollider           _collider;
    protected TrailRenderer         _trailRenderer;
    protected CreatureController    _owner;
    protected GameObject            _weapon;

    public virtual int Damage {  get; set; }
    public virtual float CoolTime { get; set; } 
    public virtual string ProjectileName {  get; set; }
    public virtual GameObject Weapon { get {return _weapon; } set {_weapon = value; } } 
    public override bool Init()
    {
        base.Init();
        return true;
    }
    public override void SetInfo(CreatureController owner, ItemData itemData)
    {
        base.SetInfo(owner, itemData);
        ObjectType = Define.ObjectType.Weapon;
        ItemData = itemData;
        _weapon = this.transform.Find(itemData.Name).gameObject;
        _weapon.SetActive(true);
        _owner = owner;
        _equip = true;
        gameObject.SetActive(false);
    }
    public override void Clear()
    {
        base.Clear();
        _weapon = null;
        gameObject.SetActive(false);
        Managers.Game.Player.PlayerWeaponType = Define.PlayerWeaponType.None;
    }
}
