using Data;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using static Define;

public class GameScene : BaseScene
{
    [SerializeField]
    private NPCController shopNPC;
    [SerializeField]
    private NPCController questNPC;

    private Timer               timer;
    private SpawningPool        spawningPool;
    private UI_GameScene        ui;
    private BossController      boss;
    private StageData           stageData;
    public virtual SpawningPool SpawningPool 
    { 
        get { return spawningPool; } 
        set { spawningPool = value; } 
    }
    void Start()
    {
        Start_init();
    }
    public void Start_init()
    {
        Managers.Data.Init();

        SceneType = Define.Scene.GameScene;

        ui =  Managers.UI.ShowSceneUI<UI_GameScene>();
        spawningPool = gameObject.GetOrAddComponent<SpawningPool>();
        timer = gameObject.GetOrAddComponent<Timer>();

        var player = Managers.Object.Spawn<PlayerController>(Vector3.zero, transform.rotation);

        Managers.Game.Player.sceneType = SceneType;

        Camera.main.GetComponent<CameraController>()._player = player.gameObject;

        player.OnPlayerDead -= OnPlayerDead;
        player.OnPlayerDead += OnPlayerDead;
        player.PlayerInfoUpdate -= ui.PlayerInfoUpdate;
        player.PlayerInfoUpdate += ui.PlayerInfoUpdate;

        Managers.Game.OnGoldCountChanged -= OnGoldCountChanged;
        Managers.Game.OnGoldCountChanged += OnGoldCountChanged;
        Managers.Game.OnKillCountChanged -= OnKillCountChanged;
        Managers.Game.OnKillCountChanged += OnKillCountChanged;
        Managers.Game.OnScoreChanged -= OnScoreChanged;
        Managers.Game.OnScoreChanged += OnScoreChanged;

        Managers.UI.ShowPopup<UI_Inventory>();
        Managers.UI.CloseAllPopup();

        //Managers.Sound.Play("MainBGM", Define.Sound.Bgm,1,0.5f);
        Managers.Game.Gold = 10000000;
    }
    protected override void Init()
    {
        
    }
    public override void Clear()
    {

    }
    private void FixedUpdate()
    {
    }

    public void OnGoldCountChanged(int gold)
	{
        Managers.Game.Player.Gold = gold;
        ui.SetGold(gold);
        if (Managers.Game.Player.Inventory != null)
            Managers.Game.Player.Inventory.SetGold(gold);
    }
    public void OnKillCountChanged(int killcount)
    {
        if(killcount == stageData.BossSpawnCount)
        {
            boss = Managers.Object.Spawn<BossController>(spawningPool.gameObject.transform.position,transform.rotation,0, stageData.BossName);
            boss.MonsterInfoUpdate -= ui.MonsterInfoUpdate;  
            boss.MonsterInfoUpdate += ui.MonsterInfoUpdate;
            boss.InvokeMonsterData();
        }
    }
    public void OnScoreChanged(int score)
    {
        ui.SetScore(score);
    }
    public void OnStage()
    {
        Managers.Data.StageDataDic.TryGetValue(25000, out stageData);
        shopNPC.gameObject.SetActive(false);
        questNPC.gameObject.SetActive(false);
        spawningPool.SetInfo(25000);
        spawningPool.StartSpawn();
        timer.StartTimer();
        ui.ActiveScore();
    }
    public void OutStage()
    {
        Managers.Object.Despawn(boss);
        Managers.Game.KillCount = 0;
        Managers.Game.Score = 0;    
        shopNPC.gameObject.SetActive(true);
        questNPC.gameObject.SetActive(true);
        spawningPool.StopSpawn();
        timer.StopTimer();
        ui.DisActiveScore();    
    }
    #region 플레이어 사망시 스테이지 종료
    void OnPlayerDead()
    {
        StartCoroutine(CoEndStage(1.25f));
    }
    IEnumerator CoEndStage(float delay)
    {
        yield return new WaitForSeconds(delay);
        Managers.UI.ShowPopup<UI_GameResultPopup>();
        OutStage();
    }
    #endregion
    private void OnDestroy()
    {
        if (Managers.Game != null)
        {
            Managers.Game.OnGoldCountChanged -= OnGoldCountChanged;
            Managers.Game.OnKillCountChanged -= OnKillCountChanged; 
        }
    }
}
