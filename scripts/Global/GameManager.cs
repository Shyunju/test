using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Transform Player { get; private set; }
    [SerializeField] private string playerTag = "Player";
    private HealthSystem playerHealthSystem;

    [SerializeField] private TextMeshProUGUI waveText;
    [SerializeField] private Slider hpGaugeSlider;
    [SerializeField] private GameObject gameOverUI;

    [SerializeField] private int currentWaveIndex = 0;   // 웨이브 번호
    private int currentSpawnCount = 0;
    private int waveSpawnCount = 0;
    private int waveSpawnPosCount = 0;

    public float spawnInterval = .5f;
    public List<GameObject> enemyprefebs = new List<GameObject>();

    [SerializeField] private Transform spawnPositionsRoot;
    private List<Transform> spawnPositions = new List<Transform>();

    public List<GameObject> rewards = new List<GameObject> ();

    [SerializeField] private CharacterStats defaultStats;
    [SerializeField] private CharacterStats rangedStats;

    private void Awake()
    {
        instance = this;
        Player = GameObject.FindGameObjectWithTag(playerTag).transform;

        playerHealthSystem = Player.GetComponent<HealthSystem>();
        playerHealthSystem.OnDamage += UpdateHealthUI;
        playerHealthSystem.OnHeal += UpdateHealthUI;
        playerHealthSystem.OnDeath += GameOver;

        gameOverUI.SetActive(false);
        for( int i = 0; i < spawnPositionsRoot.childCount; i++)
        {
            spawnPositions.Add(spawnPositionsRoot.GetChild(i));     //스폰 포지션 위치 저장
        }
    }
    private void Start()
    {
        UpgradeStatInit();
        StartCoroutine("StartNextWave");
    }
    IEnumerator StartNextWave()
    {
        while(true)
        {
            if(currentSpawnCount == 0)  //소환되어잇는 적의 수 (첫 시작, 모두퇴치의 경우)
            {
                UpdateWaveUI();
                yield return new WaitForSeconds(2f);
                if(currentWaveIndex % 20 == 0)
                {
                    RandomUpgrade();
                }
                if(currentWaveIndex % 10 == 0)
                {
                    //현재 스폰 지점의 수를 하나 늘리는데 총 스폰지점의 수보다 크면 불가
                    waveSpawnPosCount = waveSpawnPosCount + 1 > spawnPositions.Count ? waveSpawnPosCount : waveSpawnPosCount + 1;
                    waveSpawnCount = 0;  //동시에 스폰되는 적의 수
                }

                if(currentWaveIndex % 5 == 0)
                {
                    CreateReward();
                }
                if(currentWaveIndex % 3 == 0)
                {
                    waveSpawnCount += 1;
                }

                for(int i = 0; i < waveSpawnPosCount; i++) //위치 갯수
                {
                    int posIdx = Random.Range(0, spawnPositions.Count);
                    for(int j  = 0; j < waveSpawnCount; j++)  // 적의 갯수
                    {
                        int prefabIdx = Random.Range(0, enemyprefebs.Count);
                        GameObject enemy = Instantiate(enemyprefebs[prefabIdx], spawnPositions[posIdx].position, Quaternion.identity);
                        enemy.GetComponent<HealthSystem>().OnDeath += OnEnemyDeath;
                        enemy.GetComponent<CharacterStatsHandler>().AddStatModifier(defaultStats);
                        enemy.GetComponent<CharacterStatsHandler>().AddStatModifier(rangedStats);
                        currentSpawnCount++;
                        yield return new WaitForSeconds(spawnInterval);
                    }
                }
                currentWaveIndex++;
            }
            yield return null;
        }
    }

    private void OnEnemyDeath()
    {
        currentSpawnCount--;
    }

    private void UpdateHealthUI()
    {
        hpGaugeSlider.value = playerHealthSystem.CurrentHealth / playerHealthSystem.MaxHealth;
    }

    private void GameOver()
    {
        gameOverUI.SetActive(true);
        StopAllCoroutines();
    }
    private void UpdateWaveUI()
    {
        waveText.text = (currentWaveIndex + 1).ToString();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);   //현재 실행되고 있는 씬의 인덱스 번호를 가져와 실행함(제시작), 씬은 번호로 관리됨
    }
    public void ExitGame()
    {
        Application.Quit();
    }

    void CreateReward()
    {
        int idx = Random.Range(0, rewards.Count);
        int posIdx = Random.Range(0, spawnPositions.Count);

        GameObject obj = rewards[idx];
        Instantiate(obj, spawnPositions[posIdx].position, Quaternion.identity);
    }

    void UpgradeStatInit()
    {
        defaultStats.statsChangeType = StatsChangeType.Add;
        defaultStats.attackSO = Instantiate(defaultStats.attackSO);

        rangedStats.statsChangeType = StatsChangeType.Add;
        rangedStats.attackSO = Instantiate(defaultStats.attackSO);

    }

    void RandomUpgrade()
    {
        switch (Random.Range(0, 6))
        {
            case 0:
                defaultStats.maxHealth += 2;
                break;
            case 1:
                defaultStats.attackSO.power += 1;
                break;
            case 2:
                defaultStats.speed += 0.1f;
                break;
            case 3:
                defaultStats.attackSO.isOnKnockback = true;
                defaultStats.attackSO.knockbackPower += 1;
                defaultStats.attackSO.knockbackTime += 0.1f;
                break;
            case 4:
                defaultStats.attackSO.delay -= 0.05f;
                break;
            case 5:
                RangedAttackData rangedAttackData = rangedStats.attackSO as RangedAttackData;
                rangedAttackData.numberofProjectilesPerShot += 1;
                break;

            default:
                break;

        }
    }
}
