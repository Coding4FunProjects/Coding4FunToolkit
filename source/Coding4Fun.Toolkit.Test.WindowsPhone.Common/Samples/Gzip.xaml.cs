using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Windows;

using Coding4Fun.Toolkit.Net;

using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;

namespace Coding4Fun.Toolkit.Test.WindowsPhone.Samples
{
	public partial class Gzip : PhoneApplicationPage
	{
		public ObservableCollection<Measurement> BeginGetResponse { get; private set; }
		public ObservableCollection<Measurement> BeginGetCompressedResponse { get; private set; }
		public ObservableCollection<Measurement> WebClient { get; private set; }
		public ObservableCollection<Measurement> GzipWebClient { get; private set; }
		
		private readonly Uri _uri = new Uri("http://microsoft.com/");
		private readonly Stopwatch _stopwatch = new Stopwatch();
		private readonly byte[] _buffer = new byte[4096];
		private int _scenario = -1;
		bool _stop = false;

		public Gzip()
		{
			InitializeComponent();

			BeginGetResponse = new ObservableCollection<Measurement>();
			BeginGetCompressedResponse = new ObservableCollection<Measurement>();
			WebClient = new ObservableCollection<Measurement>();
			GzipWebClient = new ObservableCollection<Measurement>();
			DataContext = this;

			StartNextScenario();
		}

		private void StartNextScenario()
		{
			if (_stop)
				return;

			_stopwatch.Reset();
			_stopwatch.Start();
			HttpWebRequest request = null;
			WebClient client = null;

			switch (_scenario)
			{
				case -1:
				case 0:
					request = WebRequest.CreateHttp(_uri);
					request.BeginGetResponse(ContinueScenario, request);
					break;
				case 1:
					request = WebRequestCreator.Gzip.Create(_uri) as HttpWebRequest;
					request.BeginGetResponse(ContinueScenario, request);
					break;
				case 2:
					client = new WebClient();
					break;
				case 3:
					client = new GzipWebClient();
					break;
				default:
					Debug.Assert(false);
					break;
			}

			if (null != client)
			{
				client.OpenReadCompleted += ContinueScenario;
				client.OpenReadAsync(_uri);
			}
		}

		private void ContinueScenario(IAsyncResult result)
		{
			var request = (HttpWebRequest)result.AsyncState;
			try
			{
				var response = (HttpWebResponse)request.EndGetResponse(result);
				switch (_scenario)
				{
					case -1:
					case 0:
					case 1:
						ReadToEndOfStreamAndContinue(response.GetResponseStream());
						break;
					default:
						Debug.Assert(false);
						break;
				}
			}
			catch (WebException)
			{
				StartNextScenario();
			}
		}

		private void ContinueScenario(object sender, OpenReadCompletedEventArgs e)
		{
			ReadToEndOfStreamAndContinue(e.Result);
		}

		private void ReadToEndOfStreamAndContinue(Stream responseStream)
		{
			using (responseStream)
			{
				while (0 != responseStream.Read(_buffer, 0, _buffer.Length))
				{
					// Keep reading...
				}
			}
			_stopwatch.Stop();

			Dispatcher.BeginInvoke(FinishScenario);
		}

		private void FinishScenario()
		{
			var measurement = new Measurement(DateTime.UtcNow.Ticks, _stopwatch.ElapsedMilliseconds);
			switch (_scenario)
			{
				case -1:
					// Don't record the first measurement; it's always very high
					_scenario++;
					break;
				case 0:
					BeginGetResponse.Add(measurement);
					_scenario++;
					break;
				case 1:
					BeginGetCompressedResponse.Add(measurement);
					_scenario++;
					break;
				case 2:
					WebClient.Add(measurement);
					_scenario++;
					break;
				case 3:
					GzipWebClient.Add(measurement);
					_scenario = 0;
					break;
				default:
					Debug.Assert(false);
					break;
			}

			StartNextScenario();
		}

		private void EmailResult(object sender, RoutedEventArgs e)
		{
			var sb = new StringBuilder();
			foreach (var name in new[] { "BeginGetResponse", "BeginGetCompressedResponse", "WebClient", "GzipWebClient" })
			{
				sb.AppendLine(name);

				var property = GetType().GetProperty(name);
				var measurements = (IEnumerable<Measurement>)property.GetValue(this, null);

				foreach (var measurement in measurements)
				{
					sb.AppendLine(measurement.Elapsed.ToString(CultureInfo.InvariantCulture));
				}

				sb.AppendLine();
			}

			new EmailComposeTask()
			{
				Subject = "GzipDemo Data",
				Body = sb.ToString(),
			}.Show();
		}

		private void ChartTap(object sender, System.Windows.Input.GestureEventArgs e)
		{
			_stop = true;
		}
	}

	public class Measurement
	{
		public long Time { get; private set; }
		public long Elapsed { get; private set; }

		public Measurement(long time, long elapsed)
		{
			Time = time;
			Elapsed = elapsed;
		}
	}
}