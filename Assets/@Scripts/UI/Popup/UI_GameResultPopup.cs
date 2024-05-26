using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_GameResultPopup : UI_Base
{
    #region UI ��� ����Ʈ
    // ���� ����
    // ResultStageValueText : �ش� �������� ��
    // ResultSurvivalTimeValueText : �������� Ŭ���� ���� �ɸ� �ð� ( mm:ss �� ǥ��)
    // ResultGoldValueText : �ױ��� ���� ���� ���
    // ResultKillValueText : �ױ��� ���� ų ��
    // ResultRewardScrollContentObject : : �������� ��Ե� �������� �� �θ� ��ü
    // (���, ����ġ, ������, ĳ���� ��ȭ�� ���� ��������)

    // ���ö���¡ �ؽ�Ʈ
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
        GetText((int)Texts.ResultSurvivalTimeValueText).text = time;// �ð�
        GetText((int)Texts.ResultKillValueText).text = $"{Managers.Game.KillCount}"; // ų�� 
        GetText((int)Texts.ConfirmButtonText).text = "OK";

    }

    void OnClickConfirmButton()
    {
        Managers.Clear();
        Managers.Scene.LoadScene(Define.Scene.LobbyScene);
	}
}
