using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyManager : SingletonBehaviour<LobbyManager>
{
    public LobbyUIController lobbyUIController { get; private set; }

    protected override void Init()
    {
        // �κ� �Ŵ����� �ٸ� ������ ��ȯ�Ǹ� ������ ���̴�.
        isDestroyOnLoad = true;

        base.Init();
    }

    private void Start()
    {
        lobbyUIController = FindObjectOfType<LobbyUIController>();
        if (!lobbyUIController)
        {
            return;
        }

        lobbyUIController.Init();
        AudioManager.Instance.PlayBGM(BGM.lobby);
    }
}
