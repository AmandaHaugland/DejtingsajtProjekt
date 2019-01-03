using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
                Exists = exists
            });
        }

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
                    Birthday = model.Birthday.Value

                });
            }
            else
            {
                currentProfile.Firstname = model.Firstname;
                currentProfile.Lastname = model.Lastname;
                currentProfile.Birthday = model.Birthday.Value;
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
    }
}