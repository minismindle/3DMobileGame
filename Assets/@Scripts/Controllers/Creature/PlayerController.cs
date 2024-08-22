using Data;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static Define;

public class PlayerController : CreatureController
{
    Vector3 _moveDir = Vector3.zero;
    Vector3 _shootDir = Vector3.zero;

    public UI_Inventory Inventory;

    public Transform _shootPos;
    public Transform _ThrowPos;

    PlayerWeaponType PlayerWeaponType;
    RaycastHit slopeHit;
    //public Data.LevelData LevelData { get; set; }

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
        Managers.Game.OnMoveDirChanged += HandleOnMoveDirChanged;
        ObjectType = ObjectType.Player;
        CreatureState = CreatureState.Idle;
        PlayerWeaponType = PlayerWeaponType.None;
        _rigid.velocity = Vector3.zero;
        Hp = MaxHp;
        return true;
    }

    void OnDestroy()
    {
        if (Managers.Game != null)
            Managers.Game.OnMoveDirChanged -= HandleOnMoveDirChanged;
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
        }
    }

    #endregion
    public override void SetInfo(int templateID)
    {
    }
    public override void SetInfoInit(int templateID)
    {
        //CreatureData = Managers.Data.CreatureDic[templateID];
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
    public void EquipWeapon(ItemData itemData)
    {
        switch (itemData.WeaponType) 
        {
            case WeaponType.Melee:
                _meleeWeapon.SetInfo(itemData.Name, this,itemData);
                Managers.UI.GetSceneUI<UI_GameScene>().SetMeleeWeaponSlot(itemData.Image);
                Inventory.SetMeleeWeaponSlot(itemData.Image);
                break;
            case WeaponType.Manual:
                _manualWeapon.SetInfo(itemData.Name,this,itemData);
                Managers.UI.GetSceneUI<UI_GameScene>().SetManualWeaponSlot(itemData.Image);
                Inventory.SetManualWeaponSlot(itemData.Image);
                break;
            case WeaponType.Auto:
                _autoWeapon.SetInfo(itemData.Name,this,itemData );
                Managers.UI.GetSceneUI<UI_GameScene>().SetAutoWeaponSlot(itemData.Image);
                Inventory.SetAutoWeaponSlot(itemData.Image);
                break;
        }
    }
    public void ReturnMeleeWeaponToInventory()
    {
        if (!_meleeWeapon._equip)
            return;
        _meleeWeapon.gameObject.SetActive(false);
        Managers.UI.GetSceneUI<UI_GameScene>().SetMeleeWeaponSlot("");
        Inventory.SetMeleeWeaponSlot("");
        Inventory.InsertItem(_meleeWeapon.itemData,1);
        _meleeWeapon.Clear();
    }
    public void ReturnManualWeaponToInventory()
    {
        if(!_manualWeapon._equip) 
            return;
        _manualWeapon.gameObject.SetActive(false);
        Managers.UI.GetSceneUI<UI_GameScene>().SetManualWeaponSlot("");
        Inventory.SetManualWeaponSlot("");
        Inventory.InsertItem(_manualWeapon.itemData, 1);
        _manualWeapon.Clear();
    }
    public void ReturnAutoWeaponToInventory()
    {
        if (!_autoWeapon._equip)
            return;
        _autoWeapon.gameObject.SetActive(false);
        Managers.UI.GetSceneUI<UI_GameScene>().SetAutoWeaponSlot("");
        Inventory.SetAutoWeaponSlot("");
        Inventory.InsertItem(_autoWeapon.itemData, 1);
        _autoWeapon.Clear();
    }
    public void EquipGrenade(string GrenadeName, string imageName)
    {
        _grenade.SetInfo(GrenadeName);
        Managers.UI.GetSceneUI<UI_GameScene>().SetGrenadeSlot(imageName);
        Inventory.SetGrenadeSlot(imageName, Grenade);
    }
    public void EquipPotion(string PotionName, string imageName)
    {
        Managers.UI.GetSceneUI<UI_GameScene>().SetPotionSlot(imageName);
        Inventory.SetPotionSlot(imageName,Potion);
    }
    public void JumpPlayer()
    {
        if (sceneType != Scene.GameScene)
            return;
        if (CreatureState != CreatureState.Idle)
            return;

        CreatureState = CreatureState.Jump;

        _rigid.AddForce((Vector3.up + MoveDir) * 30f, ForceMode.Impulse);
    }
    public void DivePlayer()
    {
        if (sceneType != Scene.GameScene)
            return;
        if (CreatureState != CreatureState.Moving)
            return;

        CreatureState = CreatureState.Dodge;

        _rigid.AddForce(MoveDir * 30f, ForceMode.Impulse);

        SetAnimationDelay(0.8f);
    }
    public void SwingPlayer()
    {
        if (sceneType != Scene.GameScene)
            return;
        if (CreatureState != CreatureState.Idle)
            return;
        CreatureState = CreatureState.Swing;

        _meleeWeapon.Use();

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
            if (_manualWeapon.ammo == 0)
            {
                ReloadPlayer(); 
                return;
            }
            CreatureState = CreatureState.Shot;
            _manualWeapon.Use(this, _shootPos.position, transform.forward, transform.rotation, "Bullet_HandGun");
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
        if (sceneType != Scene.GameScene)
            return;
        if (CreatureState != CreatureState.Idle)
            return;
        if (PlayerWeaponType == PlayerWeaponType.Melee)
            return;
        if (_meleeWeapon._weapon == null)
            return;
        SwapPlayer();
        PlayerWeaponType = PlayerWeaponType.Melee;
        _meleeWeapon.gameObject.SetActive(true);
        _manualWeapon.gameObject.SetActive(false);
        _autoWeapon.gameObject.SetActive(false);
        _grenade.gameObject.SetActive(false);
        AttackCoolTime = _meleeWeapon.CoolTime;
    }
    public void SwapToManualWeapon()
    {
        if (sceneType != Scene.GameScene)
            return;
        if (CreatureState != CreatureState.Idle)
            return;
        if (PlayerWeaponType == PlayerWeaponType.Manual)
            return;
        if (_manualWeapon._weapon == null)
            return;
        SwapPlayer();
        PlayerWeaponType = PlayerWeaponType.Manual;
        _meleeWeapon.gameObject.SetActive(false);
        _manualWeapon.gameObject.SetActive(true);
        _autoWeapon.gameObject.SetActive(false);
        _grenade.gameObject.SetActive(false);
        AttackCoolTime = _manualWeapon.CoolTime;
    }
    public void SwapToAutoWeapon()
    {
        if (sceneType != Scene.GameScene)
            return;
        if (CreatureState != CreatureState.Idle)
            return;
        if (PlayerWeaponType == PlayerWeaponType.Auto)
            return;
        if(_autoWeapon._weapon == null) 
            return;
        SwapPlayer();
        PlayerWeaponType = PlayerWeaponType.Auto;
        _meleeWeapon.gameObject.SetActive(false);
        _manualWeapon.gameObject.SetActive(false);
        _autoWeapon.gameObject.SetActive(true);
        _grenade.gameObject.SetActive(false);
        AttackCoolTime = _autoWeapon.CoolTime;
    }
    public void SwapToGrenade()
    {
        if (sceneType != Scene.GameScene)
            return;
        if (CreatureState != CreatureState.Idle)
            return;
        if (PlayerWeaponType == PlayerWeaponType.Grenade)
            return;
        SwapPlayer();
        PlayerWeaponType = PlayerWeaponType.Grenade;
        _meleeWeapon.gameObject.SetActive(false);
        _manualWeapon.gameObject.SetActive(false);
        _autoWeapon.gameObject.SetActive(false);
        _grenade.gameObject.SetActive(true);
    }
    public void ThrowPlayer()
    {
        if (Grenade == 0)
            return;
        if (CreatureState != CreatureState.Idle)
            return;

        CreatureState = CreatureState.Throw;

        _grenade.Use(this, _ThrowPos.position, transform.forward);

        SetAnimationDelay(0.6f);
    }
    public void ReloadPlayer()
    {
        if (CreatureState != CreatureState.Idle && CreatureState != CreatureState.Shot)
            return;
        if (PlayerWeaponType != PlayerWeaponType.Manual && PlayerWeaponType != PlayerWeaponType.Auto)
            return;

        CreatureState = CreatureState.Reload;

        SetAnimationDelay(2.5f);

        int ammoCount;

        if (PlayerWeaponType == PlayerWeaponType.Manual)
        {
            ammoCount = Mathf.Min(_manualWeapon.maxAmmo - _manualWeapon.ammo, Ammo);
            _manualWeapon.ammo += ammoCount;
            Managers.Game.TotalAmmo -= ammoCount;    
        }
        else if(PlayerWeaponType == PlayerWeaponType.Auto) 
        {
            ammoCount = Mathf.Min(_autoWeapon.maxAmmo - _autoWeapon.ammo,Ammo);
            _autoWeapon.ammo += ammoCount;
            Managers.Game.TotalAmmo -= ammoCount;
        }
    }
    public void UsePotion()
    {
        if (Potion == 0)
            return;

        Managers.Game.Potion -= 1;
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

        CreatureState = CreatureState.Dead;
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
            if (_autoWeapon.ammo == 0)
            {
                ReloadPlayer();
                StopAutoAttack();
                break;
            }
            CreatureState = CreatureState.Shot;
            _autoWeapon.Use(this, _shootPos.position, transform.forward, transform.rotation, "Bullet_SubMachineGun");
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
