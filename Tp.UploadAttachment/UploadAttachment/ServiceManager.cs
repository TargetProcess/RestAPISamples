using Microsoft.Web.Services3;
using System.Configuration;
using System.Net;
using Tp.Service.Proxies;

namespace UploadAttachment
{
    public class ServiceManager
    {
        private readonly string _path;
        private readonly string _login;
        private readonly string _password;

        public ServiceManager()
        {
            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12; 

            _path = ConfigurationManager.AppSettings["TargetProcessPath"];
            _login = ConfigurationManager.AppSettings["AdminLogin"];
            _password = ConfigurationManager.AppSettings["AdminPassword"];
        }

        public T GetService<T>() where T : WebServicesClientProtocol, new()
        {
            var clientProto = new T
            {
                Url = $"{_path}/Services/{typeof(T).Name}.asmx"
            };

            CredentialInitializer.InitCredentials(clientProto, false, _login, _password);
            return clientProto;
        }
    }
}
