using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MovieReservation.Server.Application.Shows.Commands.CreateShow
{
    public record CreateShowCommand : IRequest<int>
    {
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public DateTime Date { get; set; }
        public int MovieId { get; set; }
        public int TheaterId { get; set; }
        public ShowStatus Status { get; set; }
        public ShowType Type { get; set; }
    }
    public class CreateShowCommandHandler : IRequestHandler<CreateShowCommand, int>
    {
        private IMovieReservationDbContext _context;

        public CreateShowCommandHandler(IMovieReservationDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(CreateShowCommand request, CancellationToken CancellationToken)
        {
            var show = new Show
            {
                StartTime = request.StartTime,
                EndTime = request.EndTime,
                Date = request.Date,
                MovieId = request.MovieId,
                TheaterId = request.TheaterId,
                Status = request.Status,
                Type = request.Type,
            };

            _context.Shows.Add(show);

            await _context.SaveChangesAsync(CancellationToken);
            return show.Id;
        }
    }
}