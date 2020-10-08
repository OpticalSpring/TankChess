using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Vector3 nowPos = gameObject.transform.position;
        MapManager.instance.mapPoint[(int)nowPos.x,(int)nowPos.z] = 2;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
