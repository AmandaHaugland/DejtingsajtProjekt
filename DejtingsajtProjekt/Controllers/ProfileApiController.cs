using DejtingsajtProjekt.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DejtingsajtProjekt.Controllers
{
    [RoutePrefix("api/profiles")]
    public class ProfileApiController : ApiController
    {
        [Route("message/add")]
        [HttpGet]
        public void AddMessage(string reciverId, string messageText)
        {
            var ctx = new ProfileDbContext();
            var recieverProfile = ctx.Profiles.FirstOrDefault(p => p.UserId == reciverId);
            var senderId = User.Identity.GetUserId();

            recieverProfile.Messages.Add(new Message
            {
                MessageText = messageText,
                Sender = senderId,
                Reciver = reciverId,

            });
            ctx.SaveChanges();
        }


    }
}