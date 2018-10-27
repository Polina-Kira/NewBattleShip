using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickPole : MonoBehaviour {

    public GameObject WhoPerent = null;
    //cell position on the field
    public int CoordX, CoordY;


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnMouseDown()
    {
        //if the link exists then we will do something
        if(WhoPerent!=null)
        {
            WhoPerent.GetComponent<GamePole>().WhoClick(CoordX, CoordY);
        }
    }

}
