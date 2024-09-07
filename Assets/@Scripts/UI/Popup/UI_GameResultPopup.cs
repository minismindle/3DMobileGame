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
    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindButton(typeof(Buttons));
        BindEvents();
        return true;
    }
    protected override void  BindEvents()
    {
        GetButton((int)Buttons.ToVillageButton).gameObject.BindEvent(OnClickToVillageButton);
    }
    void OnClickToVillageButton()
    {
        StopAllCoroutines();
        Managers.Game.Player.transform.position = Vector3.zero;
        Managers.Game.Player.Resurrection();
        Managers.Game.Boss.IdleMonster();
        Managers.Object.DespawnAllMonsters();
        Managers.Object.DespawnAllProjectiles();
        Managers.Object.DespawnAllCoins();
        Managers.UI.CloseAllPopup();
    }
}
