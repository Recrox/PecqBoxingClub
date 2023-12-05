using Hangfire;
using Hangfire.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RamDam.BackEnd.Core.Services.Implementations
{
    public class RecurringJobsService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        public RecurringJobsService(
            IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                ClearRecurringJobs();
                var movieService = scope.ServiceProvider.GetService<IMovieService>();
                RecurringJob.AddOrUpdate("sync_movies", () => movieService.UpdateMovies(), "*/10 * * * *", System.TimeZoneInfo.Local); //Job every 10 minutes
                await Task.CompletedTask;
            }
        }
        private void ClearRecurringJobs()
        {
            using (var connection = JobStorage.Current.GetConnection())
            {
                foreach (var recurringJob in connection.GetRecurringJobs())
                {
                    RecurringJob.RemoveIfExists(recurringJob.Id);
                }
            }
        }
    }
}
