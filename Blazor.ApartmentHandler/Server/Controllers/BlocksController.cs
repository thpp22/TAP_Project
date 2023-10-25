#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Blazor.ApartmentHandler.Shared.Entities;
using TAP.ApartmentHandler.Api.Data;

namespace Blazor.ApartmentHandler.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlocksController : ControllerBase
    {
        private readonly DataContext _context;

        public BlocksController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Blocks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BlockDTO>>> GetBlocks()
        {
            var blocks =  await _context.Blocks.Include(b => b.ApartmentList).Select(b =>
                    new BlockDTO()
                    {
                        Id = b.Id,
                        ApartmentIds = b.ApartmentList.Select(a => a.Id).ToList()
                    }).ToListAsync();

            return Ok(blocks);
        }

        // GET: api/Blocks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BlockDTO>> GetBlock(int id) // returnezi un obiect BlockDTO in loc de Block
        {
            var block = await _context.Blocks.FindAsync(id);

            if (block == null)
            {
                return NotFound();
            }

            var apartments = _context.Apartments // cauti apartamentele ce au BlockId = id
                .Where(a => a.BlockId == id)
                .Select(a => a.Id)
                .ToList();

            return Ok(new BlockDTO()
            {
                Id = block.Id,
                ApartmentIds = apartments
            });
        }

        // PUT: api/Blocks/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBlock(int id, BlockDTONoApartments blockDTO)
        {
            var corresp_block = await _context.Blocks.FindAsync(id);
            if (corresp_block == null)
            {
                return NotFound("Block not found");
            }

            corresp_block.Id = blockDTO.Id;

            _context.Entry(corresp_block).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BlockExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Blocks
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<BlockDTO>> PostBlock(BlockDTO blockDTO)
        {
            var block = new Block()
            {
                Id=blockDTO.Id,
                ApartmentList=new List<Apartment>()                
            };
            _context.Blocks.Add(block);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBlock", new { id = block.Id }, block.Id);
        }

        // DELETE: api/Blocks/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBlock(int id)
        {
            var block = await _context.Blocks.FindAsync(id);
            if (block == null)
            {
                return NotFound();
            }

            _context.Blocks.Remove(block);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BlockExists(int id)
        {
            return _context.Blocks.Any(e => e.Id == id);
        }
    }
}
