using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace TCP
{
    public class DBAccess
    {
        private static DBAccess db;


        public static DBAccess GetInstance()
        {
            if (db == null)
            {
                return new DBAccess();
            }
            else
            {
                return db;
            }
        }

        // メッセージを貯めるDataTable
        private DataTable Message { get; set; }

        // 接続文字列
        private string connectionString { get; set; }

        // コネクション
        private SqlConnection Connection { get; set; }

        // 初期化処理
        private DBAccess()
        {          
            // 接続文字列
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            // 接続先インスタンス
            builder.DataSource = "localhost";
            // ユーザー名
            builder.UserID = "sa";
            // パスワード
            builder.Password = "yourStrong(!)Password";
            // 接続するデータベース名
            builder.InitialCatalog = "testdb";
            
            // 接続準備
            Connection = new SqlConnection(builder.ConnectionString);

            Message = new DataTable("Message");
            Message.Columns.Add("Id");
            Message.Columns.Add("Content");
        }


        // メモリ領域にデータを追加する
        public void Add(Message message)
        {
            DataRow row = Message.NewRow();
            row["Id"] = message.Id;
            row["Content"] = message.Content;

            Message.Rows.Add(row);
        }


        // DBにデータを登録する
        public void Insert(object sender, ElapsedEventArgs e)
        {
            if (Message.Rows.Count > 0)
            {
                DataTable insertTable = Message.Copy();
                Message.Clear();

                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connectionString))
                {
                    bulkCopy.BulkCopyTimeout = 600; // in seconds
                    bulkCopy.DestinationTableName = "testdb";
                    bulkCopy.WriteToServer(Message);
                }
            }

        }
    }
}
