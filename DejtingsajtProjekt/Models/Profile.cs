using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace DejtingsajtProjekt.Models
{
    public class ProfileModel
    {
        [Key]
        public string UserId { get; set; }

        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public DateTime Birthday { get; set; }

        public string Description { get; set; }

        public virtual ICollection<Friend> Friends { get; set; }
        public ProfileModel()
        {
            Friends = new HashSet<Friend>();
        }

      //  public string ImageName { get; set; }
        public string ImageName { get; set; }

    }

     public class Friend
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FriendId { get; set; }

        public string Sender { get; set; }
        public bool FriendshipAccepted { get; set; }

        public string Reciver { get; set; }
        public virtual ProfileModel  ProfileModel { get; set; }
    }

    public class ProfileDbContext : DbContext
    {
        public DbSet<ProfileModel> Profiles { get; set; }
        public DbSet<Friend> Friends { get; set; }

        public ProfileDbContext() : base("ProfileDb") { }
    }
}