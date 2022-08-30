// See https://aka.ms/new-console-template for more information

using MassTransit;

Console.WriteLine("Hello, World!");

foreach (var i in Enumerable.Range(1,20).ToList())
{
    Thread.Sleep(1000);
    Console.WriteLine(NewId.Next());
}

