using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FirebaseAdmin.Auth;

namespace VortexCore.DtoClasses
{
    public class User
    {
        public User()
        {

        }

        public async static Task<User> GetUser(ClaimsPrincipal principal)
        {
            var uid = principal.FindFirstValue("user_id");
            return await GetUser(uid);
        }
        public async static Task<User> GetUser(string uid)
        {
            var user = await FirebaseAuth.DefaultInstance.GetUserAsync(uid);

            return new User
            {
                Uid = uid,
                Email = user.Email,
                Name = user.DisplayName,
                PhotoUrl = user.PhotoUrl
            };
        }

        public string Uid { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string PhotoUrl { get; set; }
    }
}
