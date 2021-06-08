using RestaurantFoodOrder.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace RestaurantFoodOrder.Areas.Restaurant.Controllers
{
    public class RestaurantHomeController : Controller
    {
        Guid guid;

        // GET: Restaurant/RestaurantHome
        public ActionResult SignUp()
        {
            using (var db = new RestaurantFoodDBEntities())
            {
                // Get Categories of Restaurent Type
                ViewBag.RestaurentCategory = db.MainFoodType.ToList();
                return View();
            }

        }
        [HttpPost]
        public ActionResult SignUp(RestaurentRegistration obj)
        {
            if (ModelState.IsValid)
            {
                using (var db = new RestaurantFoodDBEntities())
                {
                    try
                    {
                        #region Code For Registration
                        // First Check that Email ID is already registered and Verified
                        var checkEmail = db.RestaurentRegistration.Any(x => x.RestaurentEmail == obj.RestaurentEmail & x.emailvarified == true);
                        if (checkEmail == true)
                        {
                            return Content("<script>alert('This Email ID is aleray Registered and Verified.');location.href='/Restaurant/RestaurantHome/SignUp';</script>");
                        }
                        else
                        {
                            // Create new Unique identifier
                            guid = Guid.NewGuid();
                            obj.guid = guid;
                            db.RestaurentRegistration.Add(obj);
                            var result = db.SaveChanges();
                            #endregion
                            #region Sending Email on Registered Email ID for Account Verification
                            // Generate Unique ID for Email Verification                       
                            /*string msg = "Account Verification ";
                            MailMessage mail = new MailMessage();
                            mail.To.Add(obj.RestaurentEmail.Trim().Replace("'", "''"));
                            mail.From = new MailAddress("aftabalam19786@gmail.com");
                            mail.Subject = "Online Restaurant: Account Varification Activation";
                            string Body = "Please click below on given link for your Account Varification <br/> ";
                            //Body += "http://neelabhkrishna-001-site1.gtempurl.com/Restaurent/RestaurentHome/AccountVerification?uniqueid=" + guid;
                            Body += "http://localhost:44367/Restaurant/RestaurantHome/AccountVerification?uniqueid=" + guid;


                            Body += "<br /> Regards:" + "<br />" + "Online Restaurent";
                            Body += "<br /> Contact for assistance: 0141-000000";
                            mail.Body = Body;
                            mail.IsBodyHtml = true;
                            // Adding the Creddentials
                            System.Net.NetworkCredential networkcredential = new System.Net.NetworkCredential("aftabalam19786@gmail.com", "9759648146");
                            SmtpClient smtp = new SmtpClient();
                            smtp.EnableSsl = false;
                            smtp.UseDefaultCredentials = false;
                            smtp.Credentials = networkcredential;
                            smtp.Port = 587; //25 or use 587 or 465
                            smtp.Host = "smtp.gmail.com";
                            smtp.Send(mail);*/
                            #endregion

                            #region Alternate Sending Email on Registered Email ID for Account Verification
                            try
                            {
                                // Check Unique Identifier value if ok then update emailvarified
                                var checkAccount = db.RestaurentRegistration.FirstOrDefault(x => x.guid == guid);
                                if (checkAccount != null)
                                {
                                    // Update EmailVerified field in Restaurent Registration
                                    checkAccount.emailvarified = true;
                                    checkAccount.Regdate = DateTime.Now.ToShortDateString();
                                    ModelState.Clear();
                                    TryUpdateModel(checkAccount);
                                    var data2 = db.SaveChanges();
                                    if (data2 > 0)
                                    {
                                        return Content("<script>alert('Your Account is Successfully Verified and Now You may Login');location.href='/Restaurant/RestaurantHome/Login';</script>");

                                    }
                                    return View();
                                }
                                else
                                {
                                    return Content("<script>alert('You have changed something in Activation URL');location.href='/'</script>");
                                }
                            }
                            catch (Exception e)
                            {
                                return Content("<script>alert('Something went Wrong');location.href='/'</script>");
                            }
                            #endregion

                            /*if (result > 0)
                            {
                                return Content("<script>alert('Your Restaurant has been Successfully Registered. An Account activation link has been sent to your email id , please verify your account');location.href='/'</script>");
                            }
                            return View();*/
                        }
                    }
                    catch (Exception e)
                    {
                        return Content("<script>alert('Something went Wrong');location.href='/'</script>");
                    }
                }
            }
            else
            {
                return View();
            }
        }
        public ActionResult AccountVerification(string uniqueid)
        {
            if(uniqueid != null)
            {
                using (var db = new RestaurantFoodDBEntities())
                {
                    try
                    {
                        // Check Unique Identifier value if ok then update emailvarified
                        var checkAccount = db.RestaurentRegistration.SingleOrDefault(x => x.guid.ToString() == uniqueid);
                        if(checkAccount != null)
                        {
                            // Update EmailVerified field in Restaurent Registration
                            checkAccount.emailvarified = true;
                            checkAccount.Regdate = DateTime.Now.ToShortDateString();
                            ModelState.Clear();
                            TryUpdateModel(checkAccount);
                            var data2 = db.SaveChanges();
                            if (data2 > 0)
                            {
                                return Content("<script>alert('Your Account is Successfully Verified and Now You may Login');location.href='/Restaurant/RestaurantHome/Login';</script>");

                            }
                            return View();
                        }
                        else
                        {
                            return Content("<script>alert('You have changed something in Activation URL');location.href='/'</script>");
                        }
                    }
                    catch (Exception e)
                    {
                        return Content("<script>alert('Something went Wrong');location.href='/'</script>");
                    }
                }
            }
            else
            {
                return RedirectToAction("SignUp");
            }
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(string RestaurentEmail, string password)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using(var db = new RestaurantFoodDBEntities())
                    {
                        // Check Email and Password
                        var checkLogin = db.RestaurentRegistration.SingleOrDefault(x => x.RestaurentEmail == RestaurentEmail & x.password == password);
                        if(checkLogin != null)
                        {
                            // Check Email Verification is done or not
                            var checkVerification = db.RestaurentRegistration.Any(x => x.RestaurentEmail == RestaurentEmail & x.password == password & x.emailvarified == true);
                            if (checkVerification == true)
                            {
                                Session.Add("RestaurentID", checkLogin.RestaurentId);
                                Session.Add("RestaurentName", checkLogin.RestaurentName);
                                return RedirectToAction("ControlPanelhome", "ControlPanel");
                            }
                            else
                            {
                                return Content("<script>alert('Your Email Verification has not done yet, So please check your email and verify account');location.href='/'</script>");

                            }
                        }
                        else
                        {
                            return Content("<script>alert('Invalid Username or Password');location.href='/Restaurant/RestaurantHome/Login';</script>");
                        }
                    }
                }
                catch(Exception e)
                {
                    return Content("<script>alert('Something went Wrong');location.href='/'</script>");
                }
            }
            else
            {
                return View();
            }
        }
    }
}