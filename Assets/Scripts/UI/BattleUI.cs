using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using AI.FSM;

public class BattleUI : MonoBehaviour
{
    public static BattleUI Instance { get; private set; }

    public Slider hpSlider;
    public Slider levelSlider;
    public Slider balanceSlider;
    public Slider jumpChangeSlider;
    public Transform energyContent;
    public EnergyItem startItem;
    public EnergyItem endItem;
    public List<GameObject> stars;
    public Text gameOverTip;
    public Transform startPos;
    public Transform endPos;
    public Transform bg1;
    public Transform bg2;
    public int scrollSpeed = 1;

    public float _spriteWidth = 902.3438f;

    private List<EnergyItem> energyItems = new List<EnergyItem>();

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

    public void Init(int maxHealth, int maxEnergy,int maxImbalance)
    {
        hpSlider.maxValue = maxHealth;
        hpSlider.value = maxHealth;
        levelSlider.maxValue = 100f;
        levelSlider.value = 0;

        balanceSlider.minValue = -maxImbalance;
        balanceSlider.maxValue = maxImbalance;
        balanceSlider.value = 0;
        CreateEnergy(maxEnergy);
        jumpChangeSlider.value = 0;
        gameOverTip.gameObject.SetActive(false);

       // _spriteWidth = bg1.GetComponent<RectTransform>().rect.width;

        /*
        bg1.DOLocalMove(endPos.localPosition, (bg1.localPosition.x - endPos.localPosition.x) / 20f).OnComplete(()=> 
        {
            bg1.localPosition = startPos.localPosition;
            bg1.DOLocalMove(endPos.localPosition, (bg1.localPosition.x - endPos.localPosition.x) / 20f).SetLoops(-1);
        });

        bg2.DOLocalMove(endPos.localPosition, (bg2.localPosition.x - endPos.localPosition.x) / 20f).OnComplete(() =>
        {
            bg2.localPosition = startPos.localPosition;
            bg2.DOLocalMove(endPos.localPosition, (bg2.localPosition.x - endPos.localPosition.x) / 20f).SetLoops(-1);
        });
        */
    }

    public void InitLevel(List<SpecialWaveConfig> specialWaveConfigs)
    {
        var character = GameLauncher.Instance.player.GetComponent<Character.Character>();

        Init(character.status.maxHealth, character.status.maxEnergy,character.status.maxImbalance);

        for (int i = 0; i < stars.Count; i++)
        {
            if (i<specialWaveConfigs.Count)
            {
                stars[i].SetActive(true);
                var rect = stars[i].GetComponent<RectTransform>();
                var fatherRect = levelSlider.GetComponent<RectTransform>();
                rect.anchoredPosition = new Vector2(fatherRect.rect.width* specialWaveConfigs[i].triggerProgress / 100, 0);
            }
            else
            {
                stars[i].SetActive(false);
            }
        }
    }

    private void CreateEnergy(int maxEnergy)
    {
        startItem.acive = false;
        endItem.acive = false;
        for (int i = 0; i < maxEnergy; i++)
        {
            if (i > 0 && i < maxEnergy - 1)
            {
                var obj = GameObject.Instantiate(energyItems[0].gameObject, energyContent);
                energyItems.Add(obj.GetComponent<EnergyItem>());
                energyItems[i - 1].acive = false;
            }
        }
    }

    public void SetBalance(int value)
    {
        DOTween.To(() => balanceSlider.value, x => balanceSlider.value = x, value, 0.5f);
    }

    public void SetHp(int value)
    {
        DOTween.To(() => hpSlider.value, x => hpSlider.value = x, value, 0.5f);
    }

    public void SetEnergy(int energy)
    {
        startItem.acive = energy > 0;
        endItem.acive = (energy == (energyItems.Count + 2));
        for (int i = 0; i < energyItems.Count; i++)
        {
            energyItems[i].acive = i < energy - 1;
        }
    }

    public void ShowGameOver(bool pass)
    {
        gameOverTip.gameObject.SetActive(true);
        if (pass)
        {

            gameOverTip.text = "成功";
        }
        else
        {
            gameOverTip.text = "失败";
        }
    }

    // Start is called before the first frame update
    void Start()
    {      
        var items = energyContent.GetComponentsInChildren<EnergyItem>();
        energyItems.AddRange(items);
    }



    // Update is called once per frame
    void Update()
    {
        levelSlider.value = LevelManager.Instance.currentProgress;
        var character = GameLauncher.Instance.player.GetComponent<Character.Character>();
        var status = character.GetDecoratedStatus();
        SetBalance(status.imbalance);
        SetHp(status.health);
        SetEnergy(status.energy);

        var fsm = GameLauncher.Instance.player.GetComponent<CharacterBattleActionFSM>();
        jumpChangeSlider.value = fsm.jumpCharge;

        // 向左移动背景
        bg1.Translate(Vector3.left * scrollSpeed * Time.deltaTime);
        bg2.Translate(Vector3.left * scrollSpeed * Time.deltaTime);

        // 检查是否完全移出屏幕左侧
        if (bg1.position.x <= endPos.position.x)
        {
            // 将bg1重置到bg2右侧
            bg1.position = startPos.position;
        }

        if (bg2.position.x <= endPos.position.x)
        {
            // 将bg2重置到bg1右侧
            bg2.position = startPos.position;
        }


    }
}
