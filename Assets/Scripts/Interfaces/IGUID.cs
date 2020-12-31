using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniqueIdentifierAttribute : PropertyAttribute { }

public interface IGUID
{
    string GUID { get; }
}
