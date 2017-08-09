using System;
using System.Linq;

namespace SwEngHomework.DescriptiveStatistics
{
    public class StatsCalculator : IStatsCalculator
    {
        public Stats Calculate(string semicolonDelimitedContributions)
        {
            // Format input string by splitting on the semi-colon, replacing $ character, and trimming whitespace.
            // Look through formatted strings and only select those that begin with a number. Parse to double array.
            double[] contributions = semicolonDelimitedContributions
                .Split(';')
                .Select(c => c.Replace('$', ' ').Trim())
                .Where(c => Char.IsNumber(c[0]))
                .Select(c => double.Parse(c))
                .ToArray();

            if (contributions.Length > 0)
            {
                // Sort the array by ascending values
                double[] sortedContributions = CountingSort(contributions);

                return new Stats
                {
                    Average = Math.Round(sortedContributions.Sum() / sortedContributions.Length, 2),
                    Median = Math.Round(CalculateMedian(sortedContributions), 2),
                    Range = Math.Round((sortedContributions[sortedContributions.Length - 1] - sortedContributions[0]), 2)
                };
            }
            else // No elements in the array, return Stats object with 0 values
            {
                return new Stats
                {
                    Average = 0,
                    Median = 0,
                    Range = 0,
                };
            }
        }

        // Using counting sort here because Array.Sort C# method uses quicksort which is worst case O(n^2)
        // Counting sort allows the sorting to be done in worst case O(n + k) time
        private double[] CountingSort(double[] arr)
        {
            double minVal = arr[0];
            double maxVal = arr[0];
            double[] sortedArr = new double[arr.Length];

            // Find min and max values
            for (int i = 1; i < arr.Length; i++)
            {
                if (arr[i] < minVal) { minVal = arr[i]; }
                else if (arr[i] > maxVal) { maxVal = arr[i]; }
            }

            int[] counts = new int[(int)maxVal - (int)minVal + 1];

            // Calculate the frequencies
            for (int i = 0; i < arr.Length; i++)
                counts[(int)arr[i] - (int)minVal]++;

            // Recalculate
            counts[0]--;

            for (int i = 1; i < counts.Length; i++)
                counts[i] = counts[i] + counts[i - 1];

            // Sort
            for (int i = arr.Length - 1; i >= 0; i--)
                sortedArr[counts[(int)arr[i] - (int)minVal]--] = arr[i];

            return sortedArr;
        }

        // Calculates the median of an array for both even and odd length arrays
        private double CalculateMedian(double[] arr)
        {
            int numElements = arr.Length;

            // Array is even, calculate average of two middle elements
            if (numElements % 2 == 0)
            {
                return (arr[(numElements / 2) - 1] + arr[numElements / 2]) / 2;
            }
            // Array is odd, return middle element
            else
            {
                return arr[(int)Math.Floor((decimal)numElements / 2)];
            }
        }
    }
}
