using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieReservation.Server.Application.Shows.Commands.UpdateShow
{
    public record UpdateShowCommand : IRequest
    {
        public int Id { get; set; }
        public int TheaterId { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public DateTime Date { get; set; }
    }
    public class UpdateShowCommandHandler : IRequestHandler<UpdateShowCommand>
    {
        private readonly IMovieReservationDbContext _context;
        public UpdateShowCommandHandler(IMovieReservationDbContext context)
        {
            _context = context;
        }

        public async Task Handle(UpdateShowCommand request, CancellationToken cancellationToken)
        {
            var show = await _context.Shows.FindAsync(new object[] { request.Id }, cancellationToken);
            if (show == null)
            {
                throw new Exception($"Show not found: {request.Id}");
            }

            show.TheaterId = request.TheaterId;
            show.StartTime = request.StartTime;
            show.EndTime = request.EndTime;
            show.Date = request.Date;
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}