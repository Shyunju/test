using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] private float healthChangeDelay = .5f;

    private CharacterStatsHandler _statsHandler;
    private float _timeSinceLastChange = float.MaxValue;    //시간저장

    public event Action OnDamage;
    public event Action OnHeal;
    public event Action OnDeath;
    public event Action OnInvincibilityEnd;

    public float CurrentHealth { get; private set; }  //현재 체력
    public float MaxHealth => _statsHandler.CurrentStats.maxHealth;

    private void Awake()
    {
        _statsHandler = GetComponent<CharacterStatsHandler>();
    }
    // Start is called before the first frame update
    void Start()
    {
        CurrentHealth = _statsHandler.CurrentStats.maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if(_timeSinceLastChange < healthChangeDelay)    //피격 시점에 0으로 설정하고 .5초보다 작은 동안
        {
            _timeSinceLastChange += Time.deltaTime;     //시간은 누적하다가
            if(_timeSinceLastChange >= healthChangeDelay )      //딜레이시간보다 커지면
            {
                OnInvincibilityEnd?.Invoke();       //무적해제
            }
        }
    }

    public bool ChangeHealth(float change)
    {
        if(change == 0 || _timeSinceLastChange < healthChangeDelay)
        {
            return false;
        }

        _timeSinceLastChange = 0f;
        CurrentHealth += change;
        CurrentHealth = CurrentHealth > MaxHealth ? MaxHealth : CurrentHealth;
        CurrentHealth = CurrentHealth < 0 ? 0 : CurrentHealth;

        if(change > 0)
        {
            OnHeal?.Invoke();
        }
        else
        {
            OnDamage?.Invoke();
        }

        if(CurrentHealth <= 0f)
        {
            CallDeath();
        }

        return true;
    }

    private void CallDeath()
    {
        OnDeath?.Invoke();
    }
}
