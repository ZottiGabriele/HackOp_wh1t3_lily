using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// Serializable emulated file system that provides all the operations to interact with it.
/// </summary>

[System.Serializable]
public class VirtualFileSystem
{
    public VirtualFileSystemEntry childs;
    public VirtualFileSystemEntry ActiveEntry { get => Query(TerminalHandler.Instance.TerminalConfig.CurrentPath); }
    public VirtualFileSystemEntry HomeEntry { get => Query(TerminalHandler.Instance.TerminalConfig.HomePath); }
    public List<VirtualFileSystemEntry> AvailableCommands { get => getAvailableCommands(); }
    private Dictionary<string, VirtualFileSystemEntry> _hashTable = new Dictionary<string, VirtualFileSystemEntry>();

    /// <summary>
    /// Static method to deserialize a Json file rapresenting a VirtualFileSystem.
    /// </summary>
    /// <param name="jsonString">The json text to deserialize.</param>
    /// <returns>The serialized VirtualFileSystem.</returns>
    public static VirtualFileSystem CreateFromJson(string jsonString)
    {
        var output = JsonUtility.FromJson<VirtualFileSystem>(jsonString);

        output.childs.BuildVirtualFileSystem(ref output._hashTable);

        return output;
    }

    /// <summary>
    /// Returns a List containing all the VirtualFileSystemEntry of the commands found following the current value of the $PATH envinroment variable.
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    /// Queries the VirtualFileSystem for the existence of the requested file.
    /// </summary>
    /// <param name="path">The relative or absolute path to the requested file.</param>
    /// <returns>The requested VirtualFileSystemEntry, or null if the file can't be found.</returns>
    public VirtualFileSystemEntry Query(string path)
    {
        VirtualFileSystemEntry output = null;

        path = GetFinalPath(path);
        _hashTable.TryGetValue(path, out output);

        return output;
    }

    /// <summary>
    /// <para>Elaborates the given path and returns the full absolute path to the requested entry.</para>
    /// 
    /// Use this before passing the path to other methods in order to make sure that the path follows the required specifications
    /// </summary>
    /// <param name="path">The path to elaborate.</param>
    /// <returns></returns>
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

    /// <summary>
    /// Creates a copy of the source VirtualFileSystemEntry. If the destination file already exists then it will be replaced with the copy.
    /// </summary>
    /// <param name="source">The source VirtualFileSystemEntry</param>
    /// <param name="destination">The destination path of the copy</param>
    /// <returns></returns>
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

    /// <summary>
    /// Moves the source VirtualFileSystemEntry. If the destination file already exists then it will be replaced with the moved file.
    /// </summary>
    /// <param name="source">The source VirtualFileSystemEntry</param>
    /// <param name="destination">The destination path where to move the file</param>
    /// <returns></returns>
    public VirtualFileSystemEntry MoveEntry(VirtualFileSystemEntry source, string destination)
    {
        var copy = CopyEntry(source, destination);
        RemoveEntry(source);
        return copy;
    }

    /// <summary>
    /// Adds an entry to the VirtualFileSystem. If the entry already exists it will be replaced with the new one.
    /// </summary>
    /// <param name="entry">The entry to add to the VirtualFileSystem.</param>
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

    /// <summary>
    /// Removes an entry from the VirtualFileSystem.
    /// </summary>
    /// <param name="entry">The entry to remove from the VirtualFileSystem</param>
    public void RemoveEntry(VirtualFileSystemEntry entry)
    {
        var parent_path = entry.full_path.Remove(entry.full_path.LastIndexOf('/'));
        var parent = Query(parent_path);

        if (parent == null) return;

        var parent_childs = new List<VirtualFileSystemEntry>(parent.childs);

        _hashTable.Remove(entry.full_path);
        parent_childs.Remove(entry);

        parent.childs = parent_childs.ToArray();
    }

    /// <summary>
    /// Recursively fixes a newly added entry and all his childs paths relative to the new parent.
    /// </summary>
    /// <param name="parent">Parent entry.</param>
    /// <param name="entry">Target entry.</param>
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