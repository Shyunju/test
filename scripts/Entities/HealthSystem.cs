using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] private float healthChangeDelay = .5f;

    private CharacterStatsHandler _statsHandler;
    private float _timeSinceLastChange = float.MaxValue;    //�ð�����

    public event Action OnDamage;
    public event Action OnHeal;
    public event Action OnDeath;
    public event Action OnInvincibilityEnd;

    public float CurrentHealth { get; private set; }  //���� ü��
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
        if(_timeSinceLastChange < healthChangeDelay)    //�ǰ� ������ 0���� �����ϰ� .5�ʺ��� ���� ����
        {
            _timeSinceLastChange += Time.deltaTime;     //�ð��� �����ϴٰ�
            if(_timeSinceLastChange >= healthChangeDelay )      //�����̽ð����� Ŀ����
            {
                OnInvincibilityEnd?.Invoke();       //��������
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
