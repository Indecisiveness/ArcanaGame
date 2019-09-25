using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class SavedInfo : MonoBehaviour
{

    public Dropdown dropdown;

    public IntroMap pickedMap;


    // Start is called before the first frame update
    void Start()
    {
        Object.DontDestroyOnLoad(gameObject);

        StartCoroutine(UpdateDropdown());
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator UpdateDropdown()
    {
        yield return new WaitForSeconds(.5f);

        dropdown.ClearOptions();

        List<string> maps = new List<string>();

        maps.Add("Select a map...");

        Object[] mapObs = Resources.LoadAll("Maps", typeof(IntroMap));

        foreach (Object o in mapObs)
        {
            maps.Add(o.name);
        }

        dropdown.AddOptions(maps);

    }

    public void PickMap()
    {
        string pick = dropdown.options[dropdown.value].text;
        if (pick == "Select a map...")
        {
            pickedMap = null;
        }
        else
        {
            pickedMap = Resources.Load<IntroMap>("Maps/" + pick);
        }
    }

    public void BeginMap()
    {
        if (pickedMap != null)
        {
            SceneManager.LoadScene("ActiveBattle");
        }
    }

}
