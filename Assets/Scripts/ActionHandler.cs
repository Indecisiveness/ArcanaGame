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
            int move;
            string[] effects;

            switch (args[0])
            {
                case "move1":
                    move = Int32.Parse(args[2]);
                    for (int i = 3; i < args.Length; i++)
                    {
                        tags.Add(args[i]);
                    }
                    if (args[1] == "self")
                    {
                        yield return StartCoroutine(MoveUnit(attacker.currentTile,self, move, tags));
                    }
                    else
                    {
                        if (actionList.actions[action].shape == "blast")
                        {
                            yield return StartCoroutine(MoveUnit(tiles[0], defender, move, tags));
                        }
                        else
                        {
                            yield return StartCoroutine(MoveUnit(attacker.currentTile, defender, move, tags));
                        }
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

                    yield return StartCoroutine(Status(attacker, defender, effects, dur));
                    break;
                case "tile":
                    effects = new string[args.Length - 3];
                    for (int i = 3; i< args.Length; i++)
                    {
                        effects[i-3] = args[i];
                    }
                    yield return StartCoroutine(TileChange(tiles, args[1], Int32.Parse(args[2]), effects));
                    break;
                case "self":
                    int quant = (args.Length - 1) / 2;
                    effects = new string[quant];
                    int[] duration = new int[quant];
                    for (int i = 0; i < quant; i++)
                    {
                        effects[i] = args[i + 1];
                        duration[i] = Int32.Parse(args[quant + i + 1]);
                    }
                    yield return StartCoroutine(SelfEffect(attacker, effects, duration));
                    break;
                default:
                    Debug.Log("error in move");
                    break;
            }
        }

        if (attacker.faction == "player")
        {
            if (attacker.status.ContainsKey("another"))
            {
                attacker.status.Remove("another");
               
            }
            else if (attacker.status.ContainsKey("double"))
            {
                attacker.status.Remove("double");
                attacker.status.Add("another", 1);
            }
            else
            {
                turnOrder.TakeTurn();
            }
        }

    }

    IEnumerator MoveUnit (GameObject center, List<UnitInfo> targets, int distance, List<string> mods)
    {
        Debug.Log("moving targts");

        foreach (UnitInfo target in targets)
        {

            int persDist = distance;

            Debug.Log("moving target: " + target.name);

            int x = (int)target.transform.position.x;
            int z = (int)target.transform.position.z;

            int ele = target.elevation;

            int moved = 0;

            if (mods.Contains("free"))
            {
                moveCalc.selected = target;
                mods.Add(target.faction);
                moveCalc.MoveFinder(distance, target.currentTile, mods);
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
                    string tileName;
                    int tileX = x;
                    int tileZ = z;

                    tileName = "x" + tileX + "y" + tileZ;

                    Debug.Log("start position: " + tileName);

                    int centerX = (int)center.transform.position.x;
                    int centerZ = (int)center.transform.position.z;


                    while (persDist != 0)
                    {
                        int diffX = tileX - centerX;
                        int diffZ = tileZ - centerZ;

                        if (diffX == 0 && diffZ == 0)
                        {
                            break;
                        }

                        if (Mathf.Abs(diffX) > Mathf.Abs(diffZ))
                        {
                            if ((diffX > 0 && mods.Contains("push")) || (diffX < 0 && mods.Contains("pull")))
                            {
                                tileName = "x" + (tileX + 1) + "y" + tileZ;
                                tileX++;
                                persDist--;
                            }
                            else
                            {
                                tileName = "x" + (tileX - 1) + "y" + tileZ;
                                tileX--;
                                persDist--;
                            }
                        }
                        else
                        {
                            if ((diffZ > 0 && mods.Contains("push")) || (diffZ < 0 && mods.Contains("pull")))
                            {
                                tileName = "x" + tileX + "y" + (tileZ + 1);
                                tileZ++;
                                persDist--;
                            }
                            else
                            {
                                tileName = "x" + tileX + "y" + (tileZ-1);
                                tileZ--;
                                persDist--;
                            }
                        }



                        GameObject nextTile = mapParent.transform.Find(tileName).gameObject;

                        if (nextTile == null)
                        {
                            Debug.Log("edge of map");
                            break;
                        }



                        if ((int)(nextTile.transform.position.y) > (ele + 2) || nextTile.GetComponent<TileTextInfo>().currOcc != null)
                        {
                            Debug.Log("move blocked");
                            break;
                        }

                        Debug.Log("moving to: " + nextTile.name);

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

        if (tags.Contains("replenish"))
        {
            int num = attacker.currMP;
            attacker.SpendMP(num);
            defenders[0].SpendMP(-num);

        }


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

            if (attacker.status.ContainsKey("bloodinversion"))
            {
                if (attacker.maxHP/attacker.currHP > 2)
                {
                    multi *= attacker.maxHP / attacker.currHP;
                }
                else
                {
                    multi *= 2;
                }
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

            if (attacker.status.ContainsKey("combo"))
            {
                damageNum *= 1.3f;
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
                if (defender.passives.Contains("Counterblow") && !attacker.passives.Contains("Counterblow"))
                {
                    List<UnitInfo> att = new List<UnitInfo>();
                    att.Add(attacker);
                    List<string> blank = new List<string>();
                    StartCoroutine(Damage(defender, att, "swords", blank));
                }
            }
            if (tags.Contains("area")) {
                if (defender.currentTile.tag == "Water")
                {
                    damageNum *= .75f;
                }
                if (defender.passives.Contains("Evade"))
                {
                    damageNum *= .75f;
                }
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
                if (tags.Contains("MPDam"))
                {
                    defender.SpendMP((int)damageNum);
                    if (tags.Contains("Siphon"))
                    {
                        attacker.SpendMP((int)-damageNum);
                    }

                    turnHist.AddMessage(attacker.name + " drains " + damageNum + " MP from " + defender.name);
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

            
            
        }

        yield return new WaitForSeconds(0.0f);
    }

    IEnumerator Status (UnitInfo attacker, List<UnitInfo> targets, string[] effects, int[] duration)
    {
        if (effects[0] == "swap")
        {
            if (targets.Count == 2)
            {
                GameObject tile0 = targets[0].currentTile;
                GameObject tile1 = targets[1].currentTile;
                cubeClick.MoveToTile(tile0, targets[1].gameObject);
                cubeClick.MoveToTile(tile1, targets[0].gameObject);
            }
            else if (targets.Count == 1)
            {
                GameObject tile0 = attacker.currentTile;
                GameObject tile1 = targets[0].currentTile;
                cubeClick.MoveToTile(tile0, targets[0].gameObject);
                cubeClick.MoveToTile(tile1, attacker.gameObject);
            }

        }

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
                else if (effects[i] == "MPRegen")
                {
                    target.MPRegen();
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

    IEnumerator SelfEffect(UnitInfo self, string[] effects, int[] duration)
    {

        Dictionary<string, int> currentStatus = self.status;
        for (int i = 0; i < effects.Length; i++)
        {

            if (effects[i].Contains("HP"))
            {
                self.statMods[0] += Int32.Parse(effects[i].Substring(2));
                self.modDur[0] = duration[i];
            }
            else if (effects[i].Contains("MP"))
            {
                self.statMods[1] += Int32.Parse(effects[i].Substring(2));
                self.modDur[1] = duration[i];
            }
            else if (effects[i].Contains("swords"))
            {
                self.statMods[2] += Int32.Parse(effects[i].Substring(6));
                self.modDur[2] = duration[i];
            }
            else if (effects[i].Contains("cups"))
            {
                self.statMods[3] += Int32.Parse(effects[i].Substring(4));
                self.modDur[3] = duration[i];
            }
            else if (effects[i].Contains("wands"))
            {
                self.statMods[4] += Int32.Parse(effects[i].Substring(5));
                self.modDur[4] = duration[i];
            }
            else if (effects[i].Contains("pent"))
            {
                self.statMods[5] += Int32.Parse(effects[i].Substring(4));
                self.modDur[5] = duration[i];
            }
            else if (effects[i].Contains("move"))
            {
                self.statMods[6] += Int32.Parse(effects[i].Substring(4));
                self.modDur[6] = duration[i];
            }
            else if (effects[i] == "bloodinversion")
            {
                int amt = self.currHP - 1;
                self.currHP = 1;
                self.currMP += amt;
                currentStatus.Add("bloodinversion", 3);
            }
            else if (effects[i] == "chakraswap")
            {
                int hold = self.currHP;
                self.currHP = self.currMP;
                self.currMP = hold;
            }
            else if (effects[i] == "MPRegen")
            {
                self.MPRegen();
            }
            else if (effects[i] == "dispel")
            {
                currentStatus.Clear();
            }
            else if (effects[i] == "widen")
            {
                if (currentStatus.ContainsKey("widen"))
                {
                    currentStatus.Remove("widen");
                }
                else
                {
                    currentStatus.Add("widen", 1);
                }
            }
            else
            {
                string name = effects[i];
                currentStatus.Add(name, duration[i]);
            }
        }

        self.status = currentStatus;


        yield return new WaitForSeconds(0.0f);
    }


}
