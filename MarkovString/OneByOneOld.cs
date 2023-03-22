/*
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MarkovString
{
    public class OneByOneOld : IStringGenerator
    {        
        private List<char> alphabet;
        private readonly int[] starts;
        private readonly int[][] pairs;
        private readonly int[] totals;
        private readonly int[] ends;
        private readonly StringBuilder sb;
        private readonly WeightedRandom wrandom;

        public OneByOneOld(string[] sample, WeightedRandom wrandom)
        {
            if (sample == null)
            {
                throw new ArgumentNullException();
            }

            sample = sample.Where(line => !string.IsNullOrEmpty(line)).ToArray();

            if (sample.Length == 0)
            {
                throw new ArgumentException();
            }

            MaxLength = int.MaxValue;
            this.wrandom = wrandom;
            sb = new StringBuilder();
            InitializeAlphabet(sample);
            int i, j;
            starts = new int[alphabet.Count];
            pairs = new int[alphabet.Count][];
            ends = new int[alphabet.Count];

            for (i = 0; i < alphabet.Count; i++)
            {
                pairs[i] = new int[alphabet.Count];
            }

            foreach (string s in sample)
            {
                AnalyzeString(s);
            }
            
            totals = new int[alphabet.Count];

            for (i = 0; i < alphabet.Count; i++)
            {
                for (j = 0; j < alphabet.Count; j++)
                {
                    totals[i] += pairs[i][j];
                }
            }
        }

        public OneByOneOld(string[] sample) : this(sample, new WeightedRandom())
        {
        }

        public OneByOneOld(string[] sample, Random random) : this(sample, new WeightedRandom(random))
        {
        }

        public int MaxLength { get; set; }

        public string GenerateString()
        {
            sb.Clear();
            int i, j;
            i = wrandom.GetRandomIndex(starts);
            sb.Append(alphabet[i]);

            while (sb.Length < MaxLength && wrandom.GetRandomBool(totals[i], ends[i]))
            {
                j = wrandom.GetRandomIndex(pairs[i]);
                sb.Append(alphabet[j]);
                i = j;
            }

            return sb.ToString();
        }

        private void InitializeAlphabet(string[] sample)
        {
            alphabet = new List<char>();

            foreach (string s in sample)
            {
                foreach (char c in s)
                {
                    if (!alphabet.Contains(c))
                    {
                        alphabet.Add(c);
                    }
                }
            }
        }

        private void AnalyzeString(string s)
        {
            int i, j;
            i = alphabet.IndexOf(s[0]);
            starts[i]++;

            for (int pos = 1; pos < s.Length; pos++)
            {
                j = alphabet.IndexOf(s[pos]);
                pairs[i][j]++;
                i = j;
            }

            ends[i]++;
        }
    }
}
*/