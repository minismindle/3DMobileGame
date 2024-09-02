using Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.ResourceManagement.ResourceProviders.Simulation;
using static Define;

public class PlayerController : CreatureController
{
    Vector3 _moveDir = Vector3.zero;
    Vector3 _shootDir = Vector3.zero;

    public UI_Inventory Inventory;

    [SerializeField]
    Transform _shootPos;
    [SerializeField]
    Transform _throwPos;
    [SerializeField]
    AmmoController _ammo;

    public virtual AmmoController Ammo {  get { return _ammo; } set { _ammo = value; } }
    public override int HP 
    { get {return base.HP; } set { base.HP = value; Managers.UI.GetSceneUI<UI_GameScene>().SetPlayerHP(HP); } }
    public override int MaxHP 
    { get { return base.MaxHP; } set { base.MaxHP = value; Managers.UI.GetSceneUI<UI_GameScene>().SetPlayerMaxHP(MaxHP); } }
    PlayerWeaponType PlayerWeaponType;
    RaycastHit slopeHit;


    public Vector3 MoveDir
    {
        get { return _moveDir; }
        set { _moveDir = value.normalized; }
    }
    public override bool Init()
    {
        base.Init();
        _rigid = GetComponent<Rigidbody>();
        _animator = GetComponentInChildren<Animator>();
        _collider = GetComponent<CapsuleCollider>();
        _meshrenderers = GetComponentsInChildren<MeshRenderer>();
        _rigid.velocity = Vector3.zero;
        Managers.Game.OnMoveDirChanged += HandleOnMoveDirChanged;
        ObjectType = ObjectType.Player;
        CreatureState = CreatureState.Idle;
        PlayerWeaponType = PlayerWeaponType.None;
        MaxHP = 1000;
        HP = MaxHP;
        return true;
    }

    public void Resurrection()
    {
        MeleeWeapon.gameObject.SetActive(false);
        ManualWeapon.gameObject.SetActive(false);
        AutoWeapon.gameObject.SetActive(false);
        Grenade.gameObject.SetActive(false);
        Managers.Game.KillCount = 0;
        ChangeColor(Color.white);
        HP = MaxHP;
    }
    void OnDestroy()
    {
        if (Managers.Game != null)
        {
            Managers.Game.OnMoveDirChanged -= HandleOnMoveDirChanged;
        }
    }
    void HandleOnMoveDirChanged(Vector3 dir)
    {
        _moveDir = dir;
    }

    #region PlayerAnimation
    public override void UpdateAnimation()
    {
        switch (CreatureState)
        {
            case Define.CreatureState.Moving:
                _animator.SetBool("IsRun", true);
                break;
            case Define.CreatureState.Idle:
                _animator.SetBool("IsRun", false);
                break;
            case Define.CreatureState.Land:
                _animator.SetBool("IsJump", false);
                break;
            case Define.CreatureState.Jump:
                _animator.SetBool("IsJump", true);
                _animator.SetTrigger("DoJump");
                break;
            case Define.CreatureState.Dodge:
                _animator.SetTrigger("DoDodge");
                break;
            case Define.CreatureState.Swap:
                _animator.SetTrigger("DoSwap");
                break;
            case Define.CreatureState.Swing:
                _animator.SetTrigger("DoSwing");
                break;
            case Define.CreatureState.Shot:
                _animator.SetTrigger("DoShot");
                break;
            case Define.CreatureState.Reload:
                _animator.SetTrigger("DoReload");
                break;
            case Define.CreatureState.Throw:
                _animator.SetTrigger("DoThrow");
                break;
            case Define.CreatureState.Dead:
                _animator.SetTrigger("DoDie");
                break;
        }
    }
    #endregion
    public override void SetInfo(int templateID)
    {
    }
    public override void SetInfoInit(int templateID)
    {
        ObjectType = ObjectType.Player;
        CreatureState = CreatureState.Idle;
    }
    void FixedUpdate()
    {
        if (CreatureState == CreatureState.Dead)
            return;
        TurnPlayer();
        MovePlayer();
    }
    public bool IsOnSlope()
    {
        int mask = 1 << (LayerMask.NameToLayer("Floor"));

        Ray ray = new Ray(transform.position + transform.up, Vector3.down);

        if (Physics.Raycast(ray, out slopeHit, 2f, mask))
        {
            var angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle != 0f && angle < 45;
        }
        return false;
    }
    public void ClickJumpButton()
    {
        if (CreatureState == CreatureState.Idle)
            JumpPlayer();
        else if (CreatureState == CreatureState.Moving)
            DivePlayer();
    }
    public void ClickAttackButton()
    {
        switch (PlayerWeaponType)
        {
            case PlayerWeaponType.None:
                break;
            case PlayerWeaponType.Melee:
                SwingPlayer();
                break;
            case PlayerWeaponType.Manual:
                ShotPlayer();
                break;
            case PlayerWeaponType.Auto:
                ShotPlayer();
                break;  
            case PlayerWeaponType.Grenade:
                ThrowPlayer();
                break;
        }
    }
    public void StopAttack()
    {
        switch(PlayerWeaponType)
        {
            case PlayerWeaponType.None:
                break;
            case PlayerWeaponType.Auto:
                StopAutoAttack();
                break;
        }
    }
    void MovePlayer()
    {
        if (CreatureState != CreatureState.Idle && CreatureState != CreatureState.Moving)
            return;

        if (MoveDir == Vector3.zero) CreatureState = CreatureState.Idle;
        else CreatureState = CreatureState.Moving;

        if (IsOnSlope())
        {
            _moveDir = Vector3.ProjectOnPlane(_moveDir, slopeHit.normal).normalized;
        }

        Vector3 dir = _moveDir * _speed * Time.deltaTime;

        transform.position += dir;

        FreezeVelocity();
    }
    void FreezeVelocity()
    {
        _rigid.velocity = Vector3.zero;
        _rigid.angularVelocity = Vector3.zero;
    }
    void TurnPlayer()
    {
        if (CreatureState != CreatureState.Idle && CreatureState != CreatureState.Moving && CreatureState != Define.CreatureState.Shot)
            return;
        transform.LookAt(transform.position + _moveDir);
    }
    public void EquipItem(ItemData itemData,int count)
    {
        switch (itemData.ItemType)
        {
            case ItemType.Weapon:
                EquipWeapon(itemData);
                break;
            case ItemType.Grenade:
                EquipGrenade(itemData,count);
                break;
            case ItemType.Consumable:
                EquipConsumable(itemData,count);  
                break;
        }
    }
    void EquipWeapon(ItemData itemData)
    {
        switch (itemData.WeaponType)
        {
            case WeaponType.Melee:
                MeleeWeapon.SetInfo(this, itemData);
                Managers.UI.GetSceneUI<UI_GameScene>().SetMeleeWeaponSlot(itemData.Image);
                Inventory.SetMeleeWeaponSlot(itemData.Image);
                break;
            case WeaponType.Manual:
                ManualWeapon.SetInfo(this, itemData);
                Managers.UI.GetSceneUI<UI_GameScene>().SetManualWeaponSlot(itemData.Image);
                Inventory.SetManualWeaponSlot(itemData.Image);
                break;
            case WeaponType.Auto:
                AutoWeapon.SetInfo( this, itemData);
                Managers.UI.GetSceneUI<UI_GameScene>().SetAutoWeaponSlot(itemData.Image);
                Inventory.SetAutoWeaponSlot(itemData.Image);
                break;
        }
    }
    public void EquipGrenade(ItemData itemData,int count)
    {
        Grenade.SetInfo(this,itemData,count);
        Managers.UI.GetSceneUI<UI_GameScene>().SetGrenadeSlot(itemData.Image);
        Inventory.SetGrenadeSlot(itemData.Image);
    }
    public void EquipConsumable(ItemData itemData,int count)
    {
        Consumable.SetInfo(this,itemData,count); 
        Managers.UI.GetSceneUI<UI_GameScene>().SetConsumableSlot(itemData.Image);
        Inventory.SetConsumableSlot(itemData.Image);
    }
    public void ReturnMeleeWeaponToInventory()
    {
        if (!MeleeWeapon.Equip)
            return;
        Inventory.ClearMeleeWeaponSlot();
        Inventory.InsertItem(MeleeWeapon.ItemData,1);
        MeleeWeapon.Clear();
        Managers.UI.GetSceneUI<UI_GameScene>().ClearMeleeWeaponSlot();
        Managers.UI.GetSceneUI<UI_GameScene>().SetNoneWeaponAmmoCount();
        PlayerWeaponType = PlayerWeaponType.None;
    }
    public void ReturnManualWeaponToInventory()
    {
        if(!ManualWeapon.Equip) 
            return;
        Ammo.Count += ManualWeapon.Ammo;
        Inventory.ReturnItem(Ammo.ItemData, ManualWeapon.Ammo);
        Inventory.ClearManualWeaponSlot();
        Inventory.InsertItem(ManualWeapon.ItemData, 1);
        ManualWeapon.Clear();
        Managers.UI.GetSceneUI<UI_GameScene>().ClearManualWeaponSlot();
        Managers.UI.GetSceneUI<UI_GameScene>().SetNoneWeaponAmmoCount();
        PlayerWeaponType = PlayerWeaponType.None;
    }
    public void ReturnAutoWeaponToInventory()
    {
        if (!AutoWeapon.Equip)
            return;
        Ammo.Count += AutoWeapon.Ammo;
        Inventory.ReturnItem(Ammo.ItemData, AutoWeapon.Ammo);
        Inventory.ClearAutoWeaponSlot();    
        Inventory.InsertItem(AutoWeapon.ItemData, 1);
        AutoWeapon.Clear();
        Managers.UI.GetSceneUI<UI_GameScene>().ClearAutoWeaponSlot();
        Managers.UI.GetSceneUI<UI_GameScene>().SetNoneWeaponAmmoCount();
        PlayerWeaponType = PlayerWeaponType.None;
    }
    public void ReturnGrenadeToInventory()
    {
        if(!Grenade.Equip) 
            return;
        ItemData itemData = Grenade.ItemData;
        int count = Grenade.Count;
        Grenade.Clear();
        Inventory.ClearGrenadeSlot();
        Inventory.InsertItem(itemData, count);
        Managers.UI.GetSceneUI<UI_GameScene>().ClearGrenadeSlot();
    }
    public void ReturnConsumableToInventory()
    {
        if(!Consumable.Equip) 
            return;
        ItemData itemData = Consumable.ItemData;
        int count = Consumable.Count;
        Consumable.Clear();
        Inventory.ClearConsumableSlot();    
        Inventory.InsertItem(itemData, count);
        Managers.UI.GetSceneUI<UI_GameScene>().ClearConsumableSlot();
    }
    public void JumpPlayer()
    {
        if (CreatureState != CreatureState.Idle)
            return;

        CreatureState = CreatureState.Jump;

        _rigid.AddForce((Vector3.up + MoveDir) * 30f, ForceMode.Impulse);
    }
    public void DivePlayer()
    {

        if (CreatureState != CreatureState.Moving)
            return;

        CreatureState = CreatureState.Dodge;

        _rigid.AddForce(MoveDir * 30f, ForceMode.Impulse);

        SetAnimationDelay(0.8f);
    }
    public void SwingPlayer()
    {

        if (CreatureState != CreatureState.Idle)
            return;
        CreatureState = CreatureState.Swing;

        MeleeWeapon.Use();

        SetAnimationDelay(AttackCoolTime);
    }
    public void ShotPlayer()
    {
        if (CreatureState != CreatureState.Idle)
            return;
        if (CreatureState == CreatureState.Shot)
            return;
        if (CreatureState == CreatureState.Reload)
            return;
        if (PlayerWeaponType == PlayerWeaponType.Manual)
        {
            if (ManualWeapon.Ammo == 0)
            {
                ReloadPlayer(); 
                return;
            }
            CreatureState = CreatureState.Shot;
            ManualWeapon.Use(this, _shootPos.position, transform.forward, transform.rotation, "Bullet_HandGun");
            SetAnimationDelay(0.1f);
        }
        else if (PlayerWeaponType == PlayerWeaponType.Auto)
        {
            StartAutoAttack(0.1f);
        }
    }
    public void SwapPlayer()
    {
        if (CreatureState != CreatureState.Idle)
            return;
        
        CreatureState = CreatureState.Swap;

        SetAnimationDelay(0.4f);
    }
    public void SwapToMeleeWeapon()
    {
        if (CreatureState != CreatureState.Idle)
            return;
        if (PlayerWeaponType == PlayerWeaponType.Melee)
            return;
        if (MeleeWeapon.Weapon == null)
            return;
        SwapPlayer();
        PlayerWeaponType = PlayerWeaponType.Melee;
        MeleeWeapon.gameObject.SetActive(true);
        ManualWeapon.gameObject.SetActive(false);
        AutoWeapon.gameObject.SetActive(false);
        Grenade.gameObject.SetActive(false);
        Managers.UI.GetSceneUI<UI_GameScene>().SetNoneWeaponAmmoCount();
        AttackCoolTime = MeleeWeapon.CoolTime;
    }
    public void SwapToManualWeapon()
    {
        if (sceneType != Scene.GameScene)
            return;
        if (CreatureState != CreatureState.Idle)
            return;
        if (PlayerWeaponType == PlayerWeaponType.Manual)
            return;
        if (ManualWeapon.Weapon == null)
            return;
        SwapPlayer();
        PlayerWeaponType = PlayerWeaponType.Manual;
        MeleeWeapon.gameObject.SetActive(false);
        ManualWeapon.gameObject.SetActive(true);
        AutoWeapon.gameObject.SetActive(false);
        Grenade.gameObject.SetActive(false);
        Managers.UI.GetSceneUI<UI_GameScene>().SetAmmoCount(ManualWeapon.Ammo);
        AttackCoolTime = ManualWeapon.CoolTime;
    }
    public void SwapToAutoWeapon()
    {
        if (CreatureState != CreatureState.Idle)
            return;
        if (PlayerWeaponType == PlayerWeaponType.Auto)
            return;
        if(AutoWeapon.Weapon == null) 
            return;
        SwapPlayer();
        PlayerWeaponType = PlayerWeaponType.Auto;
        MeleeWeapon.gameObject.SetActive(false);
        ManualWeapon.gameObject.SetActive(false);
        AutoWeapon.gameObject.SetActive(true);
        Grenade.gameObject.SetActive(false);
        Managers.UI.GetSceneUI<UI_GameScene>().SetAmmoCount(AutoWeapon.Ammo);
        AttackCoolTime = AutoWeapon.CoolTime;
    }
    public void SwapToGrenade()
    {
        if (CreatureState != CreatureState.Idle)
            return;
        if (PlayerWeaponType == PlayerWeaponType.Grenade)
            return;
        if (Grenade.Weapon == null)
            return;
        SwapPlayer();
        PlayerWeaponType = PlayerWeaponType.Grenade;
        MeleeWeapon.gameObject.SetActive(false);
        ManualWeapon.gameObject.SetActive(false);
        AutoWeapon.gameObject.SetActive(false);
        Grenade.gameObject.SetActive(true);
    }
    public void ThrowPlayer()
    {
        if (Grenade.Count == 0)
            return;
        if (CreatureState != CreatureState.Idle)
            return;
        if (Grenade.Weapon == null)
            return;
        CreatureState = CreatureState.Throw;

        Grenade.Use(this, _throwPos.position, transform.forward);

        SetAnimationDelay(0.6f);
    }
    public void ReloadPlayer()
    {
        if (CreatureState != CreatureState.Idle && CreatureState != CreatureState.Shot)
            return;
        if (PlayerWeaponType != PlayerWeaponType.Manual && PlayerWeaponType != PlayerWeaponType.Auto)
            return;
        if (Ammo.Count== 0)
            return;

        CreatureState = CreatureState.Reload;

        SetAnimationDelay(2.5f);

        int ammoCount;

        switch(PlayerWeaponType)
        {
            case PlayerWeaponType.Manual:
                ammoCount = Mathf.Min(ManualWeapon.MaxAmmo - ManualWeapon.Ammo, Ammo.Count);
                ManualWeapon.Ammo += ammoCount;
                Ammo.Count -= ammoCount;
                Managers.UI.GetSceneUI<UI_GameScene>().SetAmmoCount(ManualWeapon.Ammo);
                Inventory.UseItem(Ammo.ItemData.Name, ammoCount);
                break;
            case PlayerWeaponType.Auto:
                ammoCount = Mathf.Min(AutoWeapon.MaxAmmo - AutoWeapon.Ammo, Ammo.Count);
                AutoWeapon.Ammo += ammoCount;
                Ammo.Count -= ammoCount;
                Managers.UI.GetSceneUI<UI_GameScene>().SetAmmoCount(AutoWeapon.Ammo);
                Inventory.UseItem(Ammo.ItemData.Name, ammoCount);
                break;
        }
    }
    public void UsePotion()
    {
        if (Potion == 0)
            return;

        Consumable.Count -= 1;
    }
    public override void OnDamaged(BaseController attacker, int damage)
    {
        if (CreatureState == CreatureState.Dead)
            return;

        base.OnDamaged(attacker, 10);
    }
    protected override void OnDead()
    {
        if (CreatureState == CreatureState.Dead)
            return;
        StopAllCoroutines();
        CreatureState = CreatureState.Dead;
        SetAnimationDelay(1.25f);
    }
    private void OnCollisionEnter(Collision other)
    {
        switch (other.gameObject.tag)
        {
            case "Floor":
                if (CreatureState != CreatureState.Jump)
                    return;
                CreatureState = CreatureState.Land;
                SetAnimationDelay(0.5f);
                break;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "Coin":
                CoinController _coin = other.gameObject.GetComponent<CoinController>();
                Gold += _coin.GetCoin();
                Managers.Object.Despawn(_coin);
                break;
            case "Shop":
                Managers.UI.ShowPopup<UI_Shop>();
                break;
            case "Quest":
                Managers.Scene.CurrentScene.gameObject.GetComponent<GameScene>().OnStage();
                break;
            default:
                break;
        }
    }
    
    #region WaitAnimation
    Coroutine _coWaitAnimation;
    IEnumerator CoWaitAnimation(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (CreatureState == CreatureState.Dead)
        {
            Managers.UI.ShowPopup<UI_GameResultPopup>();
        }
        CreatureState = CreatureState.Idle;
    }
    void SetAnimationDelay(float delay)
    {
        if (_coWaitAnimation != null)
            _coWaitAnimation = null;
        _coWaitAnimation = StartCoroutine(CoWaitAnimation(delay));
    }
    #endregion

    #region AutoAttack
    Coroutine _coAutoAttack;

    IEnumerator CoAutoAttack(float delay)
    {
        yield return new WaitForSeconds(delay);
        while (true)
        {
            if (AutoWeapon.Ammo == 0)
            {
                ReloadPlayer();
                StopAutoAttack();
                break;
            }
            CreatureState = CreatureState.Shot;
            AutoWeapon.Use(this, _shootPos.position, transform.forward, transform.rotation, "Bullet_SubMachineGun");
            yield return new WaitForSeconds(delay);
        }
    }
    void StartAutoAttack(float delay)
    {
        if(_coAutoAttack != null)   
            _coAutoAttack = null;
        _coAutoAttack = StartCoroutine(CoAutoAttack(delay));
    }
    void StopAutoAttack()
    {
        if (_coAutoAttack != null)
            StopCoroutine(_coAutoAttack);
        _coAutoAttack = null;
        if(CreatureState == CreatureState.Shot)    
            CreatureState = CreatureState.Idle;
    }
    #endregion

    #region DotDamage
    Coroutine _coDotDamage;
    public IEnumerator CoStartDotDamage(BaseController attacker, int damage)
    {
        OnDamaged(attacker, damage);
        ChangeColor(Color.red);
        yield return new WaitForSeconds(0.2f);
        ChangeColor(Color.white);
        _coDotDamage = null;
    }
    public void OnDotDamage(BaseController attacker, int damage)
    {
        if (_coDotDamage != null)
            StopCoroutine(_coDotDamage);
        _coDotDamage = StartCoroutine(CoStartDotDamage(attacker, damage));
    }
    void ChangeColor(Color color)
    {
        foreach (MeshRenderer meshrenderer in _meshrenderers)
        {
            if (meshrenderer.gameObject.tag == "Weapon")
                continue;
            Material material = meshrenderer.material;
            material.color = color;
        }
    }
    #endregion
}
