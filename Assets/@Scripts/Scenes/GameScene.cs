using Data;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;

public class GameScene : BaseScene
{
    [SerializeField]
    NPCController shopNPC;
    [SerializeField]
    NPCController questNPC;
    void Start()
    {
        Start_init();
    }
    public void Start_init()
    {
        this.Init();

        Managers.Data.Init();

        SceneType = Define.Scene.GameScene;

        Managers.UI.ShowSceneUI<UI_GameScene>();

        var player = Managers.Object.Spawn<PlayerController>(Vector3.zero, transform.rotation);

        Managers.Game.Player.sceneType = SceneType;

        Camera.main.GetComponent<CameraController>()._player = player.gameObject;

        Managers.Game.OnGoldCountChanged -= HandleOnGoldCountChanged;
        Managers.Game.OnTotalAmmoCountChanged -= HandleOnAmmoCountChanged;
        Managers.Game.OnPotionCountChanged -= HandleOnPotionCountChanged;
        Managers.Game.OnGrenadeCountChanged -= HandleOnGrenadeCountChanged;

        Managers.Game.OnGoldCountChanged += HandleOnGoldCountChanged;
        Managers.Game.OnTotalAmmoCountChanged += HandleOnAmmoCountChanged;
        Managers.Game.OnPotionCountChanged += HandleOnPotionCountChanged;
        Managers.Game.OnGrenadeCountChanged += HandleOnGrenadeCountChanged;

        Managers.Game.Gold = 10000000;

        Managers.UI.ShowPopup<UI_Inventory>();
        Managers.UI.CloseAllPopup();
    }

    SpawningPool _spawningPool;
    Timer _timer;
    Define.WaveType _waveType
        ;
    public Define.WaveType StageType
    {
        get { return _waveType; }
        set
        {
            _waveType = value;

            if (_spawningPool != null)
            {
                switch (value)
                {
                    case Define.WaveType.None:
                        _spawningPool.Stopped = false;
                        break;
                }
            }
        }
    }
    string SetTimeText(int time)
    {
        string timeText = "";
        if (time < 10)
        {
            timeText = $"00:0{time}";
        }
        else if (time < 60)
        {
            timeText = $"00:{time}";
        }
        else if (time < 600)
        {
            if(time % 60 < 10)
                timeText = $"0{time / 60}:0{time % 60}";
            else
                timeText = $"0{time / 60}:{time % 60}";
        }
        else if (time == 10)
        {
            timeText = "10:00";
        }
        return timeText;
    }
    protected override void Init()
    {
        Managers.Game.Gold = 0;
        Managers.Game.KillCount = 0;
        Managers.Game.Potion = 0;
        Managers.Game.TotalAmmo = 0;
    }
    public override void Clear()
    {

    }
    private void FixedUpdate()
    {
        Updatetimer();
    }
    public void Updatetimer()
    {
        if (_timer == null)
            return;
        Managers.UI.GetSceneUI<UI_GameScene>().SetTimeInfo(SetTimeText(_timer.Time));
    }
    public void HandleOnGoldCountChanged(int gold)
	{
        Managers.Game.Player.Gold = gold;
        Managers.UI.GetSceneUI<UI_GameScene>().SetGold(gold);
    }
    public void HandleOnAmmoCountChanged(int ammoCount)
    {
        Managers.Game.Player.Ammo = ammoCount;
        Managers.UI.GetSceneUI<UI_GameScene>().SetTotalAmmoCount(ammoCount);
    }
    public void HandleOnPotionCountChanged(int potionCount) 
    { 
        Managers.Game.Player.Potion = potionCount; 
        Managers.UI.GetSceneUI<UI_GameScene>().SetPotionCount(potionCount);    
    }
    public void HandleOnGrenadeCountChanged(int grenadeCount)
    {
        Managers.Game.Player.Grenade = grenadeCount;    
        Managers.UI.GetSceneUI<UI_GameScene>().SetGrenadeCount(grenadeCount);   
    }
    public void OnStage()
    {
        shopNPC.gameObject.SetActive(false);
        questNPC.gameObject.SetActive(false);
        _spawningPool = gameObject.GetOrAddComponent<SpawningPool>();
    }
    private void OnDestroy()
    {
        if (Managers.Game != null)
        {
            Managers.Game.OnGoldCountChanged -= HandleOnGoldCountChanged;
            Managers.Game.OnTotalAmmoCountChanged -= HandleOnAmmoCountChanged;
            Managers.Game.OnPotionCountChanged -= HandleOnPotionCountChanged;
            Managers.Game.OnGrenadeCountChanged -= HandleOnGrenadeCountChanged;
        }
    }
}
