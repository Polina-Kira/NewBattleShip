using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGame : MonoBehaviour {

    

    public int GameMode = 0;

    public GameObject PlayerPole, ComputerPole, Player;

    bool whoseMove = true;

    void OnGUI()
    {
        float CenterScreenX = Screen.width / 2;
        float CenterScreenY = Screen.height / 2;
        Rect LocationButton;
        //get the playing field
        GamePole PlayerPoleControl = PlayerPole.GetComponent<GamePole>();
        Camera cam;

        switch (GameMode)
        {
            case 0:
                cam = GetComponent<Camera>();
                cam.orthographicSize = 8;
                this.transform.position = new Vector3(0, 0, -10);
                LocationButton = new Rect(new Vector2(CenterScreenX - 150, CenterScreenY - 50), new Vector2(300, 200));

                GUI.Box(LocationButton, "");

                LocationButton = new Rect(new Vector2(CenterScreenX - 40, CenterScreenY - 40), new Vector2(200, 30));
                GUI.Label(LocationButton, " Game Menu");

                LocationButton = new Rect(new Vector2(CenterScreenX - 100, CenterScreenY), new Vector2(200, 30));

                if (GUI.Button(LocationButton, "Start"))
                    GameMode = 1;
                LocationButton = new Rect(new Vector2(CenterScreenX - 100, CenterScreenY + 40), new Vector2(200, 30));
                if (GUI.Button(LocationButton, "Exit"))
                    Application.Quit();
                break;
            case 1:
                cam = GetComponent<Camera>();
                cam.orthographicSize = 8;
                this.transform.position = new Vector3(30, 0, -10);

                LocationButton = new Rect(new Vector2(CenterScreenX - 150, 0), new Vector2(300, 200));

                GUI.Box(LocationButton, "");


                LocationButton = new Rect(new Vector2(CenterScreenX -20, 10), new Vector2(200, 30));
                GUI.Label(LocationButton, "Options");

                LocationButton = new Rect(new Vector2(CenterScreenX - 100, 50), new Vector2(200, 30));

                if(GUI.Button(LocationButton, "Back to Menu"))
                {
                    PlayerPoleControl.ClearPole();
                    GameMode = 0;
                }
                LocationButton = new Rect(new Vector2(CenterScreenX - 100, 90), new Vector2(200, 30));
                if (GUI.Button(LocationButton, "Place ships"))
                {
                    PlayerPoleControl.EnterRandomShip();
                }
                if(PlayerPoleControl.LifeShip() == 20)
                {
                    LocationButton = new Rect(new Vector2(CenterScreenX - 100, 130), new Vector2(200, 30));
                    if (GUI.Button(LocationButton, "Battle")) 
                    {
                        GameMode = 3;
                        PlayerPole.GetComponent<GamePole>().CopyPole();
                        ComputerPole.GetComponent<GamePole>().EnterRandomShip();

                    }
                }
                break;

            case 3:
                this.transform.position = new Vector3(30,-30, -10);
                cam = GetComponent<Camera>();
                cam.orthographicSize = 14;


                break;
            case 4:
                this.transform.position = new Vector3(120, 0, -10);

                LocationButton = new Rect(new Vector2(CenterScreenX - 150, 0), new Vector2(300, 200));
                GUI.Box(LocationButton, "");
                LocationButton = new Rect(new Vector2(CenterScreenX - 10, 10), new Vector2(200, 30));
                GUI.Label(LocationButton, "Menu");
                LocationButton = new Rect(new Vector2(CenterScreenX - 100, 50), new Vector2(200, 30));
                if(GUI.Button(LocationButton,"Back to Menu"))
                {
                    PlayerPoleControl.ClearPole();
                    GameMode = 0;
                }
                break;
            case 5:
                this.transform.position = new Vector3(70, 0, -10);
                LocationButton = new Rect(new Vector2(CenterScreenX - 150, 0), new Vector2(300, 200));
                GUI.Box(LocationButton, "");
                LocationButton = new Rect(new Vector2(CenterScreenX - 10, 10), new Vector2(200, 30));
                GUI.Label(LocationButton, "Menu");
                LocationButton = new Rect(new Vector2(CenterScreenX - 100, 50), new Vector2(200, 30));
                if (GUI.Button(LocationButton, "Back to Menu"))
                {
                    PlayerPoleControl.ClearPole();
                    GameMode = 0;
                }
                break;
        }
    }



    GamePole.TestCoord Homming()
    {
        GamePole.TestCoord XY;
        XY.X = -1;
        XY.Y = -1;
        //watch all ships and find which have Deck(Paluba)
        foreach(GamePole.Ship Test in Player.GetComponent<GamePole>().ListShip)
        {
            //watch deck of ships and check if it is in fire
            foreach(GamePole.TestCoord Paluba in Test.ShipCoord)
            {
                //check number of deck
                int Index = Player.GetComponent<GamePole>().GetIndexBlock(Paluba.X, Paluba.Y);
                if(Index==1)
                {
                    return Paluba;
                }
            }
        }
        return XY;
    }

    int ShootCount = 0;

    void ArtificialIntelligence()
    {
        if(!whoseMove)
        {
            int ShotX = Random.RandomRange(0, 9);
            int ShotY = Random.RandomRange(0, 9);
            int PC_Ship = ComputerPole.GetComponent<GamePole>().LifeShip();
            if (PC_Ship < 10)
            {
                if (ShootCount == 0)
                {


                    //shoot at deck
                    GamePole.TestCoord XY = Homming();
                    if ((XY.X >= 0) && (XY.Y >= 0))
                    {
                        ShotX = XY.X;
                        ShotY = XY.Y;
                    }
                    ShootCount++;
                }
                else
                {
                    ShootCount = 0;
                }
            }



            // check if we injured so stay playing or change role
            whoseMove = !Player.GetComponent<GamePole>().Shoot(ShotX, ShotY);
        }
    }


    void TestWhoWin(){
        int PC_Ship = ComputerPole.GetComponent<GamePole>().LifeShip();
        int Player_Ship = Player.GetComponent<GamePole>().LifeShip();
        //PC lose 
        if (PC_Ship == 0) GameMode = 4;
        //Player lose
        if (Player_Ship == 0) GameMode = 5;
    }

    public 
    void UserClick(int X, int Y)
    {
        if(whoseMove)
        {
            whoseMove = ComputerPole.GetComponent<GamePole>().Shoot(X, Y);
        }
    }


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(GameMode==3)
        {
            TestWhoWin();
            ArtificialIntelligence();
        }
	}
}
