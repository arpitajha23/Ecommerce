using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DataAccessLayer.DTO_s.CategoryDto;

namespace InfrastructureLayer.Interfaces
{
    public interface IDashboardRepository
    {
        Task<List<Category>> GetAllCategoriesAsync();

    }
}
