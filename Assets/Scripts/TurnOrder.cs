//Class for changing display of action order
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnOrder : MonoBehaviour
{

    public CubeClick cubeClick;

    public List<GameObject> currentOrder = new List<GameObject>();

    public Button moveButton;

    public Text current;
    public Text next;
    public Text third;
    public ActionDisplay actDis;
    public MoveCalc moveCalc;
    public AIBehavior aiBehavior;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowOrder()
    {
        cubeClick.HideAllMenus();
        current.text = currentOrder[0].name;
        next.text = currentOrder[1].name;
        third.text = currentOrder[2].name;

        moveButton.interactable = true;
        currentOrder[0].transform.Find("Pointer").gameObject.SetActive(true);
        currentOrder[0].GetComponent<UnitInfo>().TurnStart();

        if (currentOrder[0].GetComponent<UnitInfo>().faction == "enemy")
        {
            StartCoroutine(TakeEnemyTurn());
        }

    }

    public void TakeTurn()
    {
        actDis.acting = false;
        moveCalc.moving = false;
        GameObject turnTaker = currentOrder[0];
        currentOrder.RemoveAt(0);
        currentOrder.Add(turnTaker);
        turnTaker.transform.Find("Pointer").gameObject.SetActive(false);
        ShowOrder();
  
    }

    public IEnumerator TakeEnemyTurn()
    {
        GameObject acting = currentOrder[0];
        Debug.Log("acting enemy is: " + acting.name);

        yield return aiBehavior.TakeAction(acting.GetComponent<UnitInfo>());
        currentOrder.RemoveAt(0);
        currentOrder.Add(acting);
        acting.transform.Find("Pointer").gameObject.SetActive(false);
        ShowOrder();
    }

    public void Remove(GameObject unit)
    {
        currentOrder.Remove(unit);
        Object.Destroy(unit);
    }

}
