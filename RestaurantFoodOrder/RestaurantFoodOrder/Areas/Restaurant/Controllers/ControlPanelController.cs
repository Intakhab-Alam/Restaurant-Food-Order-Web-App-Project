using RestaurantFoodOrder.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RestaurantFoodOrder.Areas.Restaurant.Controllers
{
    public class ControlPanelController : Controller
    {
        int RestaurentID;

        // GET: Restaurant/ControlPanel
        public ActionResult ControlPanelhome()
        {
            // Check Restaurent Owner has logged in or not
            if (checkloggedin() == true)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login", "RestaurantHome");
            }
        }
        // Method to Check Restaurent Logged in or not
        private bool checkloggedin()
        {
            if(Session["RestaurentID"] != null)
            {
                RestaurentID = Convert.ToInt32(Session["RestaurentID"]);
                return true;
            }
            else
            {
                return false;
            }
        }
        public ActionResult AddFoodCategory()
        {
            // Check Restaurant admin Logged in 
            if (checkloggedin() == true)
            {
                try
                {
                    // Create instance of Data Context
                    using (var db = new RestaurantFoodDBEntities())
                    {
                        // Get Previous Filled Categories
                        var checkpreviousfilleditem = db.FoodCategory.Where(x => x.RestaurentID == RestaurentID).ToList();
                        if (checkpreviousfilleditem != null)
                        {

                            ViewBag.FoodMainID = db.MainFoodType.ToList();
                            ViewBag.PreviousFoodCategories = checkpreviousfilleditem;
                            return View();
                        }
                        else
                        {
                            // Get Main Types of Food 
                            ViewBag.RestaurentCategory = db.MainFoodType.ToList();
                            return View();
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
                return RedirectToAction("Login", "RestaurantHome");
            }
        }
        public ActionResult AddFoodItem()
        {
            if (checkloggedin() == true)
            {
                try
                {
                    using (var db = new RestaurantFoodDBEntities())
                    {
                        // Get Previous Filled Categories
                        var checkpreviousfilleditem = db.FoodCategory.Where(x => x.RestaurentID == RestaurentID).ToList();
                        if (checkpreviousfilleditem != null)
                        {
                            ViewBag.FoodMainID = db.MainFoodType.ToList();
                            // Get Previous Filled Food Items
                            var previousfooditems = db.MenuItems.Where(x => x.restaurentid == RestaurentID).ToList();
                            ViewBag.FoodItemsList = previousfooditems;
                            //ViewBag.PreviousFoodCategories = checkpreviousfilleditem;
                            return View();
                        }
                        else
                        {
                            ViewBag.FoodMainID = db.MainFoodType.ToList();
                            return View();
                        }
                    }
                }
                catch (Exception ee)
                {
                    return Content("<script>alert('Something went Wrong');location.href='/'</script>");

                }

            }
            else
            {
                return RedirectToAction("Login", "RestaurantHome");
            }            
        }
        [HttpPost]
        public ActionResult AddFoodItem(MenuItems obj, HttpPostedFileBase itempic)
        {
            if (ModelState.IsValid)
            {
                string guidname = Guid.NewGuid().ToString();
                string filename = string.Empty;
                string filepath = string.Empty;

                filename = guidname + itempic.FileName;
                string ext = Path.GetExtension(filename);
                if (ext == ".jpg" || ext == ".png")
                {
                    using (var db = new RestaurantFoodDBEntities())
                    {
                        filepath = Server.MapPath("~//Files//");
                        itempic.SaveAs(filepath + filename);
                        obj.itempic = filename;
                        int RestaurentID = Convert.ToInt32(Session["RestaurentID"].ToString());
                        obj.restaurentid = RestaurentID;
                        db.MenuItems.Add(obj);
                        int result = db.SaveChanges();
                        if (result > 0)
                        {
                            return Content("<script>alert('Menu Item added Successfully');location.href='/Restaurant/ControlPanel/AddFoodItem'</script>");
                        }
                    }
                }
                else
                {
                    return Content("<script>alert('You may upload only jpg and png files only');location.href='/Restaurant/ControlPanel/AddFoodItem'</script>");
                }
            }
            else
            {
                return View();
            }
            return View();
        }
        public ActionResult ViewOrders()
        {
            if (checkloggedin() == true)
            {
                try
                {
                    using (var db = new RestaurantFoodDBEntities())
                    {
                        // Get Previous Filled Categories
                        int RestaurentID = Convert.ToInt32(Session["RestaurentID"]);
                        var custdata = (from m in db.MenuItems 
                                        join o in db.Orders on m.menuitemid equals o.MenuItemID 
                                        join RR in db.RestaurentRegistration on o.RestaurentID equals RR.RestaurentId 
                                        join u in db.User on o.OrderByUser equals u.userid 
                                        where RR.RestaurentId == RestaurentID 
                                        select new CustomerVM 
                                        { 
                                            MenuItem = m, 
                                            Order = o, 
                                            RestaurentRegistration = RR, 
                                            User = u 
                                        }
                                        ).ToList();

                        return View(custdata);
                    }
                }
                catch (Exception ee)
                {
                    return Content("<script>alert('Something went Wrong');location.href='/'</script>");

                }

            }
            else
            {
                return RedirectToAction("Login", "RestaurentHome");
            }
        }
    }
}