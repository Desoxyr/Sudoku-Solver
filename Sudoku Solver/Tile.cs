using System;
using System.Collections.Generic;
using System.Drawing;

namespace Sudoku_Solver
{
    [Serializable]
    class Tile
    {
        public int Value { get; private set; } = 0;
        public List<int> Candidates { get; private set; } = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 }; //TODO: Move this someone central you dumbass
        public Point Position { get; }
        public Tile[] Block { get; private set; }
        public bool Locked { get; set; } = false;


        public Tile(Point pos)
        {
            Position = pos;
        }

        public void SetBlock(Tile[] block)
        {
            Block = block;
        }

        public void SetValue(int value)
        {
            Value = value;
            Candidates = null;
        }

        public void Clear()
        {
            Value = 0;
            Candidates = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            Locked = false;
        }

        //Functions as a potential copy method
        public Tile(Tile other)
        {
            Value = other.Value;
            Candidates = other.Candidates;
            Locked = other.Locked;
            Position = other.Position;
        }
    }
}
