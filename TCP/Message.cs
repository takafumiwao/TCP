using System;

namespace TCP
{
	// メッセージ
	[Serializable]
	public class Message
	{
		public int Id { get; set; }
		public string Content { get; set; }

		public Person Person { get; set; }

		public override string ToString()
		{
			return $@"{{ {nameof(Id)} = {Id}, {nameof(Content)} = ""{Content}"" }}";
		}
	}
}
