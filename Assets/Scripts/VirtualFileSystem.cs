using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class VirtualFileSystem
{
    public string type;
    public string name;
    public VirtualFileSystemEntry[] contents;

    public static VirtualFileSystem CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<VirtualFileSystem>(jsonString);
    }
}

[System.Serializable]
public class VirtualFileSystemEntry
{
    public string type;
    public string name;
    public string mode;
    public string prot;
    public string user;
    public string group;
    public string size;
    public VirtualFileSystemEntry[] contents;
}
