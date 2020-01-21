using System;
using System.Drawing;

namespace Sudoku_Solver
{
    [Serializable]
    class Board 
    {
        public Tile[,] Tiles { get; private set; } = new Tile[9, 9];
        public Tile[][] Rows { get; private set; } = new Tile[9][];
        public Tile[][] Columns { get; private set; } = new Tile[9][];
        public Tile[][] Blocks { get; private set; } = new Tile[9][];

        public Board()
        {
            //Create TileGroups
            for (int i = 0; i < 9; i++)
            {
                Rows[i] = new Tile[9];
                Columns[i] = new Tile[9];
                Blocks[i] = new Tile[9];
            }

            //Create Tiles For Board
            for (int column = 0; column < 9; column++)
            {
                for (int row = 0; row < 9; row++)
                {
                    Tile tile = new Tile(new Point(row, column));
                    Rows[column][row] = tile;
                    Columns[row][column] = tile;
                    Tiles[row, column] = tile;
                }
            }

            //Create Blocks For Board
            for (int block = 0; block < 9; block++)
            {
                int count = 0;
                int posX = block % 3 * 3,
                    posY = block / 3 * 3;
                for (int x = posX; x < posX + 3; x++)
                {
                    for (int y = posY; y < posY + 3; y++)
                    {
                        Blocks[block][count] = Tiles[x, y];
                        Tiles[x, y].SetBlock(Blocks[block]);
                        count++;
                    }
                }
            }
        }

        //Clears entire board
        public void Clear()
        {
            foreach (Tile tile in Tiles)
            {
                tile.Clear();
                tile.Locked = false;
            }
        }

        //Returns current state of the board 
        public override string ToString()
        {
            string s = "";
            s += "Debug \n";
            for (int y = 0; y < 9; y++)
            {
                for (int x = 0; x < 9; x++)
                {
                    s += Tiles[x, y].Value + ",";
                }
                s += "\n";
            }

            int candidates = 0;
            foreach (Tile tile in Tiles)
            {
                if (tile.Candidates != null)
                {
                    candidates += tile.Candidates.Count;
                }
            }
            s += $"Candidates: {candidates} \n";
            s += "---";
            return s;
        }


        //Possibly functions as a copy method 
        public Board(Board other)
        {
            //Create TileGroups
            for (int i = 0; i < 9; i++)
            {
                Rows[i] = new Tile[9];
                Columns[i] = new Tile[9];
                Blocks[i] = new Tile[9];
            }

            //Create Tiles For Board
            for (int column = 0; column < 9; column++)
            {
                for (int row = 0; row < 9; row++)
                {
                    Tile otherTile = other.Tiles[row, column];
                    Tile tile = new Tile(otherTile);
                    Rows[column][row] = tile;
                    Columns[row][column] = tile;
                    Tiles[row, column] = tile;
                }
            }

            //Create Blocks For Board
            for (int block = 0; block < 9; block++)
            {
                int count = 0;
                int posX = block % 3 * 3,
                    posY = block / 3 * 3;
                for (int x = posX; x < posX + 3; x++)
                {
                    for (int y = posY; y < posY + 3; y++)
                    {
                        Blocks[block][count] = Tiles[x, y];
                        Tiles[x, y].SetBlock(Blocks[block]);
                        count++;
                    }
                }
            }
        }
    }
}
