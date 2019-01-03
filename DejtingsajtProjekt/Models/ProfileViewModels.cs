﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DejtingsajtProjekt.Models
{
    public class ProfileViewModels
    {
        public string Firstname { get; set; }

        public string Lastname { get; set; }

        [DataType(DataType.Date)]
        public DateTime? Birthday { get; set; }

        public bool Exists { get; set; }
    }
}