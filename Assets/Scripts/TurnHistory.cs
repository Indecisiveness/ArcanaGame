using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnHistory : MonoBehaviour
{
    VerticalLayoutGroup myLayout;
    Text oriChild;
    ScrollRect scrollRect;

    // Start is called before the first frame update
    void Start()
    {
        myLayout = gameObject.transform.Find("Viewport").Find("Content").gameObject.GetComponent<VerticalLayoutGroup>();
        oriChild = myLayout.transform.Find("Text").gameObject.GetComponent<Text>();
        scrollRect = gameObject.GetComponent<ScrollRect>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddMessage (string message)
    {
        Text newText = Instantiate(oriChild, myLayout.transform); 
        newText.text = message;
        StartCoroutine(advanceScroll());

    }

    IEnumerator advanceScroll()
    {
        yield return (new WaitForEndOfFrame());
        scrollRect.verticalNormalizedPosition = 0;
    }

}
