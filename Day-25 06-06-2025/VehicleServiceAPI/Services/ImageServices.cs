using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using VehicleServiceAPI.Interfaces;
using VehicleServiceAPI.Models;
using VehicleServiceAPI.Models.DTOs;
using VehicleServiceAPI.Repositories;

namespace VehicleServiceAPI.Services
{
    public class ImageService : IImageService
    {
        private readonly ImageRepository _imageRepository;
        private readonly BookingRepository _bookingRepository;
        private readonly VehicleRepository _vehicleRepository;
        // Path where images will be stored (this can be injected via configuration)
        private readonly string _uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

        public ImageService(ImageRepository imageRepository, BookingRepository bookingRepository, VehicleRepository vehicleRepository)
        {
            _imageRepository = imageRepository;
            _bookingRepository = bookingRepository;
            _vehicleRepository = vehicleRepository;

            // Ensure the uploads folder exists.
            if (!Directory.Exists(_uploadsFolder))
            {
                Directory.CreateDirectory(_uploadsFolder);
            }
        }

        /// <summary>
        /// Retrieves an image by its ID and maps it to a DTO.
        /// </summary>
        public async Task<ImageDTO> GetImageByIdAsync(int id)
        {
            var image = await _imageRepository.GetByIdAsync(id);
            return MapImageToDto(image);
        }

        /// <summary>
        /// Retrieves all images as a collection of DTOs.
        /// </summary>
        public async Task<IEnumerable<ImageDTO>> GetAllImagesAsync()
        {
            var images = await _imageRepository.GetAllAsync();
            return images.Select(MapImageToDto);
        }

        /// <summary>
        /// Creates a new image record.
        /// </summary>
        /// <remarks>
        /// This method expects a CreateImageDTO that includes the uploaded file.
        /// </remarks>
        public async Task<ImageDTO> CreateImageAsync(CreateImageDTO request)
        {
            // Save the file and set its path.
            string filePath = await SaveFileAsync(request.File);
            // Set the generated file path in the request.
            request.FilePath = filePath;

            var imageEntity = await MapCreateDTOToImage(request);
            var createdImage = await _imageRepository.AddAsync(imageEntity);
            return MapImageToDto(createdImage);
        }

        /// <summary>
        /// Updates an existing image.
        /// </summary>
        public async Task<ImageDTO> UpdateImageAsync(int id, UpdateImageDTO request)
        {
            var image = await _imageRepository.GetByIdAsync(id);
            if (image == null)
            {
                throw new InvalidOperationException("Image not found.");
            }
            
            // If a new file is provided, update the file and generate a new file path.
            if(request.File != null)
            {
                string newFilePath = await SaveFileAsync(request.File);
                image.FilePath = newFilePath;
            }
            else
            {
                image.FilePath = request.FilePath; // or keep the current file path if no new file is provided.
            }
            
            image.BookingId = request.BookingId;
            image.VehicleId = request.VehicleId;

            var updatedImage = await _imageRepository.UpdateAsync(image);
            return MapImageToDto(updatedImage);
        }

        /// <summary>
        /// Soft-deletes an image by its ID.
        /// </summary>
        public async Task<bool> DeleteImageAsync(int id)
        {
            return await _imageRepository.DeleteAsync(id);
        }

        /// <summary>
        /// Retrieves all images associated with a booking.
        /// </summary>
        public async Task<IEnumerable<ImageDTO>> GetImagesByBookingIdAsync(int bookingId)
        {
            var images = await _imageRepository.GetImagesByBookingIdAsync(bookingId);
            return images.Select(MapImageToDto);
        }

        /// <summary>
        /// Retrieves all images associated with a vehicle.
        /// </summary>
        public async Task<IEnumerable<ImageDTO>> GetImagesByVehicleIdAsync(int vehicleId)
        {
            var images = await _imageRepository.GetImagesByVehicleIdAsync(vehicleId);
            return images.Select(MapImageToDto);
        }

        #region Mapping Methods

        // Maps an Image domain model to an ImageDTO.
        private ImageDTO MapImageToDto(Image image)
        {
            return new ImageDTO
            {
                Id = image.Id,
                FilePath = image.FilePath,
                BookingId = image.BookingId,
                VehicleId = image.VehicleId,
                // Optional: Include booking and vehicle details.
                BookingReference = image.Booking != null ? image.Booking.Status : "No Booking",
                VehicleDetails = image.Vehicle != null ? $"{image.Vehicle.Make} {image.Vehicle.Model} ({image.Vehicle.Year})" : "No Vehicle"
            };
        }

        // Maps a CreateImageDTO to an Image domain model.
        private async Task<Image> MapCreateDTOToImage(CreateImageDTO request)
        {
            var booking = await _bookingRepository.GetByIdAsync(request.BookingId);
            var vehicle = await _vehicleRepository.GetByIdAsync(request.VehicleId);

            return new Image
            {
                FilePath = request.FilePath,
                BookingId = request.BookingId,
                VehicleId = request.VehicleId,
                Booking = booking,
                Vehicle = vehicle
            };
        }

        #endregion

        /// <summary>
        /// Saves the uploaded file to disk and returns the generated file path.
        /// </summary>
        private async Task<string> SaveFileAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new InvalidOperationException("Invalid file.");

            // Generate a unique file name
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var filePath = Path.Combine(_uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Return the file path relative to wwwroot (or just the file name if you have a dedicated asset handler)
            return Path.Combine("uploads", fileName).Replace("\\", "/");
        }
    }
}
