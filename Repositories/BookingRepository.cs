﻿using Microsoft.EntityFrameworkCore;
using ResturantBooking.Data;
using ResturantBooking.Models;
using ResturantBooking.Repositories.IRepositories;

namespace ResturantBooking.Repositories
{

    public class BookingRepository : IBookingRepository
    {
        private readonly AppDBContext _context;

        public BookingRepository(AppDBContext context)
        {
            _context = context;
        }

        public async Task<List<Booking>> GetAllBookingsAsync()
        {
            return await _context.Bookings
                .Include(b => b.Table)
                .Include(b => b.Customer)
                .ToListAsync();
        }
        public async Task<Booking?> GetBookingByIdAsync(int id)
        {
            return await _context.Bookings
               .Include(b => b.Table)
               .Include(b => b.Customer)
               .FirstOrDefaultAsync(b => b.Id == id);
        }
        public async Task<int> CreateBookingAsync(Booking booking)
        {
            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();
            return booking.Id;
        }

        public async Task<bool> UpdateBookingAsync(Booking booking)
        {
            _context.Bookings.Update(booking);
            return await _context.SaveChangesAsync() > 0;
        }
        
        public async Task<bool> DeleteBookingAsync(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null) return false;

            _context.Bookings.Remove(booking);
            return await _context.SaveChangesAsync() > 0;
        }
        public async Task<List<ResturantTable>> GetAvailableTablesAsync(DateTime startTime, int guests)
        {
            var endTime = startTime.AddHours(2);

            // Hämta alla bord som är tillräckligt stora
            var query = _context.Tables
                .Where(t => t.Capacity >= guests)
                .Include(t => t.Bookings);

            var availableTables = await query
                .Where(t => !t.Bookings.Any(b =>
                    (startTime < b.StartTime.AddHours(2)) &&
                    (endTime > b.StartTime)))
                .ToListAsync();

            return availableTables;
        }

        public async Task<List<Booking>> GetBookingsByTableAsync(int tableId)
        {
            return await _context.Bookings
                .Where(b => b.TableId_FK == tableId)
                .ToListAsync();
        }
    }
}
