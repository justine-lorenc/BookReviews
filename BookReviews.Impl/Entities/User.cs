﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookReviews.Impl.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DateAdded { get; set; }
        public string DateUpdated { get; set; }
        public string EmailAddress { get; set; }
        public string PasswordHash { get; set; }
        public bool IsActive { get; set; }
    }
}
