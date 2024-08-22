using Data;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_ShopSlot : UI_Base
{
    [SerializeField]
    public ItemData itemdata;

    public int _count;
    public int _price;
    bool _isGet;

    enum Images
    {
        ItemImage,
    }
    enum Buttons
    {
    }
    enum Texts
    {
        PriceText,
    }
    private void Awake()
    {
        Init();
    }
    public override bool Init()
    {
        if (base.Init() == false)
            return false;
        BindImage(typeof(Images));
        BindText(typeof(Texts));
        BindButton(typeof(Buttons));
        BindEvents();
        return true;
    }
    public void BindEvents()
    {
        GetImage((int)Images.ItemImage).gameObject.GetComponent<Image>().sprite = Managers.Resource.Load<Sprite>(itemdata.Image);
        GetText((int)Texts.PriceText).gameObject.GetComponent<TextMeshProUGUI>().text = _price.ToString();
        this.gameObject.BindEvent(BuyItem);
    }
    public void BuyItem()
    {
        if (_isGet)
            return;
        if (Managers.Game.Gold < _price)
            return;
        Managers.Game.Player.Inventory.InsertItem(itemdata,_count);
        Managers.Game.Gold -= _price;
        if(!itemdata.Consumable)
            _isGet = true;
    }
}
