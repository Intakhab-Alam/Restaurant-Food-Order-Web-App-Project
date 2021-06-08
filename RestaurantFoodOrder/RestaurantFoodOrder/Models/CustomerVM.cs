using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RestaurantFoodOrder.Models
{
    public class CustomerVM
    {
        public User User { get; set; }
        public MenuItems MenuItem { get; set; }
        public MainFoodType MainFoodType { get; set; }
        public RestaurentRegistration RestaurentRegistration { get; set; }
        public Orders Order { get; set; }
        public FoodCategory FoodCategory { get; set; }
    }
}