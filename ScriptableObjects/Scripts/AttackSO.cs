using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="DefaultAttackData", menuName = "TopDownController/Attacks/Default", order = 0)]

public class AttackSO : ScriptableObject
{
    [Header("Arrack Info")]
    public float size;  //공격의 크기
    public float delay;    //공격 딜레이
    public float power;     //공격력
    public float speed;     //공격, 투사체 속도
    public LayerMask target;

    [Header("Knock Back Info")]
    public bool isOnKnockback;
    public float knockbackPower;
    public float knockbackTime;
}
