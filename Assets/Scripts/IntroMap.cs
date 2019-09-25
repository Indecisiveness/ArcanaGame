using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/BlankMap", order = 1)]
public class IntroMap : ScriptableObject
{
    public int wid;
    public int len;
    public string[] grid;

    public GameObject[] enemies;
    public int[] enemyLocations;
    public string[] enemyNames;

    public GameObject[] allies;
    public int[] allyLocations;

    public GameObject[] player;
    public int[] playerLocations;
    public string[] playerNames;

}
