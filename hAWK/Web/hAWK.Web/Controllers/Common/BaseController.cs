namespace hAWK.Web.Controllers.Common
{
    using System.Linq;
    using System.Web.Mvc;
    using hAWK.Data;
    using hAWK.Data.Models;

    public class BaseController : Controller
    {
        protected readonly IHawkData data;

        protected virtual User CurrentUser
        {
            get
            {
                return this.data.Users.All()
                    .FirstOrDefault(u => u.UserName == this.User.Identity.Name);
            }
        }

        public BaseController(IHawkData data)
        {
            this.data = data;
        }
	}
}