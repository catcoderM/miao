using UnityEngine;
using System.Collections;

public class GridMap : MonoBehaviour {

    static public GridMap instance = null;
    public bool m_debug = false;

    public int MapSizeX = 32;
    public int MapSizeY = 32;

    public MapData[,] m_map;

    void Awake()
    {
        instance = this;
        this.BuildMap();
    }

    [ContextMenu("BuildMap")]
    public void BuildMap()
    { 
        m_map = new MapData[MapSizeX,MapSizeY];

        for (int i = 0; i < MapSizeX; i++)
        {
            for (int j = 0; j < MapSizeY; j++)
            {
                m_map[i, j] = new MapData();
            }
        }

        GameObject[] nodes = (GameObject[])GameObject.FindGameObjectsWithTag("gridnode");
        foreach (GameObject nodeObj in nodes)
        { 
            GridNode node = nodeObj.GetComponent<GridNode>();
            Vector3 pos = nodeObj.transform.position;
            if (pos.x >= MapSizeX || pos.z >= MapSizeY)
                continue;

            m_map[(int)pos.x,(int)pos.z].fieldType = node.m_mapdata.fieldType; 
        }
    }

    void OnDrawGizmos()
    {
        if (!m_debug || m_map == null)
            return;

        Gizmos.color = Color.blue;

        float height = 0;
        for (int i = 0; i < MapSizeX; i++)
        {
            Gizmos.DrawLine(new Vector3(i,height,0),new Vector3(i,height,MapSizeY));
        }

        for (int j = 0; j < MapSizeY; j++)
        {
            Gizmos.DrawLine(new Vector3(0, height, j), new Vector3(MapSizeX, height, j));
        }

       // Gizmos.color = Color.red;

        for (int i = 0; i < MapSizeX; i++)
        {
            for (int j = 0; j < MapSizeY; j++)
            {
                if (m_map[i, j].fieldType == MapData.FieldTypeID.CanNotStand)
                {
                    Gizmos.color = new Color(1, 0, 0, 0.5f);
                    Gizmos.DrawCube(new Vector3(i+0.5f,height,j+0.5f),new Vector3(1,height+0.1f,1));
                }
            }
        }
    }
}   
