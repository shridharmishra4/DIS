using System;
using RabbitMQ.Client;
using System.Text;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace DIS
{
    public class MessagingQueue
    {
        public IConfiguration _configuration;
        public MessagingQueue(IConfiguration configuration) {
            _configuration = configuration;

        }

        public void Publish(string Filename)
        {
            var payload = new Models.EventPayload();
            payload.context = new Models.EventPayload.Context();
            payload.info = new Models.EventPayload.Info();
            payload.context.domain = "BrokerConfirmation";
            payload.context.application = "Imagica";
            payload.context.serviceName = "DIS";
            payload.context.eventVersion = "0.1";
            payload.context.eventGeneratedAtTime = DateTime.Now;
            payload.context.requestID = "";
            payload.context.correlationID = "";
            payload.info.filename = Filename;
            payload.info.source = _configuration.GetConnectionString("DeploymentMode");
            var apipath = _configuration.GetConnectionString("DeploymentServer")
                                        + _configuration
                                        .GetConnectionString("ApiEndPointPath") +
                                        Filename;
            payload.info.apipath = apipath;

            var factory = new ConnectionFactory() { HostName = _configuration
                                        .GetConnectionString("RabbitMQServer")
            };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: "DIS", type: "fanout");

                string message = JsonConvert.SerializeObject(payload);
                Console.WriteLine(message);
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "DIS",
                         routingKey: "hello",
                         basicProperties: null,
                         body: body);
                Console.WriteLine(" [x] Sent {0}", message);
            }
        }
    }
}