using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class VirtualFileSystem
{
    public string type;
    public string name;
    public VirtualFileSystemEntry[] contents;
    public VirtualFileSystemEntry ActiveEntry;
    public VirtualFileSystemEntry HomeEntry;
    private Dictionary<string, VirtualFileSystemEntry> _hashTable = new Dictionary<string, VirtualFileSystemEntry>();

    public static VirtualFileSystem CreateFromJson (string jsonString)
    {
        var output = JsonUtility.FromJson<VirtualFileSystem>(jsonString);

        output.buildHashTable();

        output.ActiveEntry = output.Query(TerminalHandler.Instance.TerminalConfig.CurrentPath.TrimEnd('/'));
        output.HomeEntry = output.Query(TerminalHandler.Instance.TerminalConfig.HomePath.TrimEnd('/'));

        return output;
    }

    private void buildHashTable() {

        //TODO: probably better to have it in the source JSON
        var root = new VirtualFileSystemEntry("directory", name, "drwxr-xr-x", "", "user", "group", "4096", contents);
        _hashTable.Add(name, root);

        if(contents == null) return;

        foreach(var f in contents) {
            f.BuildHashTable(ref _hashTable);
        }
    }

    public VirtualFileSystemEntry Query(string path) {

        VirtualFileSystemEntry output = null;
        char[] separator = {'/'};

        //Make the path absolute if relative
        if(path[0] != '/') path = TerminalHandler.Instance.TerminalConfig.CurrentPath + path;

        var path_sections = path.Split(separator, System.StringSplitOptions.RemoveEmptyEntries);
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

        _hashTable.TryGetValue("." + path.TrimEnd('/'), out output);
        return output;
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

    public VirtualFileSystemEntry(string type, string name, string mode, string prot, string user, string group, string size, VirtualFileSystemEntry[] contents) {
        this.type = type;
        this.name = name;
        this.mode = mode;
        this.prot = prot;
        this.user = user;
        this.group = group;
        this.size = size;
        this.contents = contents;
    }

    public void BuildHashTable(ref Dictionary<string, VirtualFileSystemEntry> hashTable) {
        hashTable.Add(name, this);
        
        if(contents == null) return;
        
        foreach(var f in contents) {
            f.BuildHashTable(ref hashTable);
        }
    }
}
