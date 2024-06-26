using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;
using static UnityEngine.GraphicsBuffer;


public class MeleeWeaponController : WeaponController
{
    GameObject _weapon;

    public void Use(string weaponName)
    {
        Attack(weaponName);
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
    IEnumerator MonsterB()
    {
        _collider.enabled = true;
        yield return new WaitForSeconds(0.3f);
        _collider.enabled = false;
    }
    void Attack(string weaponName)
    {
        if (_coAttack != null)
            _coAttack = null;
        _coAttack = StartCoroutine(weaponName);
    }
    #endregion
    public override bool Init()
    {
        base.Init();
        return true;
    }
    public void SetInfo(string WeaponName) 
    {
        WeaponType = WeaponType.Melee;
        ObjectType = ObjectType.Weapon;
        _weapon = this.transform.Find(WeaponName).gameObject;
        _collider = _weapon.GetComponent<BoxCollider>();
        _trailRenderer = _weapon.GetComponentInChildren<TrailRenderer>();
        _owner = Managers.Game.Player;
        CoolTime = 0.6f;
    }
    private void OnTriggerEnter(Collider target)
    {
        switch(target.gameObject.tag)
        {
            case "Monster":
                PlayerAttackToMonster(target);
                break;
            case "Player":
                MonsterAttackToPlayer(target);
                break;
        }
    }

    void MonsterAttackToPlayer(Collider target)
    {
        PlayerController player = target.GetComponent<PlayerController>();
        if (player.IsValid() == false)
            return;
        if (this.IsValid() == false)
            return;
        if (player.CreatureState == CreatureState.Dead)
            return;

        player.OnDotDamage(_owner, 10);
    }
    void PlayerAttackToMonster(Collider target)
    {
        if (Managers.Game.Player.CreatureState != CreatureState.Swing)
            return;
        MonsterController monster = target.GetComponent<MonsterController>();
        if (monster.IsValid() == false)
            return;
        if (this.IsValid() == false)
            return;
        if (monster.CreatureState == CreatureState.Dead)
            return;
        monster.OnDotDamage(_owner, 10);
    }

}
