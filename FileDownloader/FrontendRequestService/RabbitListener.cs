using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using IConnection = RabbitMQ.Client.IConnection;

namespace FrontendRequestService
{
    public class RabbitListener
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly NotifyService _service;
        private ConnectionFactory Factory { get; set; }
        private IConnection Connection { get; set; }
        IModel Channel { get; set; }

        public RabbitListener(IServiceProvider serviceProvider, NotifyService service)
        {
            _serviceProvider = serviceProvider;
            this.Factory = new ConnectionFactory() { HostName = "localhost" };
            this.Connection = Factory.CreateConnection();
            this.Channel = Connection.CreateModel();
            _service = service;
        }

        public void Register()
        {
            var consumer = new EventingBasicConsumer(Channel);
            consumer.Received += ReceivedMessage;
            Channel.BasicConsume(queue: "ScrewYou", autoAck: true, consumer: consumer);
        }

        public void ReceivedMessage(object model, BasicDeliverEventArgs ea)
        {
            var body = ea.Body;
            var message = Encoding.UTF8.GetString(body);
            Console.WriteLine(message);
            _service.SendNotificationAsync(message);


        }
        public void Deregister()
        {
            this.Connection.Close();
        }
    }

}
