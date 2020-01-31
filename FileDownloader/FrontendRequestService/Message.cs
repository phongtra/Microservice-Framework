using System;
using System.Text;
using FrontendRequestService.Models;
using RabbitMQ.Client;

namespace FrontendRequestService
{
    public class Message
    {
        public static void PublishMessage(Link link = null)
        {
            byte[] body;
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            if (link == null)
            {
                var number = GeneratedNumber();
                body = Encoding.UTF8.GetBytes(number.ToString());
            }
            else
            {
                body = Encoding.UTF8.GetBytes(link.Value);
            }
            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;

            channel.BasicPublish("",
                "Fuckyou",
                properties,
                body);
        }

        private static int GeneratedNumber()
        {
            var rnd = new Random();
            var number = rnd.Next(0, 100);
            return number;
        }
    }
}
