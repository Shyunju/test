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

        if (!receiver.CompareTag(targetTag))    //찾는 타겟이 아님
        {
            return;
        }

        _collidingTargetHealthSystem = receiver.GetComponent<HealthSystem>();    //타겟이라면 헬스시스템을 가져옴
        if(_collidingTargetHealthSystem != null)
        {
            _isCollidingWithTarget = true;  //타겟이 있음을 설정
        }

        _collidingMovement = receiver.GetComponent<TopDownMovement>();   //무브먼트도 가져옴
    }
    private void OnTriggerExit2D(Collider2D collision)   // 충돌 해제
    {
        if(!collision.CompareTag(targetTag))
        {
            return;
        }
        _isCollidingWithTarget = false;   //타겟이 없음을 설정
    }

    private void ApplyhealthChange()
    {
        AttackSO attackSO = Stats.CurrentStats.attackSO;
        bool hasBeenChanged = _collidingTargetHealthSystem.ChangeHealth(-attackSO.power);  //+면 회복
        if(attackSO.isOnKnockback && _collidingMovement != null)
        {
            _collidingMovement.ApplyKnockback(transform, attackSO.knockbackPower, attackSO.knockbackTime);  //넉백실행
        }
    }
}
