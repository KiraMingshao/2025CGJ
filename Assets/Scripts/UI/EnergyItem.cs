using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyItem : MonoBehaviour
{
    public GameObject white;

    public bool acive
    {
        get
        {
            return white.activeSelf;
        }

        set
        {
            white.SetActive(value);
        }
    }
    
}
