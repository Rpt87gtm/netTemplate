using api.Data;
using api.Dtos.Stock;
using api.Helpers;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class StockRepository : IStockRepository
    {
        private ApplicationDBContext _context;

        public StockRepository(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task<List<Stock>> GetAllAsync(QueryObject query)
        {
            var stock = _context.Stock.Include(c => c.Comments).AsQueryable();

            if (!String.IsNullOrWhiteSpace(query.Symbol)) {
                stock = stock.Where(s => s.Symbol.Contains(query.Symbol));
            }
            if (!String.IsNullOrWhiteSpace(query.CompanyName))
            {
                stock = stock.Where(s => s.Symbol.Contains(query.CompanyName));
            }
            return await stock.ToListAsync();
        }

        public async Task<Stock?> GetByIdAsync(int id)
        {
            return await _context.Stock.Include(c => c.Comments).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Stock> CreateAsync(Stock stockModel)
        {
            await _context.Stock.AddAsync(stockModel);
            await _context.SaveChangesAsync();
            return stockModel;
        }

        public async Task<Stock?> DeleteAsync(int id)
        {
            var stockModel = await _context.Stock.FirstOrDefaultAsync(x => x.Id == id);
            if (stockModel == null) {
                return null;
            }

            _context.Stock.Remove(stockModel);
            await _context.SaveChangesAsync();
            return stockModel;
        }


        public async Task<Stock?> UpdateAsync(int id, UpdateStockRequestDto updateDto)
        {
            var existingStock = await _context.Stock.FirstOrDefaultAsync(x => x.Id == id);
            if (existingStock == null) {
                return null;
            }

            existingStock.Symbol = updateDto.Symbol;
            existingStock.MarketCup = updateDto.MarketCup;
            existingStock.CompanyName = updateDto.CompanyName;
            existingStock.LastDiv = updateDto.LastDiv;
            existingStock.Purchase = updateDto.Purchase;
            existingStock.Industry = updateDto.Industry;
            
            await _context.SaveChangesAsync();  
            return existingStock;
        }

        public Task<bool> StockExists(int id)
        {
            return _context.Stock.AnyAsync(x => x.Id == id);
        }
    }
}
