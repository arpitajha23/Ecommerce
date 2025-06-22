using DataAccessLayer.Common;
using DataAccessLayer.Models;
using InfrastructureLayer.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PresentationLayer.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DataAccessLayer.DTO_s.CategoryDto;

namespace InfrastructureLayer.Services
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly IConfiguration _configuration;
        private readonly EcommerceDbContext _context;

        public DashboardRepository(IConfiguration configuration, EcommerceDbContext context)
        {
            _configuration = configuration;
            _context = context;
        }
        public async Task<List<Category>> GetAllCategoriesAsync()
        {
            return await _context.Categories.ToListAsync();
        }

    }
}
