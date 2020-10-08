using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static TankSuperClass;

public class TurnManager : MonoBehaviour
{
    public static TurnManager instance;
    private void Awake()
    {
        instance = this;
    }
    public GameObject target;
    public Team team;
    Vector3 targetPos;
    RaycastHit hit;
    public int turnState;
    public Stat stat;
    public GameObject turnEndButton;
    public GameObject unitPanel;
    public Text unitStat;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetPosition();
    }

    void GetPosition()
    {
        if (Input.GetMouseButtonUp(0))
        {
            // 마우스로 찍은 위치의 좌표 값을 가져온다
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 10000f))
            {
                targetPos = hit.point;

                switch (turnState)
                {
                    case 0:
                        if(SelectTank())
                        {
                            turnState = 1;
                            turnEndButton.SetActive(true);
                        }
                        else
                        {

                        }
                        break;
                    case 1:
                        if (SelectTank())
                        {
                            
                        }
                        else if (SelectEnemyTank())
                        {
                            turnState = 2;
                        }
                        else if (SelectPos())
                        {
                            turnState = 2;
                        }
                        else
                        {
                            
                        }
                        break;
                    case 2:
                        if (SelectEnemyTank())
                        {
                            
                        }
                        else if (SelectPos())
                        {

                        }
                            break;
                    case 3:
                        unitPanel.SetActive(false);
                        break;

                }

                
            }
        }
    }

    public void ResetTurn()
    {
        turnState = 0;
    }

    bool SelectTank()
    {
        if (hit.collider.gameObject.GetComponent<TankSuperClass>() != null)
        {
            if (team == hit.collider.gameObject.GetComponent<TankSuperClass>().team)
            {
                if (hit.collider.gameObject.GetComponent<TankSuperClass>().turnUsed == false)
                {
                    if (target != null)
                    {
                        target.GetComponent<TankSuperClass>().selected = false;
                    }
                    target = hit.collider.gameObject;
                    target.GetComponent<TankSuperClass>().selected = true;
                    UIManager.instance.SetMessage(target.name + "을 선택하였습니다.");
                    stat = target.GetComponent<TankSuperClass>().stat;
                    unitPanel.SetActive(true);
                    unitStat.text = "이름 : " + target.name + "\n 공격 포인트 : " + stat.attackPoint + "\n 방어 포인트 : " + stat.defencePoint + "\n 이동 포인트 : " + stat.movePoint;
                    return true;
                }
            }
        }
        return false;
    }
    bool SelectEnemyTank()
    {
        if (hit.collider.gameObject.GetComponent<TankSuperClass>() != null)
        {
            if (team != hit.collider.gameObject.GetComponent<TankSuperClass>().team)
            {
                if(target.GetComponent<TankSuperClass>().type == Type.SelfPropelledArtillery)
                {
                    turnEndButton.SetActive(false);
                    BattleManager.instance.StartBattle(target, hit.collider.gameObject);
                    return true;
                }
                else if (target.GetComponent<TankSuperClass>().type == Type.TankDestroyer)
                {
                    RaycastHit rayHit;
                    int mask = 1 << 8;
                    mask = ~mask;
                    float dis = Vector3.Distance(target.transform.position, hit.collider.gameObject.transform.position);
                    if (Physics.Raycast(target.transform.position, hit.collider.gameObject.transform.position - target.transform.position, out rayHit, dis, mask))
                    {

                    }
                    else
                    {
                        turnEndButton.SetActive(false);
                        BattleManager.instance.StartBattle(target, hit.collider.gameObject);
                        return true;

                    }

                }
                else
                {
                    Vector3 attPos = target.GetComponent<TankSuperClass>().targetPos;
                    Vector3 defPos = hit.collider.gameObject.GetComponent<TankSuperClass>().targetPos;
                    if (Mathf.Abs(attPos.x - defPos.x) == 1 && attPos.z == defPos.z)
                    {
                        turnEndButton.SetActive(false);
                        BattleManager.instance.StartBattle(target, hit.collider.gameObject);
                        return true;
                    }
                    else if (Mathf.Abs(attPos.z - defPos.z) == 1 && attPos.x == defPos.x)
                    {
                        turnEndButton.SetActive(false);
                        BattleManager.instance.StartBattle(target, hit.collider.gameObject);
                        return true;
                    }
                    
                    
                }
            }
        }
        return false;
    }
    bool SelectPos()
    {
        targetPos.x = Mathf.Round(targetPos.x);
        targetPos.z = Mathf.Round(targetPos.z);
        if (targetPos.x < 0)
        {
            return false;
        }
        if (targetPos.x > 15)
        {
            return false;
        }
        if (targetPos.z < 0)
        {
            return false;
        }
        if (targetPos.z > 15)
        {
            return false;
        }

        Vector3 nowPos = target.transform.position;
        nowPos.x = Mathf.Round(nowPos.x);
        nowPos.z = Mathf.Round(nowPos.z);
        if (targetPos.x - nowPos.x == -1 && targetPos.z == nowPos.z) {
            if (MapManager.instance.mapPoint[(int)nowPos.x - 1, (int)nowPos.z] == 0)
            {
                if (stat.movePoint > 0)
                {
                    UIManager.instance.SetMessage(targetPos.x + "," + targetPos.z + "로 이동, 남은 이동포인트 : " + --stat.movePoint);
                    target.GetComponent<TankSuperClass>().Move((int)targetPos.x, (int)targetPos.z);
                
                    return true;
                }
            }
        }
        if (targetPos.x - nowPos.x == 1 && targetPos.z == nowPos.z)
        {
            if (MapManager.instance.mapPoint[(int)nowPos.x + 1, (int)nowPos.z] == 0)
            {
                if (stat.movePoint > 0)
                {
                    UIManager.instance.SetMessage(targetPos.x + "," + targetPos.z + "로 이동, 남은 이동포인트 : " + --stat.movePoint);
                    target.GetComponent<TankSuperClass>().Move((int)targetPos.x, (int)targetPos.z);

                    return true;
                }
            }
        }
        if (targetPos.x == nowPos.x && targetPos.z - nowPos.z == 1)
        {
            if (MapManager.instance.mapPoint[(int)nowPos.x, (int)nowPos.z + 1] == 0)
            {
                if (stat.movePoint > 0)
                {
                    UIManager.instance.SetMessage(targetPos.x + "," + targetPos.z + "로 이동, 남은 이동포인트 : " + --stat.movePoint);
                    target.GetComponent<TankSuperClass>().Move((int)targetPos.x, (int)targetPos.z);

                    return true;
                }
            }
        }
        if (targetPos.x == nowPos.x && targetPos.z - nowPos.z == -1)
        {
            if (MapManager.instance.mapPoint[(int)nowPos.x, (int)nowPos.z - 1] == 0)
            {
                if (stat.movePoint > 0)
                {
                    UIManager.instance.SetMessage(targetPos.x + "," + targetPos.z + "로 이동, 남은 이동포인트 : " + --stat.movePoint);
                    target.GetComponent<TankSuperClass>().Move((int)targetPos.x, (int)targetPos.z);

                    return true;
                }
            }
        }


        return false;
    }

    public void TurnEnd()
    {
        Debug.Log("W");
        turnState = 3;
        target.GetComponent<TankSuperClass>().turnUsed = true;
        turnEndButton.SetActive(false);
        if (RoundManager.instance.CheckRedTurn())
        {
            UIManager.instance.SetMessage("레드팀의 모든 유닛이 턴을 마쳤습니다.");
            UIManager.instance.SetMessage("다음라운드는 레드부터 시작합니다.");
            turnState = 0;
            RoundManager.instance.roundState = 0;
        }
        if (RoundManager.instance.CheckBlueTurn())
        {
            UIManager.instance.SetMessage("블루팀의 모든 유닛이 턴을 마쳤습니다.");
            UIManager.instance.SetMessage("다음라운드는 블루부터 시작합니다.");
            turnState = 0;
            RoundManager.instance.roundState = 1;
        }
        target.GetComponent<TankSuperClass>().selected = false;
        target = null;
    }
}
