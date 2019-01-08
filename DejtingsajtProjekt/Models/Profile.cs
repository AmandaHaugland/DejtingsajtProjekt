using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        //public string Description { get; set; }

      //  public string ImageName { get; set; }

    }

    public class ProfileDbContext : DbContext
    {
        public DbSet<ProfileModel> Profiles { get; set; }

        public ProfileDbContext() : base("ProfileDb") { }
    }
}