using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BattleUI : MonoBehaviour
{
    public Slider hpSlider;
    public Slider levelSlider;
    public Slider balanceSlider;

    public void Init(int maxHealth,int maxEnergy)
    {
        hpSlider.maxValue = maxHealth;
        hpSlider.value = maxHealth;
        levelSlider.maxValue = 100f;
        levelSlider.value = 0;
    }

    public void SetBalance(int value)
    {
        DOTween.To(() => balanceSlider.value, x => balanceSlider.value = x, value, 0.5f);
    }

    public void SetHp(int value)
    {
        DOTween.To(() => hpSlider.value, x => hpSlider.value = x, value, 0.5f);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
