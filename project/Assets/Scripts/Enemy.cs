using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

    public PathNode m_currentNode;

    public int m_life = 15;
    public int m_maxLife = 15;
    public float m_speed = 2;
    public EnemySpawner m_spawn;

    public Transform m_lifebBarObject;
    protected LifeBar m_Bar;
    public enum TYPE_ID
    { 
        GROUND,
        AIR,
    }

    public TYPE_ID m_type = TYPE_ID.GROUND;
    void Start()
    {
        m_spawn.m_liveEnemy++;
        GameManager.instance.m_enemyList.Add(this);

        Transform obj = (Transform)Instantiate(m_lifebBarObject, this.transform.position, Quaternion.identity);
        m_Bar = obj.GetComponent<LifeBar>();
        m_Bar.Init(m_life,m_maxLife,2,0.2f);
    }
    // Update is called once per frame
	void Update () {
        RotateTo();
        MoveTo();
	}
    public float _dist = 1.0f;
    public void MoveTo()
    {
        Vector3 pos1 = this.transform.position;
        Vector3 pos2 = m_currentNode.transform.position;

        float dist = Vector2.Distance(new Vector2(pos1.x,pos1.z),new Vector2(pos2.x,pos2.z));
        if (dist < _dist)
        {
            if (m_currentNode.m_next == null)
            {
                GameManager.instance.SetDamage(1);
                Destroy(this.gameObject);
            }
            else
            {
                m_currentNode = m_currentNode.m_next;
            }
        }
        this.transform.Translate(new Vector3(0, 0, m_speed * Time.deltaTime));
        m_Bar.SetPosition(this.transform.position,1.0f);
    }

    public void RotateTo()
    {
        float current = this.transform.eulerAngles.y;

        this.transform.LookAt(m_currentNode.transform);
        Vector3 target = this.transform.eulerAngles;
        float next = Mathf.MoveTowardsAngle(current,target.y,120*Time.deltaTime);
        this.transform.eulerAngles = new Vector3(0,next,0);
    }
    void OnDisable()
    {
        if (m_spawn)
        {
            m_spawn.m_liveEnemy--;
        }
        if (GameManager.instance)
        {
            GameManager.instance.m_enemyList.Remove(this);
        }

        if (m_Bar)
        {
            Destroy(m_Bar.gameObject);
        }
    }

    public void setDamage(int m_power)
    {
        m_life -= m_power;
        if (m_life <= 0)
        {
            GameManager.instance.m_enemyList.Remove(this);
            GameManager.instance.SetPoint(2);
            Destroy(this.gameObject);
        }
        else
        {
            m_Bar.UpdateLife(m_life,m_maxLife);
        }
    }
}
