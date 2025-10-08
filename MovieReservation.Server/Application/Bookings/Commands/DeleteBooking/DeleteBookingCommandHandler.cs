using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using MovieReservation.Server.Application.Common.Exceptions;
using MovieReservation.Server.Application.Common.Interfaces;

namespace MovieReservation.Server.Application.Bookings.Commands.DeleteBooking
{
    public record DeleteBookingCommand : IRequest
    {
        public int Id { get; set; }
    }
    public class DeleteBookingCommandHandler : IRequestHandler<DeleteBookingCommand>
    {
        private readonly IMovieReservationDbContext _context;

        public DeleteBookingCommandHandler(IMovieReservationDbContext context)
        {
            _context = context;
        }

        public async Task Handle(DeleteBookingCommand request, CancellationToken cancellationToken)
        {
            Booking booking = await _context.Bookings.FindAsync(new object[] { request.Id }, cancellationToken) ?? throw new NotFoundException($"Booking with Id {request.Id} not found.");

            if (booking == null)
            {
                throw new NotFoundException($"Booking with Id {request.Id} not found.");
            }
            
            _context.Bookings.Remove(booking);

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}