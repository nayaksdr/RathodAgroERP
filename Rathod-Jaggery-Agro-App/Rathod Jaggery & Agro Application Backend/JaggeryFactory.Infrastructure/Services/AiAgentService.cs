using JaggeryAgro.Core.Entities;
using JaggeryAgro.Data;
using JaggeryAgro.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace JaggeryAgro.Services
{
    // [All using directives stay the same]

    public class AiAgentService
    {

        private readonly IHttpClientFactory _httpFactory;
        private readonly ApplicationDbContext _db;
        private readonly string _apiKey;
        private readonly string _baseUrl;
        private readonly string _model;

        public AiAgentService(IHttpClientFactory httpFactory, ApplicationDbContext db, IConfiguration config)
        {
            _httpFactory = httpFactory;
            _db = db;

            _apiKey = config["OpenRouterApi:ApiKey"]
                      ?? throw new ArgumentNullException("OpenRouterApi:ApiKey missing");
            _baseUrl = config["OpenRouterApi:BaseUrl"]
                       ?? throw new ArgumentNullException("OpenRouterApi:BaseUrl missing");
            _model = config["OpenRouterApi:Model"] ?? "gpt-3.5-turbo";
        }

        public async Task<string> AskAsync(string question, CancellationToken cancellation = default)
        {
            string context = await GetDatabaseContextAsync(cancellation);
            string prompt = $"You are an assistant for Rathod Jaggery & Agro.\n{context}\nQuestion: {question}";

            var client = _httpFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);

            // OpenRouter may require extra headers like X-Title or HTTP-Referer
            client.DefaultRequestHeaders.Add("X-Title", "RathodJaggeryAI");
            client.DefaultRequestHeaders.Add("HTTP-Referer", "http://localhost");

            var payload = new
            {
                model = _model,
                messages = new[]
                {
                new { role = "system", content = "You are an assistant for a jaggery factory." },
                new { role = "user", content = prompt }
            }
            };

            var json = JsonSerializer.Serialize(payload);
            using var request = new StringContent(json, Encoding.UTF8, "application/json");

            var endpoint = $"{_baseUrl}/chat/completions";

            try
            {
                using var response = await client.PostAsync(endpoint, request, cancellation);
                var respText = await response.Content.ReadAsStringAsync(cancellation);

                if (!response.IsSuccessStatusCode)
                    return $"AI request failed: {(int)response.StatusCode} - {respText}";

                using var doc = JsonDocument.Parse(respText);

                string? answer = null;
                if (doc.RootElement.TryGetProperty("choices", out var choices) &&
                    choices.GetArrayLength() > 0)
                {
                    if (choices[0].TryGetProperty("message", out var message) &&
                        message.TryGetProperty("content", out var content))
                    {
                        answer = content.GetString();
                    }
                    else if (choices[0].TryGetProperty("text", out var text))
                    {
                        answer = text.GetString(); // fallback
                    }
                }

                return answer ?? "No response generated";
            }
            catch (Exception ex)
            {
                return $"AI request failed: {ex.Message}";
            }
        }


        private async Task<string> GetDatabaseContextAsync(CancellationToken cancellation = default)
        {
            var sb = new StringBuilder();

            // Attendance
            var attendance = await _db.Attendances.Include(a => a.Labor)
                .OrderByDescending(a => a.Date).Take(5).ToListAsync(cancellation);
            sb.AppendLine("📌 Recent Attendance:");
            if (attendance.Any())
                foreach (var a in attendance)
                    sb.AppendLine($"{a.Date:dd/MM/yyyy} - {a.Labor?.Name ?? "Unknown"}: {(a.IsPresent ? "Present ✅" : "Absent ❌")}");
            else sb.AppendLine("No records.");

            // Labor Salaries
            var salaries = await _db.LaborPayments.OrderByDescending(s => s.PaymentDate).Take(5).ToListAsync(cancellation);
            sb.AppendLine("\n💰 Recent Labor Salaries:");
            if (salaries.Any())
                foreach (var s in salaries)
                    sb.AppendLine($"{s.LaborName ?? "Unknown"}: ₹{s.NetAmount} (Week {s.PaymentDate:dd/MM}) Advances Deducted: ₹{s.AdvanceAdjusted}");
            else sb.AppendLine("No records.");

            // Labor Advances
            var advances = await _db.AdvancePayments.Include(a => a.Labor)
                .OrderByDescending(a => a.DateGiven).Take(5).ToListAsync(cancellation);
            sb.AppendLine("\n💵 Recent Labor Advances:");
            if (advances.Any())
                foreach (var adv in advances)
                    sb.AppendLine($"{adv.DateGiven:dd/MM/yyyy} - {adv.Labor?.Name ?? "Unknown"}: ₹{adv.Amount}");
            else sb.AppendLine("No records.");

            // Jaggery Production
            var produce = await _db.JaggeryProduces.OrderByDescending(p => p.ProducedDate).Take(5).ToListAsync(cancellation);
            sb.AppendLine("\n🏭 Recent Jaggery Production:");
            if (produce.Any())
                foreach (var p in produce)
                    sb.AppendLine($"{p.ProducedDate:dd/MM/yyyy} - {p.QuantityKg} Quintals (Batch: {p.BatchNumber})");
            else sb.AppendLine("No records.");

            // Jaggery Sales
            var sales = await _db.JaggerySales.OrderByDescending(s => s.SaleDate).Take(5).ToListAsync(cancellation);
            sb.AppendLine("\n📦 Recent Jaggery Sales:");
            if (sales.Any())
                foreach (var s in sales)
                    sb.AppendLine($"{s.SaleDate:dd/MM/yyyy} - {s.Dealer} bought {s.QuantityInKg} Quintals @ ₹{s.RatePerKg}/Qtl = ₹{s.TotalAmount}");
            else sb.AppendLine("No records.");

            // Cane Purchases
            var canePurchases = await _db.CanePurchases.OrderByDescending(c => c.PurchaseDate).Take(5).ToListAsync(cancellation);
            sb.AppendLine("\n🌾 Recent Cane Purchases:");
            if (canePurchases.Any())
                foreach (var c in canePurchases)
                    sb.AppendLine($"{c.PurchaseDate:dd/MM/yyyy} - Farmer {c.FarmerName}, {c.QuantityTon} Tons @ ₹{c.RatePerTon}/Ton = ₹{c.TotalAmount}");
            else sb.AppendLine("No records.");

            // Cane Advances
            var caneAdvances = await _db.CaneAdvances.OrderByDescending(c => c.AdvanceDate).Take(5).ToListAsync(cancellation);
            sb.AppendLine("\n💳 Recent Cane Advances:");
            if (caneAdvances.Any())
                foreach (var c in caneAdvances)
                    sb.AppendLine($"{c.AdvanceDate:dd/MM/yyyy} - Farmer {c.Farmer}: Advance ₹{c.Amount}");
            else sb.AppendLine("No records.");

            // Farmer Cane Split Payments
            var farmers = await _db.Farmers.Include(f => f.CanePurchases).ToListAsync(cancellation);
            sb.AppendLine("\n👨‍🌾 Recent Farmer Cane Payments (Split):");
            if (farmers.Any())
                foreach (var f in farmers)
                {
                    var lastPurchase = f.CanePurchases.OrderByDescending(c => c.PurchaseDate).FirstOrDefault();
                    if (lastPurchase != null)
                        sb.AppendLine($"{lastPurchase.PurchaseDate:dd/MM/yyyy} - Farmer {f.Name}, Paid: ₹{lastPurchase.TotalAmount}, Balance: ₹{lastPurchase.TotalAmountSnapshot}");
                }
            else sb.AppendLine("No records.");

            // Splitwise Expenses
            var expenses = await _db.SplitwisePayments.OrderByDescending(e => e.PaymentDate).Take(5).ToListAsync(cancellation);
            sb.AppendLine("\n💡 Recent Splitwise Expenses:");
            if (expenses.Any())
                foreach (var e in expenses)
                    sb.AppendLine($"{e.PaymentDate:dd/MM/yyyy} - {e.PaidBy}: ₹{e.Amount}");
            else sb.AppendLine("No records.");

            return sb.ToString();
        }
    }
}
