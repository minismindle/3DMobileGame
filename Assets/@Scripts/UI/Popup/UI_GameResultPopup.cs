using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_GameResultPopup : UI_Base
{
    enum Buttons
    {
        ToVillageButton
    }
    public void Awake()
    {
        Init();
    }
    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindButton(typeof(Buttons));
        BindEvents();
        return true;
    }
    public void BindEvents()
    {
        GetButton((int)Buttons.ToVillageButton).gameObject.BindEvent(OnClickToVillageButton);
    }
    void OnClickToVillageButton()
    {
        StopAllCoroutines();
        Managers.Game.Player.transform.position = Vector3.zero;
        Managers.Game.Player.Resurrection();
        Managers.Scene.CurrentScene.GetComponent<GameScene>().OutStage();
        Managers.Object.DespawnAllMonsters();
        Managers.Object.DespawnAllProjectiles();
        Managers.UI.ClosePopup();
    }
}
