using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    public static RoundManager instance;
    private void Awake()
    {
        instance = this;
    }

    public GameObject[] redTeamTank;
    public GameObject[] blueTeamTank;
    public int roundState;
    void Start()
    {
        UIManager.instance.SetMessage("게임을 시작합니다.");
        StartCoroutine(RoundUpdate());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator RoundUpdate()
    {
        while (true)
        {
            switch (roundState)
            {
                case 0:
                    UIManager.instance.SetMessage("라운드 시작");
                    ResetUnitTurnUsed();
                    roundState = 10;
                    break;
                case 1:
                    UIManager.instance.SetMessage("라운드 시작");
                    ResetUnitTurnUsed();
                    roundState = 20;
                    break;
                case 10:
                    UIManager.instance.SetMessage("레드팀의 턴입니다.");
                    roundState = 11;
                    break;
                case 11:
                    
                    if (TurnManager.instance.turnState == 3)
                    {
                        roundState = 20;
                        TurnManager.instance.ResetTurn();
                        TurnManager.instance.team = TankSuperClass.Team.blueTeam;
                    }
                    break;
                case 20:
                    UIManager.instance.SetMessage("블루팀의 턴입니다.");
                    roundState = 21;
                    break;
                case 21:
                    
                    if (TurnManager.instance.turnState == 3)
                    {
                        roundState = 10;
                        TurnManager.instance.ResetTurn();
                        TurnManager.instance.team = TankSuperClass.Team.redTeam;
                    }
                    break;
                case 3:
                    break;
            }

            yield return new WaitForSeconds(1f);
        }
    }



    public bool CheckRedTurn()
    {
        for (int i = 0; i < redTeamTank.Length; i++)
        {
            if(redTeamTank[i] != null)
            {
                if(redTeamTank[i].GetComponent<TankSuperClass>().turnUsed == false)
                {
                    return false;
                }
            }
        }
        return true;
    }

    public bool CheckBlueTurn()
    {
        for (int i = 0; i < blueTeamTank.Length; i++)
        {
            if (blueTeamTank[i] != null)
            {
                if (blueTeamTank[i].GetComponent<TankSuperClass>().turnUsed == false)
                {
                    return false;
                }
            }
        }
        return true;
    }

    void ResetUnitTurnUsed()
    {
        for (int i = 0; i < redTeamTank.Length; i++)
        {
            if (redTeamTank[i] != null)
            {
                redTeamTank[i].GetComponent<TankSuperClass>().turnUsed = false;
            }
        }

        for (int i = 0; i < blueTeamTank.Length; i++)
        {
            if (blueTeamTank[i] != null)
            {
                blueTeamTank[i].GetComponent<TankSuperClass>().turnUsed = false;
            }
        }
    }
    
}
