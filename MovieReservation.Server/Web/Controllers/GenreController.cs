using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieReservation.Server.Infrastructure;
using MovieReservation.Server.Application.Common.Models;
using MovieReservation.Server.Domain.Entities;
using MovieReservation.Server.Web.Controllers;
using MovieReservation.Server.Application.Genre.Queries.GetGenres;
using MovieReservation.Server.Application.Genre.Queries.GetBookingById;
using MovieReservation.Server.Application.Genre.Commands.DeleteGenre;
using MovieReservation.Server.Application.Common.Exceptions;
using MovieReservation.Server.Application.Genres.Commands.CreateGenre;
using MovieReservation.Server.Application.Genres.Commands.DeleteGenre;
using MovieReservation.Server.Application.Genres.Queries.GetGenreByMovie;
using MovieReservation.Server.Application.Common.Exceptions;

namespace MovieReservation.Server.Web.Controllers
{
    public class GenreController
    {
        
    }
}