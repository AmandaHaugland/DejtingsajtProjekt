﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DejtingsajtProjekt.Models
{
    public class ProfileViewModels
    {
        public string ProfileId { get; set; }

        public string Firstname { get; set; }

        public string Lastname { get; set; }

        [DataType(DataType.Date)]
        public DateTime? Birthday { get; set; }

        public string Description { get; set; }

        public bool Exists { get; set; }

        public string ImageName { get; set; }
        public IEnumerable<string> GalleryImage { get; set; }

        public List<Friend> Friends { get; set; }

        public List<Message> Messages { get; set; }
    }

    public class ProfileListViewModel
    {
        public List<ProfileModel> Profiles { get; set; }
    }

    public class FriendListViewModel
    {
        public string UserId { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public int RequestId { get; set; }
    }

    public class MessageViewModel
    {
        public int MessageId { get; set; }

        public string SenderId { get; set; }
        public string SenderName { get; set; }
        public string MessageText { get; set; }

        public string Reciver { get; set; }
    }

    public class ProfileAddViewModel
    {
        [Required]
        [RegularExpression(@"^[A-Z]+[a-zA-Z'\s]*$")]
        [StringLength(60, MinimumLength = 3)]
        public string Firstname { get; set; }

        [Required]
        [RegularExpression(@"^[A-Z]+[a-zA-Z'\s]*$")]
        [StringLength(60, MinimumLength = 3)]
        public string Lastname { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime? Birthday { get; set; }

        [Required]
        [StringLength(300, MinimumLength = 3)]
        public string Description { get; set; }
    }

    public class ProfileUpdateViewModel
    {
        
        [RegularExpression(@"^[A-Z]+[a-zA-Z'\s]*$")]
        [StringLength(60, MinimumLength = 3)]
        public string Firstname { get; set; }
        
        [RegularExpression(@"^[A-Z]+[a-zA-Z'\s]*$")]
        [StringLength(60, MinimumLength = 3)]
        public string Lastname { get; set; }

      
        [DataType(DataType.Date)]
        public DateTime? Birthday { get; set; }

        
        [StringLength(300, MinimumLength = 3)]
        public string Description { get; set; }

        public string ImageName { get; set; }

    }
}