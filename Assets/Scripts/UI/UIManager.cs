using System.Buffers.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : SingletonBehaviour<UIManager>
{
    public Transform UICanvasTrs; // ������ ��ġ
    public Transform ClosedUITrs; // ��Ȱ�� UI ����� ��ġ

    private BaseUI frontUI; // �ֻ�� UI
    private Dictionary<System.Type, GameObject> openUIPool = new Dictionary<System.Type, GameObject>(); // Ȱ��ȭ�� UI �����
    private Dictionary<System.Type, GameObject> closeUIPool = new Dictionary<System.Type, GameObject>(); // ��Ȱ��ȭ�� UI �����


    protected override void Init()
    {
        base.Init();

    }

    private BaseUI GetUI<T>(out bool isAlreadyOpen) // UI �ν��Ͻ��� �����ϸ� ���ϴ� UI ��ȯ
    {
        System.Type uiType = typeof(T);

        BaseUI ui = null;
        isAlreadyOpen = false;

        if (openUIPool.ContainsKey(uiType)) // �̹� Ȱ��ȭ �Ǿ��ٸ�
        {
            ui = openUIPool[uiType].GetComponent<BaseUI>();
            isAlreadyOpen = true;
        }
        else if (closeUIPool.ContainsKey(uiType)) // ��Ȱ��ȭ �Ǿ��ٸ�
        {
            ui = closeUIPool[uiType].GetComponent<BaseUI>();
            closeUIPool.Remove(uiType);
        }
        else // ������ ���� ���ٸ� �������ֱ�
        {
            var uiObj = Instantiate(Resources.Load($"UI/{uiType}", typeof(GameObject))) as GameObject;
            ui = uiObj.GetComponent<BaseUI>();
        }

        return ui;
    }


    public void OpenUI<T>(BaseUIData uiData)
    {
        System.Type uiType = typeof(T);


        bool isAlredyPone = false;
        var ui = GetUI<T>(out isAlredyPone);

        if (!ui)
        {
            return;
        }

        if (isAlredyPone)
        {
            return;
        }

        var siblingIndex = UICanvasTrs.childCount - 1;
        ui.Init(UICanvasTrs);
        ui.transform.SetSiblingIndex(siblingIndex);
        ui.gameObject.SetActive(true);
        ui.SetInfo(uiData);
        ui.ShowUI();

        frontUI = ui;
        openUIPool[uiType] = ui.gameObject;
    }

    public void CloseUI(BaseUI ui)
    {
        System.Type uiType = ui.GetType();

        ui.gameObject.SetActive(false);

        openUIPool.Remove(uiType);
        closeUIPool[uiType] = ui.gameObject;

        ui.transform.SetParent(ClosedUITrs);

        frontUI = null;
        var lastChild = UICanvasTrs.GetChild(UICanvasTrs.childCount - 1);
        if (lastChild)
        {
            frontUI = lastChild.gameObject.GetComponent<BaseUI>();
        }
    }

    public BaseUI GetActiveUI<T>() // ���ϴ� ui�� �������� �������� �ƴϸ� null ��ȯ
    {
        var uiType = typeof(T);
        return openUIPool.ContainsKey(uiType) ? openUIPool[uiType].GetComponent<BaseUI>() : null;
    }

    public bool ExistOpenUI() // Ȱ����ȭ�� ui �ִ��� Ȯ��
    {
        return frontUI != null;
    }

    public BaseUI GetCurrentFrontUI() // �ֻ�� UI ����
    {
        return frontUI;
    }

    public void CloseCurrentFrontUI() // �ֻ�� UI �ݱ�
    {
        frontUI.CloseUI();
    }

    public void CloseAllOpenUI() // �����ִ� ��� UI �ݱ�
    {
        while (frontUI)
        {
            frontUI.CloseUI(true);
        }
    }

}
