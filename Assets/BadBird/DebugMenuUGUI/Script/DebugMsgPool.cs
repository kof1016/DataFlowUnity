using System.Collections.Generic;

namespace BadBird.DebugMenuUGUI
{
	public class DebugMsgPool
	{
//		public const int PoolCap = 250;
		public const int LogPoolCap = 100;
		public const int WarningPoolCap = 150;
		public const int ErrorPoolCap = 150;
		public const int OtherPoolCap = 100;

		public static List<string> LogPool = new List<string>(LogPoolCap);
		public static List<string> WarningPool = new List<string>(WarningPoolCap);
		public static List<string> ErrorPool = new List<string>(ErrorPoolCap);
		public static List<string> OtherPool = new List<string>(OtherPoolCap);

		public static void AddLog(string msg)
		{
			if(LogPool.Count >= LogPoolCap)
				LogPool.RemoveAt(0);
			LogPool.Add(msg);
		}

		public static void AddWarning(string msg)
		{
			if(WarningPool.Count >= WarningPoolCap)
				WarningPool.RemoveAt(0);
			WarningPool.Add(msg);
		}

		public static void AddError(string msg)
		{
			if(ErrorPool.Count >= ErrorPoolCap)
				ErrorPool.RemoveAt(0);
			ErrorPool.Add(msg);
		}

		public static void AddOther(string msg)
		{
			if(OtherPool.Count >= OtherPoolCap)
				OtherPool.RemoveAt(0);
			OtherPool.Add(msg);
		}

		public static void Clear()
		{
			LogPool.Clear();
			WarningPool.Clear();
			ErrorPool.Clear();
			OtherPool.Clear();
		}
	}
}