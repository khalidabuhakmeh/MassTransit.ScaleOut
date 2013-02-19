using System;
using System.Timers;
using MassTransit;
using ScaleOut.Messages;

namespace ScaleOut.Publisher
{
    class Program
    {
        static void Main(string[] args)
        {
            Bus.Initialize(cfg =>
            {
                cfg.UseRabbitMq();
                cfg.ReceiveFrom("rabbitmq://localhost/test_queue");
            });

            var i = 1;
            var _timer = new Timer(3000) { AutoReset = true, Enabled = true };
            _timer.Elapsed += (sender, eventArgs) =>
            {
                Console.WriteLine("Sending message #{0} to a random worker", i++);
                Bus.Instance.Publish(new DoWorkItem { Text = string.Format("Message #{0} says: Hello World at {1}!", i, DateTime.Now) });
            };

            Console.ReadLine();
        }
    }
}
