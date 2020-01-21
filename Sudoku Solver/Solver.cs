using System;
using System.Collections.Generic;

namespace Sudoku_Solver
{
    class Solver
    {
        private readonly Board _board;
        private bool _progress;

        public Solver(Board board) 
        {
            _board = board;
        }

        public Tuple<bool, Board> Solve()
        {
            UpdateBoard();
            while (true)
            {
                _progress = false;

                //Solve tiles with only one remaining candidate
                foreach (Tile tile in _board.Tiles)
                {
                    if (tile.Value == 0 && tile.Candidates.Count == 1) 
                    {
                        SolveTile(tile, tile.Candidates[0]);
                        _progress = true;
                        Console.WriteLine($@"Solving Tile {tile.Position.X},{tile.Position.Y} through one remaining candidate");
                    }
                }

                //Solve tileGroups where int i only fits in one tile
                for (int value = 1; value <= 9; value++)
                {
                    for (int tileGroup = 0; tileGroup < 9; tileGroup++)
                    {
                        CheckForHiddenSingles(value, _board.Rows[tileGroup]);
                        CheckForHiddenSingles(value, _board.Columns[tileGroup]);
                        CheckForHiddenSingles(value, _board.Blocks[tileGroup]);
                    }
                }

                //Return the board when the sudoku is solved 
                if (CheckIfSolved(_board))
                {
                    Console.WriteLine(@"Solved:");
                    Console.WriteLine(ToString());
                    return Tuple.Create(true, _board);
                }

                //Return false when the sudoku cannot be solved
                if (CheckIfSolvable() == false)
                    return Tuple.Create(false, _board);

                //Brute force if no more progress and made and the puzzle is unsolved
                if (_progress == false)
                {
                    Console.WriteLine(@"Brute Force:");
                    Console.WriteLine(ToString());

                    //Board copyOfBoard = new Board(Board);
                    Board copyOfBoard =  ObjectCopier.Clone(_board);
                    var copySolver = new Solver(copyOfBoard);
                    Console.WriteLine(@"New Solver (Should Match Above");
                    Console.WriteLine(copySolver.ToString());

                    //Select tile which doesn't have a value and set it to a candidate
                    foreach (Tile tile in copyOfBoard.Tiles)
                    {
                        if (tile.Candidates != null)
                        {
                            //Create a copy of the board

                            int testValue = tile.Candidates[0];
                            copySolver.SolveTile(tile, testValue);

                            Console.WriteLine(ToString());
                            Console.WriteLine(copySolver.ToString());
                            Console.WriteLine($@"Trying value {testValue} in tile {tile.Position.X},{tile.Position.Y}");
                            var solved = copySolver.Solve();

                            if (solved.Item1)
                            {
                                return Tuple.Create(true, solved.Item2);
                            }
                            else if (solved.Item1 == false)
                            {
                                _board.Tiles[tile.Position.X, tile.Position.Y].Candidates.Remove(testValue);
                                Console.WriteLine(ToString());
                                Console.WriteLine($@"Removing candidate {testValue} from tile {tile.Position.X},{tile.Position.Y}");
                                break;
                            }
                        }
                    }
                }
            }
        }

        //Update Candidates of every Tile on the board
        public void UpdateBoard()
        {
            for (int y = 0; y < 9; y++)
            {
                for (int x = 0; x < 9; x++)
                {
                    Tile tile = _board.Tiles[x, y];
                    if (tile.Value == 0)
                    {
                        UpdateTile(tile, _board.Rows[y]);
                        UpdateTile(tile, _board.Columns[x]);
                        UpdateTile(tile, tile.Block);
                    }
                }
            }
        }

        //Remove Candidates from a Tile based on values in its TileGroups
        private void UpdateTile(Tile tile, Tile[] tiles)
        {
            foreach (Tile groupedTile in tiles)
            {
                if (groupedTile.Value != 0)
                {
                    tile.Candidates.Remove(groupedTile.Value);
                }
            }
        }

        //Set a Tile's value & update all Tiles in its TileGroups
        private void SolveTile(Tile tile, int value)
        {
            tile.SetValue(value);
            UpdateGroupedTiles(value, tile.Block);
            UpdateGroupedTiles(value, _board.Rows[tile.Position.Y]); 
            UpdateGroupedTiles(value, _board.Columns[tile.Position.X]);
        }

        //Remove Candidates of int i from Tiles in a TileGroup
        private void UpdateGroupedTiles(int tileValue, Tile[] tiles)
        {
            foreach (Tile groupedTile in tiles)
            {
                groupedTile.Candidates?.Remove(tileValue);
            }
        }

        //Check for hidden singles of int i in a tileGroup
        private void CheckForHiddenSingles(int value, Tile[] tileGroup)
        {
            List<Tile> tileList = new List<Tile>();
            foreach (Tile tile in tileGroup)
            {
                if (tile.Candidates != null && tile.Candidates.Contains(value))
                {
                    tileList.Add(tile);
                }
            }

            if (tileList.Count == 1)
            {
                SolveTile(tileList[0], value);
                _progress = true;
                Console.WriteLine($@"Solving Tile {tileList[0].Position.X},{tileList[0].Position.Y} as hidden single");
            }
        }

        //Check whether board is solved
        private bool CheckIfSolved(Board board)
        {
            bool solved = true;
            foreach (Tile tile in _board.Tiles)
            {
                if (tile.Value == 0)
                {
                    solved = false;
                    break;
                }
            }
            return solved;
        }

        //Check whether board can still be solved
        private bool CheckIfSolvable()
        {
            bool solvable = true;
            foreach (Tile tile in _board.Tiles)
            {
                if (tile.Value == 0 && tile.Candidates.Count == 0)
                {
                    solvable = false;
                    break;
                }
            }
            return solvable;
        }
    }
}
