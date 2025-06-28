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
    public Transform energyContent;
    public EnergyItem startItem;
    public EnergyItem endItem;
    private List<EnergyItem> energyItems;

    public void Init(int maxHealth, int maxEnergy)
    {
        hpSlider.maxValue = maxHealth;
        hpSlider.value = maxHealth;
        levelSlider.maxValue = 100f;
        levelSlider.value = 0;
        balanceSlider.value = 0;
        CreateEnergy(maxEnergy);
    }

    private void CreateEnergy(int maxEnergy)
    {
        startItem.acive = false;
        endItem.acive = false;
        for (int i = 0; i < maxEnergy; i++)
        {
            if (i > 0 && i < energyItems.Count - 1)
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

    // Start is called before the first frame update
    void Start()
    {
        energyItems.AddRange(energyContent.GetComponentsInChildren<EnergyItem>());
    }

    // Update is called once per frame
    void Update()
    {

    }
}
