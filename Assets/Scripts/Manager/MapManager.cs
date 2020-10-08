using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public static MapManager instance;
    private void Awake()
    {
        instance = this;
    }

    public int[,] mapPoint = new int[16, 16];
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CheckMap();
        }
    }

    void CheckMap()
    {
        for (int j = 0; j < 16; j++)
        {
            for (int i = 0; i < 16; i++)
            {
                if (mapPoint[i, j] != 0)
                    Debug.Log(i + "," + j + " : " + mapPoint[i, j]);
            }
        }
    }
}
