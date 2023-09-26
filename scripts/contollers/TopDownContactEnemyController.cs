using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownContactEnemyController : TopDownEnemyController
{
    [SerializeField][Range(0f, 100f)] private float followRange;
    [SerializeField] private string targetTag = "Player";
    [SerializeField] private SpriteRenderer characterRanderer;
    private bool _isCollidingWithTarget;

    private HealthSystem healthSystem;
    private HealthSystem _collidingTargetHealthSystem;
    private TopDownMovement _collidingMovement;

    protected override void Start()
    {
        base.Start();
        healthSystem = GetComponent<HealthSystem>();
        healthSystem.OnDamage += OnDamage;
    }

    private void OnDamage()
    {
        followRange = 100f;
    }
    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (_isCollidingWithTarget)
        {
            ApplyhealthChange();
        }

        Vector2 direction = Vector2.zero;
        if(DistanceToTarget() < followRange)
        {
            direction = DirectionToTarget();
        }

        CallMoveEvent(direction);
        Rotate(direction);
    }

    private void Rotate(Vector2 direction)
    {
        float rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        characterRanderer.flipX = Mathf.Abs(rotZ) > 90f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject receiver = collision.gameObject;

        if (!receiver.CompareTag(targetTag))    //ã�� Ÿ���� �ƴ�
        {
            return;
        }

        _collidingTargetHealthSystem = receiver.GetComponent<HealthSystem>();    //Ÿ���̶�� �ｺ�ý����� ������
        if(_collidingTargetHealthSystem != null)
        {
            _isCollidingWithTarget = true;  //Ÿ���� ������ ����
        }

        _collidingMovement = receiver.GetComponent<TopDownMovement>();   //�����Ʈ�� ������
    }
    private void OnTriggerExit2D(Collider2D collision)   // �浹 ����
    {
        if(!collision.CompareTag(targetTag))
        {
            return;
        }
        _isCollidingWithTarget = false;   //Ÿ���� ������ ����
    }

    private void ApplyhealthChange()
    {
        AttackSO attackSO = Stats.CurrentStats.attackSO;
        bool hasBeenChanged = _collidingTargetHealthSystem.ChangeHealth(-attackSO.power);  //+�� ȸ��
        if(attackSO.isOnKnockback && _collidingMovement != null)
        {
            _collidingMovement.ApplyKnockback(transform, attackSO.knockbackPower, attackSO.knockbackTime);  //�˹����
        }
    }
}
