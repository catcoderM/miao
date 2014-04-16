using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour {

    [System.Serializable]
    public class EnemyTable
    {
        public string enemyName = "";
        public Transform enemyPrefab;
    }

    public class SpawnData
    {
        public int wave = 1;
        public string enemyname = "";
        public int level = 1;
        public float wait = 1.0f;
    }

    public EnemyTable[] m_enemies;
    public PathNode m_startNode;
    public TextAsset xmlData;

    ArrayList m_enemyList;

    float m_timer = 0;
    int m_index = 0;

    public int m_liveEnemy = 0;

    void Start()
    {
        ReadXml();

        SpawnData data = (SpawnData)m_enemyList[m_index];
        m_timer = data.wait;
    }

    private void ReadXml()
    {
        m_enemyList = new ArrayList();

        XMLParser xmlParse = new XMLParser();
        XMLNode node = xmlParse.Parse(xmlData.text);

        XMLNodeList list = node.GetNodeList("ROOT>0>table");
        for (int i = 0; i < list.Count; i++)
        {
            string wave = node.GetValue("ROOT>0>table>" + i + ">@wave");
            string enemyname = node.GetValue("ROOT>0>table>" + i + ">@enemyname");
            string level = node.GetValue("ROOT>0>table>" + i + ">@level");
            string wait = node.GetValue("ROOT>0>table>" + i + ">@wait");
           
            SpawnData data = new SpawnData();
            data.wave = int.Parse(wave);
            data.enemyname = enemyname;
            data.level = int.Parse(level);
            data.wait = float.Parse(wait);

            m_enemyList.Add(data);
        }
    }

    void Update()
    {
        SpawnEnemy();
    }

    private void SpawnEnemy()
    {
        if (m_index >= m_enemyList.Count)
        {
            return;
        }
     
        SpawnData data = (SpawnData)m_enemyList[m_index];
        if (GameManager.instance.m_wave < data.wave && m_liveEnemy > 0)
        {
            return;
        }
        m_timer -= Time.deltaTime;
        if (m_timer > 0)
            return;

      
        if (GameManager.instance.m_wave < data.wave)
        {
            GameManager.instance.SetWave(data.wave);
        }
        Debug.Log(1);
        Transform enemyprefab = FindEnemy(data.enemyname);
        if (enemyprefab != null)
        {
            Transform trans = (Transform)Instantiate(enemyprefab,this.transform.position,Quaternion.identity);
            Enemy enemy = trans.GetComponent<Enemy>();
            enemy.m_currentNode = m_startNode;
            enemy.m_spawn = this;

            enemy.transform.LookAt(m_startNode.transform);
            float ry = enemy.transform.eulerAngles.y;
            enemy.transform.eulerAngles = new Vector3(0,ry,0);

        }
        m_index++;

        if (m_index >= m_enemyList.Count)
        {
            return;
        }
        SpawnData nextdata = (SpawnData)m_enemyList[m_index];
        m_timer = data.wait;
    }

    Transform FindEnemy(string name)
    {
        foreach (EnemyTable enemy in m_enemies)
        {
            if (enemy.enemyName.CompareTo(name) == 0)
            {
                return enemy.enemyPrefab;
            }
        }
        return null;
    }
}
