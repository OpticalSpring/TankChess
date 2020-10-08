using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceObject : MonoBehaviour
{
    public GameObject[] Mesh;
    public GameObject UpMesh;
    public int UpNum;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        for (int i = 0; i < 6; i++)
        {
           if(UpMesh.transform.position.y < Mesh[i].transform.position.y)
            {
                UpMesh = Mesh[i];
                   UpNum = i;
            }
        }
    }
}
