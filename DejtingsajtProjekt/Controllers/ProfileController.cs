using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using DejtingsajtProjekt.Models;
using Microsoft.AspNet.Identity;

namespace DejtingsajtProjekt.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        // GET: Profile
        public ActionResult Index()
        {
            var profileCtx = new ProfileDbContext();
            var currentUser = User.Identity.GetUserId();
            var currentProfile = profileCtx.Profiles.FirstOrDefault(p => p.UserId == currentUser);
            var exists = false;

            if (currentProfile != null)
            {
                exists = true;
            }

            return View(new ProfileViewModels {
                Firstname = currentProfile?.Firstname,
                Lastname = currentProfile?.Lastname,
                Birthday = currentProfile?.Birthday,
                ImageName = currentProfile?.ImageName,
                Exists = exists
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProfileViewModels model, ProfileModel img, HttpPostedFileBase file)
        {
            var profileCtx = new ProfileDbContext();
            var currentUser = User.Identity.GetUserId();
            var currentProfile = profileCtx.Profiles.FirstOrDefault(p => p.UserId == currentUser);

           
          
            
            string mainconn = ConfigurationManager.ConnectionStrings["ProfileDB"].ConnectionString;
            SqlConnection sqlConn = new SqlConnection(mainconn);

            string sqlQuery = "update [dbo].[ProfileModels] set ImageName = '" +"/images/"+ file.FileName + "' where UserId = '" +currentUser+"'";
            SqlCommand sqlCommandet = new SqlCommand(sqlQuery, sqlConn);


            if (currentProfile == null)
            {
                profileCtx.Profiles.Add(new ProfileModel
                {
                    UserId = currentUser,
                    Firstname = model.Firstname,
                    Lastname = model.Lastname,
                    Birthday = model.Birthday.Value,

                    ImageName = model.ImageName
                });
               
            }


            sqlConn.Open();

              if (file !=null && file.ContentLength > 0)
              {
                  string filename = Path.GetFileName(file.FileName);
                  string imagePath = Path.Combine(Server.MapPath("/images/"), filename);
                  file.SaveAs(imagePath);
              }


            else
                {
                    currentProfile.Firstname = model.Firstname ?? currentProfile.Firstname;
                    currentProfile.Lastname = model.Lastname ?? currentProfile.Lastname;
                    if (model.Birthday == null)
                    {
                        currentProfile.Birthday = currentProfile.Birthday;
                    }
                    else
                    {
                        currentProfile.Birthday = model.Birthday.Value;
                    }

                    currentProfile.ImageName = model.ImageName;

                }

             sqlCommandet.Parameters.AddWithValue("@ImageName", "/images/" + file.FileName);
             sqlCommandet.ExecuteNonQuery();
             sqlConn.Close();

            
            profileCtx.SaveChanges();
            
            return RedirectToAction("Index", "Profile");
           

        }

       

        public bool CheckCurrentProfile()
        {
            var profileCtx = new ProfileDbContext();
            var currentUser = User.Identity.GetUserId();
            var currentProfile = profileCtx.Profiles.FirstOrDefault(p => p.UserId == currentUser);

            if(currentProfile == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

       //För att redigera anvendarens profil
      /*  public ActionResult EditProfile ()
        {
            ProfileViewModels modell;
            modell = new ProfileViewModels();
            return View(modell);
        }*/


        public ActionResult ProfileList()
        {
            var ctx = new ProfileDbContext();

            var viewModel = new ProfileListViewModel {
                Profiles = ctx.Profiles.ToList() };

            return View(viewModel);
        }

        public ActionResult UserProfile(string id)
        {
            var ctx = new ProfileDbContext();
            var profile = ctx.Profiles.FirstOrDefault(p => p.UserId == id);
            return View(new ProfileViewModels
            {
                Firstname = profile?.Firstname,
                Lastname = profile?.Lastname,
                Birthday = profile?.Birthday
            });
        }

     

     /*  [HttpPost]
        public ActionResult EditImage( HttpPostedFileBase file)
        {
          

            
            string mainconn = ConfigurationManager.ConnectionStrings["ProfileDB"].ConnectionString;
            SqlConnection sqlConn = new SqlConnection(mainconn);
            string sqlQuery = "insert into [dbo].[ProfileModels] (ImageName) values (@ImageName) select UserId from [dbo].[ProfileModels] where Firstname = @Firstname  ";
            SqlCommand sqlCommandet = new SqlCommand(sqlQuery, sqlConn);

            sqlConn.Open();

            if (file != null && file.ContentLength > 0)
            {
                string filename = Path.GetFileName(file.FileName);
                string imagePath = Path.Combine(Server.MapPath("/images/"), filename);
                file.SaveAs(imagePath);
            }
            
            sqlCommandet.Parameters.AddWithValue("@ImageName", "/images/" + file.FileName);
            sqlCommandet.ExecuteNonQuery();
            sqlConn.Close();

            return RedirectToAction("Index", "Profile");


        }*/

        public ActionResult ProfilImage()
        {
            var profileCtx = new ProfileDbContext();
            var currentUser = User.Identity.GetUserId();
            var currentProfile = profileCtx.Profiles.FirstOrDefault(p => p.UserId == currentUser);

            string imageD = (string)Session["ProfileId"];
            string mainconn = ConfigurationManager.ConnectionStrings["ProfileDB"].ConnectionString;
            SqlConnection sqlConn = new SqlConnection(mainconn);
            string sqlQuery = "select ImageName from  [dbo].[ProfileModels] where UserId='" + currentUser+ "'";
            sqlConn.Open();
            SqlCommand sqlCommandet = new SqlCommand(sqlQuery, sqlConn);
           // sqlCommandet.Parameters.AddWithValue("UserId", Session["ProfileId"].ToString());
            SqlDataReader sdr = sqlCommandet.ExecuteReader();
            if (sdr.Read())
            {
                string s = sdr["ImageName"].ToString();
                ViewData["Img"] = s;
            }

            sqlConn.Close();
            return View();
        }
    }

}