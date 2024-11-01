using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum ConfirmType
{
    OK, // �ܼ��� �˸��� �˾����� Ư�� ����� �Բ� Ȯ�� ��ư�� �ִ� �˾�
    OK_CANCEL, // � ������ �Ϸ��� ���� �´��� ����� �˾�
}

public class ConfirmUIData : BaseUIData
{
    public ConfirmType confirmType;
    public string titleTxt; // �˾� ����
    public string descTxt; // �˾� ����
    public string okBtnTxt; // Ȯ�� ��ư �ؽ�Ʈ // ���� �ʿ��Ѱ�? ��Ȳ�� �޶� �ؽ�Ʈ�� �޶����� ������ ����
    public Action onClickOKBtn; // Ȯ�� ������ �� �����ϴ� ����
    public string cancelBtnTxt; // ��� ��ư �ؽ�Ʈ
    public Action onClickCancelBtn; // ��� ������ �� �����ϴ� ����
}

public class ConfirmUI : BaseUI
{
    public TextMeshProUGUI titleTxt;
    public TextMeshProUGUI descTxt;
    public Button okBtn;
    public Button cancelBtn;
    public TextMeshProUGUI okBtnTxt;
    public TextMeshProUGUI cancelBtnTxt;

    private ConfirmUIData confirmUIData;
    private Action onClickOKBtn;
    private Action onClickCancelBtn;

    public override void SetInfo(BaseUIData uiData)
    {
        base.SetInfo(uiData);

        confirmUIData = uiData as ConfirmUIData;

        titleTxt.text = confirmUIData.titleTxt;
        descTxt.text = confirmUIData.descTxt;
        okBtnTxt.text = confirmUIData.okBtnTxt;
        cancelBtnTxt.text = confirmUIData.cancelBtnTxt;

        onClickOKBtn = confirmUIData.onClickOKBtn;
        onClickCancelBtn = confirmUIData.onClickCancelBtn;

        okBtn.gameObject.SetActive(true);
        cancelBtn.gameObject.SetActive(confirmUIData.confirmType == ConfirmType.OK_CANCEL);
    }

    public void OnClickOKBtn()
    {
        onClickOKBtn?.Invoke();
        onClickOKBtn = null;
        CloseUI();
    }

    public void OnClickCancelBtn()
    {
        onClickCancelBtn?.Invoke();
        onClickCancelBtn = null;
        CloseUI();
    }
}
