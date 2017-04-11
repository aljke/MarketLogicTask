using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Test.Helpers;
using Test.Models;

namespace Test.ModelViews
{
    public class UsersViewModel
    {
        public List<UserProfile> Users { get; set; }
        public SortChoice SortChoice { get; set; }
        public List<string> AllCities { get; set; }
    }
}