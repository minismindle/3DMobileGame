using Data;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemController : BaseController
{
    public virtual Define.ItemType ItemType { get; set; } = Define.ItemType.None;

    public virtual ItemData ItemData {  get; set; } 
    private int _count;
    protected bool  _equip;
    public virtual bool Equip 
    { 
        get { return _equip; } 
        set { _equip = value; } 
    }
    public virtual int Count 
    {
        get { return _count; }
        set { _count = value;}
    }
    public override bool Init()
    {
        base.Init();
        return true;
    }
    public virtual void SetInfo(CreatureController owner,ItemData itemData) 
    { 
        ItemData = itemData;
        _equip = true;
    }
    public virtual void Clear()
    {
        ItemData = null;
        _equip = false;
    }
}
