using Newtonsoft.Json;
using MauiAppPrevisaodoTempo.Data;
using PrevisaoDoTempoApp.Models;
using System.Net.Http;
using System.Threading.Tasks;
using System;
using System.Diagnostics;
using Microsoft.Maui.Controls;

namespace MauiAppPrevisaodoTempo
{
    public partial class PrevisaoPage : ContentPage
    {
        private readonly AppDbContext _dbContext;

        public PrevisaoPage()
        {
            InitializeComponent();
            _dbContext = new AppDbContext();
            _dbContext.Database.EnsureCreated();
        }

        private async void OnBuscarClicked(object sender, EventArgs e)
        {
            var cidade = CidadeEntry.Text;
            var data = DataPicker.Date;

            var previsao = await ObterPrevisaoDoTempo(cidade);
            if (previsao != null)
            {
                previsao.Data = data;
                await _dbContext.PrevisaoTempos.AddAsync(previsao);
                await _dbContext.SaveChangesAsync();

                ResultadoLabel.Text = $"Cidade: {previsao.Cidade}\nTemperatura: {previsao.Temperatura}°C\nDescrição: {previsao.Descricao}";
            }
            else
            {
                ResultadoLabel.Text = "Cidade não encontrada.";
            }
        }

        private async Task<PrevisaoTempo> ObterPrevisaoDoTempo(string cidade)
        {
            string appId = "6135072afe7f6cec1537d5cb08a5a1a2"; // Substitua pela sua chave da API
            string url = $"https://api.openweathermap.org/data/2.5/weather?q={cidade}&units=metric&appid={appId}";

            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    dynamic data = JsonConvert.DeserializeObject(json);

                    return new PrevisaoTempo
                    {
                        Cidade = data.name,
                        Temperatura = data.main.temp,
                        Descricao = data.weather[0].description
                    };
                }
            }
            return null;
        }
    }
}
