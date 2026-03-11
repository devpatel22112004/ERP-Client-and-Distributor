using MVCMarketing.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCMarketing.Controllers
{
    [SessionTimeout]
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult HttpError403()
        {
            return View();
        }
        public ActionResult HttpError404()
        {
            return View();
        }
        public ActionResult HttpError500()
        {
            return View();
        }
    }
}