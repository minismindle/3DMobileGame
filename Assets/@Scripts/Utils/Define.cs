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
          { 50306, 1 }, // ���� ��� ���� ��� ���� 
          { 50001, 30000 }, // ���
          { 50301, 0 }, // ���� ��ũ��
          { 50203, 3 } // ���Ű(��í��)
    };

    //������ ���̵� ���� ���� ȸ����
    public static readonly Dictionary<int, float> DicPotionAmount = new Dictionary<int, float>
    {
          { 60001, 0.3f }, // ���� ��� ���� ��� ���� 
          { 60002, 0.5f }, // ���
          { 60003, 1 }, // ���� ��ũ��
    };

    public static float POTION_COLLECT_DISTANCE = 2.6F;
    public static float BOX_COLLECT_DISTANCE = 2.6F;
    public static int STAMINA_RECHARGE_INTERVAL = 300;

    #region �˹� ������
    /// <summary>
    ///  KNOCKBACK_SPEED�� ���ǵ�� KNOCKBACK_TIME�ð����� �з�����.
    ///  KNOCKBACK_COOLTIME ������ �˹��� ��߻����� �ʴ´�
    /// </summary>
    public static float KNOCKBACK_TIME = 0.1f;// �з����½ð�
    public static float KNOCKBACK_SPEED = 10;  // �ӵ� 
    public static float KNOCKBACK_COOLTIME = 0.5f;
    #endregion
    #region �����;��̵�
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

    #region ������������ ������
    public static readonly int STAGE_SOULCOUNT = 10;
    public static readonly float STAGE_SOULDROP_RATE = 0.05f;
    public static readonly int BOSS_GEN_TIME = 5; // ���̺� ���� 10�� �� ���� ��
    public static readonly float MAPSIZE_REDUCTION_VALUE = 0.9f; // ���̺� ���۽� �پ��� �� ũ��
    #endregion

    #region ��í Ȯ��
    public static readonly float[] SUPPORTSKILL_GRADE_PROB = new float[]
    {
        //0.04f,   // Common Ȯ��
        //0.04f,   // Uncommon Ȯ��
        //0.01f,   // Rare Ȯ��
        //0.5f,  // Epic Ȯ��
        //0.45f,  // Legend Ȯ��

        0.4f,   // Common Ȯ��
        0.4f,   // Uncommon Ȯ��
        0.1f,   // Rare Ȯ��
        0.07f,  // Epic Ȯ��
        0.03f,  // Legend Ȯ��

    };

    public static readonly float[] COMMON_GACHA_GRADE_PROB = new float[]
    {
        0,
        0.62f,   // Common Ȯ��
        0.18f,   // Uncommon Ȯ��
        0.15f,   // Rare Ȯ��
        0.05f,  // Epic Ȯ��
    };

    public static readonly float[] ADVENCED_GACHA_GRADE_PROB = new float[]
    {
        0,
        0.55f,   // Common Ȯ��
        0.20f,   // Uncommon Ȯ��
        0.15f,   // Rare Ȯ��
        0.10f,  // Epic Ȯ��
    };

    public static readonly float[] PICKUP_GACHA_GRADE_PROB = new float[]
    {
            0,
        0.55f,   // Common Ȯ��
        0.20f,   // Uncommon Ȯ��
        0.15f,   // Rare Ȯ��
        0.10f,  // Epic Ȯ��
    };

    public static readonly float[] SOUL_SHOP_COST_PROB = new float[]
    {
        0,
        45,   // Common ����
        55,   // Uncommon ����
        80,  // Rare ����
        160,  // Epic ����
        320,  // Legend ����
        80,   // ���� ����
    };
    #endregion

    #region ���� ����ġ ȹ�淮
    public const int SMALL_EXP_AMOUNT = 1;
    public const int GREEN_EXP_AMOUNT = 2;
    public const int BLUE_EXP_AMOUNT = 5;
    public const int YELLOW_EXP_AMOUNT = 10;
    #endregion

    #region ����Ʈ ���/�ɸ��� ���̵�
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

    //�ҿ��� �̵����϶� ���� ����
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
    //�������ۿ��� �κ��丮�� �ִ��� �ɸ��� ��� �� �ִ���
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
        RedZone,//�ڱ��� ���
        Elete,// ����Ʈ�� ����
        Boss,
        //
    }
    public enum MissionType
    {
        Complete, // �Ϸ��
        Daily,
        Weekly,
    }

    public enum MissionTarget // �̼� ����
    {
        DailyComplete, // ���ϸ� �Ϸ�
        WeeklyComplete, // ��Ŭ�� �Ϸ�
        StageEnter, // �������� ����
        StageClear, // �������� Ŭ����
        EquipmentLevelUp, // ��� ������
        CommonGachaOpen, // �Ϲ� ��í ���� (���� ��������)
        AdvancedGachaOpen, // ��� ��í ���� (���� ��������)
        OfflineRewardGet, // �������� ���� 
        FastOfflineRewardGet, // ���� �������� ����
        ShopProductBuy, // ���� ��ǰ ����
        Login, // �α���
        EquipmentMerge, // ��� �ռ�
        MonsterAttack, // ���� ����
        MonsterKill, // ���� ų
        EliteMonsterAttack, // ����Ʈ ����
        EliteMonsterKill, // ����Ʈ ų
        BossKill, // ���� ų
        DailyShopBuy, // ���ϸ� ���� ��ǰ ����
        GachaOpen, // ��í ���� (�Ϲ�, ��ް�í ����)
        ADWatchIng, // ���� ��û
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
    #region ��� �̸� ����
    public static readonly Color CommonNameColor = HexToColor("A2A2A2");
    public static readonly Color UncommonNameColor = HexToColor("57FF0B");
    public static readonly Color RareNameColor = HexToColor("2471E0");
    public static readonly Color EpicNameColor = HexToColor("9F37F2");
    public static readonly Color LegendaryNameColor = HexToColor("F67B09");
    public static readonly Color MythNameColor = HexToColor("F1331A");
    #endregion
    #region �׵θ� ����
    public static readonly Color Common = HexToColor("AC9B83");
    public static readonly Color Uncommon = HexToColor("73EC4E");
    public static readonly Color Rare = HexToColor("0F84FF");
    public static readonly Color Epic = HexToColor("B740EA");
    public static readonly Color Legendary = HexToColor("F19B02");
    public static readonly Color Myth = HexToColor("FC2302");
    #endregion
    #region ������
    public static readonly Color EpicBg = HexToColor("D094FF");
    public static readonly Color LegendaryBg = HexToColor("F8BE56");
    public static readonly Color MythBg = HexToColor("FF7F6E");
    #endregion
}
