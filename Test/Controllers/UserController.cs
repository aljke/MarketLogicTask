using Newtonsoft.Json.Linq;
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
            var users = GetSelectedUsers(); //in this case return's all users

            var userViewModel = new UsersViewModel
            {
                Users = users.ToList(),
                AllCities = _context.UserProfiles.Select(x => x.City).Distinct().ToList() //data for combobox of cities
            };

            return View(userViewModel);
        }

        /// <summary>
        /// Return filtered and ordered list in JSON
        /// </summary>
        /// <param name="cityFilter">City filter from client</param>
        /// <param name="ageOrder">Order choice from client</param>
        /// <returns>JSON of users</returns>
        public ActionResult GetUsersJson(string cityFilter, string ageOrder)
        {
            int order = 0;
            Int32.TryParse(ageOrder, out order);
            if (ageOrder == null)
                ageOrder = "";
            return Json(GetSelectedUsers(cityFilter, order).ToList());
        }

        /// <summary>
        /// Return IEnumerable of users
        /// </summary>
        /// <param name="cityFilter">City filter</param>
        /// <param name="sortChoice">Order choice</param>
        /// <returns>IEnumerable of users</returns>
        private IEnumerable<UserProfile> GetSelectedUsers(string cityFilter = "", int sortChoice = 0)
        {
            // get all users
            IEnumerable<UserProfile> selectedUsers = _context.UserProfiles;

            //if table is empty
            if (!selectedUsers.Any())
            {
                _context.UserProfiles.Add(new UserProfile { Name = "Саша", City = "Київ", Age = 18 });
                _context.UserProfiles.Add(new UserProfile { Name = "Аня", City = "Львів", Age = 25 });
                _context.UserProfiles.Add(new UserProfile { Name = "Віка", City = "Харьків", Age = 22 });
                _context.UserProfiles.Add(new UserProfile { Name = "Ігор", City = "Київ", Age = 40 });
                _context.UserProfiles.Add(new UserProfile { Name = "Ваня", City = "Львів", Age = 16 });

                _context.SaveChanges();
            }

            //do filtering
            if (cityFilter.Trim() != "")
                selectedUsers = selectedUsers.Where(x => x.City == cityFilter);

            //do ordering
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
