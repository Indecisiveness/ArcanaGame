//class for interpreting user clicks as actions on grid
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.EventSystems;


public class CubeClick : MonoBehaviour, IPointerDownHandler, IPointerClickHandler,
    IPointerUpHandler, IPointerExitHandler, IPointerEnterHandler,
    IBeginDragHandler, IDragHandler, IEndDragHandler
{

    public MoveCalc moveCalc;

    public GameObject cubeMenu;

    public GameObject unitMenu;

    public Canvas canvas;

    GameObject currentMenu;

    public TurnOrder turnOrder;

    public GameObject actionPanel;

    public Button moveButton;

    public ActionDisplay actDis;


    public void OnBeginDrag(PointerEventData eventData)
    {
    }

    public void OnDrag(PointerEventData eventData)
    {
    }

    public void OnEndDrag(PointerEventData eventData)
    {
    }

    public void OnPointerClick(PointerEventData eventData)
    {


        if (eventData.pointerCurrentRaycast.gameObject.tag == "Unit")
        {
            if (!actDis.acting)
            {
                this.UnitMenu(eventData.pointerCurrentRaycast.gameObject.transform.parent.gameObject);
            }
            else
            {
                actDis.TargetPicked(eventData.pointerCurrentRaycast.gameObject.transform.parent.gameObject);
            }
        }
        else if (moveCalc.selectedTiles.Contains(eventData.pointerCurrentRaycast.gameObject)){
            if (moveCalc.moving){
                this.MoveToTile(eventData.pointerCurrentRaycast.gameObject, moveCalc.selected.gameObject);
            }
            else{
                actDis.TargetPicked(eventData.pointerCurrentRaycast.gameObject);
            }
        }


        else{
            this.CubeMenu(eventData.pointerCurrentRaycast.gameObject);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
    }

    public void OnPointerExit(PointerEventData eventData)
    {
    }

    public void OnPointerUp(PointerEventData eventData)
    {
    }

    public void CubeMenu(GameObject cube)
    {
        GameObject newMenu = Instantiate(cubeMenu, canvas.transform);

        Object.Destroy(currentMenu);

        currentMenu = newMenu;

        GameObject matDis = newMenu.transform.Find("MatDisplay").gameObject;

        Image current = matDis.GetComponent<Image>();

        Material newMat = Instantiate(cube.GetComponent<MeshRenderer>().material);

        Debug.Log(newMat.name);

        current.material = newMat;

        current.material.shader = Shader.Find("Unlit/Texture");

        Dropdown terrain = newMenu.transform.Find("TerrainDropdown").GetComponent<Dropdown>();

        string tag = cube.tag;

        switch (tag) {
            case "Dirt":
                terrain.value = 0;
                break;
            case "Forest":
                terrain.value = 1;
                break;
            case "Sand":
                terrain.value = 2;
                break;
            case "Stone":
                terrain.value = 3;
                break;
            case "Water":
                terrain.value = 4;
                break;
            default:
                break;
        }

        InputField elevation = newMenu.transform.Find("ElevationVal").GetComponent<InputField>();
        Debug.Log(((int)(System.Math.Floor(cube.transform.position.y))).ToString());
        elevation.text = ((int)(System.Math.Floor(cube.transform.position.y))).ToString();

        Text myText = newMenu.transform.Find("Terrain Effects").GetComponent<Text>();
        myText.text = cube.GetComponent<TileTextInfo>().info;

    }

    public void UnitMenu (GameObject unit)
    {
        GameObject newMenu = Instantiate(unitMenu, canvas.transform);

        Object.Destroy(currentMenu);

        currentMenu = newMenu;

        UnitInfo unIn = unit.GetComponent<UnitInfo>();

        Text UnitName = newMenu.transform.Find("UnitName").GetComponent<Text>();
        UnitName.text = unit.name;

        Text levelDis = newMenu.transform.Find("LevelVal").GetComponent<Text>();
        levelDis.text = ""+unIn.level;

        Slider HPBar = newMenu.transform.Find("HPBar").GetComponent<Slider>();
        HPBar.maxValue = unIn.maxHP;
        HPBar.value = unIn.currHP;

        Text HPDis = HPBar.transform.Find("Fill Area").Find("HP").GetComponent<Text>();
        HPDis.text = unIn.currHP + "/" + unIn.maxHP;

        Slider MPBar = newMenu.transform.Find("MPBar").GetComponent<Slider>();
        MPBar.maxValue = unIn.maxMP;
        MPBar.value = unIn.currMP;

        Text MPDis = MPBar.transform.Find("Fill Area").Find("MP").GetComponent<Text>();
        MPDis.text = unIn.currMP + "/" + unIn.maxMP;

        Text SwordVal = newMenu.transform.Find("SwordsVal").GetComponent<Text>();
        SwordVal.text = ""+unIn.swords;

        Text CupVal = newMenu.transform.Find("CupsVal").GetComponent<Text>();
        CupVal.text = ""+unIn.cups;

        Text WandsVal = newMenu.transform.Find("WandsVal").GetComponent<Text>();
        WandsVal.text = ""+unIn.wands;

        Text PentVal = newMenu.transform.Find("PentsVal").GetComponent<Text>();
        PentVal.text = ""+unIn.pentacles;

        if (unit == turnOrder.currentOrder[0])
        {
            actionPanel.SetActive(true);
            if (unit.GetComponent<UnitInfo>().status.ContainsKey("immobile"))
            {
                moveButton.interactable = false;
            }
            if (unit.GetComponent<UnitInfo>().status.ContainsKey("disarm"))
            {
                actionPanel.transform.Find("Action").gameObject.GetComponent<Button>().interactable = false;
            }

        }
        else
        {
            actionPanel.SetActive(false);
        }

    }

    public void MoveToTile(GameObject tile, GameObject unit)
    {

        UnitInfo thisUnit = unit.GetComponent<UnitInfo>();

        thisUnit.currentTile.GetComponent<TileTextInfo>().ClearUnit();
      
        unit.transform.position = new Vector3(tile.transform.position.x, tile.transform.position.y + 1, tile.transform.position.z);
        unit.GetComponent<UnitInfo>().currentTile = tile;

        TileTextInfo occTile = tile.GetComponent<TileTextInfo>();
        occTile.currOcc = unit.GetComponent<UnitInfo>();


        moveCalc.HighlightClear();
        moveButton.interactable = false;

    }


    public void HideAllMenus()
    {
        actionPanel.SetActive(false);
        actDis.Hide();

        Destroy(currentMenu);
    }




}