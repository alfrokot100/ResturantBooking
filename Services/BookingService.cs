using ResturantBooking.DTOs.BookingDTOs;
using ResturantBooking.Models;
using ResturantBooking.Repositories.IRepositories;
using ResturantBooking.Services.IServices;

namespace ResturantBooking.Services
{
    public class BookingService : IBookingService
    {
        private readonly ITableRepository _tableRepo;
        private readonly IBookingRepository _bookingRepo;

        public BookingService(ITableRepository tableRepo, IBookingRepository bookingRepo)
        {
            _tableRepo = tableRepo;
            _bookingRepo = bookingRepo;
        }
        public async Task<List<BookingDTO>> GetAllBookingsAsync()
        {
            var bookings = await _bookingRepo.GetAllBookingsAsync();
            return bookings.Select(b => new BookingDTO
            {
                Id = b.Id,
                TableId_FK = b.TableId_FK,
                CustomerId_FK = b.CustomerId_FK,
                Guest = b.Guest,
                StartTime = b.StartTime,
                CustomerName = b.Customer?.Name,
                TableNumber = b.Table?.Number ?? 0
            }).ToList();
        }
        
        public async Task<BookingDTO?> GetBookingByIdAsync(int id)
        {
            var booking = await _bookingRepo.GetBookingByIdAsync(id);
            if (booking == null) return null;

            return new BookingDTO
            {
                Id = booking.Id,
                TableId_FK = booking.TableId_FK,
                CustomerId_FK = booking.CustomerId_FK,
                Guest = booking.Guest,
                StartTime = booking.StartTime
            };
        }
        public async Task<BookingDTO?> CreateBookingAsync(BookingCreateDTO dto)
        {
            

            var existingBookings = await _bookingRepo.GetBookingsByTableAsync(dto.TableId_FK);

            bool conflict = existingBookings.Any(b =>
            b.StartTime.AddHours(-2) <= dto.StartTime && dto.StartTime <= b.StartTime.AddHours(2)
            );

            if (conflict) { return null; }

            var booking = new Booking
            {
                TableId_FK = dto.TableId_FK,
                CustomerId_FK = dto.CustomerId_FK,
                Guest = dto.Guest,
                StartTime = dto.StartTime
            };
            var bookingId = await _bookingRepo.CreateBookingAsync(booking);
            var createdBooking = await _bookingRepo.GetBookingByIdAsync(bookingId);

            if(createdBooking == null) { return null; }


            return new BookingDTO
            {
                Id = createdBooking.Id,
                TableId_FK = createdBooking.TableId_FK,
                CustomerId_FK = createdBooking.CustomerId_FK,
                Guest = createdBooking.Guest,
                StartTime = createdBooking.StartTime,
                CustomerName = createdBooking.Customer?.Name,
                TableNumber = createdBooking.Table?.Number ?? 0
            };

        }
        public async Task<bool> UpdateBookingAsync(BookingUpdateDTO bookingDto)
        {
            var booking = new Booking
            {
                Id = bookingDto.Id,
                TableId_FK = bookingDto.TableId_FK,
                CustomerId_FK = bookingDto.CustomerId_FK,
                Guest = bookingDto.Guest,
                StartTime = bookingDto.StartTime
            };

            return await _bookingRepo.UpdateBookingAsync(booking);
        }
        public async Task<bool> DeleteBookingAsync(int id)
        {
            return await _bookingRepo.DeleteBookingAsync(id);
        }
        public async Task<List<ResturantTable>> GetAvailableTablesAsync(DateTime startTime, int guests)
        {
            return await _bookingRepo.GetAvailableTablesAsync(startTime, guests);
        }
    }
}
