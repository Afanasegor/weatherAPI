using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace WebRequestWeather
{
    class Program
    {
        static void Main(string[] args)
        {
            double lat = 55.7185054;
            double lon = 52.3721038;
            byte cnt = 5;
            string units = "metric";
            string APIKey = "d5fbd76510300ee10069e359f755a2ae";

            double averageTemp;
            double maxTemp;

            string url = "http://" + $"api.openweathermap.org/data/2.5/onecall?lat={lat}&lon={lon}&units={units}&exclude=current,minutely,hourly,alerts&appid={APIKey}";
            
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();

            string response;

            using (StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream()))
            {
                response = streamReader.ReadToEnd();
            }

            WeatherInfo weather = JsonConvert.DeserializeObject<WeatherInfo>(response);

            averageTemp = FindAverageTemp(weather, cnt);
            maxTemp = FindMaxTemp(weather, cnt);

            Console.WriteLine($"Средняя утренняя температура за предстоящие {cnt} дней (включая сегодняшний): {averageTemp}.");
            Console.WriteLine($"Максимаильная утренняя температура за предстоящие {cnt} дней (включая сегодняшний): {maxTemp}.");

            // Delay
            Console.ReadKey();
        }

        // Возвращает максимальную утреннюю температуру за указанный период
        public static double FindMaxTemp(WeatherInfo weather, int cnt)
        {
            double maxTemp = weather.daily[0].temp.morn;
            for (int i = 1; i < cnt; i++)
            {
                if (maxTemp < weather.daily[i].temp.morn)
                {
                    maxTemp = weather.daily[i].temp.morn;
                }
            }
            maxTemp = Math.Round(maxTemp, 2);
            return maxTemp;
        }

        // Возвращает среднюю утреннюю температуру за указанный период
        public static double FindAverageTemp(WeatherInfo weather, int cnt)
        {
            double averageTemp;
            double sum = 0;
            for (int i = 0; i < cnt; i++)
            {
                sum += weather.daily[i].temp.morn;
            }
            averageTemp = Math.Round(sum / cnt, 2);
            return averageTemp;
        }
    }
}
