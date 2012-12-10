using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace Coding4Fun.Toolkit.Net
{
	public static class HttpWebRequestExtensions
	{
		public static Task<Stream> GetRequestStreamAsync(this HttpWebRequest request)
		{
			var taskComplete = new TaskCompletionSource<Stream>();

			request.BeginGetRequestStream(ar =>
			{
				var requestStream = request.EndGetRequestStream(ar);
				taskComplete.TrySetResult(requestStream);
			}, request);

			return taskComplete.Task;
		}

		public static Task<WebResponse> GetResponseAsync(this HttpWebRequest request)
		{
			var taskComplete = new TaskCompletionSource<WebResponse>();
			request.BeginGetResponse(asyncResponse =>
			{
				try
				{
					var responseRequest = (HttpWebRequest)asyncResponse.AsyncState;
					var someResponse = responseRequest.EndGetResponse(asyncResponse);

					taskComplete.TrySetResult(someResponse);
				}
				catch (WebException webExc)
				{
					var failedResponse = webExc.Response;
					taskComplete.TrySetResult(failedResponse);
				}
			}, request);

			return taskComplete.Task;
		}
	}
}
