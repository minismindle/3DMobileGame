using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawningPool : MonoBehaviour
{
	Coroutine _coUpdateSpawningPool;
	public Data.StageData StageData { get; set; }
	public virtual int stageLevel { get; set; }
	public virtual int prevLevel { get; set; }
	public virtual int nextLevel { get; set; }
	public virtual int updateSec { get; set; }
	public virtual float spawnInterval { get; set; }
	public int maxMonsterCount = 100;
	public bool Stopped { get; set; } = false;
	public bool IsUpdated { get; set; } = false;
	void Start()
    {
		if (_coUpdateSpawningPool != null)
			StopCoroutine(CoUpdateSpawningPool());
		_coUpdateSpawningPool = StartCoroutine(CoUpdateSpawningPool());
	}
	public void SetInfo(int stageLevel)
	{

		
	}
	IEnumerator CoUpdateSpawningPool()
	{
		while (true)
		{
			TrySpawn();
			yield return new WaitForSeconds(0.2f);
		}
	}
    void TrySpawn()
	{
		if (Stopped)
			return;

		int monsterCount = Managers.Object.Monsters.Count;

		if (monsterCount >= 300)
			return;

		Vector3 randPos = Utils.GenerateMonsterSpawnPosition(Managers.Game.Player.transform.position, 10, 20);
		MonsterController mc = Managers.Object.Spawn<MonsterController>(randPos);
	}
	
}
