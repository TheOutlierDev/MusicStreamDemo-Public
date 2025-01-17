namespace MusicStreamDemo.WAS.Http
{
    public class HttpClientService
    {
        public string BaseAddress = "http://87.106.72.35:5000";

        public HttpClient Create()
        {
            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(BaseAddress);
            return httpClient;
        }
    }
}
