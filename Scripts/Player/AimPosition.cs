using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimPosition : MonoBehaviour
{
    //the object to move
    public Transform aimPositionObj;
    //the layers the ray can hit
    public LayerMask hitLayers;
    void Update()
    {
        Vector3 mouse = Input.mousePosition;
        Ray castPoint = Camera.main.ScreenPointToRay(mouse);
        RaycastHit hit;
        if (Physics.Raycast(castPoint, out hit, Mathf.Infinity, ~hitLayers))
        {
            aimPositionObj.transform.position = hit.point;
        }
    }
}