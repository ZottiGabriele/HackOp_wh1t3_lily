using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class VirtualFileSystem
{
    public VirtualFileSystemEntry childs;
    public VirtualFileSystemEntry ActiveEntry { get => Query(TerminalHandler.Instance.TerminalConfig.CurrentPath); }
    public VirtualFileSystemEntry HomeEntry { get => Query(TerminalHandler.Instance.TerminalConfig.HomePath); }
    public List<VirtualFileSystemEntry> AvailableCommands { get => getAvailableCommands(); }
    private Dictionary<string, VirtualFileSystemEntry> _hashTable = new Dictionary<string, VirtualFileSystemEntry>();

    public static VirtualFileSystem CreateFromJson(string jsonString)
    {
        var output = JsonUtility.FromJson<VirtualFileSystem>(jsonString);

        output.childs.BuildVirtualFileSystem(ref output._hashTable);

        return output;
    }

    public List<VirtualFileSystemEntry> getAvailableCommands()
    {
        var output = new List<VirtualFileSystemEntry>();
        var targets = TerminalHandler.Instance.TerminalConfig.TryGetEnvVar("$PATH").Split(':');

        foreach (var t in targets)
        {
            var t_query = Query(t);
            if (t_query != null && t_query.type == "directory")
            {
                foreach (var c in t_query.childs)
                {
                    if (c.type == "cmd" && !output.Contains(c))
                    {
                        output.Add(c);
                    }
                }
            }
        }

        return output;
    }

    public VirtualFileSystemEntry Query(string path)
    {
        VirtualFileSystemEntry output = null;

        path = GetFinalPath(path);
        _hashTable.TryGetValue(path, out output);

        return output;
    }

    public string GetFinalPath(string path)
    {
        if (path == "") return "";

        if (path[0] != '/') path = TerminalHandler.Instance.TerminalConfig.CurrentPath + path;

        var path_sections = path.Split(new[] { '/' }, System.StringSplitOptions.RemoveEmptyEntries);
        List<string> final_path = new List<string>();

        foreach (var s in path_sections)
        {
            if (final_path.Count > 0 && s == "..")
            {
                final_path.RemoveAt(final_path.Count - 1);
            }
            else if (s == "..")
            {
                return "/";
            }
            else
            {
                final_path.Add(s);
            }
        }

        path = "/";
        foreach (var s in final_path)
        {
            path += s + "/";
        }
        if (path != "/") path = path.Remove(path.LastIndexOf('/'));

        return path;
    }

    public VirtualFileSystemEntry CopyEntry(VirtualFileSystemEntry source, string destination)
    {

        var parent_path = destination.Remove(destination.LastIndexOf('/'));
        var parent = Query(parent_path);
        var parent_childs = new List<VirtualFileSystemEntry>(parent.childs);
        VirtualFileSystemEntry target = null;

        if (_hashTable.TryGetValue(destination, out target))
        {
            _hashTable.Remove(destination);
            parent_childs.Remove(target);
        }

        var copy = source.Clone();
        copy.full_path = destination;
        copy.name = Path.GetFileName(destination).Replace('\\', '/');

        _hashTable.Add(destination, copy);

        parent_childs.Add(copy);
        parent_childs.Sort();
        parent.childs = parent_childs.ToArray();

        fixFullPath(parent, copy);

        return copy;
    }

    public VirtualFileSystemEntry MoveEntry(VirtualFileSystemEntry source, string destination)
    {
        var copy = CopyEntry(source, destination);
        RemoveEntry(source);
        return copy;
    }

    public void AddEntry(VirtualFileSystemEntry entry)
    {
        var parent_path = entry.full_path.Remove(entry.full_path.LastIndexOf('/'));
        var parent = Query(parent_path);
        var parent_childs = new List<VirtualFileSystemEntry>(parent.childs);
        VirtualFileSystemEntry target = null;

        if (_hashTable.TryGetValue(entry.full_path, out target))
        {
            _hashTable.Remove(entry.full_path);
            parent_childs.Remove(target);
        }

        _hashTable.Add(entry.full_path, entry);

        parent_childs.Add(entry);
        parent_childs.Sort();
        parent.childs = parent_childs.ToArray();

        fixFullPath(parent, entry);
    }

    public void RemoveEntry(VirtualFileSystemEntry entry)
    {
        var parent_path = entry.full_path.Remove(entry.full_path.LastIndexOf('/'));
        var parent = Query(parent_path);
        var parent_childs = new List<VirtualFileSystemEntry>(parent.childs);

        _hashTable.Remove(entry.full_path);
        parent_childs.Remove(entry);

        parent.childs = parent_childs.ToArray();
    }

    void fixFullPath(VirtualFileSystemEntry parent, VirtualFileSystemEntry entry)
    {

        entry.full_path = parent.full_path + "/" + entry.name;

        if (_hashTable.TryGetValue(entry.full_path, out _))
        {
            _hashTable.Remove(entry.full_path);
        }

        _hashTable.Add(entry.full_path, entry);

        if (entry.childs == null) return;
        foreach (var c in entry.childs)
        {
            if (c == entry) return;
            fixFullPath(entry, c);
        }
    }
}