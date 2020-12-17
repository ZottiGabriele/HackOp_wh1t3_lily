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
    public string r_path;
    public string flags;
    public string user;
    public string group;
    public string type;
    public string content;
    public VirtualFileSystemEntry[] childs;

    public VirtualFileSystemEntry(bool hidden, bool readable, string name, string full_path, string r_path, string flags, string user, string group, string type, string content, VirtualFileSystemEntry[] childs)
    {
        this.hidden = hidden;
        this.readable = readable;
        this.name = name;
        this.full_path = full_path;
        this.r_path = r_path;
        this.flags = flags;
        this.user = user;
        this.group = group;
        this.type = type;
        this.content = content;
        this.childs = childs;
    }

    public VirtualFileSystemEntry Clone()
    {
        var output = new VirtualFileSystemEntry(
            hidden,
            readable,
            name,
            full_path,
            r_path,
            flags,
            user,
            group,
            type,
            content,
            childs
        );

        return output;
    }

    public void BuildVirtualFileSystem(ref Dictionary<string, VirtualFileSystemEntry> hashTable)
    {
        hashTable.Add(full_path, this);

        if (childs == null) return;

        foreach (var f in childs)
        {
            f.BuildVirtualFileSystem(ref hashTable);
        }
    }

    public int CompareTo(object obj)
    {
        var other = obj as VirtualFileSystemEntry;
        return name.CompareTo(other.name);
    }
}
