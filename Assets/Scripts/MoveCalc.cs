//class for determining available squares to move to

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCalc : MonoBehaviour
{

    public Material highlight;
    public UnitInfo selected;
    public TurnOrder turnOrder;

    public List<GameObject> selectedTiles = new List<GameObject>();

    public bool moving;
    

    // Start is called before the first frame update
    void Start()
    {
        moving = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MoveClick()
    {
        moving = true;
        GameObject unit = turnOrder.currentOrder[0];
        selected = unit.GetComponent<UnitInfo>();
        MoveFinder(selected.move, selected.currentTile, selected.status.Keys);

    }

    public void MoveFinder(int moveLeft, GameObject current, ICollection<string> status)
    {
        if (moveLeft > 0)
        {
            checkupX(moveLeft, current, status);
            checkdownX(moveLeft, current, status);
            checkupZ(moveLeft, current, status);
            checkdownZ(moveLeft, current, status);
        }
        else return;

    }

    public void HighlightClear()
    {
        foreach (GameObject tile in selectedTiles)
        {
            Material[] mats = tile.GetComponent<Renderer>().materials;
            mats[1] = mats[0];
            tile.GetComponent<Renderer>().materials = mats;
        }
        selectedTiles.Clear();
        moving = false;
    }

    public List<UnitInfo> BlastTargetFinder (int radius, GameObject center)
    {
        List<UnitInfo> hits = new List<UnitInfo>();

        Vector3 position = center.transform.position;

        RaycastHit hit;

        for (int i = 0; i<= radius; i++)
        {
            for (int j = 0; (j+i)<=radius; j++)
            {
                if (Physics.Raycast(new Vector3(position.x + i, 10, position.z + j), new Vector3(0, -1, 0), out hit)){
                    if (hit.transform.gameObject.tag == "Unit")
                    {
                        hits.Add(hit.transform.parent.gameObject.GetComponent<UnitInfo>());
                    }
                }
                if (i > 0) {
                    if (Physics.Raycast(new Vector3(position.x - i, 10, position.z + j), new Vector3(0, -1, 0), out hit)){
                        if (hit.transform.gameObject.tag == "Unit")
                        {
                            hits.Add(hit.transform.parent.gameObject.GetComponent<UnitInfo>());
                        }
                    }
                }
                if (j > 0) {
                    if (Physics.Raycast(new Vector3(position.x + i, 10, position.z - j), new Vector3(0, -1, 0), out hit)){
                        if (hit.transform.gameObject.tag == "Unit")
                        {
                            hits.Add(hit.transform.parent.gameObject.GetComponent<UnitInfo>());
                        }
                    }
                }
                if (i > 0 && j > 0) {
                    if (Physics.Raycast(new Vector3(position.x - i, 10, position.z - j), new Vector3(0, -1, 0), out hit)){
                        if (hit.transform.gameObject.tag == "Unit")
                        {
                            hits.Add(hit.transform.parent.gameObject.GetComponent<UnitInfo>());
                        }
                    }
                }
            }
        }


        return hits;
    }

    public List<UnitInfo> SingleTargetFinder(UnitInfo unit, int radius, GameObject center, string targ)
    {
        List<UnitInfo> hits = new List<UnitInfo>();

        Vector3 position = center.transform.position;

        RaycastHit hit;

        for (int i = 0; i <= radius; i++)
        {
            for (int j = 0; (j + i) <= radius; j++)
            {
                if (Physics.Raycast(new Vector3(position.x + i, 10, position.z + j), new Vector3(0, -1, 0), out hit))
                {
                    if (hit.transform.gameObject.tag == "Unit")
                    {
                        if (CheckTarget(unit, hit.transform.parent.gameObject.GetComponent<UnitInfo>(), targ))
                        {
                            hits.Add(hit.transform.parent.gameObject.GetComponent<UnitInfo>());
                        }
                    }
                }
                if (i > 0)
                {
                    if (Physics.Raycast(new Vector3(position.x - i, 10, position.z + j), new Vector3(0, -1, 0), out hit))
                    {
                        if (hit.transform.gameObject.tag == "Unit")
                        {
                            if (CheckTarget(unit, hit.transform.parent.gameObject.GetComponent<UnitInfo>(), targ))
                            {
                                hits.Add(hit.transform.parent.gameObject.GetComponent<UnitInfo>());
                            }
                        }
                    }
                }
                if (j > 0)
                {
                    if (Physics.Raycast(new Vector3(position.x + i, 10, position.z - j), new Vector3(0, -1, 0), out hit))
                    {
                        if (hit.transform.gameObject.tag == "Unit")
                        {
                            if (CheckTarget(unit, hit.transform.parent.gameObject.GetComponent<UnitInfo>(), targ))
                            {
                                hits.Add(hit.transform.parent.gameObject.GetComponent<UnitInfo>());
                            }
                        }
                    }
                }
                if (i > 0 && j > 0)
                {
                    if (Physics.Raycast(new Vector3(position.x - i, 10, position.z - j), new Vector3(0, -1, 0), out hit))
                    {
                        if (hit.transform.gameObject.tag == "Unit")
                        {
                            if (CheckTarget(unit, hit.transform.parent.gameObject.GetComponent<UnitInfo>(), targ))
                            {
                                hits.Add(hit.transform.parent.gameObject.GetComponent<UnitInfo>());
                            }
                        }
                    }
                }
            }
        }


        return hits;
    }


    bool CheckTarget(UnitInfo attacker, UnitInfo target, string moveTarget)
    {
        if (moveTarget == "enemy" || moveTarget == "enemy")
        {
            if (attacker.faction != target.faction)
            {
                return true;
            }
        }
        if (moveTarget == "ally" || moveTarget == "allies")
        {
            if (attacker.faction == target.faction)
            {
                return true;
            }
        }

        if (moveTarget == "all")
        {
            return true;
        }

        return false;


    }





    void checkupX(int moveLeft, GameObject current, ICollection<string> status)
    {
        Vector3 position = current.transform.position;
        RaycastHit hit;
        int newMove = moveLeft - 1;


        if (Physics.Raycast(new Vector3(position.x + 1, 10, position.z), new Vector3(0, -1, 0), out hit))
        {
            int height = (int)(10.0 - hit.distance);

            string terrain = hit.transform.gameObject.tag;


            if (terrain == "Unit")
            {
                return;
            }

            int eleDiff = (int)(height - position.y);


            newMove -= checkCost(terrain, eleDiff, status);

            if (newMove > -1)
            {
                GameObject nextTile = hit.transform.gameObject;

                Material[] mats = nextTile.GetComponent<Renderer>().materials;
                mats[1] = highlight;
                nextTile.GetComponent<Renderer>().materials = mats;

                selectedTiles.Add(nextTile);


                if (newMove > 0)
                {
                    checkupX(newMove, nextTile, status);
                    checkupZ(newMove, nextTile, status);
                    checkdownZ(newMove, nextTile, status);
                }

            }
        }
        return;
    }

    void checkdownX(int moveLeft, GameObject current, ICollection<string> status)
    {
        Vector3 position = current.transform.position;
        RaycastHit hit;
        int newMove = moveLeft - 1;

        if (Physics.Raycast(new Vector3(position.x - 1, 10, position.z), new Vector3(0, -1, 0), out hit))
        {
            int height = (int)(10.0 - hit.distance);

            string terrain = hit.transform.gameObject.tag;
            if (terrain == "Unit")
            {
                return;
            }



            int eleDiff = (int)(height - position.y);

            newMove -= checkCost(terrain, eleDiff, status);

            if (newMove > -1)
            {

                GameObject nextTile = hit.transform.gameObject;

                Material[] mats = nextTile.GetComponent<Renderer>().materials;
                mats[1] = highlight;
                nextTile.GetComponent<Renderer>().materials = mats;

                selectedTiles.Add(nextTile);


                if (newMove > 0)
                {
                    checkdownX(newMove, nextTile, status);
                    checkupZ(newMove, nextTile, status);
                    checkdownZ(newMove, nextTile, status);
                }


            }
        }
        return;
    }

    void checkupZ(int moveLeft, GameObject current, ICollection<string> status)
    {
        Vector3 position = current.transform.position;
        RaycastHit hit;
        int newMove = moveLeft - 1;

        if (Physics.Raycast(new Vector3(position.x, 10, position.z + 1), new Vector3(0, -1, 0), out hit))
        {
            int height = (int)(10.0 - hit.distance);

            string terrain = hit.transform.gameObject.tag;
            if (terrain == "Unit")
            {
                return;
            }




            int eleDiff = (int)(height - position.y);


            newMove -= checkCost(terrain, eleDiff, status);

            if (newMove > -1)
            {

                GameObject nextTile = hit.transform.gameObject;

                Material[] mats = nextTile.GetComponent<Renderer>().materials;
                mats[1] = highlight;
                nextTile.GetComponent<Renderer>().materials = mats;

                selectedTiles.Add(nextTile);


                if (newMove > 0)
                {
                    checkupX(newMove, nextTile, status);
                    checkupZ(newMove, nextTile, status);
                    checkdownX(newMove, nextTile, status);
                }

            }
        }
    }

    void checkdownZ(int moveLeft, GameObject current, ICollection<string> status)
    {
        Vector3 position = current.transform.position;
        RaycastHit hit;
        int newMove = moveLeft -1;

        if (Physics.Raycast(new Vector3(position.x, 10, position.z - 1), new Vector3(0, -1, 0), out hit))
        {
            int height = (int)(10.0 - hit.distance);

            string terrain = hit.transform.gameObject.tag;

            if (terrain == "Unit")
            {
                return;
            }




            int eleDiff = (int)(height - position.y);


            newMove -= checkCost(terrain, eleDiff, status);

            if (newMove > -1)
            {

                GameObject nextTile = hit.transform.gameObject;

                Material[] mats = nextTile.GetComponent<Renderer>().materials;
                mats[1] = highlight;
                nextTile.GetComponent<Renderer>().materials = mats;

                selectedTiles.Add(nextTile);


                if (newMove > 0)
                {
                    checkupX(newMove, nextTile, status);
                    checkdownX(newMove, nextTile, status);
                    checkdownZ(newMove, nextTile, status);
                }

            }
        }
    }


    int checkCost (string terrain, int heightDiff, ICollection<string> status)
    {
        int cost = 0;
        if (!status.Contains("noTerrain"))
        {
            if (terrain == "Forest")
            {
                cost++;
            }
            if (terrain == "Water")
            {
                cost++;
            }
        }
        if (!status.Contains("noElevation"))
        {
            if (heightDiff > 2)
            {
                cost = 30;
            }
            if (heightDiff == 1)
            {
                cost++;
            }
        }
        return cost;
    }



}
