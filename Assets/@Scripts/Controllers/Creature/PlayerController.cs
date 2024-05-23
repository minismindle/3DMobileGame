using Data;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static Define;

public class PlayerController : CreatureController
{
    Vector3 _moveDir = Vector3.zero;
    Vector3 _shootDir = Vector3.zero;

    public Transform _shootPos;
    public Transform _ThrowPos;
    public GrenadeController _grenade;

    PlayerWeaponType PlayerWeaponType;
    RaycastHit slopeHit;
    float clickTime = 0f;

    public Data.LevelData LevelData { get; set; }

    public Vector3 MoveDir
    {
        get { return _moveDir; }
        set { _moveDir = value.normalized; }
    }

    public override bool Init()
    {
        base.Init();
        Hp = 100;
        _rigid = GetComponent<Rigidbody>();
        _animator = GetComponentInChildren<Animator>();
        _collider = GetComponent<CapsuleCollider>();
        _meshrenderers = GetComponentsInChildren<MeshRenderer>();
        Managers.Game.OnMoveDirChanged += HandleOnMoveDirChanged;
        ObjectType = Define.ObjectType.Player;
        CreatureState = Define.CreatureState.Idle;
        this.PlayerWeaponType = PlayerWeaponType.Melee;
        _meleeWeapon.SetInfo("Hammer");
        AttackCoolTime = 0.6f;
        _rigid.velocity = Vector3.zero;
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
        Managers.UI.GetSceneUI<UI_GameScene>().SetCharaterLevelInfo(level);
    }
    public override void SetInfoInit(int templateID)
    {
        CreatureData = Managers.Data.CreatureDic[templateID];
        ObjectType = Define.ObjectType.Player;
        CreatureState = Define.CreatureState.Idle;

        Managers.UI.GetSceneUI<UI_GameScene>().SetCharaterLevelInfo(level);
    }
    void FixedUpdate()
    {
        if (CreatureState == Define.CreatureState.Dead)
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
        if (CreatureState == Define.CreatureState.Idle)
            JumpPlayer();
        else if (CreatureState == Define.CreatureState.Moving)
            DodgePlayer();
    }
    public void ClickAttackButton()
    {
        switch (this.PlayerWeaponType)
        {
            case PlayerWeaponType.None:
                break;
            case PlayerWeaponType.Melee:
                SwingPlayer();
                break;
            case PlayerWeaponType.Range:
                ShotPlayer();
                break;
        }
    }
    public void ClickSwapButton()
    {
        switch (this.PlayerWeaponType)
        {
            case PlayerWeaponType.None:
                break;
            case PlayerWeaponType.Melee:
                SwapToRangeWeapon();
                break;
            case PlayerWeaponType.Range:
                SwapToMeleeWeapon();
                break;
        }
    }
    void MovePlayer()
    {
        if (CreatureState != Define.CreatureState.Idle && CreatureState != Define.CreatureState.Moving)
            return;

        if (MoveDir == Vector3.zero) CreatureState = Define.CreatureState.Idle;
        else CreatureState = Define.CreatureState.Moving;

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
        if (CreatureState != Define.CreatureState.Idle && CreatureState != Define.CreatureState.Moving && CreatureState != Define.CreatureState.Shot)
            return;
        transform.LookAt(transform.position + _moveDir);
    }
    public void JumpPlayer()
    {
        if (CreatureState != Define.CreatureState.Idle)
            return;

        CreatureState = Define.CreatureState.Jump;

        _rigid.AddForce(Vector3.up * 30f, ForceMode.Impulse);
    }
    public void DodgePlayer()
    {
        if (CreatureState != Define.CreatureState.Moving)
            return;

        CreatureState = Define.CreatureState.Dodge;

        _rigid.AddForce(MoveDir * 30f, ForceMode.Impulse);

        SetAnimationDelay(0.8f);
    }
    public void SwingPlayer()
    {
        if (CreatureState != Define.CreatureState.Idle)
            return;
        CreatureState = Define.CreatureState.Swing;

        _meleeWeapon.Use();

        SetAnimationDelay(AttackCoolTime);
    }
    public void ShotPlayer()
    {
        if (CreatureState != Define.CreatureState.Idle)
            return;
        if (CreatureState == Define.CreatureState.Shot)
            return;

        CreatureState = Define.CreatureState.Shot;

        _rangeWeapon.Use(this, _shootPos.position, transform.forward, "Bullet_SubMachineGun");


        SetAnimationDelay(AttackCoolTime);
    }
    public void SwapPlayer()
    {
        if (CreatureState != Define.CreatureState.Idle)
            return;

        CreatureState = Define.CreatureState.Swap;

        SetAnimationDelay(0.4f);
    }
    public void SwapToMeleeWeapon()
    {
        if (CreatureState != Define.CreatureState.Idle)
            return;
        SwapPlayer();
        this.PlayerWeaponType = PlayerWeaponType.Melee;
        _meleeWeapon.gameObject.SetActive(true);
        _rangeWeapon.gameObject.SetActive(false);
        AttackCoolTime = _meleeWeapon.CoolTime;
    }
    public void SwapToRangeWeapon()
    {
        if (CreatureState != Define.CreatureState.Idle)
            return;
        SwapPlayer();
        this.PlayerWeaponType = PlayerWeaponType.Range;
        _meleeWeapon.gameObject.SetActive(false);
        _rangeWeapon.gameObject.SetActive(true);
        AttackCoolTime = _rangeWeapon.CoolTime;
    }
    public void ThrowPlayer()
    {
        if (CreatureState != Define.CreatureState.Idle)
            return;

        CreatureState = Define.CreatureState.Throw;

        NoneWeapon();

        _grenade.Use(this, _ThrowPos.position, transform.forward);


        SetAnimationDelay(0.6f);
    }
    void NoneWeapon()
    {
        _meleeWeapon.gameObject.SetActive(false);
        _rangeWeapon.gameObject.SetActive(false);
    }
    void UseMeleeWeapon()
    {
        _meleeWeapon.gameObject.SetActive(true);
        _rangeWeapon.gameObject.SetActive(false);
    }
    void UseRangeWeapon()
    {
        _meleeWeapon.gameObject.SetActive(false);
        _rangeWeapon.gameObject.SetActive(true);
    }
    public void ReloadPlayer()
    {
        if (CreatureState != Define.CreatureState.Idle)
            return;

        CreatureState = Define.CreatureState.Reload;

        SetAnimationDelay(2.7f);
    }
    public override void OnDamaged(BaseController attacker, int damage)
    {
        if (CreatureState == Define.CreatureState.Dead)
            return;

        base.OnDamaged(attacker, 10);
    }
    protected override void OnDead()
    {
        if (CreatureState == Define.CreatureState.Dead)
            return;

        CreatureState = Define.CreatureState.Dead;
    }
    private void OnCollisionEnter(Collision other)
    {
        switch (other.gameObject.tag)
        {
            case "Floor":
                if (CreatureState != Define.CreatureState.Jump)
                    return;
                CreatureState = Define.CreatureState.Land;
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
        ThrowAnimation();
        CreatureState = Define.CreatureState.Idle;
    }
    void ThrowAnimation()
    {
        if (CreatureState != Define.CreatureState.Throw)
            return;
        if (PlayerWeaponType == Define.PlayerWeaponType.Melee)
            UseMeleeWeapon();
        else if (PlayerWeaponType == Define.PlayerWeaponType.Range)
            UseRangeWeapon();

    }
    void SetAnimationDelay(float delay)
    {
        if (_coWaitAnimation != null)
            _coWaitAnimation = null;
        _coWaitAnimation = StartCoroutine(CoWaitAnimation(delay));
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
