using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PocketGoogle
{
    public static class StringExtension
    {
        public static List<int> IndexOfAll(this string documentText, string word, int startIndex = 0)
        {
            var ints = new List<int>();
            var index = documentText.IndexOf(word, startIndex);
            while (index > -1)
            {
                if (word.Length == 1)
                {
                    var previous = index != 0 ? documentText[index - 1] : default(char);
                    var next = index + 1 < documentText.Length ? documentText[index + 1] : default(char);
                    if (char.IsWhiteSpace(previous) && char.IsWhiteSpace(next)
                        || char.IsWhiteSpace(previous) && next == default(char)
                        || char.IsWhiteSpace(next) || char.IsPunctuation(next)
                        || previous == default(char) && next == default(char))
                        ints.Add(index);
                }
                else
                    ints.Add(index);
                index = documentText.IndexOf(word, index + word.Length);
            }
            return ints;
        }
    }

    public class Indexer : IIndexer
    {
        private Dictionary<string, Dictionary<int, List<int>>> Set { get; set; }

        public Indexer()
        {
            Set = new Dictionary<string, Dictionary<int, List<int>>>();
        }

        public void Add(int id, string documentText)
        {
            var separator = new char[] { ' ', '.', ',', '!', '?', ':', '-', '\r', '\n' };
            var words = documentText
                .Split(separator, StringSplitOptions.RemoveEmptyEntries)
                .Distinct();
            foreach (var word in words)
            {
                if (!Set.ContainsKey(word))
                    Set[word] = new Dictionary<int, List<int>>();
                if (!Set[word].ContainsKey(id))
                    Set[word][id] = new List<int>();
                Set[word][id].AddRange(documentText.IndexOfAll(word));
            }
        }

        public List<int> GetIds(string word)
        {
            if (Set.ContainsKey(word))
                return Set.First(kvp => kvp.Key == word).Value.Keys.ToList();
            return new List<int>();
        }

        public List<int> GetPositions(int id, string word)
        {
            if (Set.ContainsKey(word) && Set[word].ContainsKey(id))
                return Set[word][id];
            return new List<int>();
        }

        public void Remove(int id)
        {
            foreach (var value in Set.Values)
                if (value.ContainsKey(id))
                    value.Remove(id);
        }
    }
}
