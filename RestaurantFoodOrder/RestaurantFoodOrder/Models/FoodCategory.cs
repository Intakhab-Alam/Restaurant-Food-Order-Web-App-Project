//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RestaurantFoodOrder.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class FoodCategory
    {
        public int FoodCategoryid { get; set; }
        public string CategoryName { get; set; }
        public int FoodMainID { get; set; }
        public int RestaurentID { get; set; }
    
        public virtual MainFoodType MainFoodType { get; set; }
        public virtual RestaurentRegistration RestaurentRegistration { get; set; }
    }
}
