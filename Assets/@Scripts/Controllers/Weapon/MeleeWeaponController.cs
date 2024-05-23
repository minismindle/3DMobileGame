using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;


public class MeleeWeaponController : WeaponController
{
    GameObject _weapon;
    public void Use()
    {
        Attack();
    }
    #region Attack
    Coroutine _coAttack;
    IEnumerator Hammer()
    {
        yield return new WaitForSeconds(0.2f);
        _collider.enabled = true;
        _trailRenderer.enabled = true;
        yield return new WaitForSeconds(0.3f);
        _collider.enabled = false;
        _trailRenderer.enabled = false;
    }
    void Attack()
    {
        if (_coAttack != null)
            _coAttack = null;
        _coAttack = StartCoroutine(Hammer());
    }
    #endregion
    public override bool Init()
    {
        base.Init();
        return true;
    }
    public void SetInfo(string WeaponName) 
    {
        WeaponType = Define.WeaponType.Melee;
        ObjectType = Define.ObjectType.Weapon;
        _weapon = this.transform.Find(WeaponName).gameObject;
        _collider = _weapon.GetComponent<BoxCollider>();
        _trailRenderer = _weapon.GetComponentInChildren<TrailRenderer>();
        _owner = Managers.Game.Player;
        CoolTime = 0.6f;
    }

    private void OnTriggerEnter(Collider target)
    {
        if (Managers.Game.Player.CreatureState != CreatureState.Swing)
            return;
        MonsterController monster = target.GetComponent<MonsterController>();
        if (monster.IsValid() == false)
            return;
        if(this.IsValid() == false) 
            return;
        monster.OnDotDamage(_owner, 10);
    }
}
