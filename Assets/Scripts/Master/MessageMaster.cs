using UnityEngine;
using System.Collections;

public class MessageMasterTable : MasterTableBase<MessageMaster>
{
	private static readonly string FilePath = "CSV/message";
	public void Load() { Load(FilePath); }
}

public class MessageMaster : MasterBase
{
	public string Message { get; private set; }
	public string SEName { get; private set; }
	public int Type { get; private set; }
	public int Delay { get; private set; }
	public int Timing { get; private set; }
}