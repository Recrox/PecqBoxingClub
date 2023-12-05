using Hangfire;
using Hangfire.Storage;
using HtmlAgilityPack;
using Newtonsoft.Json;
using RamDam.BackEnd.Core.Models.Api;
using RamDam.BackEnd.Core.Models.RamDamApi;
using RamDam.BackEnd.Core.Repositories;
using RamDam.BackEnd.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RamDam.BackEnd.Core.Services.Implementations
{
    public class MovieService : IMovieService
    {
        private readonly IRequestService _requestService;
        private readonly IMovieRepository _movieRepository;
        private readonly ISchedulingRepository _schedulingRepository;
        private readonly INotificationService _notificationService;
        private readonly IFavoritesRepository _favoritesRepository;

        public MovieService(
            IRequestService requestService, 
            IMovieRepository movieRepository, 
            ISchedulingRepository schedulingRepository, 
            INotificationService notificationService,
            IFavoritesRepository favoritesRepository)
        {
            _requestService = requestService;
            _movieRepository = movieRepository;
            _schedulingRepository = schedulingRepository;
            _notificationService = notificationService;
            _favoritesRepository = favoritesRepository;
        }

        public async Task UpdateMovies()
        {
            using (var connection = JobStorage.Current.GetConnection())
            {
                foreach (var recurringJob in connection.GetRecurringJobs())
                {
                    RecurringJob.RemoveIfExists(recurringJob.Id);
                }
            }
            var ramDamMovies = await _requestService.GetRamDamMovies();
            ramDamMovies.ForEach(mv => mv.Acf.Screening = mv.Acf.Screening.GetType() == typeof(bool) ? Array.Empty<Screening>() : ((Newtonsoft.Json.Linq.JArray)mv.Acf.Screening).ToObject<Screening[]>());
            var movies = await _movieRepository.GetManyAsync();
            await this.UpdateMovies(ramDamMovies, movies.ToList());
        }

        private async Task UpdateMovies(List<RamDamMovie> ramDamMovies, List<Movie> movies)
        {
            foreach(var ramDamMovie in ramDamMovies)
            {
                var movie = movies.Find(movie => movie.IdRamDamMovie == ramDamMovie.Id);
                if (movie == null)
                {
                    var parsedHtml = this.ParseHtml(ramDamMovie.Content.Rendered);
                    var newMovie = new Movie()
                    {
                        IdRamDamMovie = ramDamMovie.Id,
                        Length = (System.Web.HttpUtility.HtmlDecode(ramDamMovie.Acf.Length)).GetHours(),
                        Realisator = System.Web.HttpUtility.HtmlDecode(ramDamMovie.Acf.Realisator),
                        Title = System.Web.HttpUtility.HtmlDecode(ramDamMovie.Title.Rendered),
                        Attach = System.Web.HttpUtility.HtmlDecode(parsedHtml.ImageUrl),
                       Description = System.Web.HttpUtility.HtmlDecode(parsedHtml.Summary),
                       //Description = System.Web.HttpUtility.HtmlDecode(ramDamMovie.Yoast_head_json.Og_description.Replace("_", "")),
                        Trailer = parsedHtml.TrailerUrl,
                        Link = System.Web.HttpUtility.HtmlDecode(ramDamMovie.Link)
                    };
                    var createdMovie = await _movieRepository.CreateAsync(newMovie);
                    await this.CreateNewNotifications((Screening[])ramDamMovie.Acf.Screening, createdMovie);
                }
                else
                {
                    var parsedHtml = this.ParseHtml(ramDamMovie.Content.Rendered);
                    if (!ramDamMovie.Equals(movie) || (parsedHtml.TrailerUrl == null && movie.Trailer != null) || (parsedHtml.TrailerUrl != null && movie.Trailer == null) 
                        || (!parsedHtml.TrailerUrl?.Equals(movie.Trailer) ?? false) || !System.Web.HttpUtility.HtmlDecode(parsedHtml.ImageUrl).Equals(movie.Attach) || !System.Web.HttpUtility.HtmlDecode(parsedHtml.Summary).Equals(System.Web.HttpUtility.HtmlDecode(movie.Description)))
                    {
                        movie.Title = System.Web.HttpUtility.HtmlDecode(ramDamMovie.Title.Rendered);
                        movie.Realisator = System.Web.HttpUtility.HtmlDecode(ramDamMovie.Acf.Realisator);
                        movie.Length = (System.Web.HttpUtility.HtmlDecode(ramDamMovie.Acf.Length)).GetHours();
                        movie.Attach = parsedHtml.ImageUrl;
                        movie.Description = System.Web.HttpUtility.HtmlDecode(parsedHtml.Summary);
                        //movie.Description = System.Web.HttpUtility.HtmlDecode(ramDamMovie.Yoast_head_json.Og_description.Replace("_", ""));

                        movie.Trailer = parsedHtml.TrailerUrl;
                        movie.Link = System.Web.HttpUtility.HtmlDecode(ramDamMovie.Link);
                        if (!ramDamMovie.CheckScheduling(movie.Scheduling, (Screening[])ramDamMovie.Acf.Screening))
                        {
                            foreach (var sch in movie.Scheduling.Where(sch => !this.IsSchedulingStillExisting(sch, (Screening[])ramDamMovie.Acf.Screening)))
                            {
                                if (sch.IdJob != null)
                                    _notificationService.CancelNotification(sch.IdJob ?? 0);
                                var favorites = await _favoritesRepository.GetManyAsync(fav => fav.IdScheduling.Equals(sch.Id));
                                foreach (var favorite in favorites)
                                {
                                    await _favoritesRepository.DeleteAsync(favorite.Id);
                                }
                                await _schedulingRepository.DeleteAsync(sch.Id);
                            }
                            await this.CreateNewNotifications(((Screening[])ramDamMovie.Acf.Screening).Where(scr => this.IsNewScheduling(movie.Scheduling, scr)).ToArray(), movie);
                        }
                        await _movieRepository.UpsertAsync(movie);
                    }
                    
                }
            }
        }

        private bool IsSchedulingStillExisting(Scheduling scheduling, Screening[] screenings)
        {
            return screenings.Select(scr =>
            {
                var hour = scr.Hour.Length == 1 ? $"0{scr.Hour}" : scr.Hour;
                return DateTime.ParseExact($"{scr.Day} {hour}:{scr.Minute}", "yyyyMMdd HH:mm", CultureInfo.InvariantCulture);
            }).Contains(scheduling.StartTime);
        }

        private bool IsNewScheduling(List<Scheduling> schedulings, Screening screening)
        {
            var hour = screening.Hour.Length == 1 ? $"0{screening.Hour}" : screening.Hour;
            var dt = DateTime.ParseExact($"{screening.Day} {hour}:{screening.Minute}", "yyyyMMdd HH:mm", CultureInfo.InvariantCulture);
            return !schedulings.Select(sch => sch.StartTime).Contains(dt);
        }

        private async Task<List<Scheduling>> CreateNewNotifications(Screening[] screenings, Movie movie)
        {
            var schedulings = new List<Scheduling>();
            foreach (var sch in screenings)
            {
                var startTime = DateTime.ParseExact(sch.Day, "yyyyMMdd", CultureInfo.InvariantCulture).AddHours(int.Parse(sch.Hour)).AddMinutes(int.Parse(sch.Minute));
                var newScheduling = await _schedulingRepository.CreateAsync(new Scheduling() { Movie = movie, StartTime = startTime });
                var notif = await _notificationService.SendNotification(new Notification()
                {
                    SendDate = startTime.AddMinutes(-15),
                    Title = $"RamDam - {movie.Title}",
                    Body = "Votre film commence dans 15 minutes !",
                    Topic = newScheduling.Id.ToString()
                });
                newScheduling.IdJob = notif.IdJob;
                await _schedulingRepository.UpsertAsync(newScheduling);
                schedulings.Add(newScheduling);
            }
            return schedulings;
        }

        public async Task<Movie> TestMovieParsing(string jsonResult)
        {
            var ramDamMovie = JsonConvert.DeserializeObject<List<RamDamMovie>>(jsonResult);
            var parsedHtml = this.ParseHtml(ramDamMovie.FirstOrDefault()?.Content?.Rendered);
            return new Movie()
            {
                IdRamDamMovie = ramDamMovie.FirstOrDefault()?.Id,
                Link = ramDamMovie.FirstOrDefault()?.Link,
                Length = ramDamMovie.FirstOrDefault()?.Acf?.Length,
                Realisator = ramDamMovie.FirstOrDefault()?.Acf?.Realisator,
                Title = ramDamMovie.FirstOrDefault()?.Title?.Rendered,
                Attach = parsedHtml?.ImageUrl,
                Description = parsedHtml?.Summary
            };
        }

        private ParsedRamDamHtml ParseHtml(string html)
        {
            var htmlDocument = new HtmlDocument();
            string summary = string.Empty;
            htmlDocument.LoadHtml(html);
            var image = htmlDocument.DocumentNode.SelectSingleNode("//img");
            var trailer = htmlDocument.DocumentNode.SelectSingleNode("//iframe");
            foreach(var node in htmlDocument.DocumentNode.ChildNodes)
            {
                if (!node.InnerHtml.StartsWith("<img") || !node.InnerHtml.StartsWith("<iframe"))
                {
                    var text = node.InnerText.Replace("\n", "") + "\n\n";
                    if (!text.Equals("\n\n"))
                        summary += text;
                }
            }
            return new ParsedRamDamHtml() 
            {
                ImageUrl = image.Attributes["src"].Value,
                TrailerUrl = trailer?.Attributes["src"]?.Value,
                Summary = summary
            };
        }

        public async Task<bool> UpdateNotifications()
        {
            var movies = await _movieRepository.GetManyAsync();
            foreach(var movie in movies)
            {
                foreach(var scheduling in movie.Scheduling)
                {
                    if (scheduling.StartTime >= DateTime.Now)
                    {
                        _notificationService.CancelNotification(scheduling.IdJob ?? 0);
                        var startTime = scheduling.StartTime;
                    
                        var notif = await _notificationService.SendNotification(new Notification()
                        {
                            SendDate = startTime.AddMinutes(-15),
                            Title = $"RamDam - {movie.Title}",
                            Body = "Votre film commence dans 15 minutes !",
                            Topic = scheduling.Id.ToString()
                        });
                        scheduling.IdJob = notif.IdJob;
                        scheduling.Movie = movie;
                        await _schedulingRepository.UpsertAsync(scheduling);
                    }                    
                }
            }
            return true;
        }
    }
}
