using Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public interface ILoader<Key, Value>
{
    Dictionary<Key, Value> MakeDict();
}
public class DataManager
{
    public Dictionary<int, ItemData> ItemDataDic { get; private set; } = new Dictionary<int, ItemData>();
    public Dictionary<int, ShopData> ShopDataDic { get; private set; } = new Dictionary<int, ShopData>();
    public Dictionary<int, MonsterData> MonsterDataDic { get; private set; } = new Dictionary<int, MonsterData>();
    public Dictionary<int, PlayerData> PlayerDataDic { get; private set; } = new Dictionary<int, PlayerData>();
    public Dictionary<int, ProjectileData> ProjectileDataDic { get; private set; } = new Dictionary<int, ProjectileData>();
    public Dictionary<int, StageData> StageDataDic { get; private set; } = new Dictionary<int, StageData>();
    public void Init()
    {
        ItemDataDic = LoadJson<ItemDataLoader,int,ItemData>("ItemData").MakeDict();
        ShopDataDic = LoadJson<ShopDataLoader, int,ShopData>("ShopData").MakeDict();
        MonsterDataDic = LoadJson<MonsterDataLoader, int, MonsterData>("MonsterData").MakeDict();
        PlayerDataDic = LoadJson<PlayerDataLoader, int,PlayerData>("PlayerData").MakeDict();
        StageDataDic = LoadJson<StageDataLoader, int, StageData>("StageData").MakeDict();
    }
    Loader LoadJson<Loader, Key, Value>(string path) where Loader : ILoader<Key, Value>
    {
        TextAsset textAsset = Managers.Resource.Load<TextAsset>($"{path}");
        return JsonUtility.FromJson<Loader>(textAsset.text);
    }


}
