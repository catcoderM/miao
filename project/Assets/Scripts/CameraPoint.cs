using UnityEngine;
using System.Collections;

public class CameraPoint : MonoBehaviour {

    public static CameraPoint instance;

    void Awake()
    {
        instance = this;
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawIcon(transform.position,"CameraPoint.tif");
    }
}
