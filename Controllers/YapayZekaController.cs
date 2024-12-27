using Guzellik.Models;
using Newtonsoft.Json;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;  // IFormFile için gerekli namespace
using Microsoft.AspNetCore.Mvc;  // ASP.NET Core MVC kullanıyorsanız

namespace Guzellik.Controllers
{
    public class YapayZekaController : Controller
    {
        // API anahtarını Web.config veya appsettings.json'dan alabilirsiniz
        private string apiAnahtari = "YOUR_DALLE_API_KEY";  // API anahtarınızı buraya ekleyin

        public IActionResult AnaSayfa()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResmiIsle(IFormFile dosya)  // HttpPostedFileBase yerine IFormFile kullanıyoruz
        {
            if (dosya != null && dosya.Length > 0)
            {
                // Fotoğrafı byte dizisine dönüştür
                byte[] resimBaytlari;
                using (var memoryStream = new MemoryStream())
                {
                    await dosya.CopyToAsync(memoryStream);
                    resimBaytlari = memoryStream.ToArray();
                }

                // DALL-E API'ye gönder
                string sonuc = await ResmiOlustur(resimBaytlari);

                // JSON yanıtını işleyerek modeli oluştur
                var model = new YapayZeka
                {
                    ResimUrl = sonuc,
                    Oneriler = "Saç kesimi ve renk önerileri burada görünecek"
                };

                return View("Sonuc", model);
            }

            return View("AnaSayfa");
        }

        private async Task<string> ResmiOlustur(byte[] resimBaytlari)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiAnahtari}");

                using (MultipartFormDataContent form = new MultipartFormDataContent())
                {
                    ByteArrayContent byteContent = new ByteArrayContent(resimBaytlari);
                    byteContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");

                    form.Add(byteContent, "file", "foto.jpg");

                    HttpResponseMessage response = await client.PostAsync("https://api.openai.com/v1/images/generations", form);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseContent = await response.Content.ReadAsStringAsync();
                        dynamic json = JsonConvert.DeserializeObject(responseContent);
                        string resimUrl = json.data[0].url;  // DALL-E tarafından döndürülen URL
                        return resimUrl;
                    }
                    else
                    {
                        return "API çağrısı başarısız oldu!";
                    }
                }
            }
        }
    }
}
