using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace CurrencyTracker
{
    // ZORUNLU MODEL SINIFLARI
    class CurrencyResponse
    {
        public string Base { get; set; }
        public Dictionary<string, decimal> Rates { get; set; }
    }

    class Currency
    {
        public string Code { get; set; }
        public decimal Rate { get; set; }
    }

    class Program
    {
        private static List<Currency> _currencies = new List<Currency>();
        private static readonly HttpClient _httpClient = new HttpClient();

        static async Task Main(string[] args)
        {
            Console.WriteLine("Veriler yükleniyor, lütfen bekleyin...");
            await LoadCurrencyData();

            bool running = true;
            while (running)
            {
                Console.WriteLine("\n===== CurrencyTracker =====");
                Console.WriteLine("1. Tüm dövizleri listele");
                Console.WriteLine("2. Koda göre döviz ara");
                Console.WriteLine("3. Belirli bir değerden büyük dövizleri listele");
                Console.WriteLine("4. Dövizleri değere göre sırala");
                Console.WriteLine("5. İstatistiksel özet göster");
                Console.WriteLine("0. Çıkış");
                Console.Write("Seçiminiz: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1": ListAllCurrencies(); break;
                    case "2": SearchByCode(); break;
                    case "3": ListHigherThanValue(); break;
                    case "4": SortCurrencies(); break;
                    case "5": ShowStatistics(); break;
                    case "0": running = false; break;
                    default: Console.WriteLine("Geçersiz seçim!"); break;
                }
            }
        }

        static async Task LoadCurrencyData()
        {
            try
            {
                string url = "https://api.frankfurter.app/latest?from=TRY";
                string json = await _httpClient.GetStringAsync(url);
                
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var response = JsonSerializer.Deserialize<CurrencyResponse>(json, options);

                if (response?.Rates != null)
                {
                    // Dictionary'yi List<Currency>'ye çevirme
                    _currencies = response.Rates.Select(r => new Currency 
                    { 
                        Code = r.Key, 
                        Rate = r.Value 
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hata oluştu: {ex.Message}");
            }
        }

        // 1. Tüm Dövizleri Listele (LINQ Select)
        static void ListAllCurrencies()
        {
            var list = _currencies.Select(c => $"{c.Code}: {c.Rate}");
            foreach (var item in list) Console.WriteLine(item);
        }

        // 2. Koda Göre Döviz Ara (LINQ Where)
        static void SearchByCode()
        {
            Console.Write("Aranacak döviz kodu (örn: USD): ");
            string code = Console.ReadLine().ToUpper();
            
            var result = _currencies.Where(c => c.Code.Equals(code, StringComparison.OrdinalIgnoreCase)).ToList();

            if (result.Any())
                result.ForEach(c => Console.WriteLine($"{c.Code} bulundu. Kur: {c.Rate}"));
            else
                Console.WriteLine("Döviz kodu bulunamadı.");
        }

        // 3. Belirli Bir Değerden Büyük Dövizler (LINQ Where)
        static void ListHigherThanValue()
        {
            Console.Write("Minimum değer giriniz: ");
            if (decimal.TryParse(Console.ReadLine(), out decimal limit))
            {
                var filtered = _currencies.Where(c => c.Rate > limit);
                foreach (var c in filtered) Console.WriteLine($"{c.Code}: {c.Rate}");
            }
        }

        // 4. Dövizleri Değere Göre Sırala (LINQ OrderBy)
        static void SortCurrencies()
        {
            Console.WriteLine("1. Artan (Küçükten Büyüğe)\n2. Azalan (Büyükten Küçüğe)");
            string sortChoice = Console.ReadLine();

            var sorted = sortChoice == "1" 
                ? _currencies.OrderBy(c => c.Rate) 
                : _currencies.OrderByDescending(c => c.Rate);

            foreach (var c in sorted) Console.WriteLine($"{c.Code}: {c.Rate}");
        }

        // 5. İstatistiksel Özet (LINQ Count, Max, Min, Average)
        static void ShowStatistics()
        {
            if (!_currencies.Any()) return;

            int count = _currencies.Count();
            decimal max = _currencies.Max(c => c.Rate);
            decimal min = _currencies.Min(c => c.Rate);
            decimal average = _currencies.Average(c => c.Rate);

            Console.WriteLine($"\n--- İstatistiksel Özet ---");
            Console.WriteLine($"Toplam Döviz Sayısı : {count}");
            Console.WriteLine($"En Yüksek Kur       : {max}");
            Console.WriteLine($"En Düşük Kur        : {min}");
            Console.WriteLine($"Ortalama Kur        : {average:F4}");
        }
    }
}