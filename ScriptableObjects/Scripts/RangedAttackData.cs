using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RangedAttackData", menuName = "TopDownController/Attacks/Ranged", order = 1)]

public class RangedAttackData : AttackSO
{
    [Header("Ranged Attack Data")]
    public string bulletNameTag;
    public float duration;  //���ӽð�(?)
    public float spread;    //ź����
    public int numberofProjectilesPerShot;  //ź ��(?)
    public float multipleProjectilesAngel;
    public Color projectileColor;
}
