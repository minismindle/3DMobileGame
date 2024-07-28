using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Inventory : UI_Base
{
    WeaponController[] _equipWeapons;
    WeaponController[] _weapons;
    enum GameObjects
    {
        Hotbar,
    }
    private void Awake()
    {
        Init();
    }
    public override bool Init()
    {
        if (base.Init() == false)
            return false;
        BindObject(typeof(GameObjects));
        BindEvents();
        return true;
    }
    public void BindEvents()
    {

    }
}
