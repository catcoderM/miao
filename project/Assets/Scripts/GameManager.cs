using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    [HideInInspector]
    public int m_wave = 1;
    public int m_life = 10;
    public int m_point = 0;

    GUIText m_txt_wave;
    GUIText m_txt_life;
    GUIText m_txt_point;


    public bool m_debug = false;
    public ArrayList m_pathNodes;

    public ArrayList m_enemyList = new ArrayList();

    CUIButton m_button;

    int m_ID;
    public Transform m_groundPrefab;
    public LayerMask m_groundlayer;
    void Awake()
    {
        instance = this;
    }

	// Use this for initialization
	void Start () {
        m_txt_life = this.transform.FindChild("txt_life").GetComponent<GUIText>();
        m_txt_point = this.transform.FindChild("txt_point").GetComponent<GUIText>();
        m_txt_wave = this.transform.FindChild("txt_wave").GetComponent<GUIText>();

        m_txt_wave.text = "<color=red>wave</color>" + m_wave;
        m_txt_life.text = "<color=red>life</color>" + m_life;
        m_txt_point.text = "<color=red>point</color>" + m_point;
        m_button = this.transform.FindChild("ui_turret_n").GetComponent<CUIButton>();
	}
	
	// Update is called once per frame
	void Update () {

        if (m_life <= 0)
            return;

        bool press = Input.GetMouseButton(0);
        bool mouseup = Input.GetMouseButtonUp(0);

        Vector3 mousepos = Input.mousePosition;
        float mx = Input.GetAxis("Mouse X");
        float my = Input.GetAxis("Mouse Y");

        if (m_ID > 0 && mouseup)
        {
            if (m_point < 5)
            {
                m_ID = 0;
                m_button.SetOnActive(false);
                return;
            }
            Ray ray = Camera.main.ScreenPointToRay(mousepos);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100, m_groundlayer))
            {
                int ix = (int)hit.point.x;
                int iz = (int)hit.point.z;
                if (ix >= GridMap.instance.MapSizeX
                    || iz >= GridMap.instance.MapSizeY || ix < 0 || iz < 0)
                    return;

                if (GridMap.instance.m_map[ix, iz].fieldType == MapData.FieldTypeID.GuardPosition)
                {
                    Vector3 pos = new Vector3((int)hit.point.x + 0.5f, 0, (int)hit.point.z + 0.5f);
                    Instantiate(m_groundPrefab, pos, Quaternion.identity);
                    m_ID = 0;
                    m_button.SetOnActive(false);
                    SetPoint(-5);
                }
            }
        }

      

        int id = m_button.UpdateState(mouseup,Input.mousePosition);

        if (id > 0)
        {
            m_ID = id;
            m_button.SetOnActive(true);
            return;
        }

        GameCamera.instance.Control(press,mx,my);
	}

    public void SetWave(int wave)
    {
        m_wave = wave;
        m_txt_wave.text = "<color=red>wave</color>" + m_wave;
    }

    public void SetDamage(int damage)
    {
        m_life -= damage;
        m_txt_life.text = "<color=red>life</color>" + m_life;
    }

    public void SetPoint(int point)
    {
        m_point += point;
        m_txt_point.text = "<color=red>point</color>" + m_point;
    }

    [ContextMenu("BuildMap")]
    void BuildMap()
    {
        m_pathNodes = new ArrayList();

        GameObject[] objs = GameObject.FindGameObjectsWithTag("pathnode");

        for (int i = 0; i < objs.Length; i++)
        { 
            PathNode node = objs[i].GetComponent<PathNode>();

            m_pathNodes.Add(node);
        }
    }

    void OnDrawGizmos()
    {
        if (!m_debug || m_pathNodes == null)
        {
            return;
        }

        Gizmos.color = Color.green;

        foreach (PathNode node in m_pathNodes)
        {
            if (node.m_next != null)
            {
                Gizmos.DrawLine(node.transform.position,node.m_next.transform.position);
            }
        }
    }
}
