using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Test.Helpers;
using Test.Models;
using Test.ModelViews;

namespace Test.Controllers
{
    public class UserController : Controller
    {
        private readonly UserModel _context = new UserModel();

        public ActionResult List()
        {
            string cityFilter = Request.QueryString["CityFilter"] as string;
            int sortChoice = 0;
            if (Request.QueryString["sortChoice"] != null)
                Int32.TryParse(Request.QueryString["sortChoice"], out sortChoice);

            var users = GetSelectedUsers(cityFilter, sortChoice);

            var userViewModel = new UsersViewModel
            {
                Users = users.ToList(),
                AllCities = _context.UserProfiles.Select(x => x.City).Distinct().ToList()
            };

            return View(userViewModel);
        }

        private IEnumerable<UserProfile> GetSelectedUsers(string cityFilter, int sortChoice = 0)
        {
            IEnumerable<UserProfile> selectedUsers = _context.UserProfiles;

            if (!selectedUsers.Any())
            {
                _context.UserProfiles.Add(new UserProfile { Name = "Саша", City = "Київ", Age = 18 });
                _context.UserProfiles.Add(new UserProfile { Name = "Аня", City = "Львів", Age = 25 });
                _context.UserProfiles.Add(new UserProfile { Name = "Віка", City = "Харьків", Age = 22 });
                _context.UserProfiles.Add(new UserProfile { Name = "Ігор", City = "Київ", Age = 40 });
                _context.UserProfiles.Add(new UserProfile { Name = "Ваня", City = "Львів", Age = 16 });

                _context.SaveChanges();
            }

            if ((cityFilter != null) && (cityFilter.Trim() != ""))
                selectedUsers = selectedUsers.Where(x => x.City == cityFilter);

            switch (sortChoice)
            {
                case (int)SortChoice.YoungerFirst:
                    {
                        selectedUsers = selectedUsers.OrderBy(x => x.Age);
                        break;
                    }
                case (int)SortChoice.OlderFirst:
                    {
                        selectedUsers = selectedUsers.OrderByDescending(x => x.Age);
                        break;
                    }
                default:
                    break;
            }
            
            return selectedUsers;
        }
    }
}
