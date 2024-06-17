using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public TextMeshProUGUI healthText;
    public TextMeshProUGUI WaveCount;
    public static GameManager Instance { get; private set; }

    [SerializeField] private int WaveNumber = 1;
    [SerializeField] private int spawnEnemies = 3;
    [SerializeField] private int Enemycount;
    [SerializeField] private List<GameObject> Enemies;

    [SerializeField] private int Enemiesleft;
    public GameObject EnemyPrefab;

    public float spawnOffset = 5f;

    private Vector3 screenBounds;
    private bool stopIt = true;

    private float timer;
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        SetWaveCount();
        SpawnWave();
    }

    void Update()
    {
        if (Enemiesleft == 0 && stopIt)
        {
            stopIt = false;
            StartCoroutine(NextWave());
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("quitting...");
            Application.Quit();
        }
    }

    public void SetHealh(float health)
    {
        if (health < 0)
        {
            healthText.text = "Hp: 0";
        }
        else
        {
            healthText.text = $"Hp: {health}";
        }

    }

    void SetWaveCount()
    {
        WaveCount.text = $"Wave: {WaveNumber}";
    }

    IEnumerator NextWave()
    {
        AudioManager.Instance.PlaySFX("next_wave");
        yield return new WaitForSeconds(5);
        WaveNumber++;
        SetWaveCount();
        SpawnWave();
    }

    void SpawnWave()
    {
        Enemycount = WaveNumber * spawnEnemies;
        Enemiesleft = Enemycount;

        StartCoroutine(SpawnEnemy(1));
        stopIt = true;
    }

    IEnumerator SpawnEnemy(int i)
    {
        // randomize sides
        int side = UnityEngine.Random.Range(1, 3);
        float spawnSide = side == 1 ? screenBounds.x + spawnOffset : -screenBounds.x - spawnOffset;
        //spawn orcie
        Enemies.Add(Instantiate(EnemyPrefab, new Vector2(spawnSide, -3.75f), quaternion.identity));

        // fill up till that enemy count
        if (i < Enemycount)
        {
            i++;
            int randomTime = UnityEngine.Random.Range(1, 4);
            yield return new WaitForSeconds(randomTime);
            StartCoroutine(SpawnEnemy(i));
        }
    }

    public void EnemyDied(GameObject enemy)
    {
        Enemies.Remove(enemy);
        Enemiesleft--;
    }
}
