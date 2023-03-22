using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MarkovString
{
    public class OneByOne : IStringGenerator
    {     
        private List<char> alphabet;
        private readonly WeightedRandom wrandom;
        private readonly int[][] pairs;
        private readonly StringBuilder sb;
        private const char nullCharacter = '\0';

        public OneByOne(string[] sample, WeightedRandom wrandom)
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
            int i;
            pairs = new int[alphabet.Count][];

            for (i = 0; i < alphabet.Count; i++)
            {
                pairs[i] = new int[alphabet.Count];
            }

            foreach (string s in sample)
            {
                AnalyzeString(s);
            }
        }

        public OneByOne(string[] sample) : this(sample, new WeightedRandom())
        {
        }

        public OneByOne(string[] sample, Random random) : this(sample, new WeightedRandom(random))
        {
        }

        public int MaxLength { get; set; }

        public string GenerateString()
        {
            sb.Clear();
            int i, j;
            i = 0;

            while (sb.Length < MaxLength)
            {
                j = wrandom.GetRandomIndex(pairs[i]);

                if (j > 0)
                {
                    sb.Append(alphabet[j]);
                    i = j;
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
            int i, j;
            i = 0;

            for (int pos = 0; pos < s.Length; pos++)
            {
                j = alphabet.LastIndexOf(s[pos]);
                pairs[i][j]++;
                i = j;
            }

            pairs[i][0]++;
        }
    }
}