using UnityEngine;
using System.Collections;

public class Defender : MonoBehaviour {

    Enemy m_targetEnemy;

    public float m_attackArea = 4.0f;
    public int m_power = 1;
    public float m_attackTime = 1.0f;

    public float m_timer = 0;
	// Use this for initialization
	void Start () {
	    GridMap.instance.m_map[(int)this.transform.position.x,(int)this.transform.position.z].fieldType = MapData.FieldTypeID.CanNotStand;
	}
	
	// Update is called once per frame
	void Update () {
        FindEnemy();
        Rotate();
        Attack();
	}

    private void Attack()
    {
        m_timer -= Time.deltaTime;

        if (m_timer > 0 || null== m_targetEnemy)
            return;

        m_targetEnemy.setDamage(m_power);
        m_timer = m_attackTime;
    }

    private void Rotate()
    {
        if (m_targetEnemy != null)
        {
            Vector3 current = this.transform.eulerAngles;
            this.transform.LookAt(m_targetEnemy.transform);
            Vector3 target = this.transform.eulerAngles;
            float next = Mathf.MoveTowardsAngle(current.y,target.y,120*Time.deltaTime);

            this.transform.eulerAngles = new Vector3(current.x,next,current.z);
        }
    }

    private void FindEnemy()
    {
        m_targetEnemy = null;
        int lastlife = 0;
        foreach (Enemy enemy in GameManager.instance.m_enemyList)
        {
            if (enemy.m_life == 0)
            {
                continue;
            }

            Vector3 pos1 = this.transform.position;
            Vector3 pos2 = enemy.transform.position;
            float dist = Vector2.Distance(new Vector2(pos1.x,pos1.z),new Vector2(pos2.x,pos2.z));

            if (dist > m_attackArea)
                continue;

            if (lastlife == 0 || lastlife > enemy.m_life)
            {
                m_targetEnemy = enemy;
            }
            lastlife = enemy.m_life;

        }
    }
}
