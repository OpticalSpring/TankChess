using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class TankSuperClass : MonoBehaviour
{
    public enum Team
    {
        redTeam,
        blueTeam
    }
    public Team team;

    public bool turnUsed;
    public enum Type
    {
        LightTank,
        MediumTank,
        HeavyTank,
        SuperHeavyTank,
        TankDestroyer,
        SelfPropelledArtillery
    }public Type type;

    [System.Serializable]
    public struct Stat
    {
        public int attackPoint;
        public int defencePoint;
        public int movePoint;
    }public Stat stat;
    public int movePointNow;
    public float movePointValue;
    public Vector3 targetPos;
    public Image UIImage;
    public bool selected;
    // Start is called before the first frame update
    void Start()
    {
        movePointNow = stat.movePoint;
        Vector3 nowPos = gameObject.transform.position;
        nowPos.x = Mathf.Round(nowPos.x);
        nowPos.z = Mathf.Round(nowPos.z);
        MapManager.instance.mapPoint[(int)nowPos.x,(int)nowPos.z] = 1;
        targetPos = new Vector3((int)nowPos.x,0, (int)nowPos.z);
    }

    // Update is called once per frame
    void Update()
    {
        if (turnUsed == true)
        {
            movePointNow = stat.movePoint;
            UIImage.color = Color.red;
        }else if(selected == true)
        {
            UIImage.color = Color.green;
        }
        else
        {
            
            UIImage.color = Color.white;
        }
        movePointValue = Mathf.Lerp(movePointValue, movePointNow, Time.deltaTime * 5);
        UIImage.fillAmount = movePointValue / stat.movePoint;
    }

    public void Hit()
    {
        Vector3 nowPos = gameObject.transform.position;
        nowPos.x = Mathf.Round(nowPos.x);
        nowPos.z = Mathf.Round(nowPos.z);
        MapManager.instance.mapPoint[(int)nowPos.x,(int)nowPos.z] = 0;
        Destroy(gameObject);
    }


    public void Move(int x, int y)
    {
        movePointNow--;
        Vector3 nowPos = gameObject.transform.position;
        nowPos.x = Mathf.Round(nowPos.x);
        nowPos.z = Mathf.Round(nowPos.z);
        MapManager.instance.mapPoint[(int)nowPos.x, (int)nowPos.z] = 0;
        MapManager.instance.mapPoint[x, y] = 1;
        targetPos = new Vector3(x, 0, y);
        GetComponent<NavMeshAgent>().SetDestination(targetPos);
        
    }
}
