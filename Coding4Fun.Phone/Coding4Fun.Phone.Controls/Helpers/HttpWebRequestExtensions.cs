using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Coding4Fun.Phone.Controls.Helpers
{
    public static class HttpWebRequestExtensions
    {
        public static Task<Stream> GetRequestStreamAsync(this HttpWebRequest request)
        {
            var taskComplete = new TaskCompletionSource<Stream>();

            request.BeginGetRequestStream(ar =>
            {
                Stream requestStream = request.EndGetRequestStream(ar);
                taskComplete.TrySetResult(requestStream);
            }, request);

            return taskComplete.Task;
        }

        public static Task<HttpWebResponse> GetResponseAsync(this HttpWebRequest request)
        {
            var taskComplete = new TaskCompletionSource<HttpWebResponse>();
            request.BeginGetResponse(asyncResponse =>
            {
                try
                {
                    HttpWebRequest responseRequest = (HttpWebRequest)asyncResponse.AsyncState;
                    HttpWebResponse someResponse = (HttpWebResponse)responseRequest.EndGetResponse(asyncResponse);
                    taskComplete.TrySetResult(someResponse);
                }
                catch (WebException webExc)
                {
                    HttpWebResponse failedResponse = (HttpWebResponse)webExc.Response;
                    taskComplete.TrySetResult(failedResponse);
                }
            }, request);

            return taskComplete.Task;
        }
    }
}
