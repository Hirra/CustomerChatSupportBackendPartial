using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using QueueSubscribers.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace QueueSubscribers.Implementations
{
    public class ChatSessionSubscriber : IQueueSubscriber
    {
        private readonly IConfiguration configuration;
        private readonly ILogger<ChatSessionSubscriber> logger;
        private readonly string exchange;
        private readonly string queue;
        private readonly string routingKey;
        private IModel channel;
        private bool disposed;

        public ChatSessionSubscriber(IConfiguration configuration, ILogger<ChatSessionSubscriber> logger)
        {
            this.configuration = configuration;
            this.logger = logger; 

            this.exchange = this.configuration["SessionQueue:Exchange"];
            this.queue = this.configuration["SessionQueue:ChatSessionQueue"];
            this.routingKey = this.configuration["SessionQueue:ChatSessionRoutingKey"]; 
        }

        public bool Consume(Func<string, IDictionary<string, object>, bool> callback)
        {
            try
            {  
                this.CreateChannel();
                this.SetupChannel();

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (sender, e) =>
                {
                    var body = e.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    bool success = callback.Invoke(message, e.BasicProperties.Headers);
                    if (success)
                    {
                        channel.BasicAck(e.DeliveryTag, true);
                    }
                }; 

                channel.BasicConsume(this.queue, false, consumer);

                this.logger.LogDebug("complete");
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
