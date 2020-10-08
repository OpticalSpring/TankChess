using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static BattleManager instance;
    private void Awake()
    {
        instance = this;
    }
    public GameObject attacker;
    public GameObject defender;
    public int attackPoint;
    public int defencePoint;
    public GameObject attackDice;
    public GameObject defenceDice;
    public GameObject attackPos;
    public GameObject defencePos;
    public int state;
    public int aplus;
    public int dplus;
    public GameObject diceButton;
    public GameObject turnButton;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case 1:
                state++;
                
                UIManager.instance.SetMessage("공격자의 주사위를 굴려주세요.");
                break;
            case 2:
                CheckAttackCritical();
                break;
            case 3:
                state++;
                UIManager.instance.SetMessage("방어자의 주사위를 굴려주세요.");
                break;
            case 4:
                CheckDefenceCritical();
                break;
            case 5:
                state++;
                UIManager.instance.SetMessage("공수를 합산합니다.");
                CheckBattleResult();
                break;
            case 6:

                break;
        }
    }

    public void StartBattle(GameObject att, GameObject def)
    {
        UIManager.instance.SetMessage(def.name + "을 대상으로 공격 개시");
        attacker = att;
        defender = def;
        attackPoint = att.GetComponent<TankSuperClass>().stat.attackPoint;
        defencePoint = def.GetComponent<TankSuperClass>().stat.defencePoint;
        state = 1;
        aplus = 0;
        dplus = 0;
        diceButton.SetActive(true);
        turnButton.SetActive(true);
    }


    public void RollDice()
    {
        switch (state)
        {
            case 2:
                RollAttackDice();
                break;

            case 4:
                RollDefenceDice();
                break;
        }

    }

    public void RollAttackDice()
    {

        for (int i = 0; i < attackPoint + aplus; i++)
        {
            if (attackDice.transform.GetChild(i).gameObject.activeSelf == false)
            {
                attackDice.transform.GetChild(i).position = attackPos.transform.position + new Vector3(0, 0, i);
                attackDice.transform.GetChild(i).eulerAngles = new Vector3(1, 1, 1) * Random.Range(0, 360);
                attackDice.transform.GetChild(i).gameObject.SetActive(true);
                attackDice.transform.GetChild(i).gameObject.GetComponent<Rigidbody>().AddForce(Vector3.forward * Random.Range(40, 60), ForceMode.VelocityChange);
            }
        }
    }

    public void RollDefenceDice()
    {

        for (int i = 0; i < defencePoint + dplus; i++)
        {
            if (defenceDice.transform.GetChild(i).gameObject.activeSelf == false)
            {
                defenceDice.transform.GetChild(i).position = defencePos.transform.position + new Vector3(0, 0, i);
                defenceDice.transform.GetChild(i).eulerAngles = new Vector3(1, 1, 1) * Random.Range(0, 360);
                defenceDice.transform.GetChild(i).gameObject.SetActive(true);
                defenceDice.transform.GetChild(i).gameObject.GetComponent<Rigidbody>().AddForce(Vector3.forward * Random.Range(40, 60), ForceMode.VelocityChange);
            }
        }
    }

    void CheckAttackCritical()
    {
        aplus = 0;
        for (int i = 0; i < attackPoint + aplus; i++)
        {
            if (attackDice.transform.GetChild(i).gameObject.activeSelf == true)
            {
                if (attackDice.transform.GetChild(i).gameObject.GetComponent<DiceObject>().UpNum == 5)
                {
                    aplus++;
                }
            }
        }
    }
    void CheckDefenceCritical()
    {
        dplus = 0;
        for (int i = 0; i < defencePoint + dplus; i++)
        {
            if (defenceDice.transform.GetChild(i).gameObject.activeSelf == true)
            {
                if (defenceDice.transform.GetChild(i).gameObject.GetComponent<DiceObject>().UpNum == 5)
                {
                    dplus++;
                }
            }
        }
    }

    public void TurnEnd()
    {
        state++;
    }

    void CheckBattleResult()
    {
        int anum = 0;
        int dnum = 0;
        for (int i = 0; i < attackPoint + aplus; i++)
        {

            if (attackDice.transform.GetChild(i).gameObject.activeSelf == true)
            {
                if(attacker.GetComponent<TankSuperClass>().type == TankSuperClass.Type.SelfPropelledArtillery ||
                   attacker.GetComponent<TankSuperClass>().type == TankSuperClass.Type.TankDestroyer)
                {
                    if (attackDice.transform.GetChild(i).gameObject.GetComponent<DiceObject>().UpNum >= 3)
                    {
                        anum++;
                    }
                }
                else
                {
                    if (attackDice.transform.GetChild(i).gameObject.GetComponent<DiceObject>().UpNum <= 2 ||
                        attackDice.transform.GetChild(i).gameObject.GetComponent<DiceObject>().UpNum == 5)
                    {
                        anum++;
                    }
                }
                
            }

        }

        for (int i = 0; i < defencePoint + dplus; i++)
        {
            if (defenceDice.transform.GetChild(i).gameObject.activeSelf == true)
            {
                if (defenceDice.transform.GetChild(i).gameObject.GetComponent<DiceObject>().UpNum >= 4)
                {
                    dnum++;
                }
                
            }
        }

        UIManager.instance.SetMessage("공격수치 : " + anum + "/방어수치 : " + dnum);
        if(anum > dnum)
        {
            UIManager.instance.SetMessage("공격 성공, 타겟이 파괴됩니다.");
            defender.GetComponent<TankSuperClass>().Hit();
        }
        else
        {
            UIManager.instance.SetMessage("공격 실패, 도탄되었습니다.");
        }

        diceButton.SetActive(false);
        turnButton.SetActive(false);

        for (int i = 0; i < attackDice.transform.childCount; i++)
        {
            attackDice.transform.GetChild(i).gameObject.SetActive(false);
        }
        for (int i = 0; i < defenceDice.transform.childCount; i++)
        {
            defenceDice.transform.GetChild(i).gameObject.SetActive(false);
        }

        TurnManager.instance.TurnEnd();
    }
}
