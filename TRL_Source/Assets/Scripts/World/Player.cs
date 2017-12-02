using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    [SerializeField]
    private Tank Tank;


    //-----------------------------Unity Functions-----------------------------

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            var mouseScreenPos = Input.mousePosition;
            var mousePosRay = Camera.main.ScreenPointToRay(mouseScreenPos);
            RaycastHit hit;
            if(Physics.Raycast(mousePosRay, out hit))
            {
                Tank.SetDestination(hit.point);
            }
        }

        if (Input.GetMouseButton(1))
        {
            var mouseScreenPos = Input.mousePosition;
            var mousePosRay = Camera.main.ScreenPointToRay(mouseScreenPos);
            RaycastHit hit;
            if (Physics.Raycast(mousePosRay, out hit))
            {
                if (hit.collider.GetComponent<Tank>())
                    Tank.Target.SetTargetByTransform(hit.transform);
                else
                    Tank.Target.SetTargetByPosition(hit.point);
            }
        }
    }
}
