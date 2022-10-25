using System.Diagnostics;

class Program
{
    static void Main(string[] args)
    {
        int ARRAY_SIZE = 200_000_000;
        int[] bigArray = Enumerable.Range(0, ARRAY_SIZE).ToArray();

        Random rng = new Random();

        for (int i = 0; i < ARRAY_SIZE; i++) {
            bigArray[i] = rng.Next(1, 11);
        }

        Stopwatch watch = Stopwatch.StartNew();
        long classicalSum = GetClassicalSum(bigArray);
        watch.Stop();

        Stopwatch parallelWatch = Stopwatch.StartNew();
        long parallelSum = GetThreadedSum(bigArray);
        parallelWatch.Stop();

        Console.WriteLine($"Classical approach took {watch.ElapsedMilliseconds} ms and calculated {classicalSum}.");
        Console.WriteLine($"Parallel apporach took {parallelWatch.ElapsedMilliseconds} ms and calculated {parallelSum}.");
    }

    private static long GetClassicalSum(int[] numbers) {
        long sum = 0;

        for (int i = 0; i < numbers.Length; i++) {
            sum += numbers[i];
        }

        return sum;
    }

    private static long GetThreadedSum(int[] numbers) {
        long sum = 0;

        Parallel.For<long>(0, numbers.Length, () => 0, (i, loop, subtotal) =>
        {
            subtotal += numbers[i];
            return subtotal;
        },
            (x) => Interlocked.Add(ref sum, x)
        );

        return sum;
    }
}