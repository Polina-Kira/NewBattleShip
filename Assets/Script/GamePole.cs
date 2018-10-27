using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePole : MonoBehaviour {

    //*****************************************************************
    public GameObject GamMain;//the main script that make all decisions
    //*****************************************************************

    public GameObject eLetters, eNumbers, ePole, eState;
    public GameObject MapDestination;

    public bool HideShip = false;

    //letter list
    GameObject[] Letters;

    //numbers list
    GameObject[] Numbers;

    // game pole
    public
    GameObject[,] Pole;


    public
    struct TestCoord
    {
        public int X, Y;
    }

    public
    struct Ship
    {
        public TestCoord[] ShipCoord;
    }

    public
    List<Ship> ListShip = new List<Ship>();

    int Time = 40,
        Deltatime = 0;

    //declare a field size of 10 by 10 cells
    int lenghtPole = 10;

    public int[] ShipsCount = { 0, 4, 3, 2, 1 };


    public void CopyPole()
    {
        if (MapDestination!=null)  
        {
            for (int Y = 0; Y < lenghtPole;Y++)
            {
                for (int X = 0; X < lenghtPole;X++)
                {
                    MapDestination.GetComponent<GamePole>().Pole[X, Y].GetComponent<Chanks>().Index = Pole[X, Y].GetComponent<Chanks>().Index;
                }
            }
            MapDestination.GetComponent<GamePole>().ListShip.Clear();
            MapDestination.GetComponent<GamePole>().ListShip.AddRange(ListShip);
        }


    }
    bool CountShips()
    {
        int Amaunt = 0;
        foreach (int Ship in ShipsCount) Amaunt += Ship;
        if(Amaunt != 0) return true;
        return false;
    }

    public
    void ClearPole()
    {
        ShipsCount = new int[] { 0, 4, 3, 2, 1 };
        ListShip.Clear();
        for (int Y = 0; Y < lenghtPole; Y++)
        {
            for (int X = 0; X < lenghtPole; X++)
            {
                Pole[X, Y].GetComponent<Chanks>().Index = 0;
            }

        }
    }
    public
    void EnterRandomShip()
    {
        ClearPole();
        int SelectShip = 4;
        int X, Y;
        int Direction;

        while(CountShips())
        {
            X = Random.RandomRange(0, 10);
            Y = Random.RandomRange(0, 10);
            Direction = Random.RandomRange(0, 2);

            if(EnterDeck(SelectShip, Direction, X, Y))
            {
                ShipsCount[SelectShip]--;
                if(ShipsCount[SelectShip] == 0)
                {
                    SelectShip--;
                }
            }

        }

    }

   

    void CreatePole()
    {
        Vector3 StartPose = transform.position;

        float XX = StartPose.x + 1;
        float YY = StartPose.y - 1;

        Letters = new GameObject[lenghtPole];
        Numbers = new GameObject[lenghtPole];

        //create a loop for the game field using a loop
        for (int Inscription = 0; Inscription < lenghtPole; Inscription++)
        {
            Letters[Inscription] = Instantiate(eLetters);
            Letters[Inscription].transform.position = new Vector3(XX, StartPose.y, StartPose.z);
            Letters[Inscription].GetComponent<Chanks>().Index = Inscription;
            XX++;

            Numbers[Inscription] = Instantiate(eNumbers);
            Numbers[Inscription].transform.position = new Vector3(StartPose.x, YY, StartPose.z);
            Numbers[Inscription].GetComponent<Chanks>().Index = Inscription;
            YY--;

        }
        XX = StartPose.x + 1;
        YY = StartPose.y - 1;

        Pole = new GameObject[lenghtPole, lenghtPole];

        //cycle of field drawing by y
        for (int Y = 0; Y < lenghtPole; Y++)
        {
            //cycle of field drawing by x
            for (int X = 0; X < lenghtPole; X++)
            {
                Pole[X, Y] = Instantiate(ePole);
                Pole[X, Y].GetComponent<Chanks>().Index = 0;
                Pole[X, Y].GetComponent<Chanks>().HideChank = HideShip;


                Pole[X, Y].transform.position = new Vector3(XX, YY, StartPose.z);
                if(HideShip)
                    Pole[X, Y].GetComponent<ClickPole>().WhoPerent = this.gameObject;

                Pole[X, Y].GetComponent<ClickPole>().CoordX = X;
                Pole[X, Y].GetComponent<ClickPole>().CoordY = Y;

                XX++;
            }
            XX = StartPose.x + 1;
            YY--;
        }
    }

    bool TestEnterDeck(int X, int Y)
    {
        //if our point of verification is outside the field boundaries

        if((X > -1) && (Y > -1) && (X < 10) && (Y < 10))
        {
            //create a set of values from 9 coordinates
            //|?|?|?|
            //|?|X|?|
            //|?|?|?|
            int[] XX = new int[9], YY = new int[9];
            //calculate the checked coordinates
            //---------------------------------------------------------------
            /*|*/XX[0] = X + 1;   /*|*/XX[1] = X;   /*|*/XX[2] = X - 1; /*|*/
            /*|*/YY[0] = Y + 1;  /*|*/YY[1] = Y + 1;/*|*/YY[2] = Y + 1; /*|*/
            //---------------------------------------------------------------
            /*|*/XX[3] = X + 1;   /*|*/XX[4] = X;   /*|*/XX[5] = X - 1; /*|*/
            /*|*/YY[3] = Y;       /*|*/YY[4] = Y;   /*|*/YY[5] = Y;     /*|*/
            //---------------------------------------------------------------
            /*|*/XX[6] = X + 1;   /*|*/XX[7] = X;   /*|*/XX[8] = X - 1; /*|*/
            /*|*/YY[6] = Y - 1;  /*|*/YY[7] = Y - 1;/*|*/YY[8] = Y - 1; /*|*/
            //---------------------------------------------------------------

            for (int I = 0; I < 9; I++)
            {
                //check if coordinate exists
                if((XX[I] > -1) && (YY[I] > -1) && (XX[I] < 10) && (YY[I] < 10))
                {
                    if (Pole[XX[I], YY[I]].GetComponent<Chanks>().Index != 0) return false;
                }
            }
            return true;
        }
        return false;
    }

    TestCoord[]TestEnterShipDirect(int ShipType,int XD, int YD, int X, int Y)
    {
        TestCoord[] ResultCoord = new TestCoord[ShipType];

        for (int P = 0; P < ShipType; P++)
        {
            if (TestEnterDeck(X, Y))
            {
                ResultCoord[P].X = X;
                ResultCoord[P].Y = Y;
            }
            else
                return null;

            X += XD;
            Y += YD;
        }
        return ResultCoord;
    }

    TestCoord[] TestEnterShip(int ShipType, int Direction, int X, int Y)
    {
        TestCoord[] ResultCoord = new TestCoord[ShipType];
        if(TestEnterDeck(X, Y))
        {
            switch(Direction)
            {
                case 0:
                    ResultCoord = TestEnterShipDirect(ShipType, 1, 0, X, Y);
                    if(ResultCoord == null) ResultCoord = TestEnterShipDirect(ShipType, -1, 0, X, Y);
                    break;
                case 1:
                    ResultCoord = TestEnterShipDirect(ShipType, 0, 1, X, Y);
                    if (ResultCoord == null) ResultCoord = TestEnterShipDirect(ShipType, 0, -1, X, Y);
                    break;
            }
            return ResultCoord;
        }


        return null;
    }

    bool EnterDeck(int ShipType, int Direction, int X, int Y)
    {
        TestCoord[] P = TestEnterShip(ShipType, Direction, X, Y);
        if(P!=null )
        {
            foreach(TestCoord T in P)
            {
                Pole[T.X, T.Y].GetComponent<Chanks>().Index = 1;
            }
            Ship Deck;
            Deck.ShipCoord = P;
            ListShip.Add(Deck);

            return true;
        }
        return false;
    }


    // Use this for initialization
    void Start () {
        CreatePole();

        if (HideShip) EnterRandomShip();

    }
	
	// Update is called once per frame
	void Update () {
        Deltatime++;

        if(Deltatime > Time)
        {
            if (eState != null) eState.GetComponent<Chanks>().Index = 0;
            Deltatime = 0;
        }
		
	}

    //the cell that was clicked will tell us this

    public void WhoClick(int X, int Y)
    {
        //if(TestEnterDeck(X, Y)) Pole[X, Y].GetComponent<Chanks>().Index = 1;
        //EnterDeck(4, 1, X, Y);
        //Shoot(X, Y);
        if (GamMain != null) GamMain.GetComponent<MainGame>().UserClick(X,Y);

    }

    public int GetIndexBlock(int X, int Y)
    {
        return Pole[X, Y].GetComponent<Chanks>().Index;
    }



    public
    bool Shoot(int X, int Y)
    {
        if(eState!=null) eState.GetComponent<Chanks>().Index = 0;

        int PoleSelect = Pole[X, Y].GetComponent<Chanks>().Index;
        bool Result = false;
        switch(PoleSelect)
        {
            case 0:
                Pole[X, Y].GetComponent<Chanks>().Index = 2;
                Result = false;
                eState.GetComponent<Chanks>().Index = 3;
                break;
            case 1:
                Pole[X, Y].GetComponent<Chanks>().Index = 3;
                Result = true;
                if(TestShoot(X, Y))
                {
                    if (eState != null) eState.GetComponent<Chanks>().Index = 1;
                }
                else
                {
                    if (eState != null) eState.GetComponent<Chanks>().Index = 2;
                }
                break;
        }
        return Result;
    }

    bool TestShoot(int X,int Y)
    {
        bool Result = false;

        foreach(Ship Test in ListShip)
        {
            foreach(TestCoord Paluba in Test.ShipCoord)
            {
                if((Paluba.X == X) && (Paluba.Y == Y))
                {
                    int CountKill = 0;
                    foreach(TestCoord KillPaluba in Test.ShipCoord)
                    {
                        int TestBlock = Pole[KillPaluba.X, KillPaluba.Y].GetComponent<Chanks>().Index;
                        if (TestBlock == 3) CountKill++;
                    }
                    if (CountKill == Test.ShipCoord.Length)
                        Result = true;
                    else
                        Result = false;
                }
            }
        }

        return Result;
    }

    public int LifeShip()
    {
        int countLife = 0;

        foreach(Ship Test in ListShip)
        {
            foreach(TestCoord Paluba in Test.ShipCoord)
            {
                int TestBlock = Pole[Paluba.X, Paluba.Y].GetComponent<Chanks>().Index;
                if (TestBlock == 1) countLife++;
            }
        }

        return countLife;
    }
}
