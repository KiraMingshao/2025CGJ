using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLauncher : MonoBehaviour
{

    void Start()
    {
        // ��ʼ����Ϸϵͳ������չ��
        InitializeGameSystems();

        DontDestroyOnLoad(this.gameObject);
    }

    void InitializeGameSystems()
    {
        // �ڴ˴���ʼ����Ϸ���������浵ϵͳ��
        // ʾ����if (!GameManager.Instance) Instantiate(gameManagerPrefab);
    }
}
