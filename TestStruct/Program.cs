using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace TestStruct
{

	[Serializable]
	public struct Message
	{
		public int number;
		public double realnumber;
		public string somechars;
	}


	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Hello World!");

			Message msg = new Message()
			{
				number = 2,
				realnumber = 3.14,
				somechars = "pityiripalkopityiripalkopityiripalkopityiripalkopityiripalkopityiripalko"
			};

			IFormatter formatter = new BinaryFormatter();
			MemoryStream stream = new MemoryStream();
			string tosendSTR = "";
			try
			{				
				formatter.Serialize(stream, msg);
				tosendSTR = Convert.ToBase64String(stream.ToArray());
			}
			finally
			{
				stream.Close();
			}

			// kuld es fogad 

			string fogadott = tosendSTR; // Receive

			BinaryFormatter bf = new BinaryFormatter();
			byte[] bytes = Convert.FromBase64String(fogadott);
			MemoryStream ms = new MemoryStream(bytes);

			Message receivedMsg;
			try
			{
				receivedMsg = (Message)bf.Deserialize(ms);
			}
			finally
			{
				ms.Close();
			}
		}
	}
}
