using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class EnemyManager : MonoBehaviour
{
    [System.Serializable]
    public class SpawnData
    {
        public float time;
        public Vector2 pos;
        public string type;
    }

    public TextAsset enemyTableFile;
    [System.Serializable]
    public class EnemyTypeEntry
    {
        public string type;
        public GameObject prefab;
    }
    public List<EnemyTypeEntry> enemyTypes;
    private Dictionary<string, GameObject> enemyTypeDict;

    private float elapsedTime = 0f;
    private int nextIndex = 0;
    private List<SpawnData> spawnList = new List<SpawnData>();
    private float offsetTime = 0f;

    public float MAX_LIFE_RATE = 2f;
    private float currentLifeRate = 1f;
    public float MAX_SPEED_RATE = 3f;
    private float currentSpeedRate = 1f;
    public float MIN_TIME_RATE = 0.1f;
    private float currentTimeRate = 1f;
    private int currentLoopTime = 0;


    public static EnemyManager instance;

    public bool isBoss = false;
    public GameObject bossPrefab;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        
        enemyTypeDict = new Dictionary<string, GameObject>();
        foreach (var entry in enemyTypes)
        {
            if (!enemyTypeDict.ContainsKey(entry.type))
            {
                enemyTypeDict.Add(entry.type.ToUpper(), entry.prefab);
            }
            else
            {
                Debug.LogWarning($"Duplicate enemy type key: {entry.type}");
            }
        }
        LoadSpawnTable();
        elapsedTime = 0f;
        nextIndex = 0;
        offsetTime = 0f;
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;
        if (!isBoss)
        {
            // 外部ファイルからのスポーンデータを使用して敵をスポーン
            while (nextIndex < spawnList.Count && spawnList[nextIndex].time * currentTimeRate  + offsetTime <= elapsedTime)
            {
                SpawnEnemy(spawnList[nextIndex]);
                nextIndex++;
            }
            if (nextIndex >= spawnList.Count)
            {
                nextIndex = 0;
                offsetTime = elapsedTime;
                currentLoopTime++;
                currentLifeRate = Mathf.Min(MAX_LIFE_RATE, currentLifeRate + 0.1f);
                currentSpeedRate = Mathf.Min(MAX_SPEED_RATE, currentSpeedRate + 0.1f);
                currentTimeRate = Mathf.Max(MIN_TIME_RATE, currentTimeRate - 0.05f);
            }
        }
    }

    void LoadSpawnTable()
    {
        // 外部ファイルからスポーンデータを読み込む
        string path = Path.Combine(Application.streamingAssetsPath, "enemyTable.txt");

        if (!File.Exists(path))
        {
            Debug.LogError("Enemy table file not found at: " + path);
            return;
        }

        string[] lines = File.ReadAllLines(path);

        for (int i = 1; i < lines.Length; i++)
        {
            var cols = lines[i].Split(',');
            if (cols.Length < 4) continue;

            SpawnData data = new SpawnData
            {
                time = float.Parse(cols[0]),
                pos = new Vector2(float.Parse(cols[1]), float.Parse(cols[2])),
                type = cols[3].Trim().ToUpper()
            };
            spawnList.Add(data);
        }
    }
    
    void SpawnEnemy(SpawnData data)
    {
        // 指定されたタイプの敵をスポーン
        if (enemyTypeDict.TryGetValue(data.type.ToUpper(), out GameObject prefab))
        {
            Vector2 spawnPos = data.pos;
            GameObject e = Instantiate(prefab, spawnPos, Quaternion.identity);
            BaseEnemy baseEnemy = e.GetComponent<BaseEnemy>();
            if (baseEnemy != null)
            {
                baseEnemy.SetEnemyStatus(Mathf.RoundToInt(baseEnemy.GetHealthMax() * currentLifeRate), baseEnemy.GetSpeed() * currentSpeedRate);
                EnemyUIManager.instance?.RegisterEnemy(baseEnemy); // 敵のHP表示を登録
                // 敵が倒されたときのイベントを登録
                baseEnemy.OnDefeated += (score) =>
                {
                    ScoreManager.Instance.AddScore(score);
                };
            }
            else
            {
                Debug.LogWarning($"Enemy prefab {data.type} does not implement ITakeDamage interface.");
            }
        }
        else
        {
            Debug.LogWarning($"Unknown enemy type: {data.type}");
        }
    }
    public void SpawnBoss()
    {
        if (bossPrefab != null)
        {
            Instantiate(bossPrefab, new Vector3(12f, 0f, 0f), Quaternion.identity);
            isBoss = true;
        }
        else
        {
            Debug.LogError("Boss prefab is not assigned!");
        }
    }
}