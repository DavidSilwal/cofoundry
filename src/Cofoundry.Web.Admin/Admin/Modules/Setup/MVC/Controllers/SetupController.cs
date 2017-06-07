﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cofoundry.Domain;
using Cofoundry.Domain.CQS;
using Microsoft.AspNetCore.Mvc;

namespace Cofoundry.Web.Admin
{
    [Area(RouteConstants.AdminAreaName)]
    [AdminRoute(SetupRouteLibrary.RoutePrefix)]
    public class SetupController : Controller
    {
        #region Constructor

        private readonly IQueryExecutor _queryExecutor;
        private readonly ILoginService _loginService;
        private readonly IAdminRouteLibrary _adminRouteLibrary;

        public SetupController(
            IQueryExecutor queryExecutor,
            ILoginService loginService,
            IAdminRouteLibrary adminRouteLibrary
            )
        {
            _queryExecutor = queryExecutor;
            _loginService = loginService;
            _adminRouteLibrary = adminRouteLibrary;
        }

        #endregion

        #region routes

        [Route("")]
        public async Task<ActionResult> Index()
        {
            var settings = await _queryExecutor.GetAsync<InternalSettings>();
            if (settings.IsSetup)
            {
                return RedirectToDashboard();
            }

            // force sign-out - solves a rare case where you're re-initializing a db after being signed into a previous version.
            await _loginService.SignOutAsync();

            var viewPath = ViewPathFormatter.View("Setup", nameof(Index));
            return View(viewPath);
        }

        #endregion

        #region private helpers

        private ActionResult RedirectToDashboard()
        {
            return Redirect(_adminRouteLibrary.Dashboard.Dashboard());
        }

        #endregion
    }
}