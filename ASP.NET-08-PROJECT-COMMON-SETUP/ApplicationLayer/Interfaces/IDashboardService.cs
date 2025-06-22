using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DataAccessLayer.DTO_s.CategoryDto;

namespace ApplicationLayer.Interfaces
{
    public interface IDashboardService
    {
        Task<List<CategoryWithSubcategoriesDto>> GetCategoryTreeAsync();
    }
}
