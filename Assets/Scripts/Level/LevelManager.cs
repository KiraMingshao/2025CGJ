using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class WaveGroup
{
    public List<GameObject> enemies; // 怪物组中的敌人列表
    public float spawnInterval; // 组内怪物生成间隔
}

[System.Serializable]
public class NormalWaveConfig
{
    [Range(0f, 100f)] public float startProgress; // 起始进度百分比
    [Range(0f, 100f)] public float endProgress; // 结束进度百分比
    public List<WaveGroup> waveGroups; // 该区间内的波次组
    public bool randomSelection; // 是否随机选择波次组
    public float waveInterval; // 波次间隔时间
    public int killCountToNextWave; // 击杀多少敌人后刷新下一波(0表示全部)
}

[System.Serializable]
public class SpecialWaveConfig
{
    [Range(0f, 100f)] public float triggerProgress; // 触发进度百分比
    public WaveGroup waveGroup; // 特殊波次的怪物组
}

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    public float currentProgress = 0f; // 当前关卡进度(0-100)
    public List<NormalWaveConfig> normalWaveConfigs; // 普通波次配置
    public List<SpecialWaveConfig> specialWaveConfigs; // 特殊波次配置
    public float progressSpeed = 1f; // 关卡进度增长速度(单位: %/秒)
    public BattleUI battleUI;

    private List<GameObject> activeEnemies = new List<GameObject>(); // 当前存活的敌人
    private Dictionary<SpecialWaveConfig, bool> specialWaveTriggered = new Dictionary<SpecialWaveConfig, bool>();
    private NormalWaveConfig currentNormalWaveConfig;
    private int currentWaveGroupIndex = 0;
    private float lastWaveTime = 0f;
    private bool isWaitingForNextWave = false;
    private bool isSpawning = false;
    private bool isFinalBattle = false;
    private bool isLevelFinished = false;
    public List<Vector3> RandomSkyPoints = new List<Vector3>(); // 随机生成敌人时，随机选择的位置点

    private int totalWaveEnenmy = 0;
    private int spawnedEnemy = 0;
    
    public enum LevelState
    {
        InProgress,
        FinalBattle,
        Victory,
        Defeat
    }

    private LevelState currentState = LevelState.InProgress;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        normalWaveConfigs.ForEach(config => totalWaveEnenmy += config.waveGroups.Sum(group => group.enemies.Count));
        specialWaveConfigs.ForEach(config => totalWaveEnenmy += config.waveGroup.enemies.Count);
        
        // 初始化特殊波次触发记录
        foreach (var config in specialWaveConfigs)
        {
            specialWaveTriggered[config] = false;
        }

        // 设置初始波次配置
        UpdateCurrentNormalWaveConfig();

        //设置UI关卡显示
        battleUI.InitLevel(specialWaveConfigs);
    }

    private void Update()
    {
        if (isLevelFinished) return;

        // 更新关卡状态
        UpdateLevelState();

        // 更新关卡进度
        if (currentState == LevelState.InProgress && !isSpawning)
        {
            // currentProgress += progressSpeed * Time.deltaTime;
            currentProgress = 1f * spawnedEnemy / totalWaveEnenmy;
            currentProgress = Mathf.Clamp(currentProgress, 0f, 100f);
        }

        // 检查是否需要更新波次配置
        UpdateCurrentNormalWaveConfig();

        // 检查特殊波次
        CheckSpecialWaves();

        // 波次逻辑
        if (currentState != LevelState.Victory && currentState != LevelState.Defeat)
        {
            HandleWaveSpawning();
        }
    }

    private void UpdateLevelState()
    {
        if (isLevelFinished) return;

        if (currentProgress >= 100f)
        {
            if (activeEnemies.Count > 0)
            {
                currentState = LevelState.FinalBattle;
                isFinalBattle = true;

                // 检查是否还有BOSS存活
                bool hasBoss = CheckBossAlive();
                if (!hasBoss)
                {
                    // 没有BOSS，不再生成新怪物
                    return;
                }
                // 有BOSS，继续按配置生成怪物
            }
            else
            {
                currentState = LevelState.Victory;
                isLevelFinished = true;
                Debug.Log("Level Victory!");
                // 触发胜利逻辑
            }
        }
        else
        {
            currentState = LevelState.InProgress;
            isFinalBattle = false;
        }
    }

    private bool CheckBossAlive()
    {
        // 这里需要根据你的游戏逻辑判断是否有BOSS存活
        // 示例: 检查activeEnemies中是否有标记为BOSS的敌人
        foreach (var enemy in activeEnemies)
        {
            // if (enemy.GetComponent<Enemy>().isBoss)
            //     return true;
        }
        return false;
    }

    private void UpdateCurrentNormalWaveConfig()
    {
        if (currentNormalWaveConfig != null && 
            currentProgress >= currentNormalWaveConfig.startProgress && 
            currentProgress <= currentNormalWaveConfig.endProgress)
        {
            return; // 当前配置仍然有效
        }

        // 查找适合当前进度的配置
        foreach (var config in normalWaveConfigs)
        {
            if (currentProgress >= config.startProgress && currentProgress <= config.endProgress)
            {
                currentNormalWaveConfig = config;
                currentWaveGroupIndex = 0;
                lastWaveTime = Time.time;
                isWaitingForNextWave = false;
                break;
            }
        }
    }

    private void CheckSpecialWaves()
    {
        foreach (var config in specialWaveConfigs)
        {
            if (!specialWaveTriggered[config] && currentProgress >= config.triggerProgress)
            {
                specialWaveTriggered[config] = true;
                StartCoroutine(SpawnWaveGroup(config.waveGroup));
            }
        }
    }

    private void HandleWaveSpawning()
    {
        if (currentNormalWaveConfig == null || currentNormalWaveConfig.waveGroups.Count == 0)
            return;

        // 检查是否需要等待击杀一定数量敌人
        if (currentNormalWaveConfig.killCountToNextWave > 0)
        {
            int totalKillsNeeded = currentNormalWaveConfig.killCountToNextWave;
            if (isWaitingForNextWave)
            {
                totalKillsNeeded += GetRemainingEnemiesFromLastWave();
            }

            if (GetTotalKills() >= totalKillsNeeded)
            {
                isWaitingForNextWave = false;
                lastWaveTime = Time.time;
                SpawnNextWave();
            }
            return;
        }

        // 检查是否需要等待所有敌人死亡
        if (isWaitingForNextWave && activeEnemies.Count > 0)
            return;

        // 时间间隔检查
        if (!isWaitingForNextWave && !isSpawning && Time.time - lastWaveTime >= currentNormalWaveConfig.waveInterval)
        {
            SpawnNextWave();
        }
    }

    private int GetRemainingEnemiesFromLastWave()
    {
        // 计算上一波剩余的敌人数量
        // 可能需要记录每波生成的敌人
        return 0;
    }

    private int GetTotalKills()
    {
        // 从统计中获取总击杀数
        // return GameStats.TotalKills;
        return 0;
    }

    private void SpawnNextWave()
    {
        if (currentNormalWaveConfig.waveGroups.Count == 0) return;

        WaveGroup nextWaveGroup;
        if (currentNormalWaveConfig.randomSelection)
        {
            nextWaveGroup = currentNormalWaveConfig.waveGroups[UnityEngine.Random.Range(0, currentNormalWaveConfig.waveGroups.Count)];
        }
        else
        {
            nextWaveGroup = currentNormalWaveConfig.waveGroups[currentWaveGroupIndex];
            currentWaveGroupIndex = (currentWaveGroupIndex + 1) % currentNormalWaveConfig.waveGroups.Count;
        }

        StartCoroutine(SpawnWaveGroup(nextWaveGroup));
    }

    private IEnumerator SpawnWaveGroup(WaveGroup waveGroup)
    {
        isSpawning = true;
        
        foreach (var enemyPrefab in waveGroup.enemies)
        {
            var length = RandomSkyPoints.Count;
            SpawnEnemy(enemyPrefab, RandomSkyPoints[UnityEngine.Random.Range(0, length)]);
            yield return new WaitForSeconds(waveGroup.spawnInterval);
        }

        isSpawning = false;
        isWaitingForNextWave = true;
        lastWaveTime = Time.time;
    }

    private void SpawnEnemy(GameObject enemyPrefab, Vector3 spawnPosition)
    {
        spawnedEnemy++;
        var enemy = enemyPrefab.GetComponent<Enemy.Enemy>();
        enemy.spawnPosition = spawnPosition;
        enemy.Respawn();
        activeEnemies.Add(enemyPrefab);
    }

    public void OnEnemyDeath(GameObject enemy)
    {
        if (activeEnemies.Contains(enemy))
        {
            activeEnemies.Remove(enemy);
        }

        // 检查是否满足胜利条件
        if (isFinalBattle && activeEnemies.Count == 0)
        {
            currentState = LevelState.Victory;
            isLevelFinished = true;
            Debug.Log("Level Victory!");
            // 触发胜利逻辑
        }
    }

    public void PlayerDefeated()
    {
        currentState = LevelState.Defeat;
        isLevelFinished = true;
        Debug.Log("Level Defeat!");
        // 触发失败逻辑
    }

    public LevelState GetCurrentLevelState()
    {
        return currentState;
    }
}
