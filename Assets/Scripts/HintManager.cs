using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintManager : MonoBehaviour
{
    public static HintManager Instance;

    public List<Hint> Hints => _hints;

    [SerializeField] private List<Hint> _hints;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        _hints = new List<Hint>(transform.GetComponentsInChildren<Hint>());

        int i = 0;
        foreach (var h in _hints)
        {
            if (GameStateHandler.Instance.GameData.UnlockedHintIDs.Contains(h.GUID))
            {
                h.gameObject.SetActive(true);
                i++;
            }
            else
            {
                h.gameObject.SetActive(false);
            }
        }

        if (i < _hints.Count) _hints[i].gameObject.SetActive(true);
    }

    public void UnlockHint(string GUID)
    {
        GameStateHandler.Instance.GameData.HintTokenCount--;
        GameStateHandler.Instance.GameData.UnlockedHintIDs.Add(GUID);
        GameStateHandler.Instance.SaveGame();
        int i = _hints.FindIndex(0, _hints.Count, (h) => h.GUID == GUID);
        if (i < _hints.Count - 1) _hints[i + 1].gameObject.SetActive(true);
    }
}
