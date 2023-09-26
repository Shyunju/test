using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="DefaultAttackData", menuName = "TopDownController/Attacks/Default", order = 0)]

public class AttackSO : ScriptableObject
{
    [Header("Arrack Info")]
    public float size;  //������ ũ��
    public float delay;    //���� ������
    public float power;     //���ݷ�
    public float speed;     //����, ����ü �ӵ�
    public LayerMask target;

    [Header("Knock Back Info")]
    public bool isOnKnockback;
    public float knockbackPower;
    public float knockbackTime;
}
