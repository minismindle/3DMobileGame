using Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using static Define;
using static UnityEngine.GraphicsBuffer;


public class MeleeWeaponController : WeaponController
{
    public event Action OnMeleeWeaponClear;
    public event Action<CreatureController, ItemData> OnSetMeleeWeapon;
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
        if (base.Init() == false)
            return false;
        
        return true;
    }
    protected override void SubScribe()
    {
        Managers.Game.Player.OnSetMeleeWeapon -= SetInfo;
        Managers.Game.Player.OnSetMeleeWeapon += SetInfo;
    }
    public override void SetInfo( CreatureController owner,ItemData itemData) 
    {
        base.SetInfo(owner, itemData);
        WeaponType = WeaponType.Melee;
        _collider = _weapon.GetComponent<BoxCollider>();
        _trailRenderer = _weapon.GetComponentInChildren<TrailRenderer>();
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
        OnMeleeWeaponClear?.Invoke();
    }
}
