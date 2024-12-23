using DiegoG.WebTools.Services;

namespace DiegoG.WebTools.Sandbox;

internal class Program
{
    static void Main(string[] args)
    {
        List<ulong> fibs1_set = [];
        for (int i = 0; i < 100; i++) 
            fibs1_set.Add(Fibonacci<ulong>.GetFibonacciNumberAt((ulong)i));
        var fibs1 = fibs1_set.ToArray();
        var fibs2 = new ulong[fibs1.Length];
        Fibonacci<ulong>.FillWithFibonacciSequence(fibs2);
    }
}
