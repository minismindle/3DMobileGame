using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

public class AutoWeaponController : WeaponController
{
    public int maxAmmo = 40;
    public int ammo = 0;
    public override bool Init()
    {
        base.Init();
        maxAmmo = 40;
        ammo = 0;
        return true;
    }
    public void Use(CreatureController owner, Vector3 startPos, Vector3 dir, Quaternion rotation, string prefabName)
    {
        Attack(owner, startPos, dir, rotation, prefabName);
    }

    #region Attack
    public Coroutine _coAttack;
    IEnumerator CoSubMachineGun(CreatureController owner, Vector3 startPos, Vector3 dir, Quaternion rotation, string prefabName)
    {
        yield return new WaitForSeconds(0.1f);
        GenerateProjectile(owner, startPos, dir, rotation, prefabName);
        ammo -= 1;
        yield return new WaitForSeconds(CoolTime);
    }
    void Attack(CreatureController owner, Vector3 startPos, Vector3 dir, Quaternion rotation, string prefabName)
    {
        if (_coAttack != null)
            _coAttack = null;
        _coAttack = StartCoroutine(CoSubMachineGun(owner, startPos, dir, rotation, prefabName));
    }
    #endregion
    public void SetInfo(string WeaponName, CreatureController owner, ItemData itemData)
    {
        WeaponType = Define.WeaponType.Auto;
        ObjectType = Define.ObjectType.Weapon;
        this.itemData = itemData;   
        _weapon = this.transform.Find(WeaponName).gameObject;
        _owner = owner;
        _weapon.SetActive(true);
        _equip = true;
        CoolTime = 0.1f;
    }
    void GenerateProjectile(CreatureController owner, Vector3 startPos, Vector3 dir, Quaternion rotation, string prefabName)
    {
        ProjectileController pc = Managers.Object.Spawn<ProjectileController>(startPos, rotation, 0, prefabName);
        pc.SetInfo(owner, prefabName, startPos, dir);
    }
}
