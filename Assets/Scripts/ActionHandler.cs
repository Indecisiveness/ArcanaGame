//Class for executing a selected action

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ActionHandler : MonoBehaviour
{

    public TurnOrder turnOrder;
    public MoveCalc moveCalc;
    public GameObject mapParent;
    public CubeClick cubeClick;
    public TurnHistory turnHist;

    public GameObject dirt;
    public GameObject forest;
    public GameObject sand;
    public GameObject water;
    public GameObject stone;

    public ActionDescriptions actionList;

    // Start is called before the first frame update
    void Start()
    {
        

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public IEnumerator ActionInterpreter(UnitInfo attacker, string action, List<UnitInfo> units, List<GameObject> tiles)
    {

        List<UnitInfo> self = new List<UnitInfo>();
        self.Add(attacker);

        List<UnitInfo> defender = new List<UnitInfo>();
        if (units.Count > 0)
        {
            for (int i = 0; i < units.Count; i++)
            {
               defender.Add(units[i]);
            }
        }

        string[] behavior = actionList.actionEffects[action];


        foreach (string s in behavior)
        {
            string[] args = s.Split(',');

            List<string> tags = new List<string>();
            int[] move;
            string[] effects;

            switch (args[0])
            {
                case "move1":
                    move = new int[1];
                    move[0] = Int32.Parse(args[2]);
                    for (int i = 3; i < args.Length; i++)
                    {
                        tags.Add(args[i]);
                    }
                    if (args[1] == "self")
                    {
                     
                        yield return StartCoroutine(MoveUnit(self, move, tags));
                    }
                    else
                    {
                        yield return StartCoroutine(MoveUnit(defender, move, tags));
                    }
                    break;
                case "move2":
                  
                    move = new int[2];
                    move[0] = Int32.Parse(args[2]);
                    move[1] = Int32.Parse(args[3]);

                    for (int i = 4; i < args.Length; i++)
                    {
                        tags.Add(args[i]);
                    }
                    if (args[1] == "self")
                    {
                        yield return StartCoroutine(MoveUnit(self, move, tags));
                    }
                    else
                    {  
                        yield return StartCoroutine(MoveUnit(defender, move, tags));
                    }
                    break;
                case "attack":
                    for (int i = 2; i<args.Length; i++)
                    {
                        tags.Add(args[i]);
                    }
                    yield return StartCoroutine(Damage(attacker, defender, args[1], tags));
                    break;
                case "status":
                    int num = (args.Length - 1) / 2;
                    effects = new string[num];
                    int[] dur = new int[num];
                    for (int i = 0; i < num; i++)
                    {
                        effects[i] = args[i + 1];
                        dur[i] = Int32.Parse(args[num + i + 1]);
                    }

                    yield return StartCoroutine(Status(defender, effects, dur));
                    break;
                case "tile":
                    effects = new string[args.Length - 3];
                    for (int i = 3; i< args.Length; i++)
                    {
                        effects[i-3] = args[i];
                    }
                    yield return StartCoroutine(TileChange(tiles, args[1], Int32.Parse(args[2]), effects));
                    break;
                default:
                    Debug.Log("error in move");
                    break;
            }
        }

        if (attacker.faction == "player")
        {
            turnOrder.TakeTurn();
        }

    }

    IEnumerator MoveUnit (List<UnitInfo> targets, int[] distance, List<string> mods)
    {

        foreach (UnitInfo target in targets)
        {

            int x = (int)target.transform.position.x;
            int z = (int)target.transform.position.z;

            int ele = target.elevation;

            int moved = 0;

            if (mods.Contains("free"))
            {
                moveCalc.selected = target;
                moveCalc.MoveFinder(distance[0], target.currentTile, mods);
                moveCalc.moving = true;
                cubeClick.HideAllMenus();
                
              
            }

            else
            {
                if (mods.Contains("noElevation"))
                {
                    ele += 20;
                }


                if (mods.Contains("forced"))
                {
                    while (distance[0] != 0)
                    {
                        string tileName;
                        int tileX = x;
                        int tileZ = z;

                        if (distance[0] > 0)
                        {
                            tileName = "x" + (x + 1) + "y" + z;
                            tileX++;
                            distance[0]--;
                        }
                        else
                        {
                            tileName = "x" + (x - 1) + "y" + z;
                            tileX--;
                            distance[0]++;
                        }

                        GameObject nextTile = mapParent.transform.Find(tileName).gameObject;

                        if (nextTile == null)
                        {
                            break;
                        }

                        if ((int)(nextTile.transform.position.y) > (ele + 2))
                        {
                            break;
                        }

                        target.transform.position = new Vector3(tileX, (nextTile.transform.position.y + 1), tileZ);
                        target.currentTile.GetComponent<TileTextInfo>().currOcc = null;
                        target.currentTile = nextTile;
                        moved++;

                    }

                    while (distance[1] != 0)
                    {
                        string tileName;
                        int tileX = x;
                        int tileZ = z;

                        if (distance[1] > 0)
                        {
                            tileName = "x" + x + "y" + (z + 1);
                            tileZ++;
                            distance[1]--;
                        }
                        else
                        {
                            tileName = "x" + x + "y" + (z - 1);
                            tileZ--;
                            distance[1]++;
                        }

                        GameObject nextTile = mapParent.transform.Find(tileName).gameObject;

                        if (nextTile == null)
                        {
                            break;
                        }

                        if ((int)(nextTile.transform.position.y) > (ele + 2))
                        {
                            break;
                        }

                        target.transform.position = new Vector3(tileX, (nextTile.transform.position.y + 1), tileZ);
                        target.currentTile.GetComponent<TileTextInfo>().currOcc = null;
                        target.currentTile = nextTile;
                        moved++;

                    }
                }

            }
        }
        yield return StartCoroutine(MoveSelect());
    }


    IEnumerator MoveSelect()
    {
        yield return new WaitUntil(()=>moveCalc.moving == false);
    }



    IEnumerator Damage (UnitInfo attacker, List<UnitInfo> defenders, string attackStat, List<string> tags)
    {

        float multi = attacker.weaponMod;
        float levelMod = (1 + (float)(attacker.level - 1) / 3);
        float offVal = 10;

        foreach (UnitInfo defender in defenders)
        {

            float damageNum = 0;

            if (tags.Contains("spell"))
            {
                if (tags.Contains("area"))
                {
                    multi = 3 / (float)(defender.pentacles);
                    if (tags.Contains("healing"))
                    {
                        multi = .3f;
                    }
                }
                else
                {
                    multi = 5 / (float)(defender.pentacles);
                    if (tags.Contains("healing"))
                    {
                        multi = .5f;
                    }
                }
            }
            else if (attacker.status.ContainsKey("rage"))
            {
                multi *= 1.25f;
            }

            if (attackStat == "swords")
            {
                offVal = attacker.Swords();
            }

            if (attackStat == "cups")
            { 
                offVal = attacker.Cups();
            }

            if (attackStat == "wands")
            {
                offVal = attacker.Wands();
            }
            if (attackStat == "pentacles")
            {
                offVal = attacker.Pentacles();
            }



            damageNum = multi * levelMod * offVal/3;

            if (attacker.elevation > defender.elevation)
            {
                damageNum *= 1.25f;
            }
            if (defender.elevation > attacker.elevation)
            {
                damageNum *= .75f;
            }

            if (tags.Contains("melee"))
            {
                if (defender.currentTile.tag == "Forest")
                {
                    damageNum *= .75f;
                }
                if (defender.passives.Contains("Parry"))
                {
                    damageNum *= .75f;
                }
            }
            if (defender.currentTile.tag == "Water" && tags.Contains("area"))
            {
                damageNum *= .75f;
            }


            if (!tags.Contains("spell") && defender.status.ContainsKey("physBlock"))
            {
                damageNum -= (defender.level)/3;
            }

            if (defender.status.ContainsKey("careful") && damageNum > 0)
            {
                defender.status.Remove("careful");
                damageNum -= defender.level;
            }

            if (tags.Contains("spell") && defender.status.ContainsKey("shield")){
                defender.status.Remove("shield");
                damageNum = 0;
            }

            Debug.Log("Damage number: " + damageNum);

            if ((damageNum < 0))
            {
                damageNum = 0;
            }

            if (tags.Contains("healing"))
            {
                int heal = defender.Heal(damageNum);
                turnHist.AddMessage(attacker.name + " heals " + heal + " damage to " + defender.name);
            }
            else
            {
                int dam = defender.TakeDamage(damageNum);
                turnHist.AddMessage(attacker.name + " deals " + dam + " damage to " + defender.name);
                if (tags.Contains("drain"))
                {
                    attacker.Heal(dam / 2);
                    turnHist.AddMessage(attacker.name + " drained " + dam / 2 + " damage");
                }
            }

            
            
        }

        yield return new WaitForSeconds(0.0f);
    }

    IEnumerator Status (List<UnitInfo> targets, string[] effects, int[] duration)
    {
        foreach (UnitInfo target in targets)
        {
            Dictionary<string,int> currentStatus = target.status;
            for (int i = 0; i < effects.Length; i++)
            {
                if (effects[i].Contains("HP")) {
                    target.statMods[0] += Int32.Parse(effects[i].Substring(2));
                    target.modDur[0] = duration[i];
                }
                else if (effects[i].Contains("MP")) {
                    target.statMods[1] += Int32.Parse(effects[i].Substring(2));
                    target.modDur[1] = duration[i];
                }
                else if (effects[i].Contains("swords")) {
                    target.statMods[2] += Int32.Parse(effects[i].Substring(6));
                    target.modDur[2] = duration[i];
                }
                else if (effects[i].Contains("cups")) {
                    target.statMods[3] += Int32.Parse(effects[i].Substring(4));
                    target.modDur[3] = duration[i];
                }
                else if (effects[i].Contains("wands")) {
                    target.statMods[4] += Int32.Parse(effects[i].Substring(5));
                    target.modDur[4] = duration[i];
                }
                else if (effects[i].Contains("pent")) {
                    target.statMods[5] += Int32.Parse(effects[i].Substring(4));
                    target.modDur[5] = duration[i];
                }
                else if (effects[i].Contains("move")) {
                    target.statMods[6] += Int32.Parse(effects[i].Substring(4));
                    target.modDur[6] = duration[i];
                }

                else
                {
                    string name = effects[i];
                    int dur = duration[i];
                    currentStatus.Add(name,dur);
                }
            }

            target.status = currentStatus;
        }

        yield return new WaitForSeconds(0.0f);
    }

    IEnumerator TileChange (List<GameObject> targets, string terrainSwitch, int eleChange, string[] effects)
    {
        foreach (GameObject target in targets)
        {
            GameObject current = target;

            UnitInfo standing = current.GetComponent<TileTextInfo>().currOcc;

            if (terrainSwitch != null)
            {
                switch (terrainSwitch)
                {
                    case "dirt":
                        GameObject dirtCube = Instantiate(dirt, target.transform.position, dirt.transform.rotation, mapParent.transform);
                        dirtCube.name = target.name;
                        current = dirtCube;
                        Destroy(target);
                        break;
                    case "forest":
                        GameObject forestCube = Instantiate(forest, target.transform.position, dirt.transform.rotation, mapParent.transform);
                        forestCube.name = target.name;
                        current = forestCube;
                        Destroy(target);
                        break;
                    case "stone":
                        GameObject stoneCube = Instantiate(stone, target.transform.position, dirt.transform.rotation, mapParent.transform);
                        stoneCube.name = target.name;
                        current = stoneCube;
                        Destroy(target);
                        break;
                    case "sand":
                        GameObject sandCube = Instantiate(sand, target.transform.position, dirt.transform.rotation, mapParent.transform);
                        sandCube.name = target.name;
                        current = sandCube;
                        Destroy(target);
                        break;
                    case "water":
                        GameObject waterCube = Instantiate(water, target.transform.position, dirt.transform.rotation, mapParent.transform);
                        waterCube.name = target.name;
                        current = waterCube;
                        Destroy(target);
                        break;
                    default:
                        break;
                }
            }

            current.transform.position = new Vector3(current.transform.position.x, current.transform.position.y + eleChange, current.transform.position.z);
            if (standing != null)
            {
                current.GetComponent<TileTextInfo>().currOcc = standing;
                standing.currentTile = current;
            }


        }

        yield return new WaitForSeconds(0.0f);

    }


}
