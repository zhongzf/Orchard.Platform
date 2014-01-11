﻿using System;
using System.Web;
using System.Web.Security;
using Orchard.Environment.Configuration;
using Orchard.Logging;
using Orchard.ContentManagement;
using Orchard.Mvc;
using Orchard.Services;

namespace Orchard.Security.Providers {
    public class FormsAuthenticationService : IAuthenticationService {
        private readonly ShellSettings _settings;
        private readonly IClock _clock;
        private readonly IContentManager _contentManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private IUser _signedInUser;
        private bool _isAuthenticated;

        public FormsAuthenticationService(ShellSettings settings, IClock clock, IContentManager contentManager, IHttpContextAccessor httpContextAccessor) {
            _settings = settings;
            _clock = clock;
            _contentManager = contentManager;
            _httpContextAccessor = httpContextAccessor;

            Logger = NullLogger.Instance;
            
            ExpirationTimeSpan = TimeSpan.FromDays(30);
        }

        public ILogger Logger { get; set; }

        public TimeSpan ExpirationTimeSpan { get; set; }

        public void SignIn(IUser user, bool createPersistentCookie) {
            var now = _clock.UtcNow.ToLocalTime();
            var userData = Convert.ToString(user.Id);

            var ticket = new FormsAuthenticationTicket(
                1 /*version*/,
                user.UserName,
                now,
                now.Add(ExpirationTimeSpan),
                createPersistentCookie,
                userData,
                FormsAuthentication.FormsCookiePath);

            var encryptedTicket = FormsAuthentication.Encrypt(ticket);

            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket) {
                HttpOnly = true, 
                Secure = FormsAuthentication.RequireSSL, 
                Path = FormsAuthentication.FormsCookiePath
            };

            var httpContext = _httpContextAccessor.Current();

            if (!String.IsNullOrEmpty(_settings.RequestUrlPrefix)) {
                var cookiePath = httpContext.Request.ApplicationPath;
                if (cookiePath != null && cookiePath.Length > 1) {
                    cookiePath += '/';
                }

                cookiePath += _settings.RequestUrlPrefix;
                cookie.Path = cookiePath;
            }

            if (FormsAuthentication.CookieDomain != null) {
                cookie.Domain = FormsAuthentication.CookieDomain;
            }

            if (createPersistentCookie) {
                cookie.Expires = ticket.Expiration;
            }
            
            httpContext.Response.Cookies.Add(cookie);

            _isAuthenticated = true;
            _signedInUser = user;
        }

        public void SignOut() {
            _signedInUser = null;
            _isAuthenticated = false;
            FormsAuthentication.SignOut();
        }

        public void SetAuthenticatedUserForRequest(IUser user) {
            _signedInUser = user;
            _isAuthenticated = true;
        }

        public IUser GetAuthenticatedUser() {
            if (_signedInUser != null || _isAuthenticated)
                return _signedInUser;

            var httpContext = _httpContextAccessor.Current();
            if (httpContext == null || !httpContext.Request.IsAuthenticated || !(httpContext.User.Identity is FormsIdentity)) {
                return null;
            }

            var formsIdentity = (FormsIdentity)httpContext.User.Identity;
            var userData = formsIdentity.Ticket.UserData;
            int userId;
            if (!int.TryParse(userData, out userId)) {
                Logger.Fatal("User id not a parsable integer");
                return null;
            }

            _isAuthenticated = true;
            return _signedInUser = _contentManager.Get(userId).As<IUser>();
        }
    }
}
