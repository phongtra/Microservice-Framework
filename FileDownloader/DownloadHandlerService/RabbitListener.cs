using System;
using System.Text;
using IgniteDownloader;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.MessagePublishCommon;

namespace DownloadHandlerService
{
    public class RabbitListener
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly Message _message;
        private ConnectionFactory Factory { get; set; }
        private IConnection Connection { get; set; }
        IModel Channel { get; set; }

        public RabbitListener(IServiceProvider serviceProvider, Message message)
        {
            _serviceProvider = serviceProvider;
            _message = message;
            this.Factory = new ConnectionFactory() {HostName = "localhost"};
            this.Connection = Factory.CreateConnection();
            this.Channel = Connection.CreateModel();
        }

        public void Register()
        {
            var consumer = new EventingBasicConsumer(Channel);
            consumer.Received += ReceivedMessage;
            Channel.BasicConsume(queue: "DownloadLink", autoAck: true, consumer: consumer);
        }

        public void ReceivedMessage(object model, BasicDeliverEventArgs ea)
        {
            var body = ea.Body;
            var message = Encoding.UTF8.GetString(body);
            var output = Download.Initiate(message);
            _message.PublishMessage(output, "DownloadResult");
        }
        public void Deregister()
        {
            this.Connection.Close();
        }
    }
}
