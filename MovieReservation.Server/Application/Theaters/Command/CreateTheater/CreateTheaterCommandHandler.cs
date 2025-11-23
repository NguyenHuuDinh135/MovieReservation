using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

namespace MovieReservation.Server.Application.Theaters.Command.CreateTheater
{
    public record CreateTheaterCommand : IRequest<int>
    {
        public string Name { get; set; } = string.Empty;
        public int NumOfRows { get; set; }
        public int SeatsPerRow { get; set; }
        public TheaterType Type { get; set; }
    }
    public class CreateTheaterCommandHandler(IMovieReservationDbContext context, IMapper mapper) : IRequestHandler<CreateTheaterCommand, int>
    {
        private readonly IMovieReservationDbContext _context = context;
        private readonly IMapper _mapper = mapper;

        public async Task<int> Handle(CreateTheaterCommand request, CancellationToken cancellationToken)
        {
            var theater = new Theater
            {
                Name = request.Name,
                NumOfRows = request.NumOfRows,
                SeatsPerRow = request.SeatsPerRow,
                Type = request.Type
            };

            _context.Theaters.Add(theater);

            await _context.SaveChangesAsync(cancellationToken);
            
            return theater.Id;
        }
    }
}