using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

public class RangeWeaponController : WeaponController
{
    GameObject _weapon;

    public override bool Init()
    {
        base.Init();
        SetInfo("SubMachineGun");
        return true;
    }
    public void Use(CreatureController owner, Vector3 startPos, Vector3 dir, string prefabName)
    {
        Attack(owner, startPos, dir, prefabName);
    }

    #region Attack
    public Coroutine _coAttack;
    IEnumerator ShotSubMachineGun(CreatureController owner, Vector3 startPos, Vector3 dir, string prefabName)
    {
        yield return new WaitForSeconds(0.1f);
        GenerateProjectile(owner, startPos, dir, prefabName);
        yield return new WaitForSeconds(CoolTime);
    }
    void Attack(CreatureController owner, Vector3 startPos, Vector3 dir, string prefabName)
    {
        if (_coAttack != null)
            _coAttack = null;
        _coAttack = StartCoroutine(ShotSubMachineGun(owner, startPos, dir, prefabName));
    }
    #endregion


    public void SetInfo(string WeaponName)
    {
        WeaponType = Define.WeaponType.Range;
        ObjectType = Define.ObjectType.Weapon;
        _weapon = this.transform.Find("SubMachineGun").gameObject;
        _owner = Managers.Game.Player;
        _weapon.SetActive(true);
        CoolTime = 0.1f;
    }
    void GenerateProjectile(CreatureController owner, Vector3 startPos, Vector3 dir, string prefabName)
    {
        ProjectileController pc = Managers.Object.Spawn<ProjectileController>(startPos, 0, prefabName);
        pc.SetInfo(owner, prefabName, startPos, dir);

    }
}
