using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Windows.Forms;


namespace FindShoppingBoot.Controllers
{
    public class ProxyController : ApiController
    {
        ManualResetEvent oSignalEvent = new ManualResetEvent(false);
        string  html = string.Empty;
        string URL;
        Thread t;
        int i = 0;

        public async Task<HttpResponseMessage> Get(string URL)
        {
            this.URL = URL;
            /*var t = new System.Threading.Thread(BrowserThread);
            t.SetApartmentState(System.Threading.ApartmentState.STA);
            t.Start();
             //t.Join();
            oSignalEvent.WaitOne();
            oSignalEvent.Reset();*/
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, "");
            response.Content = new StringContent(html, Encoding.Unicode);
            return response;
        }

        private  void BrowserThread()
        {
            var wb = new WebBrowser();
            wb.Navigate(URL);
            wb.DocumentCompleted += (sender, args) =>
            {
                html = string.Format("<html>{0}</html>",wb.Document.Body.InnerHtml);
                if (i == 1)
                {
                    oSignalEvent.Set();
                    //Application.ExitThread();
                }
                i++;
            };
            Application.Run();
        }
    }
}
