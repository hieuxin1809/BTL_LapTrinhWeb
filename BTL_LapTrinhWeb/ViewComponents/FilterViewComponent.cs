// Components/SortComponent.cs
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace BTL_LapTrinhWeb.Components
{
    public class FilterViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(string currentSort)
        {
            var sortItems = new List<SortItem>
            {
                new SortItem { Value = "", Text = "Nothing" },
                new SortItem { Value = "price_asc", Text = "Giá từ Thấp đến Cao" },
                new SortItem { Value = "price_desc", Text = "Giá từ Cao đến Thấp" }
            };

            ViewBag.CurrentSort = currentSort;

            return View(sortItems);
        }

        public class SortItem
        {
            public string Value { get; set; }
            public string Text { get; set; }
        }
    }
}
