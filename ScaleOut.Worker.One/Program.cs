using System;
using System.Configuration;
using MassTransit;
using ScaleOut.Messages;

namespace ScaleOut.Worker
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Consumer: " + ConfigurationManager.AppSettings["worker"]);
            Bus.Initialize(sbc =>
            {
             /*   sbc.UseMsmq();
                sbc.VerifyMsmqConfiguration();
                sbc.UseMulticastSubscriptionClient();
                sbc.ReceiveFrom("msmq://localhost/test_queue_worker");*/

                sbc.UseRabbitMq();
                sbc.ReceiveFrom("rabbitmq://localhost/test_queue_worker");
                sbc.Subscribe(subs =>
                {
                    var del = new Action<IConsumeContext<DoWorkItem>, DoWorkItem>((context, msg) =>
                    {
                        Console.WriteLine(msg.Text);
                    });
                    subs.Handler(del);
                });
            });

            Console.ReadLine();
        }
    }
}
