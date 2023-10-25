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
    public class BillsController : ControllerBase
    {
        private readonly DataContext _context;

        public BillsController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Bills
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BillDTO>>> GetBills()
        {
            var bills = await _context.Bills.Select(b =>
                    new BillDTO()
                    {
                       BillId = b.BillId,
                       ApartmentId = b.ApartmentId,
                       Month = b.Month,
                       WaterConsumption = b.WaterConsumption,
                       Year = b.Year,
                       BillValue = b.BillValue
                    }).ToListAsync();

            return Ok(bills);
        }

        // GET: api/Bills/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BillDTO>> GetBill(int id)
        {
            var bill = await _context.Bills.FindAsync(id);

            if (bill == null)
            {
                return NotFound();
            }

            return Ok(new BillDTO()
            {
                BillId = bill.BillId,
                ApartmentId = bill.ApartmentId,
                Month = bill.Month,
                WaterConsumption = bill.WaterConsumption,
                Year = bill.Year,
                BillValue = bill.BillValue
            });
        }

        // PUT: api/Bills/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBill(int id, BillDTOForUpdate billDTO)
        {
            var bill = await _context.Bills.FindAsync(id);

            if (bill == null)
            {
                return NotFound("Bill not found");
            }

            bill.WaterConsumption = billDTO.WaterConsumption;
            bill.BillValue = billDTO.BillValue;

            _context.Entry(bill).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BillExists(id))
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

        // POST: api/Bills
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<BillDTO>> PostBill(BillDTO billDTO)
        {
            var apartment = _context.Apartments.FirstOrDefault(a => a.Id == billDTO.ApartmentId);
            if (apartment == null)
            {
                return BadRequest();
            }
            else
            {
                if(BillWithSameApartmentMonthAndYear(billDTO.ApartmentId, billDTO.Year, billDTO.Month))
				{
                    return BadRequest("A bill from the same apartment and with the same year and month already exists.");
                }

                var bill = new Bill()
                {
                    Apartment = apartment,
                    ApartmentId = apartment.Id,
                    BillId = billDTO.BillId,
                    Month = billDTO.Month,
                    WaterConsumption = billDTO.WaterConsumption,
                    Year = billDTO.Year,
                    BillValue = billDTO.BillValue
                };

                _context.Bills.Add(bill);
                await _context.SaveChangesAsync();

                // ar trebui verificat daca exista deja o factura care sa fie in aceeasi luna si acelasi an

                return CreatedAtAction("GetBill", new { id = bill.BillId }, bill.BillId);
            }
        }

        // DELETE: api/Bills/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBill(int id)
        {
            var bill = await _context.Bills.FindAsync(id);
            if (bill == null)
            {
                return NotFound();
            }

            _context.Bills.Remove(bill);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BillExists(int id)
        {
            return _context.Bills.Any(e => e.BillId == id);
        }

        private bool BillWithSameApartmentMonthAndYear(int apId, int year, int month)
        {
            return _context.Bills.Any(b => b.ApartmentId == apId && b.Year == year && b.Month == month);
        }
    }
}
