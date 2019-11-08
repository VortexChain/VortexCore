using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace VortexCore.Services
{
    public class FirebaseControl
    {
        public static FirebaseApp FirebaseApp { get; set; }
        public static FirebaseMessaging Messaging { get; set; }

        public static async Task SendMessage()
        {
            var messageData = new Dictionary<string, string>()
            {
                { "title", "Uhuuu" },
                { "message", "Message from .net server" }
            };
            var message = new Message()
            {
                Data = messageData,
                Token = "fGAB6Qdn13A:APA91bFx12LswYRDAz2Awsfm1ek5aqc7VrybMpBveAKYEXxnPMUIaUB0PQaRHiapTNIhevisWWBinrrS6f7SVzUgXRF4frrs2XCn4d2clPBTqKwd8wVoDc8loi7vSxfdVcPCjSO8Bos4"
            };
            await Messaging.SendAsync(message);
        }

        public FirebaseControl()
        {
            FirebaseApp = FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromFile("FireBaseCredential.json"),
            });
            Messaging = FirebaseMessaging.GetMessaging(FirebaseApp);
        }
    }
}
