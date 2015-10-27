using MessagingAPI.Models;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MessagingAPI.Controllers
{
    [System.Web.Http.Authorize]
    public abstract class AuthorizedController<THub> : ApiController
        where THub : IHub
    {
        private ApplicationDbContext efContext = new ApplicationDbContext();

        private Lazy<IHubContext> hub = new Lazy<IHubContext>(
            () => GlobalHost.ConnectionManager.GetHubContext<THub>()
        );

        private ApplicationUserManager _userManager;

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        protected ApplicationDbContext EfContext
        {
            get { return efContext; }
        }

        protected IHubContext Hub
        {
            get { return hub.Value; }
        }
    }
}
