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

    public class ProfileController : Controller
    {
        // GET: Profile
        //Skickar tillbaka information om den inloggade användaren
        [Authorize]
        public ActionResult Index()
        {
            var profileCtx = new ProfileDbContext();
            var currentUser = User.Identity.GetUserId();
            var currentProfile = profileCtx.Profiles.FirstOrDefault(p => p.UserId == currentUser);
            var exists = false;

            if (currentProfile != null)
            {
                exists = true;
            } else
            {
                return RedirectToAction("AddNewProfile", "Profile");
            }

            return View(new ProfileViewModels {
                Firstname = currentProfile?.Firstname,
                Lastname = currentProfile?.Lastname,
                Birthday = currentProfile?.Birthday,
                Description = currentProfile?.Description,
                ImageName = currentProfile?.ImageName,
                Exists = exists
            });
        }
        public ActionResult AddNewProfile()
        {
            return View();
        }
        

        [HttpPost]
        public ActionResult AddNewProfile ([Bind(Include = "Firstname,Lastname,Birthday,Description")]ProfileAddViewModel viewModel){
            if (ModelState.IsValid)
            {
                var profileCtx = new ProfileDbContext();
                var currentUser = User.Identity.GetUserId();
                var currentProfile = profileCtx.Profiles.FirstOrDefault(p => p.UserId == currentUser);
                var imgPath = "/images/placeholderImg.jpg";

                profileCtx.Profiles.Add(new ProfileModel
                {
                    UserId = currentUser,
                    Firstname = viewModel.Firstname,
                    Lastname = viewModel.Lastname,
                    Birthday = viewModel.Birthday.Value,
                    Description = viewModel.Description,
                    ImageName = imgPath
                });
                profileCtx.SaveChanges();
                return RedirectToAction("Index");
            }
            // return RedirectToAction("Index", "Profile");
            return View(viewModel);
        }

        //Redigerar användaren och dess profil
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Firstname,Lastname,Birthday,Description")]ProfileUpdateViewModel model)
        {
            var profileCtx = new ProfileDbContext();
            var currentUser = User.Identity.GetUserId();
            var currentProfile = profileCtx.Profiles.FirstOrDefault(p => p.UserId == currentUser);


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
                //if (file.ContentLength > 0)
                //{
                //    string _FileName = Path.GetFileName(file.FileName);
                //    string _path = Path.Combine(Server.MapPath("~/images"), _FileName);
                //    file.SaveAs(_path);
                //    var imgNameToSave = "/images/" + _FileName;
                //    profileCtx.Profiles.FirstOrDefault(p => p.UserId == currentUser).ImageName = imgNameToSave;
               
                //} else
                //{
                //    currentProfile.ImageName = model.ImageName;
                //}
               

            


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


        //Här får man en lista med alla profiler
        public ActionResult ProfileList()
        {
            var ctx = new ProfileDbContext();

            var viewModel = new ProfileListViewModel {
                Profiles = ctx.Profiles.ToList() };

            return View(viewModel);
        }

        //Skicka in ett id och få profilen som tillhör id
        [Authorize]
        public ActionResult UserProfile(string id)
        {
            var ctx = new ProfileDbContext();
            var profile = ctx.Profiles.FirstOrDefault(p => p.UserId == id);
            var currentUser = User.Identity.GetUserId();
            if(currentUser == id)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View(
                         new ProfileViewModels
                     {
                         Firstname = profile?.Firstname,
                         Lastname = profile?.Lastname,
                         Birthday = profile?.Birthday,
                         Description = profile?.Description,
                         ProfileId = profile?.UserId,
                         ImageName = profile?.ImageName
                     });
            }
            
        }


        //Metod för att lägga till vänner
        public ActionResult SendFriendRequest(string id)
        {
            var ctx = new ProfileDbContext();
            var currentUser = User.Identity.GetUserId();
            var recieverProfile = ctx.Profiles.FirstOrDefault(p => p.UserId == id);


            recieverProfile.Friends.Add(new Friend
            {

                Sender = currentUser,
                FriendshipAccepted = false,
                Reciver = id,
                
                
            });
            
            ctx.SaveChanges();
            return RedirectToAction("Index");

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

        //Lägg till en vän
        public ActionResult AcceptFriend (int requestId)
        {
            var ctx = new ProfileDbContext();
            var currentUser = User.Identity.GetUserId();
            var currentProfile = ctx.Profiles.FirstOrDefault(p => p.UserId == currentUser);

            var request = currentProfile.Friends.FirstOrDefault(r => r.FriendId == requestId);
            //Nu är det inte en förfrågan utan vän
            request.FriendshipAccepted = true;

            var senderProfile = ctx.Profiles.FirstOrDefault(p => p.UserId == request.Sender);

            //Lägg till i den som har skickat förfrågans vännlista också
            senderProfile.Friends.Add(new Friend
            {

                Sender = currentUser,
                FriendshipAccepted = true,
                Reciver = request.Sender,


            });

            ctx.SaveChanges();
            return RedirectToAction("FriendList");
        }

        //Ta bort vän
        public ActionResult RemoveFriend (int requestId)
        {
            var ctx = new ProfileDbContext();
            var currentUser = User.Identity.GetUserId();
            var currentProfile = ctx.Profiles.FirstOrDefault(p => p.UserId == currentUser);

            var request = currentProfile.Friends.FirstOrDefault(r => r.FriendId == requestId);

            
            //Om det finns i en annan använadres lista så ska den också tas bort där.
            var otherProfile = ctx.Profiles.FirstOrDefault(p => p.UserId == request.Sender);
            var otherRequest = otherProfile.Friends.FirstOrDefault(p => p.Sender == currentUser);
            if(otherRequest != null)
            {
                otherProfile.Friends.Remove(otherRequest);
            }
            currentProfile.Friends.Remove(request);
            ctx.SaveChanges();
            return RedirectToAction("FriendList");
        }

        //Får fram antalet vänförfrågningar
        public string NumberOfFriendRequests()
        {
            var ctx = new ProfileDbContext();
            var currentUser = User.Identity.GetUserId();
            var currentProfile = ctx.Profiles.FirstOrDefault(p => p.UserId == currentUser);
            var numberAsString = "0";
            if (currentProfile != null)
            {
                var listOfProfilesInFriendList = currentProfile.Friends.Where(f => !f.FriendshipAccepted);
                
                if(listOfProfilesInFriendList != null)
            {
                 numberAsString = listOfProfilesInFriendList.Count().ToString();
            }
            }
            
            

            return numberAsString;
        }
     

     

        public ActionResult SearchUser(string firstname, string lastname)
        {
            ProfileDbContext ctx = new ProfileDbContext();
          
            List<ProfileModel> listOfProfiles = ctx.Profiles.ToList();
            if (!String.IsNullOrEmpty(firstname) || !String.IsNullOrEmpty(lastname))
            {
                listOfProfiles = ctx.Profiles.Where(p => p.Firstname.Contains(firstname) && p.Lastname.Contains(lastname)).ToList();
            }
          
            return View(listOfProfiles);

        }


        //Hämtar ut upp till 5 användare att presentera på framsidan
        public ActionResult _GetFiveUsers()
        {
            var ctx = new ProfileDbContext();
            var fullList = new ProfileListViewModel();
            if (ctx.Profiles.Count() >= 5)
            {
                 fullList = new ProfileListViewModel
                {
                    Profiles = ctx.Profiles.Take(5).ToList()
                };

            } else
            {
                fullList = new ProfileListViewModel
                {
                    Profiles = ctx.Profiles.ToList()
                };
            }
                return View(fullList);
            
        }

      [HttpPost]
      public ActionResult EditImage(ProfileUpdateViewModel model,HttpPostedFileBase file)
        {
            var profileCtx = new ProfileDbContext();
            var currentUser = User.Identity.GetUserId();
            var currentProfile = profileCtx.Profiles.FirstOrDefault(p => p.UserId == currentUser);

            if (file.ContentLength > 0)
            {
                string _FileName = Path.GetFileName(file.FileName);
                string _path = Path.Combine(Server.MapPath("~/images"), _FileName);
                file.SaveAs(_path);
                var imgNameToSave = "/images/" + _FileName;
                profileCtx.Profiles.FirstOrDefault(p => p.UserId == currentUser).ImageName = imgNameToSave;

            }
            else
            {
                currentProfile.ImageName = model.ImageName;
            }
            profileCtx.SaveChanges();
            return RedirectToAction("Index", "Profile");
        }
        

    }

}