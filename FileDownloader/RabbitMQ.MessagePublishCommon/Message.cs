using System;
using System.Collections.Generic;
using System.Text;
using RabbitMQ.Client;

namespace RabbitMQ.MessagePublishCommon
{
    public class Message
    {
        public static void PublishMessage(string message, string routingKey)
        {
            byte[] body;
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            body = Encoding.UTF8.GetBytes(message);
            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;

            channel.BasicPublish("",
                routingKey,
                properties,
                body);
        }
    }
}
