using System;
using System.Linq;
using System.Text;
using System.Web;
using System.Net;
using System.IO;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
namespace TopMai.Domain.Services.Payments;

public class Api
{
    public string apiKey = "Your Api Key";

    private string basePath = "https://api.trackingmore.com";

    private string apiVersion = "v3";

    private string trackHeader = "Tracking-Api-Key";

    private HttpClient httpClient;

    public Api(string apiKey)
    {
        this.apiKey = apiKey;
        this.httpClient = new HttpClient();
    }

    protected string getBaseUrl(string path)
    {
        return basePath + "/" + apiVersion + "/trackings/" + path;
    }

    public HttpResponseMessage doRequest(string path, string data = "", string method = "GET")
    {
        List<KeyValuePair<string, string>> headers = new List<KeyValuePair<string, string>>();
        headers.Add(new KeyValuePair<string, string>(this.trackHeader, this.apiKey));

        method = method.ToUpper();
        if (method == "GET")
        {
            return this.Get(this.getBaseUrl(path), headers);
        }
        else if (method == "Put")
        {
            return this.Put(this.getBaseUrl(path), data, headers);
        }
        else if (method == "Delete")
        {
            return this.Delete(this.getBaseUrl(path), data, headers);
        }
        else
        {
            return this.Post(this.getBaseUrl(path), data, headers);
        }
    }


    public HttpResponseMessage Put(string url, string content, List<KeyValuePair<string, string>> headers = null)
    {
        StringContent stringContent = new StringContent(content, Encoding.UTF8);
        if (headers != null && headers.Count > 0)
        {
            stringContent.Headers.Clear();
            foreach (var header in headers)
            {
                stringContent.Headers.Add(header.Key, header.Value);
            }
        }

        HttpResponseMessage response = httpClient.PutAsync(new Uri(url), stringContent).Result;
        return response;
    }
    public HttpResponseMessage Delete(string url, string content, List<KeyValuePair<string, string>> headers = null)
    {
        StringContent stringContent = new StringContent(content, Encoding.UTF8);
        if (headers != null && headers.Count > 0)
        {
            stringContent.Headers.Clear();
            foreach (var header in headers)
            {
                stringContent.Headers.Add(header.Key, header.Value);
            }
        }

        HttpResponseMessage response = httpClient.DeleteAsync(new Uri(url)).Result;
        return response;
    }

    public HttpResponseMessage Post(string url, string content, List<KeyValuePair<string, string>> headers = null)
    {
        StringContent stringContent = new StringContent(content, Encoding.UTF8);
        if (headers != null && headers.Count > 0)
        {
            stringContent.Headers.Clear();
            foreach (var header in headers)
            {
                stringContent.Headers.Add(header.Key, header.Value);
            }
        }

        HttpResponseMessage response = httpClient.PostAsync(new Uri(url), stringContent).Result;
        return response;
    }


    public HttpResponseMessage Get(string url, List<KeyValuePair<string, string>> headers = null)
    {
        HttpRequestMessage request = new HttpRequestMessage()
        {
            RequestUri = new Uri(url),
            Method = HttpMethod.Get,
        };
        if (headers != null && headers.Count > 0)
        {
            request.Headers.Clear();

            foreach (var header in headers)
            {
                request.Headers.Add(header.Key, header.Value);

            }
        }

        HttpResponseMessage response = httpClient.SendAsync(request).Result;
        return response;
    }
}