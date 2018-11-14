using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BerlinLandingSending.Models;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Threading;
using Microsoft.Extensions.Options;
using System.Data.SQLite;

namespace BerlinLandingSending.Controllers
{
    [Route("api/[controller]")]
    public class SampleDataController : Controller
    {
        private static string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private EmailSettings EmailSettings { get; }

        public class WeatherForecast
        {
            public string DateFormatted { get; set; }
            public int TemperatureC { get; set; }
            public string Summary { get; set; }

            public int TemperatureF
            {
                get
                {
                    return 32 + (int)(TemperatureC / 0.5556);
                }
            }
        }
        public SampleDataController(IOptions<EmailSettings> emailSettings)
        {
            EmailSettings = emailSettings.Value;
        }

        [HttpGet("[action]")]
        public IEnumerable<WeatherForecast> WeatherForecasts()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                DateFormatted = DateTime.Now.AddDays(index).ToString("d"),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            });
        }

        [HttpGet("[action]")]
        public void SendMail(MailModel mailModel)
        {
            mailModel.EMail = "Guest@unicreo.com";
            mailModel.Name = "Arthur";
            mailModel.Text = "I love Unicreo";
            
            Database db = new Database();
            db.OpenConnection();
            string query = "INSERT INTO Clients  (`Name`, `Email`, `Text`) VALUES (@name, @email, @text)";
            SQLiteCommand myCommand = new SQLiteCommand(query, db.myConnection);
            myCommand.Parameters.AddWithValue("@name", mailModel.Name);
            myCommand.Parameters.AddWithValue("@email", mailModel.EMail);
            myCommand.Parameters.AddWithValue("@text", mailModel.Text);

            myCommand.ExecuteNonQuery();
            db.CloseConnection();



            var message = new MailMessage(mailModel.EMail ?? "Unknown Sender", "Ravovis@gmail.com")
            {
                Subject = "Ravovis@gmail.com",
                Body = mailModel.Name + " sent you a message via Berlin page: " + mailModel.Text
                
            };

            var smtp = new SmtpClient(EmailSettings.SmtpHost, EmailSettings.SmtpPort);
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential(EmailSettings.SmtpLogin, EmailSettings.SmtpPassword);
            smtp.EnableSsl = true;
            smtp.Send(message);



        }



    }
}
