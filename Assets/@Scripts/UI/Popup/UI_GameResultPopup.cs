using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_GameResultPopup : UI_Base
{
    #region UI 기능 리스트
    // 정보 갱신
    // ResultStageValueText : 해당 스테이지 수
    // ResultSurvivalTimeValueText : 스테이지 클리어 까지 걸린 시간 ( mm:ss 로 표기)
    // ResultGoldValueText : 죽기전 까지 얻은 골드
    // ResultKillValueText : 죽기전 까지 킬 수
    // ResultRewardScrollContentObject : : 보상으로 얻게될 아이템이 들어갈 부모 개체
    // (골드, 경헌치, 아이템, 캐릭터 강화석 등을 보상으로)

    // 로컬라이징 텍스트
    // GameResultPopupTitleText
    // ResultSurvivalTimeText
    // ConfirmButtonText
    #endregion

    enum GameObjects
    {
        ContentObject,
        ResultRewardScrollContentObject,
        ResultGoldObject,
        ResultKillObject,
    }

    enum Texts
    {
        GameResultPopupTitleText,
        ResultStageValueText,
        ResultSurvivalTimeText,
        ResultSurvivalTimeValueText,
        ResultGoldValueText,
        ResultKillValueText,
        ConfirmButtonText,
    }

    enum Buttons
    {
        StatisticsButton,
        ConfirmButton,
    }
    enum Images
    {
        ResultImage,
    }
    public void Awake()
    {
        Init();
    }
    public override bool Init()
    {
        if (base.Init() == false)
            return false;
        BindObject(typeof(GameObjects));
        BindText(typeof(Texts));
        BindButton(typeof(Buttons));
        BindImage(typeof(Images));  
        GetButton((int)Buttons.ConfirmButton).gameObject.BindEvent(OnClickConfirmButton);
        return true;
    }

    public void SetInfo(int stage,string time,bool survive)
    {
        GetText((int)Texts.GameResultPopupTitleText).text = "Game Result"; // arrive or dead
        GetText((int)Texts.ResultStageValueText).text = $"{stage} STAGE"; // stage
        GetText((int)Texts.ResultSurvivalTimeValueText).text = time;// 시간
        GetText((int)Texts.ResultKillValueText).text = $"{Managers.Game.KillCount}"; // 킬수 
        GetText((int)Texts.ConfirmButtonText).text = "OK";

    }

    void OnClickConfirmButton()
    {
        Managers.Clear();
        Managers.Scene.LoadScene(Define.Scene.LobbyScene);
	}
}
