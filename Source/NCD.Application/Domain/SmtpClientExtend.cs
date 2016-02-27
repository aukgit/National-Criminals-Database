using System.Web.Configuration;
using DevMvcComponent;
using DevMvcComponent.Encryption;
using DevMvcComponent.Mail;

namespace NCD.Application.Domain {
    public static class SmtpClientExtend {

        private static CustomMailServer _customMailServer { get; set; }

        /// <summary>
        /// Get a configured Smtp mail client.
        /// Where mail server , port , password etc will get retreived from Web.Config App keys section
        /// These app keys are required in the running application :  MailHostingServer, MailingPort, MailSender, MailSenderEncryptedPassword,EncryptedPassPhrase,enableSSL
        /// MailSenderEncryptedPassword should be encrypted from running the "PasswordEncrypt" application and giving a passphrase in the console.
        /// Remember "class library or DLL" projects cannot hold App.Config information. 
        /// It should be placed on the web app's web.config file.
        /// </summary>
        /// <returns></returns>
        public static CustomMailServer GetSmtpClient() {
            if (_customMailServer == null) {
                var assembly = System.Reflection.Assembly.GetExecutingAssembly();

                Mvc.Setup(assembly);
                var appSettings = WebConfigurationManager.AppSettings;
                var mailHostingServer = appSettings["MailHostingServer"];
                var mailSender = appSettings["MailSender"];
                var mailServerPort = int.Parse(appSettings["MailingPort"]);
                var mailSenderEncryptedPassword = appSettings["MailSenderEncryptedPassword"];
                var passPhrase = appSettings["EncryptedPassPhrase"];
                var enableSSL = appSettings["enableSSL"] == "true";
                var senderDecryptedPassword = Pbkdf2Encryption.Decrypt(mailSenderEncryptedPassword, passPhrase);
                _customMailServer = new CustomMailServer(mailSender, senderDecryptedPassword, mailHostingServer, mailServerPort, enableSSL);
                Mvc.Setup("National Criminals Database", "devorg.bd@gmail.com", assembly, _customMailServer);

            }
            return _customMailServer;
        }

    }
}
