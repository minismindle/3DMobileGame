using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEditor.Timeline;
using UnityEngine;

public class SpawningPool : MonoBehaviour
{
	Coroutine _coUpdateSpawningPool;

	public virtual float SpawnTime {get;set;}
	public virtual int KeepMonsterCount { get;set;}	
	void Start()
    {
        Vector3 spawnPos = Utils.GenerateMonsterSpawnPosition(transform.position, 10, 20);
		Managers.Object.Spawn<MonsterController>(spawnPos, transform.rotation, 0, "MonsterA");
		Managers.Object.Spawn<MonsterController>(spawnPos, transform.rotation, 0, "MonsterB");
		Managers.Object.Spawn<MonsterController>(spawnPos, transform.rotation, 0, "MonsterC");
		Managers.Object.Spawn<MonsterController>(spawnPos, transform.rotation, 0, "Boss");
    }
	public void SetInfo()
	{
	}
	public void StartSpawn()
	{
        if (_coUpdateSpawningPool != null)
            StopCoroutine(CoUpdateSpawningPool());
        _coUpdateSpawningPool = StartCoroutine(CoUpdateSpawningPool());
    }
	public void StopSpawn()
	{
        if (_coUpdateSpawningPool != null)
            StopCoroutine(CoUpdateSpawningPool());
        _coUpdateSpawningPool = null;
    }
	IEnumerator CoUpdateSpawningPool()
	{
		while (true)
		{
			//TrySpawn();
			yield return new WaitForSeconds(SpawnTime);
		}
	}
    void TrySpawn()
	{
		int monsterCount = Managers.Object.Monsters.Count;

		if (monsterCount >= 3)
			return;
		Vector3 spawnPos = Utils.GenerateMonsterSpawnPosition(transform.position, 10, 20);
		MonsterController mc = Managers.Object.Spawn<MonsterController>(spawnPos,transform.rotation,0,"MonsterA");
	}
	
}
