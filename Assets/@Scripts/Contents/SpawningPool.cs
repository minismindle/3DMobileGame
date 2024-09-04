using Data;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net.Http.Headers;
using UnityEditor.Timeline;
using UnityEngine;
using static Define;

public class SpawningPool : MonoBehaviour
{
	Coroutine _coUpdateSpawningPool;
	
	bool isSpawnBoss = false;
	public virtual int DataId { get; set; } 
	public virtual int BossSpawnCount { get; set; } 
	public virtual int MaxCount { get; set; } 
	public virtual float SpawnInterval { get; set; } 
	public virtual List<string> MonsterNames {  get; set; }
	public virtual string BossName { get; set; }	
	public virtual BossController Boss {  get; set; }	
	public virtual StageData StageData { get { return _stageData; } set {_stageData = value; } }
	[SerializeField]
	StageData _stageData;
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
    public void SetInfo(int key)
    {
		Managers.Data.StageDataDic.TryGetValue(key, out StageData stageData);
		StageData = stageData;
		MaxCount = stageData.MaxCount;
		SpawnInterval = stageData.SpawnInterval;
		MonsterNames = stageData.MonsterNames;	
		BossName = stageData.BossName;
    }
    IEnumerator CoUpdateSpawningPool()
	{
		while (true)
		{
			TrySpawn();
			yield return new WaitForSeconds(SpawnInterval);
		}
	}
    void TrySpawn()
	{
        string monsterName = MonsterNames[Random.Range(0,MonsterNames.Count)];
        int monsterCount = Managers.Object.Monsters.Count;

        if (isSpawnBoss)
            return;
        if (monsterCount >= MaxCount)
            return;
        if (Managers.Game.KillCount >= StageData.BossSpawnCount)
		{
			SpawnBoss();
			return;
		}

		Vector3 spawnPos = Utils.GenerateMonsterSpawnPosition(transform.position, 20, 30);
		Managers.Object.Spawn<MonsterController>(spawnPos,transform.rotation,0, monsterName);
	}
	void SpawnBoss()
	{
        isSpawnBoss = true;
		Managers.Object.DespawnAllMonsters();
        Boss = Managers.Object.Spawn<BossController>(transform.position, transform.rotation, 0, "Boss");
		StopSpawn();
        return;
	}
}
