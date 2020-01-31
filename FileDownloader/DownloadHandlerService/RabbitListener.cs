using System;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace DownloadHandlerService
{
    public class RabbitListener
    {
        private readonly IServiceProvider _serviceProvider;
        private ConnectionFactory Factory { get; set; }
        private IConnection Connection { get; set; }
        IModel Channel { get; set; }

        public RabbitListener(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            this.Factory = new ConnectionFactory() {HostName = "localhost"};
            this.Connection = Factory.CreateConnection();
            this.Channel = Connection.CreateModel();
        }

        public void Register()
        {
            var consumer = new EventingBasicConsumer(Channel);
            consumer.Received += ReceivedMessage;
            Channel.BasicConsume(queue: "Fuckyou", autoAck: true, consumer: consumer);
        }

        public void ReceivedMessage(object model, BasicDeliverEventArgs ea)
        {
            var body = ea.Body;
            var message = Encoding.UTF8.GetString(body);
            Console.WriteLine(" [x] Received {0}", message);
        }
        public void Deregister()
        {
            this.Connection.Close();
        }
    }
}
