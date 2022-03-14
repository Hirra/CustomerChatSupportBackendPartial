using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using QueuePublishers.Models;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace QueuePublishers.Implementations
{
    public class ChatSessionPublisher : Interfaces.IQueuePublisher
    {
        private readonly IConfiguration configuration;
        private readonly ILogger<ChatSessionPublisher> logger;

        private IModel channel;
        private QueueConfigurations QueueConfigurations;

        private readonly string exchange;
        private readonly string queue;
        private readonly string routingKey;

        private bool disposed;
        public ChatSessionPublisher(IConfiguration configuration, ILogger<ChatSessionPublisher> logger)
        {
            this.configuration = configuration;
            this.logger = logger;

            this.exchange = this.configuration["SessionQueue:Exchange"];
            this.queue = this.configuration["SessionQueue:ChatSessionQueue"];
            this.routingKey = this.configuration["SessionQueue:ChatSessionRoutingKey"];
        }

        public bool Publish(object data)
        {
            try
            {
                var serializedData = JsonConvert.SerializeObject(data);
                byte[] body = Encoding.UTF8.GetBytes(serializedData);

                this.CreateChannel();
                this.SetupChannel();
                this.channel.BasicPublish(this.exchange, this.routingKey, basicProperties: null, body);

                this.logger.LogDebug("Publishe complete");
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message + "\n" + ex.StackTrace);
                return false;
            }

            return true;
        }

        private void CreateChannel()
        {
            var factory = new ConnectionFactory { Uri = GetRabbitMqConnectionUri() };
            this.channel = factory.CreateConnection().CreateModel();
            this.logger.LogDebug("Channel created");
        }

        private void SetupChannel()
        {
            var argumnts = new Dictionary<string, object> {
                {"x-message-ttl",3000}
            };
            this.channel.ExchangeDeclare(this.exchange, ExchangeType.Direct);
            this.channel.QueueDeclare(this.queue, durable: true, exclusive: false, autoDelete: false, arguments: null);
            this.channel.QueueBind(this.queue, this.exchange, this.routingKey);
            this.channel.BasicQos(0, 20, false);
            this.logger.LogDebug("Channel setup complete");
        }

        private Uri GetRabbitMqConnectionUri()
        {
            return new Uri("amqp://" +
                this.configuration["SessionQueue:UserName"] + ":" +
                this.configuration["SessionQueue:Password"] + "@" +
                this.configuration["SessionQueue:Server"] + ":" +
                this.configuration["SessionQueue:Port"]);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
                this.channel?.Close();

            disposed = true;
        }
    }
}
