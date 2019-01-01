using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DejtingsajtProjekt.Models
{
    public class Profile
    {
        [Key]
        public int Id { get; set; }

        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public DateTime Birthday { get; set; }

        //Foreign key till User
    }
}