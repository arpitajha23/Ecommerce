using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationLayer.Interfaces;
using DataAccessLayer.Common;
using InfrastructureLayer.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using static DataAccessLayer.DTO_s.CategoryDto;

namespace ApplicationLayer.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IDashboardRepository _dashboardRepository;

        public DashboardService(IDashboardRepository dashboardRepository)
        {
            _dashboardRepository = dashboardRepository;
        }

        public async Task<List<CategoryWithSubcategoriesDto>> GetCategoryTreeAsync()
        {
            var allCategories = await _dashboardRepository.GetAllCategoriesAsync();

            var parentCategories = allCategories
                .Where(c => c.ParentCategoryId == null)
                .ToList();

            var result = parentCategories.Select(parent => new CategoryWithSubcategoriesDto
            {
                CategoryId = parent.CategoryId,
                CategoryName = parent.CategoryName,
                Subcategories = allCategories
                    .Where(sub => sub.ParentCategoryId == parent.CategoryId)
                    .Select(sub => new SubcategoryDto
                    {
                        CategoryId = sub.CategoryId,
                        CategoryName = sub.CategoryName
                    }).ToList()
            }).ToList();

            return result;
        }

    }
}
