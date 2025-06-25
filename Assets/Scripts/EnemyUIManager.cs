using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class EnemyUIManager : MonoBehaviour
{
    public GameObject hpUIPrefab;
    public RectTransform canvasTransform;
    private Camera cam;

    public static EnemyUIManager instance;

    // 敵のHP表示用のエントリを管理するクラス
    private class Entry
    {
        public BaseEnemy enemy;
        public TextMeshProUGUI text;
    }
    // 敵のHP表示用のエントリのリスト
    private readonly List<Entry> entries = new();

    void Awake()
    {
        cam = Camera.main;
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        // 画面上の敵のHP表示を更新
        foreach (var entry in entries)
        {
            if (entry.enemy == null) continue;

            Vector3 worldPos = entry.enemy.transform.position;
            Vector3 screenPos = cam.WorldToScreenPoint(worldPos);
            entry.text.transform.position = screenPos;
        }
    }

    public void RegisterEnemy(BaseEnemy enemy)
    {
        GameObject go = Instantiate(hpUIPrefab, canvasTransform);
        var text = go.GetComponent<TextMeshProUGUI>();

        Entry entry = new Entry { enemy = enemy, text = text };
        // 敵のHP表示をエントリに追加
        entries.Add(entry);

        // 敵の位置にHP表示を配置
        enemy.OnHPChanged += (e, hp) =>
        {
            text.text = hp.ToString();
        };

        // 敵が死亡したときにHP表示を削除
        enemy.OnDead += (e) =>
        {
            text.text = "";
            Destroy(text.gameObject);
            entries.RemoveAll(en => en.enemy == e);
        };

        text.text = enemy.GetHealth().ToString();
    }
}