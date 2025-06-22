using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO_s
{
    public class CategoryDto
    {
        public class CategoryWithSubcategoriesDto
        {
            public int CategoryId { get; set; }
            public string CategoryName { get; set; }
            public List<SubcategoryDto> Subcategories { get; set; } = new();
        }

        public class SubcategoryDto
        {
            public int CategoryId { get; set; }
            public string CategoryName { get; set; }
        }

    }
}
