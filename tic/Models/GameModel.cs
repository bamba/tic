using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using Dapper;

namespace tic.Models
{
    public class GameModel
    {
        string Connectionstring;// = @"Data Source=.\SQLEXPRESS;AttachDbFilename=C:\Users\ndalama\Documents\MyData.mdf;Integrated Security=True;Connect Timeout=30;User Instance=True";
 
        public GameModel()
        {
            string dir = HostingEnvironment.MapPath(@"~/App_Data/MyData.mdf");
            Connectionstring = string.Format(@"Data Source=.\SQLEXPRESS;AttachDbFilename={0};Integrated Security=True;Connect Timeout=30;User Instance=True",dir);
        }
        
        public List<game> GetAllGamesForUser(int user_id)
        {
            using (System.Data.SqlClient.SqlConnection sqlConnection = new System.Data.SqlClient.SqlConnection(Connectionstring))
            {
                sqlConnection.Open();
                List<game> games = sqlConnection.Query<game>(@"SELECT g.*, p.user_one_id, p.user_two_id
                                                               FROM game g 
                                                               INNER JOIN players p ON p.game_id = g.game_id
                                                               WHERE p.user_one_id = @userid OR p.user_two_id = @userid", new { userid = user_id }).ToList();
                sqlConnection.Close();
                return games;
            }
        }
        public List<game> GetPendingGamesForUser(int user_id)
        {
            using (System.Data.SqlClient.SqlConnection sqlConnection = new System.Data.SqlClient.SqlConnection(Connectionstring))
            {
                sqlConnection.Open();
                List<game> games = sqlConnection.Query<game>(@"SELECT g.*, p.user_one_id, p.user_two_id
                                                               FROM game g 
                                                               INNER JOIN players p ON p.game_id = g.game_id
                                                               WHERE g.status IS NULL AND (p.user_one_id = @userid OR p.user_two_id = @userid)", 
                                                             new { userid = user_id }).ToList();
                sqlConnection.Close();
                return games;
            }
        }
        public List<game> GetCompletedGamesForUser(int user_id)
        {
            using (System.Data.SqlClient.SqlConnection sqlConnection = new System.Data.SqlClient.SqlConnection(Connectionstring))
            {
                sqlConnection.Open();
                List<game> games = sqlConnection.Query<game>(@"SELECT g.*, p.user_one_id, p.user_two_id
                                                               FROM game g 
                                                               INNER JOIN players p ON p.game_id = g.game_id
                                                               WHERE g.status = 'F' AND (p.user_one_id = @userid OR p.user_two_id = @userid)", new { userid = user_id }).ToList();
                sqlConnection.Close();
                return games;
            }
        }
        public game GetGameByID(int game_id)
        {
            using (System.Data.SqlClient.SqlConnection sqlConnection = new System.Data.SqlClient.SqlConnection(Connectionstring))
            {
                sqlConnection.Open();
                game games = sqlConnection.Query<game>(@"SELECT g.*, p.user_one_id, p.user_two_id
                                                               FROM game g 
                                                               INNER JOIN players p ON p.game_id = g.game_id
                                                               WHERE g.game_id = @gameid", new { gameid = game_id }).First();
                sqlConnection.Close();
                return games;
            }
        }
        public void UpdateGame(game myGame)
        {
            using (System.Data.SqlClient.SqlConnection sqlConnection = new System.Data.SqlClient.SqlConnection(Connectionstring))
            {
                sqlConnection.Open();
                sqlConnection.Execute(@"update game set 
                                        A1 = @A1, A2 = @A2, A3 = @A3, 
                                        B1 = @B1, B2 = @B2, B3 = @B3, 
                                        C1 = @C1, C2 = @C2, C3 = @C3,
                                        status = @status, winner = @winner
                                        where game_id = @gameid",
                    new
                    {
                          A1= myGame.A1,
                          A2 = myGame.A2,
                          A3 = myGame.A3,
                          B1 = myGame.B1,
                          B2 = myGame.B2,
                          B3 = myGame.B3,
                          C1 = myGame.C1,
                          C2 = myGame.C2,
                          C3 = myGame.C3,
                          status= myGame.status,
                          winner = myGame.winner,
                          gameid = myGame.game_id
                          });
                sqlConnection.Close();
                
            }
        }
        public game AddGame(game myGame)
        {
            using (System.Data.SqlClient.SqlConnection sqlConnection = new System.Data.SqlClient.SqlConnection(Connectionstring))
            {
                sqlConnection.Open();
                myGame.game_id = sqlConnection.Query<int>(@"insert into game (A1, A2, A3, B1, B2 , B3 , C1, C2, C3 ,status , winner)
                                        values(@A1, @A2,@A3,  @B1, @B2, @B3,  @C1, @C2,  @C3,@status, @winner);
                                        SELECT CAST(SCOPE_IDENTITY() AS INT)",
                    new
                    {
                        A1 = myGame.A1,
                        A2 = myGame.A2,
                        A3 = myGame.A3,
                        B1 = myGame.B1,
                        B2 = myGame.B2,
                        B3 = myGame.B3,
                        C1 = myGame.C1,
                        C2 = myGame.C2,
                        C3 = myGame.C3,
                        status = myGame.status,
                        winner = myGame.winner,
                        gameid = myGame.game_id,
                       
                    }).Single();
                
                sqlConnection.Close();
            }
            using (System.Data.SqlClient.SqlConnection sqlConnection = new System.Data.SqlClient.SqlConnection(Connectionstring))
            {
                sqlConnection.Open();
                sqlConnection.Query<int>(@"insert into players (game_id,user_one_id,user_two_id)
                                                            values(@gameid,@userOneId,@userTwoId);",
                    new
                    {
                        gameid = myGame.game_id,
                        userOneId = myGame.user_one_id,
                        userTwoId = myGame.user_two_id
                    }).Single();

                sqlConnection.Close();
            }
            return myGame;

        }
        public user GetUserById(int id)
        {
            using (System.Data.SqlClient.SqlConnection sqlConnection = new System.Data.SqlClient.SqlConnection(Connectionstring))
            {
                sqlConnection.Open();
                user usr = sqlConnection.Query<user>(@"select * from user where user_id = @userid", new { userid = id }).First();
                sqlConnection.Close();
                return usr;
            }
        }
        public user GetUserByEmailPassword(string email, string password)
        {
            using (System.Data.SqlClient.SqlConnection sqlConnection = new System.Data.SqlClient.SqlConnection(Connectionstring))
            {
                sqlConnection.Open();
                user usr = sqlConnection.Query<user>(@"select * from user where email = @email and password = @password", new { email = email, password = password }).First();
                sqlConnection.Close();
                return usr;
            }
        }
        public user AddUser(user myUser)
        {
            using (System.Data.SqlClient.SqlConnection sqlConnection = new System.Data.SqlClient.SqlConnection(Connectionstring))
            {
                sqlConnection.Open();
                myUser.user_id = sqlConnection.Query<int>(@"insert into user (email,password)
                                        values(@myEmail,@myPassword);
                                        SELECT CAST(SCOPE_IDENTITY() AS INT)",
                    new
                    {
                        myEmail = myUser.email,
                        myPassword = myUser.password
                    }).Single();

                sqlConnection.Close();
            }
            return myUser;
        }
        public List<user> GetAllUsers(int current_user_id)
        {
            using (System.Data.SqlClient.SqlConnection sqlConnection = new System.Data.SqlClient.SqlConnection(Connectionstring))
            {
                sqlConnection.Open();
                List<user> usr = sqlConnection.Query<user>(@"select * from user where user_id != @userid", new { userid = current_user_id }).ToList();
                sqlConnection.Close();
                return usr;
            }
        }
    }
}