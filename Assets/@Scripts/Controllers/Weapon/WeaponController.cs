using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : BaseController
{
    public Define.WeaponType WeaponType;
    public Define.WeaponName WeaponName;
    public BoxCollider _collider;
    public TrailRenderer _trailRenderer;
    public CreatureController _owner;
    public virtual int Damage {  get; set; }
    public virtual float CoolTime { get; set; } 
    public virtual string ProjectileName {  get; set; }  
    public override bool Init()
    {

        base.Init();
        return true;
    }
}
