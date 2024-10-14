using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeClickManager : MonoBehaviour
{
    public Camera myCamera;

    public float sphereCastRadius = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CubeClick[] allCubes = FindObjectsOfType<CubeClick>();
            foreach (CubeClick cube in allCubes)
            {
                cube.Reset();
            }
        }
        Vector2 mousePosition = Input.mousePosition;
        //
        Ray worldRay = Camera.main.ScreenPointToRay(mousePosition);

        // Version 1 - get the Frist object hit
        //if (Physics.Raycast(worldRay, out RaycastHit hitInfo))
        //{
        //    CubeClick clicked = hitInfo.transform.GetComponent<CubeClick>();
        //    if (clicked != null)
        //    {
        //        clicked.OnClick();
        //    }
        //}

        //Version two get all objects hit along the way of the ray
        //RaycastHit[] hits = Physics.RaycastAll(worldRay);
        //    //
        ////RaycastHit[] hits = Physics.RaycastAll(worldRay);
        //foreach (RaycastHit hit in hits)
        //{
        //    CubeClick clicked = hit.transform.GetComponent<CubeClick>();
        //    if (clicked != null)
        //    {
        //        clicked.OnClick();
        //    }
        //}

        //Version 3 get all objects hit within a certian radius of the ray
        RaycastHit[] hits = Physics.SphereCastAll(worldRay, sphereCastRadius);
        //
        //RaycastHit[] hits = Physics.RaycastAll(worldRay);
        foreach (RaycastHit hit in hits)
        {
            //checking whether is it a obstacle and is it blocking or not
            CubeClickObstacle obstacle = hit.transform.GetComponent<CubeClickObstacle>();
                if (obstacle != null &&
                    obstacle.isBlcoking)
            {
                break;
            }

            CubeClick clicked = hit.transform.GetComponent<CubeClick>();
            if (clicked != null)
            {
                clicked.OnClick();
            }
        }
    }
}
