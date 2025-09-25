using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using MovieReservation.Server.Application.Common.Exceptions;

namespace MovieReservation.Server.Application.Bookings.Queries.GetBookingById
{
public class GetBookingByIdQueryHandler : IRequestHandler<GetBookingByIdQuery, GetBookingByIdQueryResponse>
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IMapper _mapper;

    public GetBookingByIdQueryHandler(IBookingRepository bookingRepository, IMapper mapper)
    {
        _bookingRepository = bookingRepository;
        _mapper = mapper;
    }

    public async Task<GetBookingByIdQueryResponse?> Handle(GetBookingByIdQuery request, CancellationToken cancellationToken)
    {
        var booking = await _bookingRepository.GetBookingByIdAsync(request.Id, cancellationToken);
        if (booking == null)
        {
            throw new NotFoundException($"Booking with ID {request.Id} not found.");
        }
        return _mapper.Map<GetBookingByIdQueryResponse>(booking);
    }
}
}