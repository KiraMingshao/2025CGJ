using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AI.FSM;
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
    //public BattleUI battleUI;

    private List<GameObject> activeEnemies = new List<GameObject>(); // 当前存活的敌人
    private Dictionary<SpecialWaveConfig, bool> specialWaveTriggered = new Dictionary<SpecialWaveConfig, bool>();

    private NormalWaveConfig currentNormalWaveConfig;

    // private int currentWaveGroupIndex = 0;
    private float lastWaveTime = 0f;
    private bool isWaitingForNextWave = false;
    private bool isSpawning = false;
    private bool isFinalBattle = false;
    private bool isLevelFinished = false;
    public List<Vector3> RandomSkyPoints = new List<Vector3>(); // 随机生成敌人时，随机选择的位置点

    private float processWaveTime = 0;
    private float totalWaveTime = 0;

    private int totalKillCount = 0;
    private bool firstWaveTrigger = false;

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
        processWaveTime = 0f; // 确保初始值为0

        // 计算总波次时间
        normalWaveConfigs.ForEach(config =>
        {
            totalWaveTime += config.waveInterval * (config.waveGroups.Count - 1);
            foreach (var group in config.waveGroups)
            {
                totalWaveTime += group.spawnInterval * (group.enemies.Count - 1);
            }
        });

        specialWaveConfigs.ForEach(config =>
        {
            totalWaveTime += config.waveGroup.spawnInterval * (config.waveGroup.enemies.Count - 1);
        });

        // 初始化特殊波次触发记录
        foreach (var config in specialWaveConfigs)
        {
            specialWaveTriggered[config] = false;
        }

        // 设置初始波次配置
        UpdateCurrentNormalWaveConfig();

        //设置UI关卡显示
        BattleUI.Instance.InitLevel(specialWaveConfigs);
    }

    private void Update()
    {
        if (isLevelFinished) return;

        // 只在非蹲下状态时更新内部刷新时间
        if (GameLauncher.Instance.player.GetComponent<CharacterBattleActionFSM>().CurrentState !=
            AI.FSM.FSMStateID.Crouch)
        {
            // 只在非蹲下状态时增加波次处理时间
            processWaveTime += Time.deltaTime * progressSpeed;
            processWaveTime = Mathf.Clamp(processWaveTime, 0f, totalWaveTime);
        }

        // 更新关卡进度（基于processWaveTime计算）
        currentProgress = 100f * processWaveTime / totalWaveTime;
        currentProgress = Mathf.Clamp(currentProgress, 0f, 100f);

        // 更新关卡状态
        UpdateLevelState();

        // 检查是否需要更新波次配置
        UpdateCurrentNormalWaveConfig();

        // 检查特殊波次
        CheckSpecialWaves();

        // 波次逻辑
        if (currentState != LevelState.Victory && currentState != LevelState.Defeat)
        {
            HandleWaveSpawning();
        }

        if (Input.GetKeyDown(KeyCode.P)) {
            StartCoroutine(this.GenerateAll());
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
                // currentWaveGroupIndex = 0;
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

            if (totalKillCount >= totalKillsNeeded)
            {
                isWaitingForNextWave = false;
                lastWaveTime = Time.time;
                StartCoroutine(SpawnNextWave());
            }

            return;
        }

        // 检查是否需要等待所有敌人死亡
        if (isWaitingForNextWave && activeEnemies.Count > 0)
            return;

        // 时间间隔检查
        if (!firstWaveTrigger)
        {
            firstWaveTrigger = true;
            StartCoroutine(SpawnNextWave());
        }
        else
        {
            if (!isWaitingForNextWave && !isSpawning && Time.time - lastWaveTime >= currentNormalWaveConfig.waveInterval)
            {
                StartCoroutine(SpawnNextWave());
            }
        }
    }

    private IEnumerator SpawnNextWave()
    {
        if (currentNormalWaveConfig.randomSelection && currentNormalWaveConfig.waveGroups.Count > 0)
        {
            var nextWaveGroup =
                currentNormalWaveConfig.waveGroups[
                    UnityEngine.Random.Range(0, currentNormalWaveConfig.waveGroups.Count)];
            StartCoroutine(SpawnWaveGroup(nextWaveGroup));
        }
        else
        {
            foreach (var waveGroup in currentNormalWaveConfig.waveGroups)
            {
                StartCoroutine(SpawnWaveGroup(waveGroup));
                yield return new WaitForSeconds(currentNormalWaveConfig.waveInterval);
            }
        }
    }

    private IEnumerator SpawnWaveGroup(WaveGroup waveGroup)
    {
        isSpawning = true;

        foreach (var enemyPrefab in waveGroup.enemies)
        {
            SpawnEnemy(enemyPrefab);
            yield return new WaitForSeconds(waveGroup.spawnInterval);
        }

        isSpawning = false;
        isWaitingForNextWave = true;
        lastWaveTime = Time.time;
    }

    private void SpawnEnemy(GameObject enemyPrefab)
    {
        var obj = GameObject.Instantiate(enemyPrefab);
        var enemy = obj.GetComponent<Enemy.Enemy>();
        if (enemy.isAir)
        {
            var length = RandomSkyPoints.Count;
            enemy.spawnPosition = RandomSkyPoints[UnityEngine.Random.Range(0, length)];
        }

        enemy.Respawn();
        activeEnemies.Add(obj);
    }

    public void OnEnemyDeath(GameObject enemy)
    {
        if (activeEnemies.Contains(enemy))
        {
            activeEnemies.Remove(enemy);
        }

        totalKillCount++;

        // 检查是否满足胜利条件
        if (isFinalBattle && activeEnemies.Count == 0)
        {
            currentState = LevelState.Victory;
            isLevelFinished = true;
            Debug.Log("Level Victory!");
            // 触发胜利逻辑
            BattleUI.Instance.ShowGameOver(true);
        }
    }

    public void PlayerDefeated()
    {        
        if (currentState == LevelState.Defeat)
        {
            return;
        }
        currentState = LevelState.Defeat;
        isLevelFinished = true;
        Debug.Log("Level Defeat!");
        // 触发失败逻辑
        BattleUI.Instance.ShowGameOver(false);
    }

    public LevelState GetCurrentLevelState()
    {
        return currentState;
    }

    public IEnumerator GenerateAll() {
        Time.timeScale = 2.5f;
        int i = 0;
        while (true) {
            foreach (var normalGroup in this.normalWaveConfigs) {
                foreach (var wave in normalGroup.waveGroups) {
                    foreach (var gameObject in wave.enemies) {
                        this.SpawnEnemy(gameObject);
                        yield return new WaitForSeconds(0.01f);
                    }
                }
            }
            foreach (var normalGroup in this.specialWaveConfigs) {
                foreach (var gameObject in normalGroup.waveGroup.enemies) {
                    this.SpawnEnemy(gameObject);
                    yield return new WaitForSeconds(0.01f);
                }
            }
            ++i;
            if (i == 5) {
                yield return new WaitForSeconds(3f);
                i = 0;
            }
        }
    }

    public void GenerateAllAsync() {
        this.StartCoroutine(this.GenerateAll());
    }
}