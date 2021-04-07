using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Rabbit
{
    public class RabbitListener : IHostedService
    {
        private readonly IConnection connection;
        private readonly IModel channel;

        public RabbitListener(IOptions<AppConfiguration> options)
        {
            try
            {
                var factory = new ConnectionFactory()
                {
                    // 这是我这边的配置,自己改成自己用就好
                    UserName = "ning",//用户名
                    Password = "tn940902",//密码
                    HostName = "localhost"//rabbitmq ip
                };
                this.connection = factory.CreateConnection();
                this.channel = connection.CreateModel();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"RabbitListener init error,ex:{ex.Message}");
            }
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Register();
            return Task.CompletedTask;
        }

        protected string RouteKey;
        protected string QueueName;
        // 处理消息的方法
        public virtual string Process(string message)
        {
            throw new NotImplementedException();
        }
        // 注册消费者监听在这里
        public void Register()
        {
            Console.WriteLine($"RabbitListener register,routeKey:{RouteKey}");
            channel.QueueDeclare(queue: "rpc_queue", durable: false, exclusive: false, autoDelete: false, arguments: null);
            channel.BasicQos(0, 1, false);
            EventingBasicConsumer consumer = new EventingBasicConsumer(channel);
            channel.BasicConsume(queue: "rpc_queue", autoAck: false, consumer: consumer);

            consumer.Received += (model, ea) =>
            {
                //传来的属性
                IBasicProperties props = ea.BasicProperties;
                //RPC反馈的属性-channel创建
                IBasicProperties replyProps = channel.CreateBasicProperties();
                //RPC 相关标识
                replyProps.CorrelationId = props.CorrelationId;

                byte[] responseBytes = null;
                var message = Encoding.UTF8.GetString(ea.Body);
                var result = Process(message);
                if (result!=null)
                {
                    responseBytes = ServerExe(ea.Body);
                    var response = Encoding.UTF8.GetBytes(result);

                    channel.BasicPublish(exchange: "", routingKey: props.ReplyTo, basicProperties: replyProps, body: response);
                    channel.BasicAck(ea.DeliveryTag, false);
                }
            };
            //channel.BasicConsume(queue: QueueName, consumer: consumer);
        }

        private static byte[] ServerExe(byte[] body)
        {
            string response = null;
            var message = Encoding.UTF8.GetString(body);
            int n = int.Parse(message);
            response = n + "";//fib(n).ToString();
            return Encoding.UTF8.GetBytes(response);
        }

        public void DeRegister()
        {
            this.connection.Close();
        }
        public Task StopAsync(CancellationToken cancellationToken)
        {
            this.connection.Close();
            return Task.CompletedTask;
        }
    }
}
