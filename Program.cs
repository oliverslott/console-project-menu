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
            //TODO
            break;
        case 3:
            quit = true;
            break;
        default:
            break;
    }
    if(quit)
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