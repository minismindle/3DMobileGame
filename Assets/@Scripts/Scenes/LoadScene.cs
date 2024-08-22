using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadScene : BaseScene
{
    void Start()
    {
        SceneType = Define.Scene.LoadScene;
        Managers.Resource.LoadAllAsync<Object>("PreLoad", (key, count, totalCount) =>
        {
            if (count == totalCount)
            {
                Start_Init();
            }
        });
    }
    public void Start_Init()
    {
        var eventsystem = Managers.Resource.Instantiate("EventSystem.prefab");
        eventsystem.name = "@EventSystem";
        DontDestroyOnLoad(eventsystem);
        Managers.Scene.LoadScene(Define.Scene.GameScene);
    }
    public override void Clear()
    {

    }
}
