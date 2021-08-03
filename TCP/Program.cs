using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace TCP
{
    class Program
    {
        static void Main(string[] args)
        {

			DBAccess db = DBAccess.GetInstance();

			/*------------------------------------
			  クライアントからデータを受信する準備をする
			 -------------------------------------*/
			// サーバが接続を待つエンドポイント
			// であり
			// クライアントが接続するサーバのエンドポイント
			var endpoint = new IPEndPoint(IPAddress.Loopback, 54321);

			// サーバ
			var server = new Server<Message, Message>(
				endpoint,
				// リクエストからレスポンスを作る処理
				request => new Message { Id = request.Id, Content = new string(request.Content.Reverse().ToArray()) }
				);

			// 接続を待機
			Task t = server.Listen();
			

			
			/*------------------------------------
			  クライアントからデータを送信する
			 -------------------------------------*/

			Person person = new Person { Id = 2, name = "test" };
			
			try
			{
				// クライアント
				Task.WaitAll(
					// リクエストを送信してレスポンスを受信
					new Client<Message, Message>(endpoint).Send(new Message { Id = 10, Content = "あいうえお", Person = person}),
					new Client<Message, Message>(endpoint).Send(new Message { Id = 20, Content = "かきくけこ", Person = person }),
					new Client<Message, Message>(endpoint).Send(new Message { Id = 10, Content = "あいうえお", Person = person }),
					new Client<Message, Message>(endpoint).Send(new Message { Id = 20, Content = "かきくけこ", Person = person }),
					new Client<Message, Message>(endpoint).Send(new Message { Id = 10, Content = "あいうえお", Person = person }),
					new Client<Message, Message>(endpoint).Send(new Message { Id = 20, Content = "かきくけこ", Person = person }),
					new Client<Message, Message>(endpoint).Send(new Message { Id = 10, Content = "あいうえお", Person = person }),
					new Client<Message, Message>(endpoint).Send(new Message { Id = 20, Content = "かきくけこ", Person = person }),
					new Client<Message, Message>(endpoint).Send(new Message { Id = 10, Content = "あいうえお", Person = person }),
					new Client<Message, Message>(endpoint).Send(new Message { Id = 20, Content = "かきくけこ", Person = person }),
					new Client<Message, Message>(endpoint).Send(new Message { Id = 10, Content = "あいうえお", Person = person }),
					new Client<Message, Message>(endpoint).Send(new Message { Id = 20, Content = "かきくけこ", Person = person }),
					new Client<Message, Message>(endpoint).Send(new Message { Id = 30, Content = "さしすせそ", Person = person })
				);
			}
			catch (Exception e)
            {
				Console.WriteLine(e);
            }

			// とりあえずメイン側は動かし続ける
			while (true)
			{
				Thread.Sleep(5000);
			}

			// サーバを終了
			server.Close();
			// サーバの終了処理、Taskの管理、エラー処理あたりが微妙

		}

		private Message CreateResponse(Message message)
        {
			return new Message();
        }
	}
}
