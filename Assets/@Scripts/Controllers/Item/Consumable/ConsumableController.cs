using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumableController : ItemController
{
    public override int Count
    {
        get { return base.Count; }
        set
        {
            base.Count = value;
            Managers.UI.GetSceneUI<UI_GameScene>().SetConsumableCount(Count);
            if(Count == 0)
            {
                base.Clear();
                Managers.UI.GetSceneUI<UI_GameScene>().ClearConsumableSlot();
                Managers.Game.Player.Inventory.ClearConsumableSlot();
            }
        }
    }
    public override bool Init()
    {
        base.Init();
        return true;
    }
    public void Use()
    {
        Count -= 1;
        switch (ItemData.Name) 
        {
            case "Potion":
                Managers.Game.Player.HP += 100;
                if(Managers.Game.Player.HP >= Managers.Game.Player.MaxHP)
                {
                    Managers.Game.Player.HP = Managers.Game.Player.MaxHP;
                }
                break;
        }
    }
    public void SetInfo(CreatureController owner, ItemData itemData, int count)
    {
        base.SetInfo(owner, itemData);
        ItemType = Define.ItemType.Consumable;
        ItemData = itemData;
        Count = count;  
        _equip = true;
    }
    public override void Clear()
    {
        base.Clear();
        Count = 0;
    }
}
