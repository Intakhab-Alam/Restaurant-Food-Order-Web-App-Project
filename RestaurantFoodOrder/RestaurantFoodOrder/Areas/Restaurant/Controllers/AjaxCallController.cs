using RestaurantFoodOrder.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RestaurantFoodOrder.Areas.Restaurant.Controllers
{
    public class AjaxCallController : Controller
    {
        // GET: Restaurant/AjaxCall
        // Ajax Request for Add Food Categories
        public JsonResult InsertFoodCategory(string CategoryName, string FoodMainID)
        {
            try
            {
                // Instance of Data Context
                using (var db = new RestaurantFoodDBEntities())
                {
                    // Check User is Logged in or not
                    if (Session["RestaurentID"] != null)
                    {
                        // Check Same CategoryName and Food Main id does not exists
                        int foodmainid = Convert.ToInt32(FoodMainID);
                        int RestaurentID = Convert.ToInt32(Session["RestaurentID"].ToString());

                        // Check Above Categories is not added by Same Restaurant
                        var checkdata = db.FoodCategory.Where(x => x.CategoryName == CategoryName & x.FoodMainID == foodmainid & x.RestaurentID == RestaurentID).Take(1).Any();
                        // if not added
                        if (checkdata != true)
                        {
                            // Create Instance of Food Category Model
                            FoodCategory obj = new FoodCategory
                            {
                                CategoryName = CategoryName,
                                FoodMainID = foodmainid,
                                RestaurentID = RestaurentID

                            };
                            // Add Reference of Food Category
                            db.FoodCategory.Add(obj);
                            var result = db.SaveChanges();
                            // if Successfully Added
                            if (result > 0)
                            {
                                return Json("added", JsonRequestBehavior.AllowGet);
                            }
                            else
                            {
                                return Json("notadded", JsonRequestBehavior.AllowGet);
                            }
                        }
                        else
                        {
                            return Json("sameitem", JsonRequestBehavior.AllowGet);

                        }

                    }
                    else
                    {
                        return Json("sessionissue", JsonRequestBehavior.AllowGet);

                    }
                }
            }
            catch (Exception ee)
            {
                return Json("exception", JsonRequestBehavior.AllowGet);

            }
        }
        // Get Last Added Food Categories as per Food Main ID
        [HttpPost]
        public JsonResult GetFoodCategory(string FoodMainID)
        {
            try
            {
                // Check User is Logged in or not
                if (Session["RestaurentID"] != null)
                {
                    using (var db = new RestaurantFoodDBEntities())
                    {
                        // Check Same CategoryName and Food Main id does not exists
                        int foodmainid = Convert.ToInt32(FoodMainID);
                        int RestaurentID = Convert.ToInt32(Session["RestaurentID"].ToString());
                        // Fetch Food Categories

                        var ddFoodCategry = (from obj in db.FoodCategory
                                             where obj.RestaurentID == RestaurentID & obj.FoodMainID == foodmainid
                                             select new DDFoodCategoryVM()
                                             {
                                                 FoodCategoryid = obj.FoodCategoryid,
                                                 CategoryName = obj.CategoryName
                                             }
                                             ).ToList();

                        if (ddFoodCategry != null)
                        {
                            return Json( new { success=true, FoodCategory = ddFoodCategry }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            //return Json("NoItem", JsonRequestBehavior.AllowGet);
                            return Json(new {  success=false, message="Food Category is empty."}, JsonRequestBehavior.AllowGet);
                        }

                    }
                }
                else
                {
                    //return Json("sessionissue", JsonRequestBehavior.AllowGet);
                    return Json("", JsonRequestBehavior.AllowGet);

                }
            }
            catch (Exception ee)
            {
                return Json("", JsonRequestBehavior.AllowGet);

            }
        }
        [HttpPost]
        public JsonResult DeleteMenuItem(string MenuItemId)
        {
            try
            {
                // Check User is Logged in or not
                if (Session["RestaurentID"] != null)
                {
                    using (var db = new RestaurantFoodDBEntities())
                    {
                        // Check Same CategoryName and Food Main id does not exists
                        int menuItemId = Convert.ToInt32(MenuItemId);
                        int RestaurentID = Convert.ToInt32(Session["RestaurentID"].ToString());
                        // Fetch Menu Item
                        var menuItem = db.MenuItems.SingleOrDefault(model => model.menuitemid == menuItemId);

                        if (menuItem != null)
                        {
                            db.MenuItems.Remove(menuItem);
                            db.SaveChanges();
                            return Json(new { success = true, message = "Successfully Deleted Menu Item" }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            //return Json("NoItem", JsonRequestBehavior.AllowGet);
                            return Json(new { success = false, message = "Menu Item is empty." }, JsonRequestBehavior.AllowGet);
                        }

                    }
                }
                else
                {
                    //return Json("sessionissue", JsonRequestBehavior.AllowGet);
                    return Json("", JsonRequestBehavior.AllowGet);

                }
            }
            catch (Exception ee)
            {
                return Json("", JsonRequestBehavior.AllowGet);

            }
        }       

    }
}