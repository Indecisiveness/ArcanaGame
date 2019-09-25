using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AIBehavior : MonoBehaviour
{
    public MoveCalc moveCalc;
    public ActionDescriptions actDesc;
    public ActionHandler actHand;
    public CubeClick cubeClick;
    public TurnOrder turnOrder;
    public TurnHistory turnHistory;

    int value;
    GameObject moveTile;
    GameObject targetCenter;
    UnitInfo targetUnit;
    List<UnitInfo> allTargets = new List<UnitInfo>();
    List<GameObject> listTiles = new List<GameObject>();
    


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator TakeAction(UnitInfo unit)
    {
        bool actionTaken = false;

        //if (unit.faction == "enemy")
        //{

            string actionName = "";

            List<string> actionNames = unit.actionNames;

            for (int i = 0; i<unit.actionNames.Count; i++)
            {
                actionName = actionNames[0];
                UnitInfo.Action thisAction = actDesc.actions[actionName];
                if (thisAction.target == "self")
                {
                    MoveClosestEnemy(unit);

                    yield return new WaitForSeconds(2.0f);

                    List<UnitInfo> self = new List<UnitInfo>();
                    self.Add(unit);
                    List<GameObject> selfTile = new List<GameObject>();
                    selfTile.Add(unit.currentTile);

                    yield return actHand.ActionInterpreter(unit, actionNames[0], self, selfTile);
                   turnHistory.AddMessage(unit.name + " used " + actionName);
                    actionTaken = true;
                    break;
                }
                if (thisAction.target == "enemy")
                {
                    if (CanReach(unit, thisAction.range, thisAction.rad, "enemy"))
                    { 
                        List<UnitInfo> targ = new List<UnitInfo>();
                        targ.Add(targetUnit);

                        List<GameObject> tile = new List<GameObject>();
                        tile.Add(targetUnit.currentTile);

                        cubeClick.MoveToTile(moveTile, unit.gameObject);
                        yield return new WaitForSeconds(2.0f);

                    turnHistory.AddMessage(unit.name + " used " + actionName + " on " + targetUnit.name);
                    yield return actHand.ActionInterpreter(unit, actionName, targ, tile);
                    

                        actionTaken = true;
                        break;
                    }
                }
                if (thisAction.target == "ally")
                {
                    if (CanReach (unit, thisAction.range, thisAction.rad, "ally"))
                    {

                        List<UnitInfo> targ = new List<UnitInfo>();
                        targ.Add(targetUnit);

                        List<GameObject> tile = new List<GameObject>();
                        tile.Add(targetUnit.currentTile);

                         cubeClick.MoveToTile(moveTile, unit.gameObject);
                         yield return new WaitForSeconds(2.0f);

                        yield return actHand.ActionInterpreter(unit, actionName, targ, tile);
                        actionTaken = true;
                        break;
                    }
                }
                else
                {
                    if (CanReach (unit, thisAction.range, thisAction.rad, thisAction.target))
                    {

                        List<GameObject> tile = listTiles;
                        
                        foreach (UnitInfo targ in allTargets)
                        {
                            turnHistory.AddMessage(unit.name + " uses " + thisAction.name + " on " + targ.name);
                            tile.Add(targ.currentTile);
                        }

                        cubeClick.MoveToTile(moveTile, unit.gameObject);

                        yield return new WaitForSeconds(2.0f);

                        yield return actHand.ActionInterpreter(unit, actionName, allTargets, tile);
                        actionTaken = true;
                        break;
                    }
                }

                actionNames.Remove(actionName);
                actionNames.Add(actionName);
            }

            if (actionTaken == true)
            {
                actionNames.Remove(actionName);
                actionNames.Add(actionName);
                unit.actionNames = actionNames;
            }
            else
            {
                turnHistory.AddMessage(unit.name + " moves");
                MoveClosestEnemy(unit);
                yield return new WaitForSeconds(2.0f);
            }
       // }
    }

    private void MoveClosestEnemy(UnitInfo unit)
    {
        //Move towards closest enemy
        List<GameObject> allUnits = new List<GameObject>();
        allUnits.AddRange(turnOrder.currentOrder);

        int i = 256;
        UnitInfo enemy = unit;

        foreach(GameObject other in allUnits)
        {
            if (other.GetComponent<UnitInfo>().faction != unit.faction)
            {
                int xdif = Mathf.Abs((int) (unit.transform.position.x - other.transform.position.x));
                int zdif = Mathf.Abs((int)(unit.transform.position.z - other.transform.position.z));
                if ((xdif+zdif) < i)
                {
                    i = xdif + zdif;
                    enemy = other.GetComponent<UnitInfo>();
                }
            }
        }

        moveCalc.MoveFinder(unit.move, unit.currentTile, unit.status.Keys);
        List<GameObject> possTiles = moveCalc.selectedTiles;
        

        GameObject closeTile = unit.currentTile;

        int dis = 256;

        foreach (GameObject tile in possTiles)
        {
            int xdif = Mathf.Abs((int)(enemy.transform.position.x - tile.transform.position.x));
            int zdif = Mathf.Abs((int)(enemy.transform.position.z - tile.transform.position.z));
            if ((xdif + zdif) < dis)
            {
                dis = xdif + zdif;
                closeTile = tile;
            }
        }

        moveCalc.HighlightClear();
        cubeClick.MoveToTile(closeTile, unit.gameObject);


    }

    private bool CanReach (UnitInfo unit, int range, int radius, string target)
    {
        moveCalc.MoveFinder(unit.move, unit.gameObject, unit.status.Keys);
        List<GameObject> positions = new List<GameObject>(); 
        positions.AddRange(moveCalc.selectedTiles);
        moveCalc.HighlightClear();


        string[] targVals = target.Split(new char[] {' '});


        List<string> cond = new List<string>();
        cond.Add("noTerrain");
        cond.Add("noElevation");

        value = 0;

        foreach (GameObject tile in positions)
        {
            if (radius == 0)
            {
                List<UnitInfo> possible = moveCalc.BlastTargetFinder(range, tile);


                if (unit.faction == "enemy" && target == "enemy")
                {
                    foreach(UnitInfo posTar in possible)
                    {
                        if (posTar.faction != "enemy")
                        {
                            moveTile = tile;
                            targetUnit = posTar;
                            return true;
                        }
                    }
                }
                if (unit.faction == "enemy" && target == "ally")
                {
                    foreach(UnitInfo posTar in possible)
                    {
                        if (posTar.faction == "enemy")
                        {
                            moveTile = tile;
                            targetUnit = posTar;
                            return true;
                        }
                    }
                }
            }
            else
            {
                
                moveCalc.MoveFinder(range, tile, cond);
                List<GameObject> centers = new List<GameObject>();
                centers.AddRange(moveCalc.selectedTiles);
                moveCalc.HighlightClear();



                foreach(GameObject center in centers)
                {
                    List<UnitInfo> hits = moveCalc.BlastTargetFinder(radius, center);

                    int i = 0;


                    foreach (UnitInfo hit in hits)
                    {
                        if (unit.faction == "enemy" && targVals[0] == "enemies" && hit.faction != "enemy")
                        {
                            i++;
                        }
                        else if (unit.faction == "enemy" && targVals[0] == "allies" && hit.faction == "enemy")
                        {
                            i++;
                        }
                    }

                    if (targVals.Length > 1)
                    {
                        foreach (UnitInfo hit in hits)
                        {
                            if (unit.faction == "enemy" && targVals[1] == "enemies" && hit.faction != "enemy")
                            {
                                i--;
                            }
                            else if (unit.faction == "enemy" && targVals[1] == "allies" && hit.faction == "enemy")
                            {
                                i--;
                            }
                        }
                    }

                    if (i > value)
                    {
                        value = i;
                        moveTile = tile;
                        targetCenter = center;
                        allTargets.Clear();
                        allTargets.AddRange(hits);
                        moveCalc.MoveFinder(radius, center, cond);
                        listTiles = new List<GameObject>();
                        listTiles.AddRange(moveCalc.selectedTiles);
                        moveCalc.HighlightClear();
                    }

                }


                if (value > 0)
                {
                    return true;
                }
            }
        }

        return false;

    }

}
