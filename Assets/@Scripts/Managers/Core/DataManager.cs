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
    //public Dictionary<int, Data.MaterialData> MaterialDic { get; private set; } = new Dictionary<int, Data.MaterialData>();
    public Dictionary<int, Data.StageData> StageDic { get; private set; } = new Dictionary<int, Data.StageData>();
    public Dictionary<int, Data.SkillData> SkillDic { get; private set; } = new Dictionary<int, Data.SkillData>();
    public Dictionary<int, Data.CreatureData> CreatureDic { get; private set; } = new Dictionary<int, Data.CreatureData>();
    public Dictionary<int, Data.LevelData> LevelDataDic { get; private set; } = new Dictionary<int, Data.LevelData>();
    public Dictionary<string, Data.EquipmentData> EquipDataDic { get; private set; } = new Dictionary<string, Data.EquipmentData>();
    public Dictionary<int, Data.EquipmentLevelData> EquipLevelDataDic { get; private set; } = new Dictionary<int, Data.EquipmentLevelData>();
    //public Dictionary<Define.GachaType, GachaTableData> GachaTableDataDic { get; private set; } = new Dictionary<Define.GachaType, GachaTableData>();
    public Dictionary<int, MissionData> MissionDataDic { get; private set; } = new Dictionary<int, MissionData>();
    public Dictionary<int, AchievementData> AchievementDataDic { get; private set; } = new Dictionary<int, AchievementData>();
    public Dictionary<int, CheckOutData> CheckOutDataDic { get; private set; } = new Dictionary<int, CheckOutData>();
    public Dictionary<int, OfflineRewardData> OfflineRewardDataDic { get; private set; } = new Dictionary<int, OfflineRewardData>();


    public void Init()
    {
        StageDic = LoadJson<Data.StageDataLoader, int, Data.StageData>("StageData").MakeDict();
        CreatureDic = LoadJson<Data.CreatureDataLoader, int, Data.CreatureData>("CreatureData").MakeDict();
        SkillDic = LoadJson<Data.SkillDataLoader, int, Data.SkillData>("SkillData").MakeDict();
        LevelDataDic = LoadJson<Data.LevelDataLoader, int, Data.LevelData>("LevelData").MakeDict();
        EquipDataDic = LoadJson<Data.EquipmentDataLoader, string, Data.EquipmentData>("EquipmentData").MakeDict();
        EquipLevelDataDic = LoadJson<Data.EquipmentLevelDataLoader, int, Data.EquipmentLevelData>("EquipmentLevelData").MakeDict();
        MissionDataDic = LoadJson<Data.MissionDataLoader, int, Data.MissionData>("MissionData").MakeDict();
        AchievementDataDic = LoadJson<Data.AchievementDataLoader, int, Data.AchievementData>("AchievementData").MakeDict();
        CheckOutDataDic = LoadJson<Data.CheckOutDataLoader, int, Data.CheckOutData>("CheckOutData").MakeDict();
        OfflineRewardDataDic = LoadJson<Data.OfflineRewardDataLoader, int, Data.OfflineRewardData>("OfflineRewardData").MakeDict();
    }

    Loader LoadJson<Loader, Key, Value>(string path) where Loader : ILoader<Key, Value>
    {
        TextAsset textAsset = Managers.Resource.Load<TextAsset>($"{path}");
        return JsonConvert.DeserializeObject<Loader>(textAsset.text);
    }


}
