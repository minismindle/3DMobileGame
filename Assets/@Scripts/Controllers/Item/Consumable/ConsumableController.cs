using Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumableController : ItemController
{
    public event Action<int> OnConsumableCountChanged;
    public event Action OnConsumableClear;
    public override int Count
    {
        get { return base.Count; }
        set
        {
            base.Count = value;
            OnConsumableCountChanged?.Invoke(value);
            if (Count == 0) { base.Clear(); OnConsumableClear?.Invoke(); }
        }
    }
    public override bool Init()
    {
        base.Init();
        return true;
    }
    protected override void SubScribe()
    {
        Managers.Game.Player.OnSetConsumable -= SetInfo;
        Managers.Game.Player.OnSetConsumable += SetInfo;
    }
    public void Use()
    {
        if (Count == 0)
            return;

        switch (ItemData.Name) 
        {
            case "Potion":
                Managers.Game.Player.HP += 100;
                Managers.Game.Player.HP = Mathf.Min(Managers.Game.Player.HP,Managers.Game.Player.MaxHP);
                break;
        }

        Count -= 1;
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
