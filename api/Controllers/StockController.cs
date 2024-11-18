using api.Data;
using api.Dtos.Stock;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Controllers
{
    [Route("api/stock")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly ApplicationDBContext _context;

        public StockController(ApplicationDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(){
            var stocks = await _context.Stock.ToListAsync();
            var stocksDto = stocks.Select(s => s.ToStockDto());

            return Ok(stocksDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id) { 
            var stock = await _context.Stock.FirstOrDefaultAsync(x => x.Id == id);

            if (stock == null) {
                return NotFound();
            }

            return Ok(stock.ToStockDto());
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateStockRequestDto stockDto)
        {
            var stockModel = stockDto.ToStockFromCreateDto();
            await _context.Stock.AddAsync(stockModel);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new {id = stockModel.Id}, stockModel.ToStockDto());   
        }
        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockRequestDto updateDto)
        {
            var stockModel = await _context.Stock.FirstOrDefaultAsync(s => s.Id == id);
            if (stockModel == null) {
                return NotFound();
            }

            stockModel.Symbol = updateDto.Symbol;
            stockModel.MarketCup = updateDto.MarketCup;
            stockModel.CompanyName = updateDto.CompanyName;
            stockModel.LastDiv = updateDto.LastDiv;
            stockModel.Purchase = updateDto.Purchase;
            stockModel.Industry = updateDto.Industry;

            await _context.SaveChangesAsync();
            
            return Ok(stockModel.ToStockDto());
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id) { 

            var stackModel = await _context.Stock.FirstOrDefaultAsync(x => x.Id == id);
            if (stackModel == null) {
                return NotFound();
            }

            _context.Stock.Remove(stackModel);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }

}