using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Utils;

public class Define
{
    public static readonly Dictionary<Type, Array> _enumDict = new Dictionary<Type, Array>();

    public static readonly Dictionary<int, int> DicFirstPayReward = new Dictionary<int, int>
    {
          { 50306, 1 }, // 에픽 등급 랜덤 장비 상자 
          { 50001, 30000 }, // 골드
          { 50301, 0 }, // 랜덤 스크롤
          { 50203, 3 } // 골드키(가챠권)
    };

    //데이터 아이디에 따른 포션 회복량
    public static readonly Dictionary<int, float> DicPotionAmount = new Dictionary<int, float>
    {
          { 60001, 0.3f }, // 에픽 등급 랜덤 장비 상자 
          { 60002, 0.5f }, // 골드
          { 60003, 1 }, // 랜덤 스크롤
    };

    public static float POTION_COLLECT_DISTANCE = 2.6F;
    public static float BOX_COLLECT_DISTANCE = 2.6F;
    public static int STAMINA_RECHARGE_INTERVAL = 300;

    #region 넉백 데이터
    /// <summary>
    ///  KNOCKBACK_SPEED의 스피드로 KNOCKBACK_TIME시간동안 밀려난다.
    ///  KNOCKBACK_COOLTIME 동안은 넉백이 재발생하지 않는다
    /// </summary>
    public static float KNOCKBACK_TIME = 0.1f;// 밀려나는시간
    public static float KNOCKBACK_SPEED = 10;  // 속도 
    public static float KNOCKBACK_COOLTIME = 0.5f;
    #endregion
    #region 데이터아이디
    public static int ID_GOLD = 50001;
    public static int ID_DIA = 50002;
    public static int ID_STAMINA = 50003;
    public static int ID_BRONZE_KEY = 50201;
    public static int ID_SILVER_KEY = 50202;
    public static int ID_GOLD_KEY = 50203;
    public static int ID_RANDOM_SCROLL = 50301;
    public static int ID_POTION = 60001;
    public static int ID_MAGNET = 60004;
    public static int ID_BOMB = 60008;

    public static int ID_WEAPON_SCROLL = 50101;
    public static int ID_GLOVES_SCROLL = 50102;
    public static int ID_RING_SCROLL = 50103;
    public static int ID_BELT_SCROLL = 50104;
    public static int ID_ARMOR_SCROLL = 50105;
    public static int ID_BOOTS_SCROLL = 50106;

    public static string GOLD_SPRITE_NAME = "Gold_Icon";
    #endregion
    public static int MAX_STAMINA = 50;
    public static int GAME_PER_STAMINA = 3;

    #region 스테이지관련 데이터
    public static readonly int STAGE_SOULCOUNT = 10;
    public static readonly float STAGE_SOULDROP_RATE = 0.05f;
    public static readonly int BOSS_GEN_TIME = 5; // 웨이브 시작 10초 후 보스 젠
    public static readonly float MAPSIZE_REDUCTION_VALUE = 0.9f; // 웨이브 시작시 줄어드는 맵 크기
    #endregion

    #region 가챠 확률
    public static readonly float[] SUPPORTSKILL_GRADE_PROB = new float[]
    {
        //0.04f,   // Common 확률
        //0.04f,   // Uncommon 확률
        //0.01f,   // Rare 확률
        //0.5f,  // Epic 확률
        //0.45f,  // Legend 확률

        0.4f,   // Common 확률
        0.4f,   // Uncommon 확률
        0.1f,   // Rare 확률
        0.07f,  // Epic 확률
        0.03f,  // Legend 확률

    };

    public static readonly float[] COMMON_GACHA_GRADE_PROB = new float[]
    {
        0,
        0.62f,   // Common 확률
        0.18f,   // Uncommon 확률
        0.15f,   // Rare 확률
        0.05f,  // Epic 확률
    };

    public static readonly float[] ADVENCED_GACHA_GRADE_PROB = new float[]
    {
        0,
        0.55f,   // Common 확률
        0.20f,   // Uncommon 확률
        0.15f,   // Rare 확률
        0.10f,  // Epic 확률
    };

    public static readonly float[] PICKUP_GACHA_GRADE_PROB = new float[]
    {
            0,
        0.55f,   // Common 확률
        0.20f,   // Uncommon 확률
        0.15f,   // Rare 확률
        0.10f,  // Epic 확률
    };

    public static readonly float[] SOUL_SHOP_COST_PROB = new float[]
    {
        0,
        45,   // Common 가격
        55,   // Uncommon 가격
        80,  // Rare 가격
        160,  // Epic 가격
        320,  // Legend 가격
        80,   // 리롤 가격
    };
    #endregion

    #region 보석 경험치 획득량
    public const int SMALL_EXP_AMOUNT = 1;
    public const int GREEN_EXP_AMOUNT = 2;
    public const int BLUE_EXP_AMOUNT = 5;
    public const int YELLOW_EXP_AMOUNT = 10;
    #endregion

    #region 디폴트 장비/케릭터 아이디
    public const int CHARACTER_DEFAULT_ID = 201000;
    public const string WEAPON_DEFAULT_ID = "N00301";
    public const string GLOVES_DEFAULT_ID = "N10101";
    public const string RING_DEFAULT_ID = "N20201";
    public const string BELT_DEFAULT_ID = "N30101";
    public const string ARMOR_DEFAULT_ID = "N40101";
    public const string BOOTS_DEFAULT_ID = "N50101";
    #endregion

    #region sortingOrder
    public static readonly int UI_GAMESCENE_SORT_CLOSED = 321;
    public static readonly int SOUL_SORT = 105;

    //소울이 이동중일때 오더 변경
    public static readonly int UI_GAMESCENE_SORT_OPEN = 323;
    public static readonly int SOUL_SORT_GETITEM = 322;
    #endregion

    #region Enum
    public enum MaterialType
    {
        Gold,
        Dia,
        Stamina,
        Exp,
        WeaponScroll,
        GlovesScroll,
        RingScroll,
        BeltScroll,
        ArmorScroll,
        BootsScroll,
        BronzeKey,
        SilverKey,
        GoldKey,
        RandomScroll,
        AllRandomEquipmentBox,
        RandomEquipmentBox,
        CommonEquipmentBox,
        UncommonEquipmentBox,
        RareEquipmentBox,
        EpicEquipmentBox,
        LegendaryEquipmentBox,
        WeaponEnchantStone,
        GlovesEnchantStone,
        RingEnchantStone,
        BeltEnchantStone,
        ArmorEnchantStone,
        BootsEnchantStone,
    }

    public enum MaterialGrade
    {
        Common,
        Uncommon,
        Rare,
        Epic,
        Epic1,
        Epic2,
        Legendary,
        Legendary1,
        Legendary2,
        Legendary3,
    }

    public enum ItemType
    {
        None,
        Potion,
        Ammo,
        Coin,
        Bomb
    }

    public enum GachaType
    {
        None,
        CommonGacha,
        AdvancedGacha,
        PickupGacha,
    }
    public enum CoinType
    {
        Bronze,
        Silver,
        Gold,
    }
    public enum WeaponName
    {
        None,
        Hammer,
        HandGun,
        SubMachineGun,
        ExecutionSword,
    }
    public enum WeaponType
    {
        None,
        Melee,
        Range,
        Grenade,
    }
    //장비아이템에서 인벤토리에 있는지 케릭터 장비 에 있는지
    public enum UI_ItemParentType
    {
        CharacterEquipmentGroup,
        EquipInventoryGroup,
        GachaResultPopup,
    }

    public enum GachaRarity
    {
        Normal,
        Special,
    }
    public enum PlayerWeaponType
    {
        None,
        Melee,
        Range,
    }
    
    public enum EquipmentType
    {
        Weapon,
        Gloves,
        Ring,
        Belt,
        Armor,
        Boots,
    }

    public enum EquipmentGrade
    {
        None,
        Common,
        Uncommon,
        Rare,
        Epic,
        Epic1,
        Epic2,
        Legendary,
        Legendary1,
        Legendary2,
        Legendary3,
        Myth,
        Myth1,
        Myth2,
        Myth3
    }

    public enum EquipmentSortType
    {
        Level,
        Grade,
    }

    public enum MergeEquipmentType
    {
        None,
        ItemCode,
        Grade,
    }

    public enum CreatureState
    {
        Idle,
        Swing,
        Shot,
        Swap,
        Jump,
        Dodge,
        Throw,
        Skill1,
        Skill2,
        Moving,
        OnDamaged,
        Dead,
        Land,
        Reload,
        Attack,
        Tanut,
        Wait,
    }

    public enum ObjectType
    {
        Player,
        NPC,
        Monster,
        BossMonster,
        Weapon,
        Projectile,
        Coin,
        Grenade,
    }
    public enum MonsterName
    {
        MonsterA,
        MonsterB,
        MonsterC,
        Boss,
    }
    public enum WaveType
    {
        None,
        RedZone,//자기장 축소
        Elete,// 엘리트몹 등장
        Boss,
        //
    }
    public enum MissionType
    {
        Complete, // 완료시
        Daily,
        Weekly,
    }

    public enum MissionTarget // 미션 조건
    {
        DailyComplete, // 데일리 완료
        WeeklyComplete, // 위클리 완료
        StageEnter, // 스테이지 입장
        StageClear, // 스테이지 클리어
        EquipmentLevelUp, // 장비 레벨업
        CommonGachaOpen, // 일반 가챠 오픈 (광고 유도목적)
        AdvancedGachaOpen, // 고급 가챠 오픈 (광고 유도목적)
        OfflineRewardGet, // 오프라인 보상 
        FastOfflineRewardGet, // 빠른 오프라인 보상
        ShopProductBuy, // 상점 상품 구매
        Login, // 로그인
        EquipmentMerge, // 장비 합성
        MonsterAttack, // 몬스터 어택
        MonsterKill, // 몬스터 킬
        EliteMonsterAttack, // 엘리트 어택
        EliteMonsterKill, // 엘리트 킬
        BossKill, // 보스 킬
        DailyShopBuy, // 데일리 상점 상품 구매
        GachaOpen, // 가챠 오픈 (일반, 고급가챠 포함)
        ADWatchIng, // 광고 시청
    }

    

    public enum Scene
    {
        Unknown,
        TitleScene,
        LobbyScene,
        GameScene,
    }

    public enum Sound
    {
        Bgm,
        SubBgm,
        Effect,
        Max,
    }

    public enum UIEvent
    {
        Click,
        Pressed,
        PointerDown,
        PointerUp,
        BeginDrag,
        Drag,
        EndDrag,
    }
    #endregion

    public enum Layer
    {
        Wall = 11,

    }
    public enum CameraMode
    {
        QuarterView,
    }
    #region Tag
    public static string MonsterProjectile = "MonsterProjectile";
    public static string PlayerProjectile = "PlayerProjectile";
    #endregion

    #region Projectile
    public static string BulletSubMachineGun = "Bullet_SubMachineGun";
    #endregion
}

public static class EquipmentUIColors
{
    #region 장비 이름 색상
    public static readonly Color CommonNameColor = HexToColor("A2A2A2");
    public static readonly Color UncommonNameColor = HexToColor("57FF0B");
    public static readonly Color RareNameColor = HexToColor("2471E0");
    public static readonly Color EpicNameColor = HexToColor("9F37F2");
    public static readonly Color LegendaryNameColor = HexToColor("F67B09");
    public static readonly Color MythNameColor = HexToColor("F1331A");
    #endregion
    #region 테두리 색상
    public static readonly Color Common = HexToColor("AC9B83");
    public static readonly Color Uncommon = HexToColor("73EC4E");
    public static readonly Color Rare = HexToColor("0F84FF");
    public static readonly Color Epic = HexToColor("B740EA");
    public static readonly Color Legendary = HexToColor("F19B02");
    public static readonly Color Myth = HexToColor("FC2302");
    #endregion
    #region 배경색상
    public static readonly Color EpicBg = HexToColor("D094FF");
    public static readonly Color LegendaryBg = HexToColor("F8BE56");
    public static readonly Color MythBg = HexToColor("FF7F6E");
    #endregion
}
