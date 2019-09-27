using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionDisplay : MonoBehaviour
{


    public VerticalLayoutGroup actionList;  //Scroll list of actions
    public TurnOrder turnOrder; 
    public Button baseButton; //Button copied by button generator
    public Text descBox;
    public MoveCalc moveCalc;
    public ActionHandler actHand;

    public ActionDescriptions actDesc; //ScriptableObject containing extended descriptions

    UnitInfo.Action currAct;

    public bool acting;        //true if an action has been selected
    public bool unitTarget;    //true if an action targets a single unit, false if it targets an area

    List<UnitInfo> possTargets;

    UnitInfo unit;


    // Start is called before the first frame update
    void Start()
    {

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PopulateActions()
    {
        descBox.text = "Choose an action";

        unit = turnOrder.currentOrder[0].GetComponent<UnitInfo>();

        foreach (Transform child in actionList.transform)
        {
            if (child != baseButton.transform)
            {
                Destroy(child.gameObject);  //clear any old buttons
            }
        }

        foreach (string action in unit.actionNames)
        {
            if (action.Contains("Basic Attack"))
            {
                Text baseLabel = baseButton.transform.Find("Text").gameObject.GetComponent<Text>();
                baseLabel.text = action;
                baseButton.onClick.AddListener(delegate { ActionSend(action); });
                if (unit.status.ContainsKey("disarm"))
                {
                    baseButton.interactable = false;
                }
            }
            else if (!unit.status.ContainsKey("rage"))
            {
                if (!unit.status.ContainsKey("disable") || actDesc.actions[action].desc.Contains("spell"))
                {
                    if (!unit.status.ContainsKey("silence") || !(actDesc.actions[action].desc.Contains("spell")))
                    {
                        Button thisButton = Instantiate(baseButton, actionList.transform);
                        Text label = thisButton.transform.Find("Text").gameObject.GetComponent<Text>();
                        label.text = action;
                        thisButton.onClick.AddListener(delegate { ActionSend(action); });
                        thisButton.interactable = true;

                    }
                }
            }
        }

        actionList.transform.parent.parent.parent.gameObject.SetActive(true);

    }
  
    void ActionSend(string action)
    {

        if (acting)
        {
            CancelAction();
        }

        unit = turnOrder.currentOrder[0].GetComponent<UnitInfo>();

        UnitInfo.Action thisAct = actDesc.actions[action];

        if (unit.currMP < thisAct.cost)
        {
            return;
        }

        currAct = thisAct;

        int range = thisAct.range;

        int mp = thisAct.cost;

        if (thisAct.shape == "blast" && unit.status.ContainsKey("widen")){
            mp *= 2;
        }


        string desc = thisAct.desc + "\n\rMP Cost: " + mp;

        descBox.text = desc;
        
        List<string> cond = new List<string>();
        cond.Add("noTerrain");
        cond.Add("noElevation");
        cond.Add("enemy");
        cond.Add("player");

        List<UnitInfo> targets = moveCalc.SingleTargetFinder(unit, range, unit.currentTile, thisAct.target);
        foreach (UnitInfo target in targets)
            {
                target.transform.Find("Pointer").gameObject.SetActive(true);
            }
            possTargets = targets;
            unitTarget = true;

        if (!(thisAct.shape == "single" || thisAct.shape == "double"))
        {
            moveCalc.MoveFinder(range, unit.currentTile, cond);

            unitTarget = false;
        }

        if (thisAct.target == "self")
        {
            possTargets.Add(unit);
        }

        acting = true;
    }

    public void TargetPicked(GameObject target)
    {


        if (target.tag == "Unit" && !possTargets.Contains(target.GetComponent<UnitInfo>()))
        {
            Debug.Log("Invalid target");
            return;
        }

        Debug.Log("target name: " + target.name);

        CancelAction();

        foreach (UnitInfo possTar in possTargets)
        {
                possTar.transform.Find("Pointer").gameObject.SetActive(false);
        }

        string shape = currAct.shape;
        int radius = currAct.rad;

        if (unit.status.ContainsKey("widen"))
        {
            radius++;
        }

        UnitInfo targIn = new UnitInfo();
        GameObject center;

        if (target.tag == "Unit")
        {
            targIn = target.GetComponent<UnitInfo>();
            center = targIn.currentTile;
        }

        else
        {
            center = target;
        }

        if (unit.status.ContainsKey("widen"))
        {
            unit.SpendMP(currAct.cost *2);
        }
        else
        {
            unit.SpendMP(currAct.cost);
        }

        List<UnitInfo> units = new List<UnitInfo>();

        List<GameObject> tiles = new List<GameObject>();

        List<string> cond = new List<string>();
        cond.Add("noElevation");
        cond.Add("noTerrain");
        cond.Add("enemy");
        cond.Add("player");


        switch (shape)
        {
            case "single":
                units.Add(targIn);
                tiles.Add(center);
                Debug.Log("single action: " + currAct.name);
                StartCoroutine(actHand.ActionInterpreter(unit, currAct.name, units, tiles));
                break;
            case "blast":
                tiles.Add(center);
                moveCalc.HighlightClear();
                moveCalc.MoveFinder(radius, center, cond);
                tiles.AddRange(moveCalc.selectedTiles);
                moveCalc.HighlightClear();
                units.AddRange(moveCalc.BlastTargetFinder(radius, center));
                foreach (UnitInfo unit in units)
                {
                    tiles.Add(unit.currentTile);
                }
                StartCoroutine(actHand.ActionInterpreter(unit, currAct.name, units, tiles));
                break;
            default:
                Debug.Log("shape not set");
                break;
        }

        this.Hide();

    }

    public void Hide()  //make action bar invisible
    {
        actionList.transform.parent.parent.parent.gameObject.SetActive(false);  
    }

    public void CancelAction()
    {

        if (acting == false)
        {
            return;
        }
        foreach (UnitInfo target in possTargets)
        {
           target.transform.Find("Pointer").gameObject.SetActive(false);
        }
        possTargets.Clear();
        if (!unitTarget)
        {
            moveCalc.HighlightClear();
        }

        acting = false;

    }

}
