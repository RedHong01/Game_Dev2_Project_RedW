using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ObjectGoals : MonoBehaviour
{
    public TextMeshPro textAmount;
    public int maxObjects = 5;
    public Vector3 cubeSize;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        int count = 0;
        // box cast transform providing the center of the cube
        RaycastHit[] hits = Physics.BoxCastAll(transform.position, cubeSize / 2f, Vector3.up, Quaternion.identity, cubeSize.y);
        foreach(RaycastHit hit in hits)
        {
            ObjectCharacters objChar = hit.transform.GetComponent<ObjectCharacters>();
            if (objChar != null)
            {
                count++;
            }
        }
        Debug.DrawLine(transform.position, transform.position + Vector3.up * cubeSize.y, Color.red);
        Debug.DrawRay(transform.position, Vector3.up * cubeSize.y, Color.green);
        textAmount.text = string.Format("objects {0} / {1}", count, maxObjects);
    }
   
        //simpler version 
        //textAmount.text = $"Objects {count} / {maxObjects};

}


