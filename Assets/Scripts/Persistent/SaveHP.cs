using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveHP : MonoBehaviour
{
    public static SaveHP instance;

    public int current_hp = 1;
    public int base_hp = 20;

    void Awake()
    {
        instance = this;
    }

    public float chanceToCapture()
    {
        return (float)current_hp / base_hp;
    }
}
