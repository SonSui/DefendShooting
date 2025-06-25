using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class EnemyUIManager : MonoBehaviour
{
    public GameObject hpUIPrefab;
    public RectTransform canvasTransform;
    private Camera cam;

    public static EnemyUIManager instance;

    // �G��HP�\���p�̃G���g�����Ǘ�����N���X
    private class Entry
    {
        public BaseEnemy enemy;
        public TextMeshProUGUI text;
    }
    // �G��HP�\���p�̃G���g���̃��X�g
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
        // ��ʏ�̓G��HP�\�����X�V
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
        // �G��HP�\�����G���g���ɒǉ�
        entries.Add(entry);

        // �G�̈ʒu��HP�\����z�u
        enemy.OnHPChanged += (e, hp) =>
        {
            text.text = hp.ToString();
        };

        // �G�����S�����Ƃ���HP�\�����폜
        enemy.OnDead += (e) =>
        {
            text.text = "";
            Destroy(text.gameObject);
            entries.RemoveAll(en => en.enemy == e);
        };

        text.text = enemy.GetHealth().ToString();
    }
}