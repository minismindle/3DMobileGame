using Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

public class AutoWeaponController : WeaponController
{
    public event Action OnAutoWeaponClear;
    public virtual int MaxAmmo { get; set; } = 40;
    public virtual int Ammo { get; set; } = 0;
    public override bool Init()
    {
        base.Init();

        return true;
    }
    protected override void SubScribe()
    {
        Managers.Game.Player.OnSetAutoWeapon -= SetInfo;
        Managers.Game.Player.OnSetAutoWeapon += SetInfo;
    }
    public void Use(CreatureController owner, Vector3 startPos, Vector3 dir, Quaternion rotation, string prefabName)
    {
        Attack(owner, startPos, dir, rotation, prefabName);
    }

    #region Attack
    public Coroutine _coAttack;
    IEnumerator CoShootAutoWeapon(CreatureController owner, Vector3 startPos, Vector3 dir, Quaternion rotation, string prefabName)
    {
        yield return new WaitForSeconds(0.1f);
        GenerateProjectile(owner, startPos, dir, rotation, prefabName);
    }
    void Attack(CreatureController owner, Vector3 startPos, Vector3 dir, Quaternion rotation, string prefabName)
    {
        if (_coAttack != null)
            _coAttack = null;
        _coAttack = StartCoroutine(CoShootAutoWeapon(owner, startPos, dir, rotation, prefabName));
    }
    #endregion
    public override void SetInfo(CreatureController owner, ItemData itemData)
    {
        base.SetInfo(owner, itemData);
        WeaponType = Define.WeaponType.Auto;
    }
    void GenerateProjectile(CreatureController owner, Vector3 startPos, Vector3 dir, Quaternion rotation, string prefabName)
    {
        ProjectileController pc = Managers.Object.Spawn<ProjectileController>(startPos, rotation, 0, prefabName);
        pc.SetInfo(owner, prefabName, startPos, dir);
    }
    public override void Clear()
    {
        base.Clear();
        Ammo = 0;
        OnAutoWeaponClear?.Invoke();
    }
}
