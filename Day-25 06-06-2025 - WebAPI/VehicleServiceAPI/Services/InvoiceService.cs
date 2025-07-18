using Microsoft.OpenApi.Any;
using VehicleServiceAPI.Interfaces;
using VehicleServiceAPI.Models;
using VehicleServiceAPI.Models.DTOs;
using VehicleServiceAPI.Repositories;
using PdfSharpCore.Pdf;
using PdfSharpCore.Drawing;

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
            var invoiceDtos = new List<InvoiceDTO>();
            foreach (var invoice in invoices)
            {
                var dto = await MapInvoiceToDto(invoice);
                invoiceDtos.Add(dto);
            }
            return invoiceDtos;
        }

        /// <summary>
        /// Creates a new invoice record.
        /// </summary>
        public async Task<InvoiceDTO> CreateInvoiceAsync(CreateInvoiceDTO request)
        {

            //here check the user bookings and update amount based on invoice flag
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

        public async Task<InvoiceDTO> GetInvoiceByBookingIdAsync(int id)
        {
            var invoiceMeta = await _invoiceRepository.GetInvoicesByBookingIdAsync(id);
            var dto = await MapInvoiceToDto(invoiceMeta);
            return dto;
        }

        /// <summary>
        /// Retrieves all invoices associated with a booking.
        /// </summary>
        public async Task<InvoicePdfDTO> GetInvoicePDFByBookingIdAsync(int id)
        {
            var invoiceMeta = await _invoiceRepository.GetInvoicesByBookingIdAsync(id);
            var dto = await MapInvoiceToDto(invoiceMeta);
            byte[] pdfBytes;

            using (var ms = new MemoryStream())
            {
                PdfDocument document = new PdfDocument();
                PdfPage page = document.AddPage();
                XGraphics gfx = XGraphics.FromPdfPage(page);

                XFont titleFont = new("Arial", 20, XFontStyle.Bold);
                XFont labelFont = new XFont("Arial", 12, XFontStyle.Bold);
                XFont valueFont = new XFont("Arial", 12, XFontStyle.Regular);

                double y = 40;
                gfx.DrawString($"Invoice #{dto.Id}", titleFont, XBrushes.DarkGray, new XRect(0, y, page.Width, 30), XStringFormats.TopCenter);
                y += 40;

                void DrawRow(string label, string value)
                {
                    gfx.DrawString(label + ":", labelFont, XBrushes.Black, new XRect(40, y, 120, 20), XStringFormats.TopLeft);
                    gfx.DrawString(value, valueFont, XBrushes.Black, new XRect(170, y, page.Width - 210, 20), XStringFormats.TopLeft);
                    y += 25;
                }

                DrawRow("Booking ID", dto.BookingId.ToString());
                DrawRow("Customer", $"{dto.Name} ({dto.Email}, {dto.Phone})");
                DrawRow("Service Date", dto.SlotDateTime.ToString("f"));
                DrawRow("Mechanic", dto.MechanicName);
                DrawRow("Vehicle", dto.RegistrationNumber);
                DrawRow("Details", dto.ServiceDetails);
                DrawRow("Amount", $"Rs.{dto.Amount:F2}");

                document.Save(ms, false);
                pdfBytes = ms.ToArray();
            }

            return new InvoicePdfDTO
            {
                FileName = $"invoice_{dto.Id}.pdf",
                ContentType = "application/pdf",
                FileContents = pdfBytes
            };
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
                DiscountFlag = invoice.DiscountFlag,
                DiscountPercentage = invoice.DiscountPercentage,
            };
        }

        // Maps a CreateInvoiceDTO to an Invoice domain model.
        private async Task<Invoice> MapCreateDTOToInvoice(CreateInvoiceDTO request)
        {
            var booking = await _bookingRepository.GetByIdAsync(request.BookingId);
            //get user booking details and.check the orders he has
            bool DiscountFlag = false;
            int DiscountPercentage = 0;
            var userId= booking.UserId;
            var user= await _userRepository.GetByIdAsync(userId);
            if(user.RoleId==2){
                DiscountFlag= true;
                DiscountPercentage= 5;
            }
            else{   
            var userbookings = await _bookingRepository.GetBookingsByUserIdAsync(userId);
            Console.WriteLine($"User {userId} has {userbookings.Count()} bookings.");
            if(userbookings.Count() > 3)
            {
                DiscountPercentage=10;
                DiscountFlag= true;
            }
            }

            return new Invoice
            {
                Amount = request.Amount,
                ServiceDetails = request.ServiceDetails,
                BookingId = request.BookingId,
                Booking = booking,
                DiscountFlag = DiscountFlag,
                DiscountPercentage = DiscountPercentage
            };
        }

        #endregion
    }
}
