using Microsoft.AspNetCore.Mvc;
using PrescriptionProjectAssignment.Context;
using PrescriptionProjectAssignment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Dapper;

namespace PrescriptionProjectAssignment.Controllers
{
    public class ActionController : Controller
    {
            private readonly DapperContext _dapperContext;

            public ActionController(DapperContext dapperContext)
            {
            _dapperContext = dapperContext;
            }

            // Return Home page.
            public ActionResult Index()
            {
                return View();
            }

            public ActionResult Login()
            {
                return View();
            }

            //The login form is posted to this method.
            [HttpPost]
            public ActionResult Login(Patient model)
            {
                //Checking the state of model passed as parameter.
                if (ModelState.IsValid)
                {

                    //Validating the user, whether the user is valid or not.
                    var isValidUser = isValidUser(model);

                    //If user is valid & present in database, we are redirecting it to Welcome page.
                    if (isValidUser != null)
                    {
                        FormsAuthentication.SetAuthCookie(model.Email, false);
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        //If the username and password combination is not present in DB then error message is shown.
                        ModelState.AddModelError("Failure", "Wrong Username and password combination !");
                        return View();
                    }
                }
                else
                {
                    //If model state is not valid, the model with error message is returned to the View.
                    return View(model);
                }
            }

            //function to check if User is valid or not
            public Patient isValidUser(Patient model)
            {
                using (var connection = _dapperContext.CreateConnection())
                {
                
                //Retireving the user details from DB based on username and password enetered by user.
                Patient user = (Patient)connection.Query("SELECT * FROM Patients WHERE Email = 'lukasbangstoltz@gmail.com'");
                
                //If user is present, then true is returned.
                if (user == null)
                        return null;
                    //If user is not present false is returned.
                    else
                        return user;
                }
            }


            public ActionResult Logout()
            {
                FormsAuthentication.SignOut();
                Session.Abandon(); // it will clear the session at the end of request
                return RedirectToAction("Index");
            }
    }

}
