using System;
using ProjectName.ServiceName.Application.Interfaces;

namespace Projectname.Infrastructure.Shared.Services
{
    public class DateTimeService : IDateTimeService
    {
        public DateTime NowUtc => DateTime.UtcNow;
    }
}