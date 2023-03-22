using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MarkovString
{
    public class OneByTwo : IStringGenerator
    {     
        private List<char> alphabet;
        private readonly WeightedRandom wrandom;
        private readonly int[,][] pairs;
        private readonly StringBuilder sb;
        private const char nullCharacter = '\0';

        public OneByTwo(string[] sample, WeightedRandom wrandom)
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
            pairs = new int[alphabet.Count, alphabet.Count][];

            for (i = 0; i < alphabet.Count; i++)
            {
                for (j = 0; j < alphabet.Count; j++)
                {
                    pairs[i, j] = new int[alphabet.Count];
                }
            }

            foreach (string s in sample)
            {
                AnalyzeString(s);
            }
        }

        public OneByTwo(string[] sample) : this(sample, new WeightedRandom())
        {
        }

        public OneByTwo(string[] sample, Random random) : this(sample, new WeightedRandom(random))
        {
        }

        public int MaxLength { get; set; }

        public string GenerateString()
        {
            sb.Clear();
            int i, j, k;
            i = 0;
            j = 0;

            while (sb.Length < MaxLength)
            {
                k = wrandom.GetRandomIndex(pairs[i, j]);

                if (k > 0)
                {
                    sb.Append(alphabet[k]);
                    i = j;
                    j = k;
                }
                else
                {
                    break;
                }
            }

            return sb.ToString();
        }

        private void InitializeAlphabet(string[] sample)
        {
            alphabet = new List<char>();
            alphabet.Add(nullCharacter);

            foreach (string s in sample)
            {
                foreach (char c in s)
                {
                    if (alphabet.IndexOf(c) <= 0)
                    {
                        alphabet.Add(c);
                    }
                }
            }
        }

        private void AnalyzeString(string s)
        {
            int i, j, k;
            i = 0;
            j = 0;

            for (int pos = 0; pos < s.Length; pos++)
            {
                k = alphabet.LastIndexOf(s[pos]);
                pairs[i, j][k]++;
                i = j;
                j = k;
            }

            pairs[i, j][0]++;
            pairs[j, 0][0]++;
        }
    }
}