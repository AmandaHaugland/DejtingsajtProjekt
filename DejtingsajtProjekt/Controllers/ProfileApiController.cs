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
        //Tar in ett meddelande och en mottagare för att skicka meddelande
        ///api/profiles/message/add?reciverId= + reciverId + &messageText= + message
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


        //Hämtar meddelanden som tillhör den id som skickas in
        ///api/profiles/message/show?reciverId= + reciverId 
        [Route("message/show")]
        [HttpGet]
        public MessageViewModel[] GetMessages(string reciverId)
        {
            var ctx = new ProfileDbContext();
            var currentUser = User.Identity.GetUserId();
            var currentProfile = ctx.Profiles.FirstOrDefault(p => p.UserId == currentUser);
           if(currentProfile == null)
            {
                var listMessageViewModels = new List<MessageViewModel>();
                return listMessageViewModels.ToArray();
            }
            var listOfMessages = currentProfile.Messages;

            if (reciverId != null) {
                var reciverProfile = ctx.Profiles.FirstOrDefault(p => p.UserId == reciverId);

                 listOfMessages = reciverProfile.Messages;
            } 
            
            
            var listMessageViewModel = new List<MessageViewModel>();
            if(listOfMessages != null)
            {
                foreach(var message in listOfMessages)
                {
                     var senderProfile = ctx.Profiles.FirstOrDefault(p => p.UserId == message.Sender);
                     var messageToAdd = new MessageViewModel
                     {
                         SenderId = message.Sender,
                         SenderName = senderProfile.Firstname + " " + senderProfile.Lastname,
                         MessageText = message.MessageText,
                         MessageId = message.MessageId,
                         Reciver = message.Reciver
                     };
                     listMessageViewModel.Add(messageToAdd);
                }
            }
           var toReturn = listMessageViewModel.ToArray();
            return toReturn;
        }


    }
}