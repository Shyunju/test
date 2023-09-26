using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class RangedAttackController : MonoBehaviour
{
    [SerializeField] private LayerMask levelCollisionLayer;

    private RangedAttackData _attackData;
    private float _currentDuration;
    private Vector2 _direction;
    private bool _isReady;
    private Rigidbody2D _rigidbody;
    private SpriteRenderer _spriteRenderer;
    private TrailRenderer _trailRenderer;
    private projectileManager _projectileManager;

    public bool fxOndestroy = true;

    private void Awake()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();  //나를 포함한 하위 자식까지
        _rigidbody = GetComponent<Rigidbody2D>();
        _trailRenderer = GetComponent<TrailRenderer>();
    }

    private void Update()
    {
        if (!_isReady)
        {
            return;
        }

        _currentDuration = Time.deltaTime;

        if(_currentDuration > _attackData.duration)
        {
            DestroyProjectile(transform.position, false);
        }
        _rigidbody.velocity = _direction * _attackData.speed;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(levelCollisionLayer.value == (levelCollisionLayer.value | ( 1<< collision.gameObject.layer ) ))  //벽이랑 부딪혔을 때(?)
        {
            DestroyProjectile(collision.ClosestPoint(transform.position) - _direction * .2f, fxOndestroy);
        }else if(_attackData.target.value == (_attackData.target.value | ( 1<< collision.gameObject.layer))){
            HealthSystem healthSystem = collision.GetComponent<HealthSystem>();  //부딪힌 물체의 헬스시스템가져오기
            if(healthSystem != null)        //없다면 피가 없는 오브젝트
            {
                healthSystem.ChangeHealth(-_attackData.power);      //공격력만큼 데이지를 줌
                if (_attackData.isOnKnockback)
                {
                    TopDownMovement movement = collision.GetComponent<TopDownMovement>();
                    if(movement != null)
                    {
                        movement.ApplyKnockback(transform, _attackData.knockbackPower, _attackData.knockbackTime);      //넉백 실행
                    }
                }
            }
            DestroyProjectile(collision.ClosestPoint(transform.position), fxOndestroy);
        }
    }
    public void InitializeAttack(Vector2 direction, RangedAttackData attackData, projectileManager projectManager)
    {
        _direction = direction;
        _attackData = attackData;
        _projectileManager = projectManager;

        UpdateProjectilSprite();
        _trailRenderer.Clear();
        _currentDuration = 0;
        _spriteRenderer.color = attackData.projectileColor;

        transform.right = _direction;
        _isReady = true;
    }

    private void UpdateProjectilSprite()
    {
        transform.localScale = Vector3.one * _attackData.size;
    }

    private void DestroyProjectile(Vector3 position, bool createFx)
    {
        if (createFx)
        {
            _projectileManager.CreateImpactParticlesAtPosiotn(position, _attackData);
        }
        gameObject.SetActive(false);
    }
}
