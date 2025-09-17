using System;
using System.Collections.Generic;
using System.Linq;

namespace TextAnalyzer
{
    public class DocumentAnalyzer
    {
        private List<string> _tokens;
        private readonly List<string> _conjunctions = new List<string>
        {
            "или", "и", "да", "не только", "но и", "но", "зато", "однако",
            "либо", "потому что", "так как", "ибо", "когда", "пока",
            "после того как", "если", "хотя", "хоть", "чтобы"
        };

        public DocumentAnalyzer(string content)
        {
            if (string.IsNullOrWhiteSpace(content))
                throw new ArgumentException("Текст не может быть пустым");

            _tokens = content.Split(new[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
                            .Where(token => NormalizeToken(token).Length > 0)
                            .ToList();

            if (_tokens.Count == 0)
                throw new ArgumentException("Текст не содержит значимых слов");
        }

        public void FilterCharacters(string charactersToRemove)
        {
            if (string.IsNullOrEmpty(charactersToRemove))
                return;

            var filteredTokens = new List<string>();
            var charsToRemove = charactersToRemove.ToLower().ToCharArray();

            foreach (var token in _tokens)
            {
                var filteredToken = token;
                foreach (var c in charsToRemove)
                {
                    filteredToken = filteredToken.Replace(c.ToString(), "")
                                                .Replace(char.ToUpper(c).ToString(), "");
                }

                if (!string.IsNullOrWhiteSpace(filteredToken))
                    filteredTokens.Add(filteredToken);
            }

            _tokens = filteredTokens;
        }

        public AnalysisResult GetAnalysisReport()
        {
            return new AnalysisResult
            {
                TotalWords = CountTotalWords(),
                NormalizedWordCount = CountMeaningfulWords(),
                ShortestWord = FindExtremeLengthWord(findLongest: false),
                LongestWord = FindExtremeLengthWord(findLongest: true),
                SentenceCount = CountSentences(),
                VowelConsonantCount = CountVowelsAndConsonants(),
                CharacterFrequency = CalculateCharacterFrequency()
            };
        }

        private uint CountTotalWords() => (uint)_tokens.Count;

        private uint CountMeaningfulWords()
        {
            uint count = 0;
            
            foreach (var token in _tokens)
            {
                var normalized = NormalizeToken(token.ToLower());
                
                if (ContainsDigits(normalized) || _conjunctions.Contains(normalized))
                    continue;
                    
                count++;
            }
            
            return count;
        }

        private string FindExtremeLengthWord(bool findLongest)
        {
            if (_tokens.Count == 0) return string.Empty;

            string extremeWord = NormalizeToken(_tokens[0]);
            
            foreach (var token in _tokens)
            {
                var current = NormalizeToken(token);
                if ((findLongest && current.Length > extremeWord.Length) ||
                    (!findLongest && current.Length < extremeWord.Length))
                {
                    extremeWord = current;
                }
            }
            
            return extremeWord;
        }

        private uint CountSentences()
        {
            uint count = 0;
            var sentenceEnders = new[] { '.', '!', '?', '…' };

            foreach (var token in _tokens)
            {
                if (token.Length > 0 && sentenceEnders.Contains(token[^1]))
                    count++;
            }
            
            return count;
        }

        private (uint Vowels, uint Consonants) CountVowelsAndConsonants()
        {
            uint vowels = 0, consonants = 0;
            var russianVowels = "аеёиоуыэюя";
            var russianConsonants = "бвгджзйклмнпрстфхцчшщъь";

            foreach (var token in _tokens)
            {
                foreach (var c in token.ToLower())
                {
                    if (russianVowels.Contains(c))
                        vowels++;
                    else if (russianConsonants.Contains(c))
                        consonants++;
                }
            }
            
            return (vowels, consonants);
        }

        private Dictionary<char, int> CalculateCharacterFrequency()
        {
            var frequency = new Dictionary<char, int>();
            var russianLetters = "абвгдеёжзийклмнопрстуфхцчшщъыьэюя";

            foreach (var token in _tokens)
            {
                foreach (var c in token.ToLower())
                {
                    if (russianLetters.Contains(c))
                    {
                        if (frequency.ContainsKey(c))
                            frequency[c]++;
                        else
                            frequency[c] = 1;
                    }
                }
            }
            
            return frequency.OrderBy(pair => pair.Key)
                           .ToDictionary(pair => pair.Key, pair => pair.Value);
        }

        private static string NormalizeToken(string token)
        {
            var punctuation = ",.;:'\"`?!()[]{}—–- ";
            return new string(token.Where(c => !punctuation.Contains(c)).ToArray());
        }

        private static bool ContainsDigits(string text) => text.Any(char.IsDigit);
    }

    public class AnalysisResult
    {
        public uint TotalWords { get; set; }
        public uint NormalizedWordCount { get; set; }
        public string ShortestWord { get; set; } = string.Empty;
        public string LongestWord { get; set; } = string.Empty;
        public uint SentenceCount { get; set; }
        public (uint Vowels, uint Consonants) VowelConsonantCount { get; set; }
        public Dictionary<char, int> CharacterFrequency { get; set; } = new Dictionary<char, int>();

        public override string ToString()
        {
            var result = $"Слов:\t\t\t{TotalWords}\n" +
                        $"Значимых слов:\t\t{NormalizedWordCount}\n" +
                        $"Самое короткое слово:\t{ShortestWord}\n" +
                        $"Самое длинное слово:\t{LongestWord}\n" +
                        $"Предложений:\t\t{SentenceCount}\n" +
                        $"Гласных/согласных:\t{VowelConsonantCount.Vowels}/{VowelConsonantCount.Consonants}\n" +
                        $"Частота букв:\n";

            foreach (var (character, count) in CharacterFrequency)
            {
                result += $"{char.ToUpper(character)}: {count,4}\n";
            }

            return result;
        }
    }

    class TextAnalysisApplication
    {
        private readonly List<DocumentAnalyzer> _documents = new List<DocumentAnalyzer>();

        public void Run()
        {
            Console.WriteLine("=== Анализатор текста ===");

            while (true)
            {
                DisplayMenu();
                var choice = GetUserChoice();

                switch (choice)
                {
                    case -1:
                        return;
                    case 0:
                        AddNewDocument();
                        break;
                    default:
                        if (choice > 0 && choice <= _documents.Count)
                            AnalyzeDocument(choice - 1);
                        else
                            Console.WriteLine("Неверный номер документа");
                        break;
                }
            }
        }

        private void DisplayMenu()
        {
            Console.WriteLine("\nВыберите действие:");
            Console.WriteLine("-1 - Выход");
            Console.WriteLine(" 0 - Добавить текст");
            
            for (int i = 0; i < _documents.Count; i++)
            {
                Console.WriteLine($" {i + 1} - Анализ текста #{i + 1}");
            }
        }

        private int GetUserChoice()
        {
            while (true)
            {
                Console.Write("Ваш выбор: ");
                if (int.TryParse(Console.ReadLine(), out int choice))
                    return choice;
                
                Console.WriteLine("Введите корректное число");
            }
        }

        private void AddNewDocument()
        {
            try
            {
                Console.WriteLine("Введите текст (минимум 100 символов):");
                Console.WriteLine("(Для завершения ввода введите пустую строку)");

                var lines = new List<string>();
                string line;

                while (!string.IsNullOrWhiteSpace(line = Console.ReadLine()))
                {
                    lines.Add(line);
                }

                var content = string.Join("\n", lines);

                if (content.Length < 100)
                {
                    Console.WriteLine("Текст должен содержать не менее 100 символов");
                    return;
                }

                var analyzer = new DocumentAnalyzer(content);
                _documents.Add(analyzer);

                Console.WriteLine($"Текст добавлен под номером {_documents.Count}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }

        private void AnalyzeDocument(int index)
        {
            try
            {
                var analyzer = _documents[index];
                var result = analyzer.GetAnalysisReport();

                Console.WriteLine("\nРезультаты анализа:");
                Console.WriteLine(result);

                Console.WriteLine("Введите символы для удаления (или Enter для пропуска):");
                var charsToRemove = Console.ReadLine();

                if (!string.IsNullOrEmpty(charsToRemove))
                {
                    analyzer.FilterCharacters(charsToRemove);
                    Console.WriteLine("Символы удалены. Новый анализ:");

                    var newResult = analyzer.GetAnalysisReport();
                    Console.WriteLine(newResult);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка анализа: {ex.Message}");
            }
        }
    }

    class Program
    {
        static void Main()
        {
            var app = new TextAnalysisApplication();
            app.Run();
        }
    }
}
