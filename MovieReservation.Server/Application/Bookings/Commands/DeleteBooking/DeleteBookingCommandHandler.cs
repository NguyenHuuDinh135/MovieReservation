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
    public class DeleteBookingCommandHandler : IRequestHandler<DeleteBookingCommand>
    {
        private readonly IBookingRepository _bookingRepository;

        public DeleteBookingCommandHandler(IBookingRepository bookingRepository)
        {
            _bookingRepository = bookingRepository;
        }

        public async Task Handle(DeleteBookingCommand request, CancellationToken cancellationToken)
        {
            Booking booking = await _bookingRepository.GetBookingByIdAsync(request.Id, cancellationToken) ?? throw new NotFoundException($"Booking with Id {request.Id} not found.");
            await _bookingRepository.DeleteBookingAsync(booking, cancellationToken);
        }
    }
}