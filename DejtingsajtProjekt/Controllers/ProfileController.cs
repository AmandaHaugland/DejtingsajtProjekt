using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
                Exists = exists
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProfileViewModels model)
        {
            var profileCtx = new ProfileDbContext();
            var currentUser = User.Identity.GetUserId();
            var currentProfile = profileCtx.Profiles.FirstOrDefault(p => p.UserId == currentUser);

            if (currentProfile == null)
            {
                profileCtx.Profiles.Add(new ProfileModel
                {
                    UserId = currentUser,
                    Firstname = model.Firstname,
                    Lastname = model.Lastname,
                    Birthday = model.Birthday.Value,
                    Description = model.Description

                   // ImageName = model.ImageName


                });
            }
            else
            {
                currentProfile.Firstname = model.Firstname ?? currentProfile.Firstname;
                currentProfile.Lastname = model.Lastname ?? currentProfile.Lastname;
                currentProfile.Description = model.Description ?? currentProfile.Description;
                if(model.Birthday == null)
                {
                    currentProfile.Birthday = currentProfile.Birthday;
                }
                else
                {
                    currentProfile.Birthday = model.Birthday.Value;
                }
                
               // currentProfile.ImageName = model.ImageName;

            }
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

            ctx.SaveChanges();
            return RedirectToAction("FriendList");
        }

        public ActionResult RemoveFriend (int requestId)
        {
            var ctx = new ProfileDbContext();
            var currentUser = User.Identity.GetUserId();
            var currentProfile = ctx.Profiles.FirstOrDefault(p => p.UserId == currentUser);

            var request = currentProfile.Friends.FirstOrDefault(r => r.FriendId == requestId);

            currentProfile.Friends.Remove(request);
            
            ctx.SaveChanges();
            return RedirectToAction("FriendList");
        }

        
    }

}