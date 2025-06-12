using VehicleServiceAPI.Interfaces;
using VehicleServiceAPI.Models;
using VehicleServiceAPI.Models.DTOs;
using VehicleServiceAPI.Repositories;

namespace VehicleServiceAPI.Services
{
    public class InvoiceService : IInvoiceService
    {
        private readonly InvoiceRepository _invoiceRepository;
        private readonly BookingRepository _bookingRepository;
        private readonly ServiceSlotRepository _serviceSlotRepository;
        private readonly UserRepository _userRepository;
        private readonly VehicleRepository _vehicleRepository;

        public InvoiceService(InvoiceRepository invoiceRepository, BookingRepository bookingRepository, UserRepository userRepository, ServiceSlotRepository serviceSlotRepository, VehicleRepository vehicleRepository)
        {
            _invoiceRepository = invoiceRepository;
            _bookingRepository = bookingRepository;
            _serviceSlotRepository = serviceSlotRepository;
            _userRepository = userRepository;
            _vehicleRepository = vehicleRepository;
        }

        /// <summary>
        /// Retrieves an invoice by its ID and maps it to a DTO.
        /// </summary>
        public async Task<InvoiceDTO> GetInvoiceByIdAsync(int id)
        {
            var invoice = await _invoiceRepository.GetByIdAsync(id);
            return await MapInvoiceToDto(invoice);
        }

        /// <summary>
        /// Retrieves all invoices as a collection of DTOs.
        /// </summary>
        public async Task<IEnumerable<InvoiceDTO>> GetAllInvoicesAsync()
        {
            var invoices = await _invoiceRepository.GetAllAsync();
            var invoiceDtos = await Task.WhenAll(invoices.Select(MapInvoiceToDto));
            return invoiceDtos;
        }

        /// <summary>
        /// Creates a new invoice record.
        /// </summary>
        public async Task<InvoiceDTO> CreateInvoiceAsync(CreateInvoiceDTO request)
        {
            var invoiceEntity = await MapCreateDTOToInvoice(request);
            if (invoiceEntity.Booking.Status != "completed")
            {
                throw new InvalidOperationException("Invoice can only be created for completed bookings.");
            }
            var createdInvoice = await _invoiceRepository.AddAsync(invoiceEntity);
            return await MapInvoiceToDto(createdInvoice);
        }

        /// <summary>
        /// Updates an existing invoice.
        /// </summary>
        public async Task<InvoiceDTO> UpdateInvoiceAsync(UpdateInvoiceDTO request)
        {
            var invoice = await _invoiceRepository.GetByIdAsync(request.Id);
            if (invoice == null)
            {
                throw new InvalidOperationException("Invoice not found.");
            }

            invoice.Amount = request.Amount;
            invoice.ServiceDetails = request.ServiceDetails;
            invoice.BookingId = request.BookingId;

            var updatedInvoice = await _invoiceRepository.UpdateAsync(invoice);
            return await MapInvoiceToDto(updatedInvoice);
        }

        /// <summary>
        /// Soft-deletes an invoice by its ID.
        /// </summary>
        public async Task<bool> DeleteInvoiceAsync(int id)
        {
            return await _invoiceRepository.DeleteAsync(id);
        }

        /// <summary>
        /// Retrieves all invoices associated with a booking.
        /// </summary>
        public async Task<IEnumerable<InvoiceDTO>> GetInvoicesByBookingIdAsync(int bookingId)
        {
            var invoices = await _invoiceRepository.GetInvoicesByBookingIdAsync(bookingId);
            var invoiceDtos = await Task.WhenAll(invoices.Select(MapInvoiceToDto));
            return invoiceDtos;
        }

        #region Mapping Methods

        // Maps an Invoice domain model to an InvoiceDTO.
        private async Task<InvoiceDTO> MapInvoiceToDto(Invoice invoice)
        {
            var user = await _userRepository.GetByIdAsync(invoice.Booking.UserId);
            var slot = await _serviceSlotRepository.GetByIdAsync(invoice.Booking.SlotId);
            var mechanic = await _userRepository.GetByIdAsync(slot.MechanicID);
            var vehicle = await _vehicleRepository.GetByIdAsync(invoice.Booking.VehicleId);

            return new InvoiceDTO
            {
                Id = invoice.Id,
                Amount = invoice.Amount,
                ServiceDetails = invoice.ServiceDetails,
                BookingId = invoice.BookingId,
                ServiceStatus = invoice.Booking.Status,
                UserId = invoice.Booking.UserId,
                Name = user.Name,
                Email = user.Email,
                Phone = user.Phone,
                SlotId = invoice.Booking.SlotId,
                SlotDateTime = slot.SlotDateTime,
                MechanicID = slot.MechanicID,
                MechanicName = mechanic.Name,
                VehicleId = invoice.Booking.VehicleId,
                RegistrationNumber = vehicle.RegistrationNumber,
            };
        }

        // Maps a CreateInvoiceDTO to an Invoice domain model.
        private async Task<Invoice> MapCreateDTOToInvoice(CreateInvoiceDTO request)
        {
            var booking = await _bookingRepository.GetByIdAsync(request.BookingId);

            return new Invoice
            {
                Amount = request.Amount,
                ServiceDetails = request.ServiceDetails,
                BookingId = request.BookingId,
                Booking = booking
            };
        }

        #endregion
    }
}
