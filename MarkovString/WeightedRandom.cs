using System;

namespace MarkovString
{
    public class WeightedRandom
    {
        private Random random;

        public WeightedRandom()
        {
            this.random = new Random();
        }

        public WeightedRandom(Random random)
        {
            if (random is null)
            {
                throw new ArgumentNullException();
            }

            this.random = random;
        }

        public int GetRandomIndex(int[] weights)
        {
            if (weights is null)
            {
                throw new ArgumentNullException();
            }

            if (weights.Length == 0)
            {
                throw new ArgumentException("Weights table must not be empty.");
            }

            int weightSum = 0;

            foreach (int w in weights)
            {
                if (w < 0)
                {
                    throw new ArgumentOutOfRangeException("All weights must be non-negative.");
                }

                weightSum = checked(weightSum + w);
            }

            if (weightSum == 0)
            {
                throw new ArgumentException("There must be at least one non-zero weight.");
            }

            int i = 0;
            int partialSum = weights[0];
            int dice = this.random.Next(weightSum);

            while (partialSum <= dice)
            {
                i++;
                partialSum += weights[i];
            }

            return i;
        }

        public int GetRandomIndex(double[] weights)
        {
            if (weights is null)
            {
                throw new ArgumentNullException();
            }

            if (weights.Length == 0)
            {
                throw new ArgumentException("Weights table must not be empty.");
            }

            double weightSum = 0.0;

            foreach (double w in weights)
            {
                if (double.IsNaN(w))
                {
                    throw new ArgumentException("Weights must not be NaN.");
                }

                if (double.IsInfinity(w))
                {
                    throw new ArgumentException("All weights must be finite.");
                }

                if (w < 0)
                {
                    throw new ArgumentOutOfRangeException("All weights must be non-negative.");
                }

                weightSum += w;

                if (double.IsInfinity(weightSum))
                {
                    throw new OverflowException();
                }
            }

            if (weightSum == 0.0)
            {
                throw new ArgumentException("There must be at least one non-zero weight.");
            }

            int i = 0;
            double partialSum = weights[0];
            double dice = this.random.NextDouble() * weightSum;

            while (partialSum <= dice)
            {
                i++;
                partialSum += weights[i];
            }

            return i;
        }

        public bool GetRandomBool(int trueWeight, int falseWeight)
        {
            if (trueWeight < 0 || falseWeight < 0)
            {
                throw new ArgumentOutOfRangeException("Both weights must be non-negative.");
            }

            if (trueWeight == 0 && falseWeight == 0)
            {
                throw new ArgumentException("At least one of the weights must be non-zero.");
            }

            return this.random.Next(trueWeight + falseWeight) < trueWeight;
        }

        public bool GetRandomBool(double trueProbability)
        {
            if (double.IsNaN(trueProbability))
            {
                throw new ArgumentException("Probability must not be NaN.");
            }

            if (double.IsInfinity(trueProbability))
            {
                throw new ArgumentException("Probability must be finite.");
            }

            if (trueProbability < 0.0 || trueProbability > 1.0)
            {
                throw new ArgumentOutOfRangeException("Probability must be between 0 and 1.");
            }

            return this.random.NextDouble() < trueProbability;
        }
    }
}
