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

        public async Task<Portfolio> CreateAsync(Portfolio portfolio)
        {
            await _dbContext.Portfolio.AddAsync(portfolio);
            await _dbContext.SaveChangesAsync();
            return portfolio;
        }

        public async Task<Portfolio> DeletePortfolioAsync(AppUser appUser, string symbol)
        {
            var portfolioModel = await _dbContext.Portfolio.FirstOrDefaultAsync(s => s.AppUserId == appUser.Id && s.Stock.Symbol.ToLower() == symbol.ToLower());
            if (portfolioModel != null) {
                _dbContext.Portfolio.Remove(portfolioModel);
                await _dbContext.SaveChangesAsync();
                return portfolioModel;
            }
            return null;
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
