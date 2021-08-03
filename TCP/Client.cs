using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TCP
{
	// クライアント
	public class Client<TRequest, TResponse>
	{
		// 接続先のエンドポイント
		private readonly IPEndPoint _endpoint;

		public Client(IPEndPoint endpoint)
		{
			_endpoint = endpoint;
		}

		// サーバにリクエストを送信してレスポンスを受信する
		public async Task<TResponse> Send(TRequest request)
		{
			using (var client = new TcpClient())
			{
				// 1. サーバに接続
				await client.ConnectAsync(_endpoint.Address, _endpoint.Port);

				using (var stream = client.GetStream())
				{
					// 2. サーバにリクエストを送信する
					Console.WriteLine($"Client send: {request}");
					stream.WriteObject(request);

					// 3. サーバからレスポンスを受信する
					var response = stream.ReadObject<TResponse>();
					Console.WriteLine($"Client received: {response}");

					return response;
				}
			}
		}
	}
}
