using ContactCenter.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpenIddict.Validation.AspNetCore;
using wCyber.Lib.FileStorage;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace ContactCenter.Web.API
{
    [Route("api/[controller]")]
    [ApiController, Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)]
    public class SysAPIController : ControllerBase
    {
        public Guid CurrentUserId => Guid.Parse(User.FindFirst(Claims.Subject).Value);

        static bool IsDbCreated;
        EDRSMContext _db;
        public EDRSMContext Db
        {
            get
            {
                if (_db == null)
                {
                    _db = Request.HttpContext.RequestServices.GetService<EDRSMContext>();
                    if (!IsDbCreated)
                    {
                        try
                        {
                            _db.Database.Migrate();
                        }
                        catch { }
                        IsDbCreated = true;
                    }
                }
                return _db;
            }
        }

        User _currentUser;
        public User CurrentUser
        {
            get
            {
                if (_currentUser == null)
                {
                    _currentUser = Db.Users
                        .Include(c => c.Agent)
                        .FirstOrDefault(c => c.Id == CurrentUserId);
                }
                return _currentUser;
            }
        }

        IFileStore _filestore;
        protected IFileStore FileStore
        {
            get
            {
                if (_filestore == null) _filestore = Request.HttpContext.RequestServices.GetService<IFileStore>();
                return _filestore;
            }
        }
    }
}
