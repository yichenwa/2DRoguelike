using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class stat
{
    [SerializeField]
    private BarScript bar;
    //for any bar stat
    [SerializeField]
    private stat health;

    [SerializeField]
    private float maxVal;

    [SerializeField]
    private float currentVal;

    public float CurrentVal
    {
        get
        {
            return currentVal;
        }

        set
        {
            
            currentVal = value;
            bar.Value = currentVal;
        }
    }

    public float MaxVal
    {
        get
        {
            return maxVal;
        }

        set
        {
           
           this.maxVal = value;
            bar.MaxValue = maxVal;
        }
    }

    public void Intialize()
    {
        this.MaxVal = maxVal;
        this.CurrentVal = currentVal;
    }
}
