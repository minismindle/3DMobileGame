using Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

public class ManualWeaponController : WeaponController
{
    public event Action OnManualWeaponClear;
    public virtual int MaxAmmo { get; set; } = 40;
    public virtual int Ammo { get; set; } = 0;
    public override bool Init()
    {
        base.Init();
        return true;
    }
    protected override void SubScribe()
    {
        Managers.Game.Player.OnSetManualWeapon -= SetInfo;
        Managers.Game.Player.OnSetManualWeapon += SetInfo;
    }
    public void Use(CreatureController owner, Vector3 startPos, Vector3 dir, Quaternion rotation, string prefabName)
    {
        Attack(owner, startPos, dir, rotation, prefabName);
    }
    #region Attack
    public Coroutine _coAttack;
    IEnumerator CoShootManualWeapon(CreatureController owner, Vector3 startPos, Vector3 dir, Quaternion rotation, string prefabName)
    {
        GenerateProjectile(owner, startPos, dir, rotation, prefabName);
        yield return null;
    }
    void Attack(CreatureController owner, Vector3 startPos, Vector3 dir, Quaternion rotation, string prefabName)
    {
        if (_coAttack != null)
            _coAttack = null;
        _coAttack = StartCoroutine(CoShootManualWeapon(owner, startPos, dir, rotation, prefabName));
    }
    #endregion
    public override void SetInfo(CreatureController owner,ItemData itemData)
    {
        base.SetInfo(owner, itemData);  
        WeaponType = Define.WeaponType.Manual;
    }
    void GenerateProjectile(CreatureController owner, Vector3 startPos, Vector3 dir, Quaternion rotation,string prefabName)
    {
        ProjectileController pc = Managers.Object.Spawn<ProjectileController>(startPos, rotation, 0, prefabName);
        pc.SetInfo(owner, prefabName, startPos, dir);
    }
    public override void Clear()
    {
        base.Clear();
        Ammo = 0;
        OnManualWeaponClear?.Invoke();
    }
}
