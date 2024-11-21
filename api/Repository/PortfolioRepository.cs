using api.Data;
using api.Interfaces;
using api.Models;
using api.Models.User;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class PortfolioRepository : IPortfolioRepository
    {
        private readonly ApplicationDBContext _dbContext;

        public PortfolioRepository(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Stock>> GetUserPortfolio(AppUser appUser)
        {
            return await _dbContext.Portfolio.Where(u => u.AppUserId == appUser.Id)
                .Select(stock => new Stock
                {
                    Id = stock.StockId,
                    Symbol = stock.Stock.Symbol,
                    CompanyName = stock.Stock.CompanyName,
                    Purchase = stock.Stock.Purchase,
                    LastDiv = stock.Stock.LastDiv,
                    Industry = stock.Stock.Industry,
                    MarketCup = stock.Stock.MarketCup,
                }).ToListAsync();
        }
    }
}
