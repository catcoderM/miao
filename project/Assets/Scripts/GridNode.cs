using UnityEngine;
using System.Collections;

[System.Serializable]   //序列化
public class MapData
{
    public enum FieldTypeID
    {
        GuardPosition,
        CanNotStand,
    }
    public FieldTypeID fieldType = FieldTypeID.GuardPosition;
}
public class GridNode : MonoBehaviour
{
    public MapData m_mapdata;

    void OnDrawGizmos()
    {
        Gizmos.DrawIcon(this.transform.position, "gridnode.tif");
    }
}



    

