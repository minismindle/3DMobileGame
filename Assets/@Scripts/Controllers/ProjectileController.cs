using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Build.Pipeline.Tasks;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using static Define;
using static UnityEngine.GraphicsBuffer;


public class ProjectileController : BaseController
{
    public CreatureController _owner;
    string _prefabName;
    NavMeshAgent _nav;
	Vector3 _spawnPos;
    Vector3 _dir;
	Vector3 _target;
    Quaternion _projectiletotargetRotation;
    float angularPower = 2f;
    float scaleValue = 0.1f;
	Rigidbody _rigid;
    [SerializeField]
	TrailRenderer _trailRenderer;
    [SerializeField]
    MeshRenderer _meshRenderer;
    [SerializeField]
    ParticleSystem _particleSystem;

    List<CreatureController> _enteredColliderList = new List<CreatureController>();
	Coroutine _coDotDamage;

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    public override bool Init()
	{
		if (base.Init() == false)
			return false;
        ObjectType = ObjectType.Projectile;
        _rigid = GetComponent<Rigidbody>();
        _nav = GetComponent<NavMeshAgent>();    
		_trailRenderer = GetComponent<TrailRenderer>();
        _meshRenderer = GetComponentInChildren<MeshRenderer>();
        _particleSystem = GetComponentInChildren<ParticleSystem>();  
        return true;
	}
    private void FixedUpdate()
    {
        switch(_prefabName)
        {
            case "Missile_Boss":
                _dir = (Managers.Game.Player.transform.position + new Vector3(0,2f,0) - transform.position).normalized;
                _projectiletotargetRotation = Quaternion.LookRotation(_dir);
                _rigid.velocity = _dir * 20;
                _rigid.MoveRotation(Quaternion.RotateTowards(transform.rotation, _projectiletotargetRotation, 20));
                break;
        }
    }
    public void SetSkillData(SkillData skillData)
	{

	}
	public void SetInfo( CreatureController owner, string prefabName,Vector3 startPos,Vector3 dir)
	{
        _meshRenderer.gameObject.SetActive(true);
        _owner = owner;
		_spawnPos = startPos;
        _dir = dir;
        _prefabName = prefabName;

        switch(_prefabName)
        {
            case "Grenade":
                _rigid.AddForce((dir + Vector3.up) * 20,ForceMode.Impulse);
                _rigid.AddTorque(Vector3.back * 10, ForceMode.Impulse);
                break;
            case "Bullet_SubMachineGun":
                _rigid.velocity = _dir * 20;
                break;
            case "Missile":
                _rigid.velocity = _dir * 20;
                break;
            case "Missile_Boss":
                break;
            case "Rock_Boss":
                break;
        }

		if (gameObject.activeInHierarchy)
			StartCoroutine(CoCheckDestroy());
	}
    private void OnTriggerEnter(Collider collision)
	{
        switch (_owner.ObjectType)
        {
            case ObjectType.Player:
                PlayerProjectile(collision);
                break;
            case ObjectType.Monster: 
                MonsterProjectile(collision); 
                break;
        }

        if (collision.gameObject.tag == "Wall")
            DestroyProjectile();
    }
    void PlayerProjectile(Collider collision)
    {
        MonsterController target = collision.gameObject.GetComponent<MonsterController>();

        if (target.IsValid() == false)
            return;
        if (this.IsValid() == false)
            return;
        if (target.CreatureState == CreatureState.Dead)
            return;

        switch (_prefabName)
        {
            case "Grenage":
                break;
            case "Bullet_SubMachineGun":
                target.OnDotDamage(_owner, 10);
                StopDestroy();
                Managers.Object.Despawn(this);
                break;
        }
    }
    void MonsterProjectile(Collider collision)
    {
        PlayerController target = collision.gameObject.GetComponent<PlayerController>();

        if (target.IsValid() == false)
            return;
        if (this.IsValid() == false)
            return;
        if (target.CreatureState == CreatureState.Dead)
            return;


        switch (_prefabName)
        {
            case "Missile":
                target.OnDotDamage(_owner, 10);
                StopDestroy();
                Managers.Object.Despawn(this);
                break;
            case "Rock_Boss":
                target.OnDotDamage(_owner, 10);
                break;
            case "Missile_Boss":
                target.OnDotDamage(_owner, 10);
                StopDestroy();  
                Managers.Object.Despawn(this);  
                break;
        }
    }
    IEnumerator CoCheckDestroy() 
	{
        switch(_prefabName)
        {
            case "Bullet_SubMachineGun":
                _trailRenderer.enabled = true;
                yield return new WaitForSeconds(1f);
                _trailRenderer.Clear();
                break;
            case "Grenade":
                yield return new WaitForSeconds(2.0f);
                _rigid.velocity = Vector3.zero;
                _rigid.angularVelocity = Vector3.zero;
                _meshRenderer.gameObject.SetActive(false);
                AttackNearestMonster(transform.position, 10f, Vector3.up, 0f, LayerMask.GetMask("Monster"),100);
                yield return new WaitForSeconds(3.0f);
                _trailRenderer.Clear();
                break;
            case "Missile":
                _rigid.angularVelocity = Vector3.zero;
                yield return new WaitForSeconds(2f);
                break;
            case "Missile_Boss":
                _rigid.angularVelocity = Vector3.zero;
                yield return new WaitForSeconds(2f);
                break;
            case "Rock_Boss":
                StartRockBoss();
                yield return new WaitForSeconds(5f);
                StopRockBoss();
                break;

        }
        DestroyProjectile();
	}
    void AttackNearestMonster(Vector3 origin,float radius,Vector3 direction,float maxDistance,int layermask,int damage)
    {
        RaycastHit[] raycastHits = Physics.SphereCastAll(origin,radius,direction,maxDistance,layermask);
        foreach (RaycastHit _monster in raycastHits)
        {
            MonsterController target = _monster.transform.GetComponent<MonsterController>();

            if (target.IsValid() == false)
                return;
            if (this.IsValid() == false)
                return;
            if (target.CreatureState == CreatureState.Dead)
                return;

            target.OnDotDamage(_owner, damage);
        }
    }
    #region Destroy
    Coroutine _coDestroy;

    public void StartDestroy(float delaySeconds)
    {
        StopDestroy();
        _coDestroy = StartCoroutine(CoDestroy(delaySeconds));
    }

    public void StopDestroy()
    {
        if (_coDestroy != null)
        {
            StopCoroutine(_coDestroy);
            _coDestroy = null;
        }
    }

    IEnumerator CoDestroy(float delaySeconds)
    {
        yield return new WaitForSeconds(delaySeconds);
        if (this.IsValid())
        {
            Managers.Object.Despawn(this);
        }
    }

    #endregion

    #region RockBoss

    Coroutine _coProjectile;
    IEnumerator CoRockBoss()
    {
        angularPower = 2f;
        scaleValue = 0.1f;
        while (true)
        {
            angularPower += 0.02f;
            scaleValue += 0.01f;
            scaleValue = Mathf.Min(1,scaleValue);
            angularPower = Mathf.Min(5,angularPower);
            transform.localScale = Vector3.one * scaleValue;
            _rigid.AddTorque(transform.right * angularPower, ForceMode.Acceleration);
            _rigid.velocity = _dir * scaleValue * 20;
            yield return null;
        }
    }

    void StartRockBoss()
    {
        if (_coProjectile != null)
            _coProjectile = null;
        _coProjectile = StartCoroutine(CoRockBoss());
    }

    void StopRockBoss()
    {
        StopCoroutine(_coProjectile);    
    }
    #endregion

    public void DestroyProjectile()
	{
        _rigid.velocity = Vector3.zero;
        _rigid.angularVelocity = Vector3.zero;
        Managers.Object.Despawn(this);
	}
}
