using Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeController : WeaponController 
{
    public event Action<int> OnGrenadeCountChanged;
    public event Action OnGrenadeClear;
    public override int Count 
    { 
        get { return base.Count; }
        set 
        {
            base.Count = value;
            OnGrenadeCountChanged?.Invoke(value);
            if (Count == 0) { base.Clear(); OnGrenadeClear?.Invoke();}
        }
    }
    public override bool Init()
    {
        base.Init();
        return true;
    }
    protected override void SubScribe()
    {
        Managers.Game.Player.OnSetGrenade -= SetInfo;
        Managers.Game.Player.OnSetGrenade += SetInfo;
    }
    public void Use(CreatureController owner, Vector3 startPos, Vector3 dir)
    {
        Throwing(owner, startPos, dir);
    }
    #region Throwing
    public Coroutine _coAttack;
    IEnumerator ThrowGrenade(CreatureController owner, Vector3 startPos, Vector3 dir)
    {
        GenerateProjectile(owner, startPos, dir);
        Count -= 1;
        yield return new WaitForSeconds(CoolTime);
    }
    void Throwing(CreatureController owner, Vector3 startPos, Vector3 dir)
    {
        if (_coAttack != null)
            _coAttack = null;
        _coAttack = StartCoroutine(ThrowGrenade(owner, startPos, dir));
    }
    #endregion
    public void SetInfo(CreatureController owner,ItemData itemData,int count)
    {
        base.SetInfo(owner, itemData);
        ItemType = Define.ItemType.Grenade;
        WeaponType = Define.WeaponType.Grenade;
        ProjectileName = itemData.Name;
        CoolTime = 0.6f;
        Count = count;
    }
    void GenerateProjectile(CreatureController owner, Vector3 startPos, Vector3 dir)
    {
        ProjectileController pc = Managers.Object.Spawn<ProjectileController>(startPos, transform.rotation, 0, ProjectileName);
        pc.SetInfo(owner, ProjectileName, startPos, dir);
    }
    public override void Clear()
    {
        base.Clear();
        Count = 0;
    }
}
