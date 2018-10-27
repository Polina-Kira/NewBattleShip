using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour {


    public GameObject MyMap;

    private void OnGUI()
    {
        Rect LocationButton;
        LocationButton = new Rect(new Vector2(10, 10), new Vector2(200, 40));
        if (GUI.Button(LocationButton, "Generate ships")) MyMap.GetComponent<GamePole>().EnterRandomShip();
        LocationButton = new Rect(new Vector2(10, 50), new Vector2(200, 40));
        if (GUI.Button(LocationButton, "Copy to second map")) MyMap.GetComponent<GamePole>().CopyPole();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
