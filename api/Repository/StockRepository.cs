using api.Data;
using api.Dtos.Stock;
using api.Helpers;
using api.Helpers.Pagination;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class StockRepository : IStockRepository
    {
        private ApplicationDBContext _context;
        private Paginator _paginator;

        public StockRepository(ApplicationDBContext context)
        {
            _context = context;
            _paginator = new Paginator();
        }
        public async Task<List<Stock>> GetAllAsync(QueryObject query, QueryPage queryPage)
        {
            var stock = _context.Stock.Include(c => c.Comments).AsQueryable();

            stock = UseQueryParameters(stock, query);
            stock = _paginator.Paginate(stock, queryPage);
            
            return await stock.ToListAsync();
        }

        private IQueryable<Stock> UseQueryParameters(IQueryable<Stock> stock, QueryObject query) {

            var updatedStock = stock;

            updatedStock = updatedStock
                .Where(s => (query.Symbol == null || query.Symbol == "" || s.Symbol.ToLower().Contains(query.Symbol.ToLower())))
                .Where(s => (query.CompanyName == null || query.CompanyName == "" || s.CompanyName.ToLower().Contains(query.CompanyName.ToLower())));
            
            if (!String.IsNullOrWhiteSpace(query.SortBy))
            {
                if (query.SortBy.Equals("Symbol", StringComparison.OrdinalIgnoreCase))
                {
                    updatedStock = query.IdDecsending ? updatedStock.OrderByDescending(s => s.Symbol) : updatedStock.OrderBy(s => s.Symbol);
                }
            }
            return updatedStock;
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
