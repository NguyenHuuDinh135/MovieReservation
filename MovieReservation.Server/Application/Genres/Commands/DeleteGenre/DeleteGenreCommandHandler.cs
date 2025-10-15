using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using MovieReservation.Server.Application.Common.Exceptions;
using MovieReservation.Server.Application.Common.Interfaces;

namespace MovieReservation.Server.Application.Genres.Commands.DeleteGenre
{
    public class DeleteGenreCommandHandler : IRequest
    {
        private readonly IMovieReservationDbContext _context;

        public DeleteGenreCommandHandler(IMovieReservationDbContext context)
        {
            _context = context;
        }
        
        public async Task Handle(DeleteGenreCommand request, CancellationToken cancellationToken)
        {
            Genre genre = await _context.Genres.FindAsync(new object[] { request.Id }, cancellationToken) ?? throw new NotFoundException($"Genre with Id {request.Id} not found.");

            if(genre == null )
            {
                throw new NotFoundException($"Genre with Id {request.Id} not found.");
            }

            _context.Genres.Remove(genre);
            await _context.SaveChangesAsync(CancellationToken);
        }
    }
}