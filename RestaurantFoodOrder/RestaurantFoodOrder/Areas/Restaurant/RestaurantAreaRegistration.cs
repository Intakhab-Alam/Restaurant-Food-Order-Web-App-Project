using System.Web.Mvc;

namespace RestaurantFoodOrder.Areas.Restaurant
{
    public class RestaurantAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Restaurant";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Restaurant_default",
                "Restaurant/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}