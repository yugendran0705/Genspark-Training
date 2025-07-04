using Microsoft.EntityFrameworkCore;
using VehicleServiceAPI.Context;
using VehicleServiceAPI.Interfaces;
using VehicleServiceAPI.Models;

namespace VehicleServiceAPI.Repositories
{
    public class InvoiceRepository : IRepository<Invoice>
    {
        private readonly VehicleServiceDbContext _context;

        public InvoiceRepository(VehicleServiceDbContext context)
        {
            _context = context;
        }

        // Retrieve an invoice by ID, including the associated booking
        public async Task<Invoice> GetByIdAsync(int id)
        {
            var invoice = await _context.Invoices
                .Include(i => i.Booking)
                .FirstOrDefaultAsync(i => i.Id == id) ?? throw new InvalidOperationException($"Invoice not found.");
            return invoice;
        }

        // Retrieve all invoices, ensuring soft-deleted entries are excluded
        public async Task<IEnumerable<Invoice>> GetAllAsync()
        {
            return await _context.Invoices
                .Include(i => i.Booking)
                .ToListAsync();
        }

        // Add a new invoice record
        public async Task<Invoice> AddAsync(Invoice invoice)
        {
            _context.Invoices.Add(invoice);
            await _context.SaveChangesAsync();
            return invoice;
        }

        // Update an existing invoice record
        public async Task<Invoice> UpdateAsync(Invoice invoice)
        {
            var existingInvoice = await _context.Invoices.FindAsync(invoice.Id) ?? throw new InvalidOperationException($"Invoice not found.");
            existingInvoice.Amount = invoice.Amount;
            existingInvoice.ServiceDetails = invoice.ServiceDetails;
            existingInvoice.BookingId = invoice.BookingId;

            _context.Invoices.Update(existingInvoice);
            await _context.SaveChangesAsync();
            return existingInvoice;
        }

        // Soft delete an invoice record
        public async Task<bool> DeleteAsync(int id)
        {
            var invoice = await _context.Invoices.FindAsync(id);
            if (invoice == null)
            {
                return false;
            }

            invoice.IsDeleted = true;
            _context.Invoices.Update(invoice);
            await _context.SaveChangesAsync();
            return true;
        }

        // Retrieve all invoices related to a specific booking
        public async Task<Invoice> GetInvoicesByBookingIdAsync(int bookingId)
        {
            return await _context.Invoices
                .Include(i => i.Booking)
                .Where(i => i.BookingId == bookingId && !i.IsDeleted)
                .FirstOrDefaultAsync(i => i.BookingId == bookingId) ?? throw new InvalidOperationException($"Invoice not found.");
        }
    }
}
