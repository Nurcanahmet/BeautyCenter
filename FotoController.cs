using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Text.Json;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text;

namespace Guzellik.Controllers
{
    public class FotoController : Controller
    {
        private readonly string _openAIApiAnahtari = "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"; // Buraya OpenAI API anahtarınızı ekleyin.

        // Fotoğraf yükleme sayfasını görüntüle
        public IActionResult Yukle()
        {
            return View();
        }

        // Fotoğraf yükleme ve öneri alma
        [HttpPost]
        public async Task<IActionResult> Olustur(IFormFile foto)
        {
            if (foto == null || foto.Length == 0)
            {
                ViewBag.Hata = "Lütfen geçerli bir fotoğraf yükleyin.";
                return View("Yukle");
            }

            // Dosya yolu
            var dosyaYolu = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "foto", foto.FileName);

            // Fotoğrafı kaydet
            using (var stream = new FileStream(dosyaYolu, FileMode.Create))
            {
                await foto.CopyToAsync(stream);
            }

            // DALL-E API'si ile saç modeli önerisi oluştur
            string olusturulanGorselUrl;
            try
            {
                olusturulanGorselUrl = await SacModeliOnerisiOlustur(dosyaYolu);
            }
            catch (Exception ex)
            {
                ViewBag.Hata = ex.Message; // Hata mesajını buraya atıyoruz
                return View("Yukle");
            }

            // Eğer öneri başarılı bir şekilde alındıysa
            if (string.IsNullOrEmpty(olusturulanGorselUrl))
            {
                ViewBag.Hata = "Öneri alınamadı. Lütfen tekrar deneyin.";
                return View("Yukle");
            }

            ViewBag.OlusturulanGorselUrl = olusturulanGorselUrl;
            return View("Sonuc"); // Sonuç sayfasına yönlendir
        }


        private async Task<string> SacModeliOnerisiOlustur(string dosyaYolu)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_openAIApiAnahtari}");

                // API isteği için gerekli parametreler
                var istekIcerigi = new
                {
                    model = "dall-e-3", // Model adını buraya ekleyin
                    prompt = "Bu kişiye uygun saç modeli ve saç rengi önerileri oluştur.",
                    size = "1024x1024"
                };

                var jsonIcerik = JsonSerializer.Serialize(istekIcerigi);
                var httpIcerik = new StringContent(jsonIcerik, Encoding.UTF8, "application/json");

                var cevap = await httpClient.PostAsync("https://api.openai.com/v1/images/generations", httpIcerik);

                if (cevap.IsSuccessStatusCode)
                {
                    var cevapVerisi = await cevap.Content.ReadAsStringAsync();
                    var jsonDokuman = JsonDocument.Parse(cevapVerisi);
                    var olusturulanGorselUrl = jsonDokuman.RootElement.GetProperty("data")[0].GetProperty("url").GetString();
                    return olusturulanGorselUrl;
                }
                else
                {
                    var hataMesaji = await cevap.Content.ReadAsStringAsync();
                    throw new Exception($"DALL-E API çağrısı başarısız oldu: {hataMesaji}");
                }
            }
        }

    }
}
