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
    NPCController shopNPC;
    [SerializeField]
    NPCController questNPC;
    [SerializeField]
    SpawningPool spawningPool;
    [SerializeField]
    Timer _timer;
    public virtual SpawningPool SpawningPool { get { return spawningPool; } set { spawningPool = value; } }
    void Start()
    {
        Start_init();
    }
    public void Start_init()
    {
        Managers.Data.Init();

        SceneType = Define.Scene.GameScene;

        Managers.UI.ShowSceneUI<UI_GameScene>();

        var player = Managers.Object.Spawn<PlayerController>(Vector3.zero, transform.rotation);

        Managers.Game.Player.sceneType = SceneType;

        Camera.main.GetComponent<CameraController>()._player = player.gameObject;

        Managers.Game.OnGoldCountChanged -= HandleOnGoldCountChanged;
        Managers.Game.OnGoldCountChanged += HandleOnGoldCountChanged;

        Managers.UI.ShowPopup<UI_Inventory>();
        Managers.UI.CloseAllPopup();

        Init();
    }
  
    protected override void Init()
    {
        Managers.Game.Gold = 10000000;
    }
    public override void Clear()
    {

    }
    private void FixedUpdate()
    {
    }

    public void HandleOnGoldCountChanged(int gold)
	{
        Managers.Game.Player.Gold = gold;
        Managers.UI.GetSceneUI<UI_GameScene>().SetGold(gold);
        if (Managers.Game.Player.Inventory != null)
            Managers.Game.Player.Inventory.SetGold(gold);
    }
    public void OnStage()
    {
        shopNPC.gameObject.SetActive(false);
        questNPC.gameObject.SetActive(false);
        spawningPool.SetInfo(25000);
        spawningPool.gameObject.SetActive(true);
        spawningPool.StartSpawn();
    }
    public void OutStage()
    {
        shopNPC.gameObject.SetActive(true);
        questNPC.gameObject.SetActive(true);
        spawningPool.gameObject.SetActive(false);
        spawningPool.StopSpawn();
        Managers.UI.GetSceneUI<UI_GameScene>().DisActivaBossHpBar();
    }
    private void OnDestroy()
    {
        if (Managers.Game != null)
        {
            Managers.Game.OnGoldCountChanged -= HandleOnGoldCountChanged;
        }
    }
}
