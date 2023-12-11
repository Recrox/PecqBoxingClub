using PecqBoxingClubApi.BackEnd.Configuration;
using PecqBoxingClubApi.BackEnd.Core;
using PecqBoxingClubApi.BackEnd.Core.Models.Table;
using Microsoft.AspNetCore.Mvc;

namespace PecqBoxingClubApi.BackEnd.Api.Controllers
{
    public class BaseController : Controller
    {
        //protected readonly CurrentContext _currentContext;
        protected readonly GlobalSettings _globalSettings;

        //protected User _user => _currentContext?.User;
        //protected string _deviceIdentifier => _currentContext?.DeviceIdentifier;

        public BaseController(
            GlobalSettings globalSettings
            //CurrentContext currentContext
            )
        {
            _globalSettings = globalSettings;
            //_currentContext = currentContext;
        }

        protected FileContentResult GetFileContentResult(string filename, string contentType, ref byte[] fileContent)
        {
            var accessKey = "Access-Control-Expose-Headers";
            var filenameKey = "Filename";
            if (Response.Headers.ContainsKey(accessKey))
                Response.Headers[accessKey] = $"{Response.Headers[accessKey]}, {filenameKey}";
            else
                Response.Headers.Add(accessKey, filenameKey);
            Response.Headers.Add(filenameKey, filename);
            return File(
                    fileContent,
                    contentType,
                    filename
                    );
        }
    }
}
