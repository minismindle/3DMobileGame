using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class ObjectManager
{
	public PlayerController Player { get; private set; }
	public BossController Boss { get; private set; }
	public HashSet<MonsterController> Monsters { get; } = new HashSet<MonsterController>();
	public HashSet<ProjectileController> Projectiles { get; } = new HashSet<ProjectileController>();
	public HashSet<CoinController> Coins { get; } = new HashSet<CoinController> ();	
	public T Spawn<T>(Vector3 position,Quaternion rotation, int templateID = 0, string prefabName = "") where T : BaseController
	{
		System.Type type = typeof(T);

		if (type == typeof(PlayerController))
		{
			string prefab = "Player";
			GameObject go = Managers.Resource.Instantiate(prefab, pooling: true);
			go.name = "Player";
			go.transform.position = position;

			PlayerController pc = go.GetOrAddComponent<PlayerController>();
			Player = pc;

			return pc as T;
		}
		else if (type == typeof(MonsterController))
		{
			GameObject go = Managers.Resource.Instantiate(prefabName, pooling: true);
			//go.transform.position = position;

			MonsterController mc = go.GetOrAddComponent<MonsterController>();
			Monsters.Add(mc);
			
			mc.transform.position = position;
            mc.Init();
            return mc as T;
		}
        else if (type == typeof(BossController))
        {
            GameObject go = Managers.Resource.Instantiate(prefabName, pooling: true);
            //go.transform.position = position;

            BossController bc = go.GetOrAddComponent<BossController>();
			Boss = bc;
			bc.Init();
			bc.transform.position = position;

            return bc as T;
        }
		else if (type == typeof(ProjectileController))
		{
            GameObject go = Managers.Resource.Instantiate(prefabName, pooling: true);
			go.transform.position = position;
			go.transform.rotation = rotation;

			ProjectileController pc = go.GetOrAddComponent<ProjectileController>();
			Projectiles.Add(pc);
			pc.Init();
			return pc as T;
		}
		else if(type == typeof(CoinController))
		{
            GameObject go = Managers.Resource.Instantiate(prefabName, pooling: true);
            go.transform.position = position;
            go.transform.rotation = rotation;

            CoinController cc = go.GetOrAddComponent<CoinController>();
			Coins.Add(cc);
            cc.Init();
            return cc as T;
        }
		return null;
	}
	
	public void Despawn<T>(T obj) where T : BaseController
	{
		if (obj.IsValid() == false)
		{
			return;
		}

		System.Type type = typeof(T);

		if (type == typeof(PlayerController))
		{
			// ?
		}
		// obj is MonsterController
		else if (type == typeof(MonsterController) || type.IsSubclassOf(typeof(MonsterController)))
		{
			Monsters.Remove(obj as MonsterController);
			Managers.Resource.Destroy(obj.gameObject);
		}
		else if(type == typeof(ProjectileController))
		{
			Projectiles.Remove(obj as ProjectileController);
			Managers.Resource.Destroy(obj.gameObject);
		}
		else if(type == typeof(CoinController))
		{
			Coins.Remove(obj as CoinController);
			Managers.Resource.Destroy (obj.gameObject);	
		}
	}
	public void DespawnAllMonsters()
	{
		var monsters = Monsters.ToList();

		foreach (var monster in monsters)
			Despawn<MonsterController>(monster);
	}
	public void DespawnAllProjectiles() 
	{ 
		var projectiles = Projectiles.ToList();
		
		foreach(var projectile in projectiles) 
			Despawn<ProjectileController>(projectile);
	}
	public void DespawnAllCoins()
	{
		var coins = Coins.ToList();

		foreach (var coin in coins)
			Despawn<CoinController>(coin);
	}
	public void Clear()
	{
		Monsters.Clear();
		Projectiles.Clear();
	}
}
