using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using tic.Models;
using System.Web.Script.Serialization;

namespace tic.Controllers
{
    public class GameController : Controller
    {
        //
        // GET: /Game/

        public JsonResult Index(string JsonString)
        {
            return Json("OK", JsonRequestBehavior.AllowGet);
        }
        public JsonResult SetUser(string email, string password, string create)
        {
            if(email == null && password == null)
                return Json(new user(), JsonRequestBehavior.AllowGet);

            GameModel data = new GameModel();
            user myUser = data.GetUserByEmailPassword(email, password);
            if (myUser == null)
            {
                if (create != null)
                {
                    myUser.email = email;
                    myUser.password = password;
                    myUser = data.AddUser(myUser);
                }
                else
                {
                    return Json(new user(), JsonRequestBehavior.AllowGet);
                }
            }

            return Json(myUser, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetGame(string game_id)
        {
            if(game_id == null)
                return Json(new game(), JsonRequestBehavior.AllowGet);
            game myGame;
            GameModel data = new GameModel();
            try
            {
                myGame = data.GetGameByID(int.Parse(game_id));
            }
            catch {
                return Json(new game(), JsonRequestBehavior.AllowGet);
            }
            if (myGame != null)
                return Json(myGame, JsonRequestBehavior.AllowGet);
            else
                return Json(new game(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetUsers(string curent_user_id)
        {
            if(curent_user_id == null)
                return Json(new user(), JsonRequestBehavior.AllowGet);
            GameModel data = new GameModel();
            List<user> users;
            try
            {
               users = data.GetAllUsers(int.Parse(curent_user_id));
            }
            catch
            {
                return Json(new user(), JsonRequestBehavior.AllowGet);
            }

            return Json(users, JsonRequestBehavior.AllowGet);
        }
        public JsonResult UpdateGame(string game)
        {
            if(game == null)
                return Json(new game(), JsonRequestBehavior.AllowGet);
            GameModel data = new GameModel();
            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            game myGame = (game)json_serializer.DeserializeObject(game);
            data.UpdateGame(myGame);
            return Json(myGame, JsonRequestBehavior.AllowGet);
        }
        public JsonResult AddGame(string game)
        {
            if(game == null)
                return Json(new game(), JsonRequestBehavior.AllowGet);
            GameModel data = new GameModel();
            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            game myGame = (game)json_serializer.DeserializeObject(game);
            myGame = data.AddGame(myGame);
            return Json(myGame, JsonRequestBehavior.AllowGet);
        }

    }
}
