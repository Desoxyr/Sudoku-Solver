using Newtonsoft.Json;
using RestSharp;

namespace Sudoku_Solver
{
    //TODO Add tuple so it returns false when lack of response 
    class SudokuRequest
    {
        public SaveFile DeserializeSaveFile { get; private set; }
        public SaveFile GetSaveFile(string level, Board board)
        {
            RestRequest request = new RestRequest("cheon/ws/sudoku/new/", Method.POST);
            RestClient client = new RestClient("http://www.cs.utep.edu");
            
            request.AddQueryParameter("size", "9");
            request.AddQueryParameter("level", level);
            IRestResponse response = client.Execute(request);
            
            DeserializeSaveFile = JsonConvert.DeserializeObject<SaveFile>(response.Content);
            if(response.IsSuccessful)
            {
                foreach (Squares square in DeserializeSaveFile.Squares)
                {
                    board.Tiles[square.X, square.Y].Locked = true;
                }
                
            }
            return DeserializeSaveFile;
        }
    }
}
