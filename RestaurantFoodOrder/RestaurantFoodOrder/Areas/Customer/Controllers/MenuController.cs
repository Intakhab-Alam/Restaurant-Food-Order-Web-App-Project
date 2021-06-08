using RestaurantFoodOrder.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RestaurantFoodOrder.Areas.Customer.Controllers
{
    public class MenuController : Controller
    {
        // GET: Customer/Menu
        public ActionResult ViewMenu(string itemtype)
        {
            using (var db = new RestaurantFoodDBEntities())
            {
                // If Customer is comming from Home Page Without Login
                if (itemtype != null)
                {
                    // Get Item Type whether is VEG, Non Veg
                    int itemtype1 = Convert.ToInt32(itemtype);
                    // Get Food Items along with their description
                    var getmenuitems = (from r1 in db.RestaurentRegistration 
                                        join r2 in db.MenuItems on r1.RestaurentId equals r2.restaurentid 
                                        where r2.MainFoodCategoryID == itemtype1 
                                        select new CustomerVM 
                                        { 
                                            RestaurentRegistration = r1,
                                            MenuItem = r2 
                                        }
                                        ).ToList();

                    if (getmenuitems != null)
                    {

                        Session.Add("loginissue_in_order", Request.QueryString["itemtype"].ToString());
                        ViewBag.SearchItems = getmenuitems;
                        return View();
                    }
                }

                // IF Customer is Comming from Login Page
                if (Session["CustomerPreference"] != null & Session["CustomerID"] != null)
                {
                    // Show Food Items as per Customer Preferences

                    return View();
                }
                else
                {
                    return View();
                }
            }
        }
        public ActionResult AddtoCart(string itemid, string restid, string price)
        {
            // If Item id is not selected
            if (itemid != null)
            {
                // Check User is Logged in or not
                if (Session["CustomerID"] != null)
                {
                    //if (Session["cart"] == null)
                    //{
                    // Instance of Data Context
                    using (var db = new RestaurantFoodDBEntities())
                    {
                        // Create Cart Instance
                        List<Orders> li = new List<Orders>();
                        // Update Order Instance
                        Orders mo = new Orders
                        {
                            MenuItemID = Convert.ToInt32(itemid),
                            OrderByUser = Convert.ToInt32(Session["CustomerID"].ToString()),
                            orderdate = DateTime.Now.ToShortDateString(),
                            RestaurentID = Convert.ToInt32(restid),
                            price = Convert.ToInt32(price),
                            session = Session.SessionID.ToString()

                        };
                        li.Add(mo);
                        Session["cart"] = li;
                        ViewBag.cart = li.Count();
                        Session["count"] = db.Orders.Where(x => x.session == Session.SessionID.ToString() & x.OrderByUser == mo.OrderByUser).Count();

                        // Add into Orders Table
                        db.Orders.Add(mo);
                        int i = db.SaveChanges();
                        if (i > 0)
                        {
                            return Content("<script>alert('Food Item is added into your cart');location.href='/Customer/Menu/ViewMenu?itemtype=" + Session["loginissue_in_order"].ToString() + "';</script>");
                        }
                    }


                    //else
                    //{
                    //    List<Order> li = (List<Order>)Session["cart"];
                    //    Order mo = new Order
                    //    {
                    //        MenuItemID = Convert.ToInt32(itemid),
                    //        OrderByUser = Convert.ToInt32(Session["CustomerID"].ToString()),
                    //        orderdate = DateTime.Now.ToShortDateString(),
                    //        RestaurentID = Convert.ToInt32(restid),
                    //        price = Convert.ToInt32(price),
                    //        session = Session.SessionID.ToString()



                    //    };
                    //    li.Add(mo);
                    //    Session["cart"] = li;
                    //    ViewBag.cart = li.Count();
                    //    Session["count"] = Convert.ToInt32(Session["count"]) + 1;

                    //}
                }
                else
                {
                    return RedirectToAction("Login", "CustomerHome", new { itemtype = itemid });
                }
            }
            else
            {
                return RedirectToAction("Login", "CustomerHome");
            }
            return RedirectToAction("ViewMenu", "Menu", new { itemtype = Session["loginissue_in_order"].ToString() });
        }
        public ActionResult Myorder()
        {
            // Check Customer Orders by Customer
            if (Session["CustomerID"] != null)
            {
                using (var db = new RestaurantFoodDBEntities())
                {
                    //List<Order> data = (List<Order>)Session["cart"];
                    //var data2 = (from f in data select new { f.MenuItemID });
                    //return View((List<Order>)Session["cart"]);
                    //var data1 = from c in db.MenuItems
                    //           where c.Contains(data2.)
                    //           select new { c., c.PartnerID };
                    int custid = Convert.ToInt32(Session["CustomerID"].ToString());
                    var custdata = (from c in db.MenuItems 
                                    join d in db.Orders on c.menuitemid equals d.MenuItemID 
                                    where d.session == Session.SessionID.ToString() & d.OrderByUser == custid & d.finalorder == false 
                                    select new CustomerVM 
                                    { 
                                        MenuItem = c, 
                                        Order = d 
                                    }
                                    ).ToList();
                    if (custdata != null)
                    {
                        return View(custdata);
                    }
                    else
                    {
                        return Content("<script>alert('No Pending Record');location.href='/';</script>");

                    }
                }
            }
            else
            {
                return Content("<script>alert('First Login then select your orders');location.href='/Customer/CustomerHome/Login';</script>");
            }
        }
        // Ajax Request Handling , User Click on Place Order Button
        public JsonResult PlaceOrder()
        {
            try
            {
                // Check User is Logged in or not
                if (Session["CustomerID"] != null)
                {
                    using (var db = new RestaurantFoodDBEntities())
                    {
                        // Get Order Details from Session
                        // Get User ID
                        int userid = Convert.ToInt32(Session["CustomerID"]);
                        // Update Orders Table field , which is final order
                        var orderstatus1 = (db.Database.ExecuteSqlCommand("update Orders set finalorder=1 where OrderByUser={0} and [session]={1} and finalorder=0", userid, Session.SessionID.ToString()));
                        int orderstatus = db.SaveChanges();
                        // Return Response
                        return Json("done", JsonRequestBehavior.AllowGet);


                    }
                    //}
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
        // Customer Last Orders
        public ActionResult LastOrder()
        {
            // Check User is Logged in or not
            if (Session["CustomerID"] != null)
            {
                // Create Instance of Data Context
                using (var db = new RestaurantFoodDBEntities())
                {
                    // Get Customer ID 
                    int custid = Convert.ToInt32(Session["CustomerID"].ToString());
                    // Fetch previous orders as per customer 
                    var custdata = (from c in db.MenuItems 
                                    join d in db.Orders on c.menuitemid equals d.MenuItemID 
                                    where d.OrderByUser == custid & d.finalorder == true 
                                    select new CustomerVM 
                                    { 
                                        MenuItem = c, 
                                        Order = d 
                                    }
                                    ).ToList();

                    if (custdata != null)
                    {
                        return View(custdata);
                    }
                    else
                    {
                        return Content("<script>alert('No Pending Record');location.href='/';</script>");

                    }
                }
            }
            else
            {
                return Content("<script>alert('First Login then See your Previous orders');location.href='/Customer/CustomerHome/Login';</script>");
            }
        }
    }
}