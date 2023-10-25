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
    public class ApartmentsController : ControllerBase
    {
        private readonly DataContext _context;

        public ApartmentsController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Apartments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ApartmentDTO>>> GetApartments()
        {
            var apartments = await _context.Apartments.Include(a => a.Bills).Select(a =>
                   new ApartmentDTO()
                   {
                       Id = a.Id,
                       BlockId = a.BlockId,
                       Nr = a.Nr,
                       NumberOfPersons = a.NumberOfPersons,
                       BillsIds = a.Bills.Select(b => b.BillId).ToList()
                   }).ToListAsync();

            return Ok(apartments);
        }

        // GET: api/Apartments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApartmentDTO>> GetApartment(int id)
        {
            var apartment = await _context.Apartments.FindAsync(id);

            if (apartment == null)
            {
                return NotFound();
            }

            var bills = _context.Bills // cauti facturile ce au ApartmentId = id
                .Where(b => b.ApartmentId == id)
                .Select(b => b.BillId)
                .ToList();

            return Ok(new ApartmentDTO()
            {
                Id = apartment.Id,
                NumberOfPersons= apartment.NumberOfPersons,
                Nr = apartment.Nr,
                BlockId= apartment.BlockId,
                BillsIds = bills
            });
        }

        // PUT: api/Apartments/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutApartment(int id, ApartmentDTOForUpdate apartmentDTO)
        {
            var corresp_apartment = await _context.Apartments.FindAsync(id);
            if (corresp_apartment == null)
            {
                return NotFound("Apartment not found");
            }

            if (ApartmentWithSameBlockAndNumber(corresp_apartment.BlockId, apartmentDTO.Nr, id))
            {
                return BadRequest("An apartment from the same block and with the same number already exists.");
            }

            corresp_apartment.Nr = apartmentDTO.Nr;
            corresp_apartment.NumberOfPersons = apartmentDTO.NumberOfPersons;

            _context.Entry(corresp_apartment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ApartmentExists(id))
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

        // POST: api/Apartments
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ApartmentDTO>> PostApartment(ApartmentDTO apartmentDTO)
        {

            var block = _context.Blocks.FirstOrDefault(b => b.Id == apartmentDTO.BlockId);
            if (block == null)
            {
                return BadRequest();
            }
            else
            {
                var apartment = new Apartment()
                {
                    Block = block,
                    Bills = new List<Bill>(),
                    BlockId = apartmentDTO.BlockId,
                    Id = apartmentDTO.Id,
                    Nr = apartmentDTO.Nr,
                    NumberOfPersons = apartmentDTO.NumberOfPersons
                };

                if (ApartmentWithSameBlockAndNumber(apartmentDTO.BlockId, apartmentDTO.Nr, -1))
				{
                    return BadRequest("An apartment from the same block and with the same number already exists.");
				}

                _context.Apartments.Add(apartment);
                await _context.SaveChangesAsync();

                // nu pare oricum sa adauge ceva in lista
                _context.Attach(block);
                _context.Entry(block).State = EntityState.Modified;
                block.ApartmentList.Add(apartment);
                await _context.SaveChangesAsync();

                
       
                return CreatedAtAction("GetApartment", new { id = apartment.Id }, apartment.Id);
            }
            
        }

        // DELETE: api/Apartments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteApartment(int id)
        {
            var apartment = await _context.Apartments.FindAsync(id);
            if (apartment == null)
            {
                return NotFound();
            }

            _context.Apartments.Remove(apartment);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ApartmentExists(int id)
        {
            return _context.Apartments.Any(e => e.Id == id);
        }

        private bool ApartmentWithSameBlockAndNumber(int blockId, int nr, int currentId)
		{
            return _context.Apartments.Any(a => a.BlockId == blockId && a.Nr == nr && a.Id != currentId);
		}
    }
}
