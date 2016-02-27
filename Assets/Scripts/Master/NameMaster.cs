using UnityEngine;
using System.Collections;

public class NameMasterTable : MasterTableBase<NameMaster>
{
	private static readonly string FilePath = "CSV/name";
	public void Load() { Load(FilePath); }
}

public class NameMaster : MasterBase
{
	public string Name { get; private set; }
	public string SE1 { get; private set; }
	public string SE2 { get; private set; }
	public string SE3 { get; private set; }
}