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
        // 初始化游戏系统（可扩展）
        InitializeGameSystems();

    }

    void InitializeGameSystems()
    {
        // 在此处初始化游戏管理器、存档系统等
        // 示例：if (!GameManager.Instance) Instantiate(gameManagerPrefab);
    }
}
