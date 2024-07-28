using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class CoinController : ItemController
{
    public Define.CoinType CoinType;
    
    Rigidbody _rigidbody;

    private int _gold;
    public override bool Init()
    {
        base.Init();
        SetInfo();
        return true;
    }
    void SetInfo()
    {
        ObjectType = Define.ObjectType.Coin;
    }
    public int GetCoin()
    {
        switch(CoinType) 
        {
            case Define.CoinType.Bronze:
                _gold = 5;
                break;
            case Define.CoinType.Silver:
                _gold = 15; 
                break;
            case Define.CoinType.Gold:
                _gold = 25;
                break;
        }
        return _gold;
    }
}
