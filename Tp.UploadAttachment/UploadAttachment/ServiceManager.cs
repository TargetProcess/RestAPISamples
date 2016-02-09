using Microsoft.Web.Services3;
using System.Configuration;
using Tp.Service.Proxies;

namespace UploadAttachment
{
    public class ServiceManager
    {
        private readonly string path;
        private readonly string login;
        private readonly string password;

        public ServiceManager()
        {
            path = ConfigurationManager.AppSettings["TargetProcessPath"];
            login = ConfigurationManager.AppSettings["AdminLogin"];
            password = ConfigurationManager.AppSettings["AdminPassword"];
        }

        public T GetService<T>() where T : WebServicesClientProtocol, new()
        {
            var serviceWse = new T
            {
                Url = path + "/Services/" + typeof(T).Name + ".asmx"
            };

            CredentialInitializer.InitCredentials(serviceWse, false, login, password);
            return serviceWse;
        }
    }
}
