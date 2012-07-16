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
        public JsonResult GetGame(int game_id)
        {
            GameModel data = new GameModel();
            game myGame = data.GetGameByID(game_id);
            if (myGame != null)
                return Json(myGame, JsonRequestBehavior.AllowGet);
            else
                return Json("", JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetUsers(int curent_user_id)
        {
            GameModel data = new GameModel();
            List<user> users = data.GetAllUsers(curent_user_id);

            return Json(users, JsonRequestBehavior.AllowGet);
        }
        public JsonResult UpdateGame(string game)
        {
            GameModel data = new GameModel();
            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            game myGame = (game)json_serializer.DeserializeObject(game);
            data.UpdateGame(myGame);
            return Json(myGame, JsonRequestBehavior.AllowGet);
        }
        public JsonResult AddGame(string game)
        {
            GameModel data = new GameModel();
            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            game myGame = (game)json_serializer.DeserializeObject(game);
            myGame = data.AddGame(myGame);
            return Json(myGame, JsonRequestBehavior.AllowGet);
        }

    }
}
