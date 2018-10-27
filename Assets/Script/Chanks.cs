using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chanks : MonoBehaviour {

    //variable containing a list of pictures for which you can change the current
    public Sprite[] imgs;

    //variable to indicate what kind the current cell has on the field
    public int Index = 0;

    public bool HideChank = false;

    //picture change function
    void ChangeImgs()
    {
        if (imgs.Length > Index)
        {
            if ((HideChank) && (Index == 1))GetComponent<SpriteRenderer>().sprite = imgs[0];
            else
            //set the block image
            GetComponent<SpriteRenderer>().sprite = imgs[Index];
        }
    }

    // Use this for initialization
    void Start () {
        ChangeImgs();
    }
	
	// Update is called once per frame
	void Update () {
        ChangeImgs();
	}
}
