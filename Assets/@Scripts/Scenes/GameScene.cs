using Data;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;

public class GameScene : BaseScene
{
    void Start()
    {
        Managers.Resource.LoadAllAsync<Object>("PreLoad", (key, count, totalCount) =>
        {
            Debug.Log($"{key} {count}/{totalCount}");

            if (count == totalCount)
            {
                Start_init();
            }
        });
    }
    void Start_init()
    {
        this.Init();

        SceneType = Define.Scene.GameScene;

        Managers.UI.ShowSceneUI<UI_GameScene>();

        _spawningPool = gameObject.GetOrAddComponent<SpawningPool>();

        var player = Managers.Object.Spawn<PlayerController>(Vector3.zero,transform.rotation);

        

        var joystick = Managers.Resource.Instantiate("UI_Joystick.prefab");
        joystick.name = "@UI_Joystick";

        var eventsystem = Managers.Resource.Instantiate("EventSystem.prefab");
        eventsystem.name = "@EventSystem";

        Camera.main.GetComponent<CameraController>()._player = player.gameObject;
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
        Managers.Game.Gem = 0;
        Managers.Game.KillCount = 0;
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
    public void HandleOnKillCountChanged(int killCount)
	{
		Managers.UI.GetSceneUI<UI_GameScene>().SetKillCount(killCount);
	}

    private void OnDestroy()
    {
        if (Managers.Game != null)
        {

        }
    }
}
