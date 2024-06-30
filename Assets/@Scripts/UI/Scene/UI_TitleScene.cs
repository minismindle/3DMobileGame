using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_TitleScene : UI_Base
{
    enum Buttons
    {
        GameStartButton,
    }
    private void Awake()
    {
        Init();
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;
        BindButton(typeof(Buttons));
        SetInfo();
        return true;
    }
    public void SetInfo()
    {
        GetButton((int)Buttons.GameStartButton).gameObject.BindEvent(OnClickStartButton);
    }
    public void OnClickStartButton()
    {
        Managers.Clear();
        Managers.Scene.LoadScene(Define.Scene.GameScene);
    }

}
