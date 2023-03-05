using System;
using System.IO;
using System.Net;

namespace DesktopApp_WPF.HttpClient
{
    public enum httpVerb
    {
        GET,
        POST,
        PUT,
        DELETE
    }

    class RestClient
    {
        public string endPoint { get; set; }
        public httpVerb httpMethod { get; set; }

        private string baseUrl { get; set; }

        public RestClient(string baseUrl)
        {
            endPoint = string.Empty;
            httpMethod = httpVerb.POST;
            this.baseUrl = baseUrl;

        }

        public string MakePostRequest(string url, string jsonBody)
        {
            ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, errors) => true;
            string result = "";

            try
            {
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    streamWriter.Write(jsonBody);
                    streamWriter.Flush();
                    streamWriter.Close();
                }

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    result = streamReader.ReadToEnd();
                }
            }
            catch (WebException ex)
            {

            }
            return result;
        }

        public string GetContainer(string endpoint, string jwtToken)
        {

            string strResponseValue = string.Empty;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(baseUrl + endpoint);
            request.Method = "GET";

            request.Headers.Add("Authorization", "Bearer " + jwtToken);
            HttpWebResponse response = null;
            try
            {
                response = (HttpWebResponse)request.GetResponse();
                using (Stream responseStream = response.GetResponseStream())
                {
                    if (responseStream != null)
                    {
                        using (StreamReader reader = new StreamReader(responseStream))
                        {
                            strResponseValue = reader.ReadToEnd();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                strResponseValue = "{\"errorMessages\":[\"" + ex.Message.ToString() + "\"],\"errors\":{}}";
            }
            finally
            {
                if (response != null)
                {
                    ((IDisposable)response).Dispose();
                }
            }

            return strResponseValue;
        }

        public string getClient(string endpoint, string jwtToken)
        {
            string strResponseValue = string.Empty;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(endpoint);
            request.Method = "GET";
            request.Headers.Add("Authorization", "Bearer " + jwtToken);
            HttpWebResponse response = null;
            try
            {
                response = (HttpWebResponse)request.GetResponse();
                using (Stream responseStream = response.GetResponseStream())
                {
                    if (responseStream != null)
                    {
                        using (StreamReader reader = new StreamReader(responseStream))
                        {
                            strResponseValue = reader.ReadToEnd();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                strResponseValue = "{\"errorMessages\":[\"" + ex.Message.ToString() + "\"],\"errors\":{}}";
            }
            finally
            {
                if (response != null)
                {
                    ((IDisposable)response).Dispose();
                }
            }


            return strResponseValue;
        }

        public void EditClient(string endpoint, string jsonBody, string jwtToken)
        {
            string strResponseValue = string.Empty;
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(endpoint);
            httpWebRequest.Headers.Add("Authorization", "Bearer " + jwtToken);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "PATCH";
            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                streamWriter.Write(jsonBody);
                streamWriter.Flush();
                streamWriter.Close();
            }
            try
            {
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    strResponseValue = streamReader.ReadToEnd();
                }
            }
            catch (WebException ex)
            {
                // Gérer les erreurs ici
            }
        }

        public void DeleteClient(string endpoint, string jwtToken)
        {
            string strResponseValue = string.Empty;
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(endpoint);
            httpWebRequest.Headers.Add("Authorization", "Bearer " + jwtToken);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "DELETE";

            try
            {
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    strResponseValue = streamReader.ReadToEnd();
                }
            }
            catch (WebException ex)
            {
                // Gérer les erreurs ici
            }
        }

        public string getPackageStat(string url, string namepackage)
        {

            string result = "";

            try
            {
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(url + namepackage);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "GET";


                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    result = streamReader.ReadToEnd();
                }
            }
            catch (WebException ex)
            {

            }
            return result;
        }



    }
}
