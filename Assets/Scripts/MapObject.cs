//class for generating map out of arrays

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapObject : MonoBehaviour
{
    public string[] grid;

    IntroMap mapData;

    public Camera mainCam;

    public GameObject dirt;
    public GameObject forest;
    public GameObject stone;
    public GameObject sand;
    public GameObject water;

    public GameObject mapObj;
    public GameObject cubeMenu;

    public GameObject[] units;

    public TurnOrder turnOrder;


    // Start is called before the first frame update
    void Start()
    {
        SavedInfo info = GameObject.FindGameObjectWithTag("Info").GetComponent<SavedInfo>();
        mapData = info.pickedMap;


        grid = mapData.grid;
        units = new GameObject[mapData.enemies.Length];


        for (int i = 0; i< mapData.len; i++)
        {
            for (int j = 0; j < mapData.wid; j++)
            {
                string tile = grid[(i*mapData.len)+j];
                string[] specs = tile.Split('/');
                int height = int.Parse(specs[1]);

                switch (specs[0])
                {
                    case "dirt":
                        GameObject dirtCube = Instantiate(dirt, new Vector3(i, height, j), dirt.transform.rotation, mapObj.transform);
                        dirtCube.name = "x" + i + "y" + j;
                        break;
                    case "forest":
                        GameObject forestCube = Instantiate(forest, new Vector3(i, height, j), forest.transform.rotation, mapObj.transform);
                        forestCube.name = "x" + i + "y" + j;
                        break;
                    case "stone":
                        GameObject stoneCube = Instantiate(stone, new Vector3(i, height, j), stone.transform.rotation, mapObj.transform);
                        stoneCube.name = "x" + i + "y" + j;
                        break;
                    case "sand":
                        GameObject sandCube = Instantiate(sand, new Vector3(i, height, j), sand.transform.rotation, mapObj.transform);
                        sandCube.name = "x" + i + "y" + j;
                        break;
                    case "water":
                        GameObject waterCube = Instantiate(water, new Vector3(i, height, j), water.transform.rotation, mapObj.transform);
                        waterCube.name = "x" + i + "y" + j;
                        break;
                    default:
                        break;
                }

            
            }
        }

        for (int i = 0; i < mapData.enemies.Length; i++)
        {
            int x = mapData.enemyLocations[i] % mapData.len;
            int height = 0;
            int z = mapData.enemyLocations[i] / mapData.len;

            RaycastHit hit;

            

            if(Physics.Raycast(new Vector3((float)(x), 10, (float)(z)), new Vector3(0, -1, 0), out hit))
            {
                height = (int) (10.0 - hit.distance);
            }
            

            GameObject unit = Instantiate(mapData.enemies[i], new Vector3(mapData.enemyLocations[i] % mapData.len, height+1, mapData.enemyLocations[i] / mapData.len), Quaternion.identity, mapObj.transform);
            unit.transform.Find("Pointer").gameObject.SetActive(false);
            this.gameObject.GetComponent<TurnOrder>().currentOrder.Add(unit);
            units[i] = unit;
            UnitInfo unIn = unit.GetComponent<UnitInfo>();
            unIn.currentTile = hit.transform.gameObject;
            unIn.faction = "enemy";
            unIn.name = mapData.enemyNames[i];

            TileTextInfo occTile = hit.transform.gameObject.GetComponent<TileTextInfo>();
            occTile.currOcc = unIn;

        }

        for (int i = 0; i<mapData.player.Length; i++)
        {
            int x = mapData.playerLocations[i] % mapData.len;
            int height = 0;
            int z = mapData.playerLocations[i] / mapData.len;

            RaycastHit hit;

            if (Physics.Raycast(new Vector3((float)(x), 10, (float)(z)), new Vector3(0, -1, 0), out hit))
            {
                height = (int)(10.0 - hit.distance);
            }


            GameObject unit = Instantiate(mapData.player[i], new Vector3(x, height + 1, z), Quaternion.identity, mapObj.transform);
            unit.transform.Find("Pointer").gameObject.SetActive(false);
            this.gameObject.GetComponent<TurnOrder>().currentOrder.Add(unit);
            units[i] = unit;
            UnitInfo unIn = unit.GetComponent<UnitInfo>();
            unIn.currentTile = hit.transform.gameObject;
            unIn.faction = "player";

            unIn.name = mapData.playerNames[i];

            TileTextInfo occTile = hit.transform.gameObject.GetComponent<TileTextInfo>();
            occTile.currOcc = unIn;

        }

        mainCam.transform.position = new Vector3(mapData.wid*1.4f, (mapData.len + mapData.wid)*.5f , mapData.len*1.4f);


        turnOrder.ShowOrder();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void RotateCamera()
    {
        float xCoord = mainCam.transform.position.x;
        float zCoord = mainCam.transform.position.z;

        float xDeg = 30;

        if (xCoord>0 && zCoord > 0) //initial position
        {
            xCoord = -(xCoord - mapData.wid);
            mainCam.transform.localRotation = Quaternion.Euler(xDeg, 140, 0);
        }

        else if (zCoord > 0)
        {
            zCoord = -(zCoord - mapData.len);
            mainCam.transform.localRotation = Quaternion.Euler(xDeg, 50, 0);

        }


        else if (xCoord > 0)
        {
            zCoord =  -(zCoord - mapData.len);
            mainCam.transform.localRotation = Quaternion.Euler(xDeg, -140, 0);
        }


        else
        {
            xCoord  = -(xCoord - mapData.wid);
            mainCam.transform.localRotation = Quaternion.Euler(xDeg, -50, 0);
        }


        mainCam.transform.position = new Vector3(xCoord, mainCam.transform.position.y, zCoord);
        
    }


}
