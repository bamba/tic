using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using tic.Models;

namespace tic.Controllers
{
    public class GameController : Controller
    {
        //
        // GET: /Game/

        public JsonResult Index(string JsonString)
        {
            string my = JsonString;

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
                    return Json("ERROR", JsonRequestBehavior.AllowGet);
                }
            }

            return Json("OK", JsonRequestBehavior.AllowGet);
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

            return Json("OK", JsonRequestBehavior.AllowGet);
        }
        public JsonResult UpdateGame(string game)
        {
            return Json("OK", JsonRequestBehavior.AllowGet);
        }
        public JsonResult AddGame(string game)
        {
            return Json("OK", JsonRequestBehavior.AllowGet);
        }
    }
}
