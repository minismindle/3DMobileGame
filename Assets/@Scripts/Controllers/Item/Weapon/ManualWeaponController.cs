using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

public class ManualWeaponController : WeaponController
{
    public virtual int MaxAmmo { get; set; } = 40;
    public virtual int Ammo { get; set; } = 0;
    public override bool Init()
    {
        base.Init();
        return true;
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
        if (owner.ObjectType == Define.ObjectType.Player)
        {
            Ammo -= 1;
            Managers.UI.GetSceneUI<UI_GameScene>().SetAmmoCount(Ammo);
        }
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
        WeaponType = Define.WeaponType.Manual;
        ObjectType = Define.ObjectType.Weapon;
        ItemData = itemData;   
        _weapon = this.transform.Find(itemData.Name).gameObject;
        _owner = owner;
        _weapon.SetActive(true);
        _equip = true;
        CoolTime = 0.1f;
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
    }
}
