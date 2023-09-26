using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RangedAttackData", menuName = "TopDownController/Attacks/Ranged", order = 1)]

public class RangedAttackData : AttackSO
{
    [Header("Ranged Attack Data")]
    public string bulletNameTag;
    public float duration;  //Áö¼Ó½Ã°£(?)
    public float spread;    //ÅºÆÛÁü
    public int numberofProjectilesPerShot;  //Åº ¼ö(?)
    public float multipleProjectilesAngel;
    public Color projectileColor;
}
