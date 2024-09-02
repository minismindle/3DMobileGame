using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoController : ItemController
{
    public override int Count 
    {
        get { return base.Count; }
        set { base.Count = value; Managers.UI.GetSceneUI<UI_GameScene>().SetTotalAmmoCount(Count);} 
    }
    public override bool Init()
    {
        base.Init();
        Managers.Data.ItemDataDic.TryGetValue(Define.AmmoItemDataID, out var item);
        ItemData = item;
        return true;
    }
}
