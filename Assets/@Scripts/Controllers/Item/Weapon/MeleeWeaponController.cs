using Data;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using static Define;
using static UnityEngine.GraphicsBuffer;


public class MeleeWeaponController : WeaponController
{
    public void Use()
    {
        StartAttack(ItemData.Name);
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
    void StartAttack(string weaponName)
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
    public override void SetInfo( CreatureController owner,ItemData itemData) 
    {
        WeaponType = WeaponType.Melee;
        ObjectType = ObjectType.Weapon;
        ItemData = itemData;   
        _weapon = transform.Find(itemData.Name).gameObject;
        _collider = _weapon.GetComponent<BoxCollider>();
        _trailRenderer = _weapon.GetComponentInChildren<TrailRenderer>();
        _owner = owner;
        _equip = true;
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

        if (_owner.ObjectType != ObjectType.Monster)
            return;
        if (player.IsValid() == false)
            return;
        if (this.IsValid() == false)
            return;
        if (player.CreatureState == CreatureState.Dead)
            return;
        if (_owner.CreatureState == CreatureState.Dead)
            return;

        player.OnDotDamage(_owner, 10);
    }
    void PlayerAttackToMonster(Collider target)
    {
        MonsterController monster = target.GetComponent<MonsterController>();

        if (_owner.ObjectType != ObjectType.Player)
            return;
        if (monster.IsValid() == false)
            return;
        if (this.IsValid() == false)
            return;
        if (monster.CreatureState == CreatureState.Dead)
            return;
        if (_owner.CreatureState == CreatureState.Dead)
            return;

        monster.OnDotDamage(_owner, 50);
    }

    public override void Clear()
    {
        base.Clear();
        
    }
}
