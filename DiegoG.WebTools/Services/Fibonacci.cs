using System.Buffers;
using System.Collections;
using System.Numerics;
using System.Runtime.InteropServices;

namespace DiegoG.WebTools.Services;

public class FibonacciEnumerable<TInt> : IEnumerator<TInt>
    where TInt : unmanaged, IBinaryInteger<TInt>
{
    private TInt a = TInt.One;
    private TInt b = TInt.One + TInt.One;

    private ulong indx = 0;

    public TInt Current => indx switch
    {
        0 => TInt.Zero,
        1 => TInt.One,
        2 => TInt.One,
        _ => b
    };

    public bool MoveNext()
    {
        if (indx++ > 2)
        {
            TInt c = b;
            b = Fibonacci<TInt>.Forward(a, b);
            a = c;
        }
        return true;
    }

    public bool MovePrevious()
    {
        if (--indx > 2)
        {
            TInt p = a;
            a = Fibonacci<TInt>.Backward(b, a);
            b = p;
        }
        return true;
    }

    public void Reset()
    {
        indx = 0;
        a = TInt.One;
        b = TInt.One + TInt.One;
    }

    object IEnumerator.Current => Current;

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}

public static class Fibonacci<TInt>
    where TInt : unmanaged, IBinaryInteger<TInt>
{
    public const double LogGoldenRatio = 0.4812118250287667;
    public const double Log5TimesHalf = 0.8047189562170501;

    public const double OneOverSqrt5 = 0.4472135954999579;
    public const double FiboA = 1.618033988749895;
    public const double FiboB = -0.6180339887498949;

    public static TInt Forward(TInt previous, TInt current)
        => previous + current;

    public static TInt Backward(TInt current, TInt previous)
        => current - previous;

    public static TInt GetFibonacciNumberAt(TInt index)
    {
        var ind = double.CreateSaturating(index);
        return TInt.CreateSaturating(double.Round(OneOverSqrt5 * (double.Pow(FiboA, ind) - double.Pow(FiboB, ind)), MidpointRounding.ToEven));
    }

    public static int CountFibonacciNumbersUpTo(TInt fibonacciNumbers)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(fibonacciNumbers, TInt.Zero);
        var fn = double.CreateSaturating(fibonacciNumbers);
        return (int)double.Ceiling((double.Log(fn + .5) + Log5TimesHalf) / LogGoldenRatio);
    }

    public static void FillWithFibonacciSequence(Span<TInt> fibs)
    {
        if (fibs.Length == 0) return;
        if (fibs.Length == 1) fibs[0] = TInt.Zero;

        fibs[0] = TInt.Zero;
        fibs[1] = TInt.One;

        for (int i = 2; i < fibs.Length; i++)
            fibs[i] = fibs[i - 1] + fibs[i - 2];
    }

    public static TInt[] ProduceFibonacciSequence(TInt max)
    {
        if (max is 0)
            return [TInt.Zero];
        else if (max is 1)
            return [TInt.Zero, TInt.One, TInt.One];

        var len = CountFibonacciNumbersUpTo(max);
        byte[]? rented = null;
        Span<TInt> fibs
            = len > 1_000 // The resulting array would be about 4 KB, big enough to warrant not putting this monstrosity on the stack
                ? MemoryMarshal.Cast<byte, TInt>(rented = ArrayPool<byte>.Shared.Rent(len * sizeof(int)))
                : stackalloc TInt[len];

        try
        {
            FillWithFibonacciSequence(fibs);
            return [.. fibs];
        }
        finally
        {
            if (rented is not null)
                ArrayPool<byte>.Shared.Rent(len * sizeof(int));
        }
    }
}
