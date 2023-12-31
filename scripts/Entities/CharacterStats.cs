using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum StatsChangeType
{
    Add, 
    Multiple,
    Override
}

[Serializable]
public class CharacterStats
{
    public StatsChangeType statsChangeType;
    [Range(1, 100)] public int maxHealth;
    [Range(1f, 20f)] public float speed;  //이동속도

    //공격 데이터
    public AttackSO attackSO;

}
