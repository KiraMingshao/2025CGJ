using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLauncher : MonoBehaviour
{
    public static GameLauncher Instance { get; private set; }

    public GameObject player;

    private void Awake() {
        if (Instance != null) {
            Destroy(gameObject);
            Debug.LogWarning("Multiple InputQueue instances detected. Destroying duplicate instance.");
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        // ��ʼ����Ϸϵͳ������չ��
        InitializeGameSystems();

    }

    void InitializeGameSystems()
    {
        // �ڴ˴���ʼ����Ϸ���������浵ϵͳ��
        // ʾ����if (!GameManager.Instance) Instantiate(gameManagerPrefab);
    }
}
