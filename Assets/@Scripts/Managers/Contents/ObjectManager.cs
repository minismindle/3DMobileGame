using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class ObjectManager
{
	public PlayerController Player { get; private set; }
	public HashSet<MonsterController> Monsters { get; } = new HashSet<MonsterController>();
	public HashSet<ProjectileController> Projectiles { get; } = new HashSet<ProjectileController>();

	public void ShowDamageFont(Vector2 pos, float damage,Transform parent, bool isCritical = false)
	{
		string prefab;

		prefab = "DamageFont";

		GameObject go = Managers.Resource.Instantiate(prefab, pooling: true);
		DamageFont damageText = go.GetOrAddComponent<DamageFont>();
		damageText.SetInfo(pos, damage, parent,isCritical);
	}
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
			go.transform.position = position;

			MonsterController mc = go.GetOrAddComponent<MonsterController>();
			Monsters.Add(mc);
			mc.Init();
			return mc as T;
		}
        else if (type == typeof(BossController))
        {
            GameObject go = Managers.Resource.Instantiate(prefabName, pooling: true);
            go.transform.position = position;

            BossController bc = go.GetOrAddComponent<BossController>();
            Monsters.Add(bc);

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
	}
	public List<MonsterController> GetNearestMonsters(int numprojectiles = 1,int distance = 0)
	{
        List<MonsterController> monsterList = Monsters.OrderBy(monster =>
       (Player.CenterPosition - monster.CenterPosition).sqrMagnitude).ToList();

        if (distance > 0)
            monsterList = monsterList.Where(monster =>
            (Player.CenterPosition - monster.CenterPosition).magnitude > distance).ToList();

        int min = Mathf.Min(numprojectiles, monsterList.Count);

        List<MonsterController> nearestMonsters = monsterList.Take(min).ToList();

        if (nearestMonsters.Count == 0) return null;

        // 요소 개수가 count와 다른 경우 마지막 요소 반복해서 추가
        while (nearestMonsters.Count < numprojectiles)
        {
            nearestMonsters.Add(nearestMonsters.Last());
        }

        return nearestMonsters;
    }
	public void DespawnAllMonsters()
	{
		var monsters = Monsters.ToList();

		foreach (var monster in monsters)
			Despawn<MonsterController>(monster);
	}
	public void Clear()
	{
		Monsters.Clear();
		Projectiles.Clear();
	}
}
