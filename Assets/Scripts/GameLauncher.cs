using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLauncher : MonoBehaviour
{

    void Start()
    {
        // 初始化游戏系统（可扩展）
        InitializeGameSystems();

        DontDestroyOnLoad(this.gameObject);
    }

    void InitializeGameSystems()
    {
        // 在此处初始化游戏管理器、存档系统等
        // 示例：if (!GameManager.Instance) Instantiate(gameManagerPrefab);
    }
}
