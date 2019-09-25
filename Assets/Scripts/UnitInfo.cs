//class for storing info about a particular unit
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitInfo : MonoBehaviour
{

    public struct Action
    {
        public string name;
        public int range;
        public int rad;
        public string shape;
        public string desc;
        public string target;
        public int cost;

        public Action(string n, int ran, int radius, string sh, string d, string t, int c)
        {
            name = n;
            range = ran;
            rad = radius;
            shape = sh;
            desc = d;
            target = t;
            cost = c;

        }

    }

    public string faction;

    public float level;
    public int currHP;
    public int maxHP;
    public int currMP;
    public int maxMP;
    public float baseMP;
    public float baseHP;
    public float swords;
    public float cups;
    public float wands;
    public float pentacles;
    public int move;
    public int elevation;

    public List<string> actionNames;

    public List<string> passives;

    public Dictionary<string, int> status = new Dictionary<string, int>();

    public float weaponMod;
    public int weaponRange;

    public int[] statMods = new int[7];
    public int[] modDur = new int[7];

    public GameObject currentTile;

    TurnOrder turnOrder;


    // Start is called before the first frame update
    void Start()
    {
        maxHP = 15 + (int)(level * (cups / 3 + baseHP));
        maxMP = 5 + (int)(((level - 1) / 4 + 1) * (pentacles / 5 + baseMP));
        currHP = maxHP;
        currMP = maxMP;

        turnOrder = gameObject.transform.parent.GetComponent<TurnOrder>();

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TurnStart()
    {
        currMP += (int)baseMP;
        if (currMP > maxMP)
        {
            currMP = maxMP;
        }

        Dictionary<string, int> newStatus = new Dictionary<string, int>();

        foreach (string effect in status.Keys)
        {
            if (effect == "immobile")
            {
                GameObject.Find("Move").GetComponent<Button>().interactable = false;
            }

            if (effect == "poison")
            {
                currHP -= (int)(level);
            }

            if (effect == "disarm")
            {
            }

            if (effect == "regen")
            {
                currHP += (int)(level + baseHP);
            }
            if (effect == "seal")
            {
                currMP -= (int)(baseMP);
            }


            int dur = status[effect] - 1;
            if (dur != 0)
            {
                newStatus.Add(effect, dur);
            }
        }

        if (passives.Contains("Flying"))
        {
            newStatus.Add("noElevation", 1);
            newStatus.Add("noTerrain", 1);
        }


        status = newStatus;

        for(int i = 0; i<statMods.Length; i++)
        {
            if(modDur[i] == 0)
            {
                statMods[i] = 0;
                
            }
            else if (modDur[i] > 0)
            {
                modDur[i]--;
            }
        }


    }

    public int TakeDamage(float n)
    {

        int damage = (int)Mathf.Round(n);

        currHP -= damage;
        if (currHP <= 0)
        {
            turnOrder.Remove(gameObject);
            
        }
        return damage;
    }

    public int Heal(float n)
    {
        int heal = (int)Mathf.Round(n);

        int ogHP = currHP;

        currHP += heal;
        if (currHP > maxHP)
        {
            currHP = maxHP;
        }

        return (currHP - ogHP);

    }

    public void SpendMP(int n)
    {
        currMP -= n;
    }

    public float Swords()
    {
        if (passives.Contains("Unholy Might"))
        {
            return ((swords + wands + statMods[2] + statMods[4]) * .6f);
        }
        else
        {
            return (swords + statMods[2]);
        }
    }

    public float Cups()
    {
        return (cups + statMods[3]);
    }

    public float Wands()
    {
        return (wands + statMods[4]);
    }

    public float Pentacles()
    {
        return (pentacles + statMods[5]);
    }
    

}
