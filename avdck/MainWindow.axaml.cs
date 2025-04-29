using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using System;

using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using  System.Net.Http.Formatting;

using Docker.DotNet;
using Docker.DotNet.Models;

namespace avdck
{
    public partial class MainWindow : Window
    {
        readonly TextBox? m_tb_Log;

        //private const string URL = "https://sub.domain.com/objects.json";
        private const string URL = "http://msk-n9e-psu4.ntc.ntcees.ru:8080/";
        
        //private string urlParameters = "?api_key=123";
        //private string urlParameters = "/api2/RastrData/TableGetSize?tname=node";
        private string urlParameters = "/api2/Login?name=nsdsd&password=asdsad";
        
        

        public MainWindow() {
            InitializeComponent();

            m_tb_Log = this.FindControl<TextBox>("Log1");
        }

        public void Log(string str_msg)
        { 
            if(m_tb_Log!=null)
            {
                str_msg = str_msg.Insert(0, DateTime.Now.ToString("HH.mm.ss: "));
                m_tb_Log.Text += str_msg + "\r\n";
            }
        }

        public void on_btn_click_send(object sender, RoutedEventArgs e)
        { 
        }

        public void on_btn_click_clear(object sender, RoutedEventArgs e)
        { 
        }

        public async void on_btn_click_tst_docker(object sender, RoutedEventArgs e){
            HttpClient? client = null;
            try
            { 
                Log("1");
                client = new HttpClient();
                Log("2");
                client.BaseAddress = new Uri(URL);
                Log("3");


                                    DockerClient docClient = new DockerClientConfiguration(
                        new Uri("http://msk-n9e-psu4.ntc.ntcees.ru:2375"))
                        .CreateClient();

                    var sd= docClient.Containers;
                    //var sds = sd.ListContainersAsync();

                    using (var client22 = new DockerClientConfiguration(new Uri("http://msk-n9e-psu4.ntc.ntcees.ru:2375")).CreateClient())
                    {
                        var parameters = new ContainersListParameters
                        {
                            Filters = new Dictionary<string, IDictionary<string, bool>>
                            {
                                {
                                    "status",
                                    new Dictionary<string, bool>
                                    {
                                        { "running", true}
                                    }
                                }

                            }
                        };
                        Log("befor");
                        var containers = await client22.Containers.ListContainersAsync(parameters);
                        Log("after");
                        foreach (var container in containers)
                        {                          
                            Console.WriteLine(container.ID);      
                            Log(container.ID.ToString());                 
                        }
                    }
                // Add an Accept header for JSON format.
                 //client.DefaultRequestHeaders.Accept.Add(         new MediaTypeWithQualityHeaderValue("application/json"));
                // List data response.
                HttpResponseMessage response = client.GetAsync(urlParameters).Result;  // Blocking call! Program will wait here until a response is received or a timeout occurs.
                Log("4");
                if (response.IsSuccessStatusCode)
                {
                    Log("5");

                    //DockerClient client1 = new DockerClientConfiguration(
                    //    new Uri("http://ubuntu-docker.cloudapp.net:4243"))
                    //    .CreateClient();
                    // Parse the response body.
                    var a = response.Content.Headers;
                    var b = response.Headers;
                    var c = response.RequestMessage;
                    var d6 = response.Version.ToString();
                    var e1 = response.TrailingHeaders;
                    var es = response.Content.ReadAsStream();
                    var dataObjects = response.Content.ReadAsAsync<IEnumerable<DataObject>>().Result;  //Make sure to add a reference to System.Net.Http.Formatting.dll
                    //var dataObjects = response.Content.R ReadAsAsync<IEnumerable<DataObject>>().Result;  //Make sure to add a reference to System.Net.Http.Formatting.dll
                    foreach (var d in dataObjects)
                    {
                        //d.GetText
                        //Console.WriteLine("{0}", d.Name);
                        Console.WriteLine( "{0}", d.GetText() );
                    }
                }
                else
                {
                    Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                    Log("connetcton errro");
                }
                // Make any other calls using HttpClient here.
            } 
            catch (Exception ex) 
            {
                Log(ex.ToString());
            }
            finally
            { 
                 // Dispose once all HttpClient calls are complete. This is not necessary if the containing object will be disposed of; for example in this case the HttpClient instance will be disposed automatically when the application terminates so the following call is superfluous.
                client?.Dispose();
            }
        }
    }
}