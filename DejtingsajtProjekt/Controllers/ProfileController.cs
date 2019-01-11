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
    //[Authorize]
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
                Description = currentProfile?.Description,
               // ImageName = currentProfile?.ImageName,
               // ImageName = currentProfile?.ImageName,
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
                    Description = model.Description,

                   // ImageName = model.ImageName
                });
               
            }


            sqlConn.Open();

            if (file != null && file.ContentLength > 0)
            {
                string filename = Path.GetFileName(file.FileName);
                string imagePath = Path.Combine(Server.MapPath("/images/"), filename);
                file.SaveAs(imagePath);
            }


            else
            {
                currentProfile.Firstname = model.Firstname ?? currentProfile.Firstname;
                currentProfile.Lastname = model.Lastname ?? currentProfile.Lastname;
                currentProfile.Description = model.Description ?? currentProfile.Description;
                if (model.Birthday == null)
                {
                    currentProfile.Birthday = currentProfile.Birthday;
                }
                else
                {
                    currentProfile.Birthday = model.Birthday.Value;
                }

                // currentProfile.ImageName = model.ImageName;
                /*{
                    currentProfile.Firstname = model.Firstname ?? currentProfile.Firstname;
                    currentProfile.Lastname = model.Lastname ?? currentProfile.Lastname;
                    currentProfile.Description = model.Description ?? currentProfile.Description;
                    if (model.Birthday == null)
                    {
                        currentProfile.Birthday = currentProfile.Birthday;
                    }
                    else
                    {
                        currentProfile.Birthday = model.Birthday.Value;
                    }

                    currentProfile.ImageName = model.ImageName;*/

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

        [Authorize]
        public ActionResult UserProfile(string id)
        {
            var ctx = new ProfileDbContext();
            var profile = ctx.Profiles.FirstOrDefault(p => p.UserId == id);
            return View(new ProfileViewModels
            {
                Firstname = profile?.Firstname,
                Lastname = profile?.Lastname,
                Birthday = profile?.Birthday,
                Description = profile?.Description,
                ProfileId = profile?.UserId
            });
        }


        //Metod för att lägga till vänner
        public void SendFriendRequest(string id)
        {
            var ctx = new ProfileDbContext();
            var currentUser = User.Identity.GetUserId();
           // var currentProfile = ctx.Profiles.FirstOrDefault(p => p.UserId == currentUser);
            var recieverProfile = ctx.Profiles.FirstOrDefault(p => p.UserId == id);


            recieverProfile.Friends.Add(new Friend
            {

                Sender = currentUser,
                FriendshipAccepted = false,
                Reciver = id,
                
                
            });
            
            ctx.SaveChanges();

        }

        public ActionResult FriendRequest()
        {
            var ctx = new ProfileDbContext();
            var currentUser = User.Identity.GetUserId();
            var currentProfile = ctx.Profiles.FirstOrDefault(p => p.UserId == currentUser);

            var listOfProfilesInFriendList = currentProfile.Friends.Where(f => !f.FriendshipAccepted);
            var listOfFriends = new List<FriendListViewModel>();
            foreach (var friend in listOfProfilesInFriendList)
            {
                var friendsProfile = ctx.Profiles.FirstOrDefault(p => p.UserId == friend.Sender);
                var friendModel = new FriendListViewModel
                {
                    Firstname = friendsProfile.Firstname,
                    Lastname = friendsProfile.Lastname,
                    UserId = friend.Reciver,
                    RequestId = friend.FriendId

                };
                listOfFriends.Add(friendModel);

            }

            return View(listOfFriends);
        }

        public ActionResult FriendList()
        {
            var ctx = new ProfileDbContext();
            var currentUser = User.Identity.GetUserId();
            var currentProfile = ctx.Profiles.FirstOrDefault(p => p.UserId == currentUser);

            var listOfProfilesInFriendList = currentProfile.Friends.Where(f => f.FriendshipAccepted);
            var listOfFriends = new List<FriendListViewModel>();
           foreach(var friend in listOfProfilesInFriendList)
            {
                var friendsProfile = ctx.Profiles.FirstOrDefault(p => p.UserId == friend.Sender);
                var friendModel = new FriendListViewModel
                {
                    Firstname = friendsProfile.Firstname,
                    Lastname = friendsProfile.Lastname,
                    UserId = friend.Reciver,
                    RequestId = friend.FriendId
                };
                listOfFriends.Add(friendModel);

            }

            return View(listOfFriends);
        }

        public ActionResult AcceptFriend (int requestId)
        {
            var ctx = new ProfileDbContext();
            var currentUser = User.Identity.GetUserId();
            var currentProfile = ctx.Profiles.FirstOrDefault(p => p.UserId == currentUser);

            var request = currentProfile.Friends.FirstOrDefault(r => r.FriendId == requestId);

            request.FriendshipAccepted = true;

            var senderProfile = ctx.Profiles.FirstOrDefault(p => p.UserId == request.Sender);


            senderProfile.Friends.Add(new Friend
            {

                Sender = currentUser,
                FriendshipAccepted = true,
                Reciver = request.Sender,


            });

            ctx.SaveChanges();
            return RedirectToAction("FriendList");
        }

        public ActionResult RemoveFriend (int requestId)
        {
            var ctx = new ProfileDbContext();
            var currentUser = User.Identity.GetUserId();
            var currentProfile = ctx.Profiles.FirstOrDefault(p => p.UserId == currentUser);

            var request = currentProfile.Friends.FirstOrDefault(r => r.FriendId == requestId);

            

            var otherProfile = ctx.Profiles.FirstOrDefault(p => p.UserId == request.Sender);
            var otherRequest = otherProfile.Friends.FirstOrDefault(p => p.Reciver == currentUser);
            if(otherRequest != null)
            {
                otherProfile.Friends.Remove(otherRequest);
            }
            currentProfile.Friends.Remove(request);
            ctx.SaveChanges();
            return RedirectToAction("FriendList");
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