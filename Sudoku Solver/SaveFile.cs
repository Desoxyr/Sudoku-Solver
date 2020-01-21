using System.Collections.Generic;
using Newtonsoft.Json;

namespace Sudoku_Solver
{
    class SaveFile
    {
        [JsonProperty("response")]
        public bool Response;
        [JsonProperty("size")]
        public int Size;
        [JsonProperty("squares")] 
        public List<Squares> Squares { get; set; }
    }

    class Squares
    {
        [JsonProperty("x")] 
        public int X;
        [JsonProperty("y")] 
        public int Y;
        [JsonProperty("value")]
        public int Value;

        public bool Locked;

        [JsonConstructor]
        private Squares(int x, int y, int value)
        {
            X = x;
            Y = y;
            Value = value;
            Locked = true;
        }
        private Squares(int x, int y, int value, bool locked) : this(x, y, value)
        {
            Locked = locked;
        }
    }
}
