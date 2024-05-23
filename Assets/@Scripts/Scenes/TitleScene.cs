using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScene : BaseScene
{
    void Start()
    {
        Init();
    }
    void SetInfo()
    {
        Managers.UI.ShowSceneUI<UI_TitleScene>();
        Managers.Sound.Play("",Define.Sound.Bgm);
        SceneType = Define.Scene.LobbyScene;

        var eventsystem = Managers.Resource.Instantiate("event");
        eventsystem.name = "@EventSystem";

    }
    bool _init = false;
    protected override void Init()
    {
        if(_init == false)
        {
            Managers.Resource.LoadAllAsync<Object>("PreLoad", (key, count, totalCount) =>
            {
                Debug.Log($"{key} {count}/{totalCount}");

                if (count == totalCount)
                {
                    SetInfo();
                }
            });
            _init = true;
            return;
        }
        else if(_init == true) 
        {
            SetInfo();
            return;
        }
    }
    public override void Clear()
    {

    }
}
