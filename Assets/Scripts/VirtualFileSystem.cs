using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class VirtualFileSystem
{
    public VirtualFileSystemEntry contents;
    public VirtualFileSystemEntry ActiveEntry;
    public VirtualFileSystemEntry HomeEntry;
    private Dictionary<string, VirtualFileSystemEntry> _hashTable = new Dictionary<string, VirtualFileSystemEntry>();

    public static VirtualFileSystem CreateFromJson (string jsonString)
    {
        var output = JsonUtility.FromJson<VirtualFileSystem>(jsonString);

        output.contents.BuildHashTable(ref output._hashTable);

        output.ActiveEntry = output.Query(TerminalHandler.Instance.TerminalConfig.CurrentPath.TrimEnd('/'));
        output.HomeEntry = output.Query(TerminalHandler.Instance.TerminalConfig.HomePath.TrimEnd('/'));

        return output;
    }

    public VirtualFileSystemEntry Query(string path) {

        VirtualFileSystemEntry output = null;

        //Make the path absolute if relative
        if(path[0] != '/') path = TerminalHandler.Instance.TerminalConfig.CurrentPath + path;

        var path_sections = path.Split(new[]{'/'}, System.StringSplitOptions.RemoveEmptyEntries);
        List<string> final_path = new List<string>();

        foreach(var s in path_sections) {
            if(final_path.Count > 0 && s == "..") {
                final_path.RemoveAt(final_path.Count - 1);
            } else if(s == "..") {
                return null;
            } else {
                final_path.Add(s);
            }
        }
        
        path = "/";
        foreach(var s in final_path) {
            path += s + "/";
        }

        _hashTable.TryGetValue(path, out output);
        return output;
    }
}

[System.Serializable]
public class VirtualFileSystemEntry
{
    public bool hidden;
    public string name;
    public string full_path;
    public string r_full_path;
    public string flags;
    public string user;
    public string group;
    public string type;
    public VirtualFileSystemEntry[] contents;

    public void BuildHashTable(ref Dictionary<string, VirtualFileSystemEntry> hashTable) {
        hashTable.Add(full_path, this);
        
        if(contents == null) return;
        
        foreach(var f in contents) {
            f.BuildHashTable(ref hashTable);
        }
    }
}
