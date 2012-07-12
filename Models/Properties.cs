using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dapper;


namespace tic.Models
{
    public class game
    {
        public int game_id { set; get; }
        public string status { set; get; }
        public int A1 { set; get; }
        public int A2 { set; get; }
        public int A3 { set; get; }
        public int B1 { set; get; }
        public int B2 { set; get; }
        public int B3 { set; get; }
        public int C1 { set; get; }
        public int C2 { set; get; }
        public int C3 { set; get; }
        public int winner { set; get; }
    }
    public class user_game
    {
        public int game_id { set; get; }
        public int user_id { set; get; }
    }
    public class user
    {
        public int user_id { set; get; }
        public string email { set; get; }
        public string password { set; get; }
    }
}