using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Hint : MonoBehaviour, ISerializationCallbackReceiver
{
    Guid _guid = Guid.Empty;
    public string GUID;
    public bool UnlockCondition;

    public void OnBeforeSerialize()
    {
        if (_guid != Guid.Empty)
        {
            GUID = _guid.ToString();
        }
    }

    public void OnAfterDeserialize()
    {
        if (GUID != null && GUID.Length == 16)
        {
            _guid = new Guid(GUID);
        }
    }

    void CreateGuid()
    {
        // if our serialized data is invalid, then we are a new object and need a new GUID
        if (GUID == null || GUID.Length != 16)
        {
            _guid = Guid.NewGuid();
            GUID = _guid.ToString();
        }
        else if (_guid == Guid.Empty)
        {
            // otherwise, we should set our system guid to our serialized guid
            _guid = new Guid(GUID);
        }
    }

    private void Awake()
    {
        CreateGuid();
    }

    public void Unlock()
    {

    }
}
