while (true)
{
    bool quit = false;
    Console.WriteLine("Choose game: ");
    Console.WriteLine("1) Minesweeper (Made by Oliver)");
    Console.WriteLine("2) Battleship (Made by Esbjørn)");
    Console.WriteLine("3) Quit");
    int choice = int.Parse(Console.ReadLine());
    switch (choice)
    {
        case 1:
            Minesweeper();
            break;
        case 2:
            Battleship();
            break;
        case 3:
            quit = true;
            break;
        default:
            break;
    }
    if (quit)
    {
        break;
    }
}

Console.WriteLine("Press any key to quit");
Console.ReadKey();

void Minesweeper()
{
    //Made by Oliver

    const byte tileSize = 8;
    const byte amountOfBombs = 8;
    bool[,] bombTiles = new bool[tileSize, tileSize];
    bool[,] openTiles = new bool[tileSize, tileSize];
    bool[,] flaggedTile = new bool[tileSize, tileSize];
    byte selectedX = 0;
    byte selectedY = 0;
    bool lostGame = false;
    bool wonGame = false;
    bool placedFirstTile = false;
    bool quit = false;

    Console.WriteLine("Controls: Arrow keys to move cursor, spacebar to open tile, F for flag and Q to quit");
    while (true)
    {
        CheckIfWon();
        if (!wonGame && !lostGame && !quit)
        {
            DrawBoard();
            Controls();
        }
        else
        {
            break;
        }
    }
    Console.WriteLine("Press any key to quit.");
    Console.ReadKey();

    void Controls()
    {
        //Console.Write("Click tile at(\"x,y\"): ");
        switch (Console.ReadKey().Key)
        {
            case ConsoleKey.LeftArrow:
                if (selectedY == 0)
                {
                    selectedY = tileSize - 1;
                }
                else
                {
                    selectedY--;
                }
                break;
            case ConsoleKey.RightArrow:
                if (selectedY == tileSize - 1)
                {
                    selectedY = 0;
                }
                else
                {
                    selectedY++;
                }
                break;
            case ConsoleKey.UpArrow:
                if (selectedX == 0)
                {
                    selectedX = tileSize - 1;
                }
                else
                {
                    selectedX--;
                }
                break;
            case ConsoleKey.DownArrow:
                if (selectedX == tileSize - 1)
                {
                    selectedX = 0;
                }
                else
                {
                    selectedX++;
                }
                break;
            case ConsoleKey.Spacebar:
                OpenTile(selectedX, selectedY);
                break;
            case ConsoleKey.F:
                FlagTile(selectedX, selectedY);
                break;
            case ConsoleKey.Q:
                quit = true;
                break;
        }
        //string[] input = Console.ReadLine().Split(',');
        //int x = int.Parse(input[1]);
        //int y = int.Parse(input[0]);
        //OpenTile(x,y);
    }

    void DrawBoard()
    {
        Console.WriteLine();
        for (int x = 0; x < tileSize; x++)
        {
            for (int y = 0; y < tileSize; y++)
            {
                //Cursor
                if (selectedX == x && selectedY == y)
                {
                    Console.Write("[");
                }
                else
                {
                    Console.Write(" ");
                }

                DrawTile(x, y);

                //Cursor
                if (selectedX == x && selectedY == y)
                {
                    Console.Write("]");
                }
                else
                {
                    Console.Write(" ");
                }
            }
            Console.WriteLine();
        }
    }

    void DrawTile(int x, int y)
    {
        if (openTiles[x, y])
        {
            if (bombTiles[x, y])
            {
                Console.Write($"X");
            }
            else
            {
                int num = GetNumberForTile(x, y);
                if (num == 0)
                {
                    Console.Write($" ");
                }
                else
                {
                    Console.Write($"{num}");
                }
            }
        }
        else
        {
            if (flaggedTile[x, y])
            {
                Console.Write("F");
            }
            else
            {
                Console.Write($"■");
            }
        }
    }

    void OpenTile(int x, int y)
    {
        openTiles[x, y] = true;

        if (!placedFirstTile)
        {
            placedFirstTile = true;
            SpawnBombs();
        }

        if (bombTiles[x, y]) //Tile is a bomb-tile
        {
            lostGame = true;
            DrawBoard(); //Draw the board one last time
            Console.WriteLine("You hit a bomb. You lose!");
            return;
        }
        if (GetNumberForTile(x, y) > 0)
        {
            return;
        }
        //Below
        if (y + 1 < tileSize && !bombTiles[x, y + 1] && !openTiles[x, y + 1])
        {
            OpenTile(x, y + 1);
        }
        // //Below-right
        if (x + 1 < tileSize && y + 1 < tileSize && !bombTiles[x + 1, y + 1] && !openTiles[x + 1, y + 1])
        {
            OpenTile(x + 1, y + 1);
        }
        //Right
        if (x + 1 < tileSize && !bombTiles[x + 1, y] && !openTiles[x + 1, y])
        {
            OpenTile(x + 1, y);
        }
        // //Top-right
        if (y - 1 >= 0 && x + 1 < tileSize && !bombTiles[x + 1, y - 1] && !openTiles[x + 1, y - 1])
        {
            OpenTile(x + 1, y - 1);
        }
        //Top
        if (y - 1 >= 0 && !bombTiles[x, y - 1] && !openTiles[x, y - 1])
        {
            OpenTile(x, y - 1);
        }
        // //Top-left
        if (y - 1 >= 0 && x - 1 >= 0 && !bombTiles[x - 1, y - 1] && !openTiles[x - 1, y - 1])
        {
            OpenTile(x - 1, y - 1);
        }
        //Left 
        if (x - 1 >= 0 && !bombTiles[x - 1, y] && !openTiles[x - 1, y])
        {
            OpenTile(x - 1, y);
        }
        //Below-left
        if (x - 1 >= 0 && y + 1 < tileSize && !bombTiles[x - 1, y + 1] && !openTiles[x - 1, y + 1])
        {
            OpenTile(x - 1, y + 1);
        }
    }

    void FlagTile(int x, int y)
    {
        flaggedTile[x, y] = !flaggedTile[x, y];
    }

    void CheckIfWon()
    {
        int tilesOpened = 0;
        for (int x = 0; x < openTiles.GetLength(0); x++)
        {
            for (int y = 0; y < openTiles.GetLength(1); y++)
            {
                if (openTiles[x, y])
                {
                    tilesOpened++;
                }
            }
        }
        if (tilesOpened == tileSize * tileSize - amountOfBombs)
        {
            DrawBoard();
            Console.WriteLine("You won!");
            wonGame = true;
        }
    }

    int GetNumberForTile(int x, int y)
    {
        //Console.WriteLine($"{x},{y}");
        int bombsAroundTile = 0;

        //Below
        if (y + 1 < tileSize && bombTiles[x, y + 1])
        {
            bombsAroundTile++;
        }
        //Below-right
        if (x + 1 < tileSize && y + 1 < tileSize && bombTiles[x + 1, y + 1])
        {
            bombsAroundTile++;
        }
        //Right
        if (x + 1 < tileSize && bombTiles[x + 1, y])
        {
            bombsAroundTile++;
        }
        //Top-right
        if (y - 1 >= 0 && x + 1 < tileSize && bombTiles[x + 1, y - 1])
        {
            bombsAroundTile++;
        }
        //Top
        if (y - 1 >= 0 && bombTiles[x, y - 1])
        {
            bombsAroundTile++;
        }
        //Top-left
        if (y - 1 >= 0 && x - 1 >= 0 && bombTiles[x - 1, y - 1])
        {
            bombsAroundTile++;
        }
        //Left 
        if (x - 1 >= 0 && bombTiles[x - 1, y])
        {
            bombsAroundTile++;
        }
        //Below-left
        if (x - 1 >= 0 && y + 1 < tileSize && bombTiles[x - 1, y + 1])
        {
            bombsAroundTile++;
        }
        return bombsAroundTile;
    }

    void SpawnBombs()
    {
        Random rand = new Random();
        for (int i = 0; i < amountOfBombs; i++)
        {
            int randX;
            int randY;
            //Loop to prevent bombs spawning the same location; breaking the game
            while (true)
            {
                randX = rand.Next(0, tileSize);
                randY = rand.Next(0, tileSize);
                if (!bombTiles[randX, randY] && !openTiles[randX, randY]) //Don't spawn bomb on first clicked tile
                {
                    break;
                }
            }
            bombTiles[randX, randY] = true;
        }
    }
}


void Battleship()
{
    //Made by Esbjørn
    string[,] PlayerArray = new string[9, 9];
    PlayerArray[0, 0] = "(B)";
    PlayerArray[1, 0] = "(B)";
    PlayerArray[2, 0] = "(B)";
    PlayerArray[3, 0] = "(B)";
    PlayerArray[4, 0] = "(B)";
    PlayerArray[5, 0] = "(B)";
    PlayerArray[6, 0] = "(B)";
    PlayerArray[7, 0] = "(B)";
    PlayerArray[8, 0] = "(B)";

    PlayerArray[0, 1] = "(B)";
    PlayerArray[1, 1] = "(B)";
    PlayerArray[2, 1] = "(B)";
    PlayerArray[3, 1] = "(B)";
    PlayerArray[4, 1] = "(B)";
    PlayerArray[5, 1] = "(B)";
    PlayerArray[6, 1] = "(B)";
    PlayerArray[7, 1] = "(B)";
    PlayerArray[8, 1] = "(B)";

    PlayerArray[0, 2] = "(B)";
    PlayerArray[1, 2] = "(B)";
    PlayerArray[2, 2] = "(B)";
    PlayerArray[3, 2] = "(B)";
    PlayerArray[4, 2] = "(B)";
    PlayerArray[5, 2] = "(B)";
    PlayerArray[6, 2] = "(B)";
    PlayerArray[7, 2] = "(B)";
    PlayerArray[8, 2] = "(B)";

    PlayerArray[0, 3] = "(B)";
    PlayerArray[1, 3] = "(B)";
    PlayerArray[2, 3] = "(B)";
    PlayerArray[3, 3] = "(B)";
    PlayerArray[4, 3] = "(B)";
    PlayerArray[5, 3] = "(B)";
    PlayerArray[6, 3] = "(B)";
    PlayerArray[7, 3] = "(B)";
    PlayerArray[8, 3] = "(B)";

    PlayerArray[0, 4] = "(B)";
    PlayerArray[1, 4] = "(B)";
    PlayerArray[2, 4] = "(B)";
    PlayerArray[3, 4] = "(B)";
    PlayerArray[4, 4] = "(B)";
    PlayerArray[5, 4] = "(B)";
    PlayerArray[6, 4] = "(B)";
    PlayerArray[7, 4] = "(B)";
    PlayerArray[8, 4] = "(B)";

    PlayerArray[0, 5] = "(B)";
    PlayerArray[1, 5] = "(B)";
    PlayerArray[2, 5] = "(B)";
    PlayerArray[3, 5] = "(B)";
    PlayerArray[4, 5] = "(B)";
    PlayerArray[5, 5] = "(B)";
    PlayerArray[6, 5] = "(B)";
    PlayerArray[7, 5] = "(B)";
    PlayerArray[8, 5] = "(B)";

    PlayerArray[0, 6] = "(B)";
    PlayerArray[1, 6] = "(B)";
    PlayerArray[2, 6] = "(B)";
    PlayerArray[3, 6] = "(B)";
    PlayerArray[4, 6] = "(B)";
    PlayerArray[5, 6] = "(B)";
    PlayerArray[6, 6] = "(B)";
    PlayerArray[7, 6] = "(B)";
    PlayerArray[8, 6] = "(B)";

    PlayerArray[0, 7] = "(B)";
    PlayerArray[1, 7] = "(B)";
    PlayerArray[2, 7] = "(B)";
    PlayerArray[3, 7] = "(B)";
    PlayerArray[4, 7] = "(B)";
    PlayerArray[5, 7] = "(B)";
    PlayerArray[6, 7] = "(B)";
    PlayerArray[7, 7] = "(B)";
    PlayerArray[8, 7] = "(B)";

    PlayerArray[0, 8] = "(B)";
    PlayerArray[1, 8] = "(B)";
    PlayerArray[2, 8] = "(B)";
    PlayerArray[3, 8] = "(B)";
    PlayerArray[4, 8] = "(B)";
    PlayerArray[5, 8] = "(B)";
    PlayerArray[6, 8] = "(B)";
    PlayerArray[7, 8] = "(B)";
    PlayerArray[8, 8] = "(B)";

    string[,] AiArray = new string[9, 9];
    AiArray[0, 0] = "(B)";
    AiArray[1, 0] = "(B)";
    AiArray[2, 0] = "(B)";
    AiArray[3, 0] = "(B)";
    AiArray[4, 0] = "(B)";
    AiArray[5, 0] = "(B)";
    AiArray[6, 0] = "(B)";
    AiArray[7, 0] = "(B)";
    AiArray[8, 0] = "(B)";

    AiArray[0, 1] = "(B)";
    AiArray[1, 1] = "(B)";
    AiArray[2, 1] = "(B)";
    AiArray[3, 1] = "(B)";
    AiArray[4, 1] = "(B)";
    AiArray[5, 1] = "(B)";
    AiArray[6, 1] = "(B)";
    AiArray[7, 1] = "(B)";
    AiArray[8, 1] = "(B)";

    AiArray[0, 2] = "(B)";
    AiArray[1, 2] = "(B)";
    AiArray[2, 2] = "(B)";
    AiArray[3, 2] = "(B)";
    AiArray[4, 2] = "(B)";
    AiArray[5, 2] = "(B)";
    AiArray[6, 2] = "(B)";
    AiArray[7, 2] = "(B)";
    AiArray[8, 2] = "(B)";

    AiArray[0, 3] = "(B)";
    AiArray[1, 3] = "(B)";
    AiArray[2, 3] = "(B)";
    AiArray[3, 3] = "(B)";
    AiArray[4, 3] = "(B)";
    AiArray[5, 3] = "(B)";
    AiArray[6, 3] = "(B)";
    AiArray[7, 3] = "(B)";
    AiArray[8, 3] = "(B)";

    AiArray[0, 4] = "(B)";
    AiArray[1, 4] = "(B)";
    AiArray[2, 4] = "(B)";
    AiArray[3, 4] = "(B)";
    AiArray[4, 4] = "(B)";
    AiArray[5, 4] = "(B)";
    AiArray[6, 4] = "(B)";
    AiArray[7, 4] = "(B)";
    AiArray[8, 4] = "(B)";

    AiArray[0, 5] = "(B)";
    AiArray[1, 5] = "(B)";
    AiArray[2, 5] = "(B)";
    AiArray[3, 5] = "(B)";
    AiArray[4, 5] = "(B)";
    AiArray[5, 5] = "(B)";
    AiArray[6, 5] = "(B)";
    AiArray[7, 5] = "(B)";
    AiArray[8, 5] = "(B)";

    AiArray[0, 6] = "(B)";
    AiArray[1, 6] = "(B)";
    AiArray[2, 6] = "(B)";
    AiArray[3, 6] = "(B)";
    AiArray[4, 6] = "(B)";
    AiArray[5, 6] = "(B)";
    AiArray[6, 6] = "(B)";
    AiArray[7, 6] = "(B)";
    AiArray[8, 6] = "(B)";

    AiArray[0, 7] = "(B)";
    AiArray[1, 7] = "(B)";
    AiArray[2, 7] = "(B)";
    AiArray[3, 7] = "(B)";
    AiArray[4, 7] = "(B)";
    AiArray[5, 7] = "(B)";
    AiArray[6, 7] = "(B)";
    AiArray[7, 7] = "(B)";
    AiArray[8, 7] = "(B)";

    AiArray[0, 8] = "(B)";
    AiArray[1, 8] = "(B)";
    AiArray[2, 8] = "(B)";
    AiArray[3, 8] = "(B)";
    AiArray[4, 8] = "(B)";
    AiArray[5, 8] = "(B)";
    AiArray[6, 8] = "(B)";
    AiArray[7, 8] = "(B)";
    AiArray[8, 8] = "(B)";

    for (int x = 0; x < PlayerArray.GetLength(0); x++)
    {
        for (int y = 0; y < PlayerArray.GetLength(1); y++)
        {
            //Console.Write(PlayerArray[x, y]);
        }
        //Console.WriteLine();


    }
    Console.ReadKey(); for (int x = 0; x < AiArray.GetLength(0); x++)
    {
        for (int y = 0; y < AiArray.GetLength(1); y++)
        {
            //Console.Write(AiArray[x, y]);
        }
        //Console.WriteLine();
    }
    //Console.ReadKey();

    Console.WriteLine("BattleShips");
    Console.WriteLine("there are 5 enemy ships");
    Console.WriteLine("B = blank space, O means a hit, X means a miss");
    Console.WriteLine("Pick your BatleShips Placemens (5 ships)");

    int PlayerShip1a;
    int PlayerShip1b;
    Console.WriteLine("where do you want to place your first ship? (1/5) (type under here the two codenent you want your ship in)");
    string PlayerShip1in = Console.ReadLine();
    PlayerShip1a = int.Parse(PlayerShip1in[0].ToString());
    PlayerShip1b = int.Parse(PlayerShip1in[1].ToString());
    //Console.WriteLine("");
    PlayerArray[PlayerShip1a, PlayerShip1b] = "(■)";

    for (int x = 0; x < AiArray.GetLength(0); x++)
    {
        for (int y = 0; y < PlayerArray.GetLength(1); y++)
        {
            Console.Write(PlayerArray[x, y]);
        }
        Console.WriteLine();
    }
    Console.WriteLine("y^ x>");

    int PlayerShip2a;
    int PlayerShip2b;
    Console.WriteLine("where do you want to place your first ship? (2/5) (type under here the two codenent you want your ship in)");
    string PlayerShip2in = Console.ReadLine();
    PlayerShip2a = int.Parse(PlayerShip2in[0].ToString());
    PlayerShip2b = int.Parse(PlayerShip2in[1].ToString());
    PlayerArray[PlayerShip2a, PlayerShip2b] = "(■)";
    for (int x = 0; x < AiArray.GetLength(0); x++)
    {
        for (int y = 0; y < PlayerArray.GetLength(1); y++)
        {
            Console.Write(PlayerArray[x, y]);
        }
        Console.WriteLine();
    }
    Console.WriteLine("y^ x>");

    int PlayerShip3a;
    int PlayerShip3b;
    Console.WriteLine("where do you want to place your first ship? (3/5) (type under here the two codenent you want your ship in)");
    string PlayerShip3in = Console.ReadLine();
    PlayerShip3a = int.Parse(PlayerShip3in[0].ToString());
    PlayerShip3b = int.Parse(PlayerShip3in[1].ToString());
    PlayerArray[PlayerShip3a, PlayerShip3b] = "(■)";
    for (int x = 0; x < AiArray.GetLength(0); x++)
    {
        for (int y = 0; y < PlayerArray.GetLength(1); y++)
        {
            Console.Write(PlayerArray[x, y]);
        }
        Console.WriteLine();
    }
    Console.WriteLine("y^ x>");

    int PlayerShip4a;
    int PlayerShip4b;
    Console.WriteLine("where do you want to place your first ship? (4/5) (type under here the two codenent you want your ship in)");
    string PlayerShip4in = Console.ReadLine();
    PlayerShip4a = int.Parse(PlayerShip4in[0].ToString());
    PlayerShip4b = int.Parse(PlayerShip4in[1].ToString());
    PlayerArray[PlayerShip4a, PlayerShip4b] = "(■)";
    for (int x = 0; x < AiArray.GetLength(0); x++)
    {
        for (int y = 0; y < PlayerArray.GetLength(1); y++)
        {
            Console.Write(PlayerArray[x, y]);
        }
        Console.WriteLine();
    }
    Console.WriteLine("y^ x>");

    int PlayerShip5a;
    int PlayerShip5b;
    Console.WriteLine("where do you want to place your first ship? (5/5) (type under here the two codenent you want your ship in)");
    string PlayerShip5in = Console.ReadLine();
    PlayerShip5a = int.Parse(PlayerShip5in[0].ToString());
    PlayerShip5b = int.Parse(PlayerShip5in[1].ToString());
    PlayerArray[PlayerShip5a, PlayerShip5b] = "(■)";
    for (int x = 0; x < PlayerArray.GetLength(0); x++)
    {
        for (int y = 0; y < PlayerArray.GetLength(1); y++)
        {
            Console.Write(PlayerArray[x, y]);
        }
        Console.WriteLine();
    }
    Console.WriteLine("y^ x>");
    Console.WriteLine("This is your board, the enemy Ai have now Randomly placed its ships on its Board");

    int AiShip1a = 0;
    int AiShip1b = 0;
    int AiShipAmount = 0;

    Random Aishipp1Placer = new Random();

    for (int i = 0; i < 5; i++)
    {
        int rndX = Aishipp1Placer.Next(0, 9);
        int rndY = Aishipp1Placer.Next(0, 9);
        AiArray[rndX, rndY] = "(■)";


    }

    for (int x = 0; x < AiArray.GetLength(0); x++)
    {
        for (int y = 0; y < AiArray.GetLength(1); y++)
        {
            Console.Write(AiArray[x, y]);
        }
        Console.WriteLine();
    }
    Console.ReadLine();
}

//Made by Esbjørn
enum PlayerShips
{
    ship1, ship2, ship3, ship4, ship5
}
enum AiShips
{
    ship1, ship2, ship3, ship4, ship5
}