using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class VirtualFileSystemEntry : IComparable
{
    public bool hidden;
    public bool readable;
    public string name;
    public string full_path;
    public string flags;
    public string user;
    public string group;
    public string type;
    public string content;
    public VirtualFileSystemEntry[] childs;

    public VirtualFileSystemEntry Clone()
    {
        var output = new VirtualFileSystemEntry();

        output.hidden = hidden;
        output.readable = readable;
        output.name = name;
        output.full_path = full_path;
        output.flags = flags;
        output.user = user;
        output.group = group;
        output.type = type;
        output.content = content;
        output.childs = childs;

        return output;
    }

    public void BuildHashTable(ref Dictionary<string, VirtualFileSystemEntry> hashTable) {
        hashTable.Add(full_path, this);
        
        if(childs == null) return;
        
        foreach(var f in childs) {
            f.BuildHashTable(ref hashTable);
        }
    }

    public int CompareTo(object obj)
    {
        var other = obj as VirtualFileSystemEntry;
        return name.CompareTo(other.name);
    }
}
