using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VortexCore.ManagersDB;
using VortexCore.ModelsDB.VortexDB;

namespace VortexCore.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class ServiceController : ControllerBase
    {
        public ServiceController(VortexDBContext context)
        {
            ManagerDB = new VortexManager(context);
        }

        private VortexManager ManagerDB { get; set; }

    
        public ActionResult GetSshConnecton(int sshUserId, string uid)
        {
            SshUser sshUser = ManagerDB.GetSshUser(sshUserId);
            if (sshUser.UserId == uid)
            {
                var connectionString = $"{sshUser.Username}@{sshUser.Server.Host}";
                var port = sshUser.Server.Port == 22 ? "" : $"-p {sshUser.Server.Port}";
                return new JsonResult(connectionString + port);
            }
            return Unauthorized();
        }

        public ActionResult CanCreateUser(string username, int serverId)
        {
            
            if (!ManagerDB.UsernameAvaible(username, serverId)) return StatusCode(403);
            var sshServer = ManagerDB.GetSshServer(serverId);
            if(sshServer == null) return StatusCode(405);
            var connectionString = $"root@{sshServer.Host}";
            var port = sshServer.Port == 22 ? "" : $"-p {sshServer.Port}";
            return new JsonResult(connectionString + port);
        }

        public ActionResult AddSshUser(string uid, string username, int serverId)
        {
            if (!ManagerDB.UsernameAvaible(username, serverId)) return new StatusCodeResult(405);
            var sshUser = new SshUser
            {
                ServerId = serverId,
                Username = username,
                UserId = uid
            };
            ManagerDB.AddSshUser(sshUser);
            return new OkResult();
        }
    }
}