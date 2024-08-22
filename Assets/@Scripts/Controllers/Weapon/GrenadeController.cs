using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeController : WeaponController
{
    public override bool Init()
    {
        base.Init();
        return true;    
    }
    public void Use(CreatureController owner,  Vector3 startPos, Vector3 dir)
    {
        Throwing(owner, startPos, dir);
    }
    #region Throwing
    public Coroutine _coAttack;
    IEnumerator ThrowGrenade(CreatureController owner,  Vector3 startPos, Vector3 dir)
    {
        GenerateProjectile(owner, startPos, dir);
        Managers.Game.Grenade -= 1;
        yield return new WaitForSeconds(CoolTime);
    }
    void Throwing(CreatureController owner, Vector3 startPos, Vector3 dir)
    {
        if (_coAttack != null)
            _coAttack = null;
        _coAttack = StartCoroutine(ThrowGrenade(owner, startPos, dir));
    }
    #endregion
    public GrenadeController SetInfo(string weaponName)
    {
        WeaponType = Define.WeaponType.Grenade;
        ProjectileName = weaponName;
        CoolTime = 0.6f;
        return this;
    }
    void GenerateProjectile(CreatureController owner, Vector3 startPos, Vector3 dir)
    {
        ProjectileController pc = Managers.Object.Spawn<ProjectileController>(startPos, transform.rotation,0,ProjectileName);
        pc.SetInfo(owner, ProjectileName,startPos, dir);
    }
}
