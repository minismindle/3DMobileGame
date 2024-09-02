using Data;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class UI_Shop : UI_Base
{
    public UI_ShopSlot[] slots;
    enum GameObjects
    {
        Slots,
    }
    enum Buttons
    {
        ExitShopButton,
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
        BindButton(typeof(Buttons));
        BindEvents();

        #region 상점세팅
        int idx = 0;
        slots = GetObject((int)GameObjects.Slots).gameObject.GetComponentsInChildren<UI_ShopSlot>();
        foreach (ShopData shopData in Managers.Data.ShopDataDic.Values)
        {
            ItemData itemData = new ItemData();
            Managers.Data.ItemDataDic.TryGetValue(shopData.DataId, out itemData);
            slots[idx].ItemData = itemData;
            slots[idx].Count = shopData.Count;
            slots[idx].Price = shopData.Price;
            idx++;
        }
        #endregion

        return true;
    }

    public void BindEvents()
    {
        GetButton((int)Buttons.ExitShopButton).gameObject.BindEvent(OnClickExitShopButton);
    }

    public void OnClickExitShopButton()
    {
        Managers.UI.ClosePopup();
    }
}
