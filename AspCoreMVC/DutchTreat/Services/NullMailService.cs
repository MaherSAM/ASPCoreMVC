using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DutchTreat.Services
{
    public class NullMailService : IMailService
    {
        private readonly ILogger _logger;

        public NullMailService(ILogger logger)
        {
            _logger = logger;
        }
        public void SendMessage(string to, string from, string subject, string message)
        {
            _logger.LogInformation(string.Format("To : {0}, From : {1}, Subject : {2}, Message : {3}",to,from,subject,message));
        }
    }
}
