using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VortexCore.Services;
using VortexCore.ManagersDB;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using VortexCore.Services.MongoDB;
using VortexCore.ModelsDB.VortexDB;
using System.Diagnostics;

namespace VortexCore.Controllers
{
    
    [Route("[controller]/[action]")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        private VortexManager ManagerDB { get; set; }
        private readonly ChatService ChatManager;
        public ApiController(VortexDBContext context, ChatService chatService)
        {
            ManagerDB = new VortexManager(context);
            ChatManager = chatService;
        }

        public ActionResult SendMessage()
        {
            FirebaseControl.SendMessage().ContinueWith((task) =>
            {
                Console.WriteLine("Message sent");
            });
            return new OkResult();
        }

        public ActionResult AddNotificationToken(string token)
        {
            var isSuccess = ManagerDB.AddNotificationToken(token);
            return isSuccess ? new OkResult() : StatusCode(500);
        }

        [Authorize]
        public ActionResult SecureResource()
        {
            var user = User.Claims;
            return new JsonResult("SecureResource");
        }

        [Authorize]
        public async Task<ActionResult> SetRole(string role)
        {
            var user = User.Claims.ToDictionary(x => x.Type, x => x.Value);
            await FirebaseAdmin.Auth.FirebaseAuth.DefaultInstance.SetCustomUserClaimsAsync(user["user_id"], new Dictionary<string, object>() { { ClaimTypes.Role, role } });
            return new OkResult();
        }

        public ActionResult GetMessages()
        {
            var res = ChatManager.GetMessages();
            return new JsonResult(res);
        }

        [Authorize]
        public ActionResult GetSshConnections()
        {
            var res = ManagerDB.GetSshUsers(User.Claims.FirstOrDefault(x => x.Type == "user_id").Value);
            return new JsonResult(res);
        }

        public ActionResult GetSshServers()
        {
            return new JsonResult(ManagerDB.GetSshServers());
        }

        public ActionResult UsernameAvaible(string username, int serverId)
        {
            return new JsonResult(ManagerDB.UsernameAvaible(username, serverId));
        }

    }
}
