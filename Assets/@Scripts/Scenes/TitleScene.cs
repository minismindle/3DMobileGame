using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScene : BaseScene
{
    void Start()
    {
        SceneType = Define.Scene.TitleScene;

        Managers.Data.Init();

        Managers.UI.ShowSceneUI<UI_TitleScene>();
    }
    void SetInfo()
    {
        var eventsystem = Managers.Resource.Instantiate("EventSystem.prefab");
        eventsystem.name = "@EventSystem";
    }
    bool _init = false;
    protected override void Init()
    {
        if(_init == false)
        {
            Managers.Resource.LoadAllAsync<Object>("PreLoad", (key, count, totalCount) =>
            {
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
