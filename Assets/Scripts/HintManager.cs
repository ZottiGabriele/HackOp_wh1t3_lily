using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintManager : MonoBehaviour
{
    public List<Hint> Hints => _hints;
    public Solution Solution => _solution;

    [SerializeField] private List<Hint> _hints;
    [SerializeField] private Solution _solution;

    private void Start()
    {
        _hints = new List<Hint>(transform.GetComponentsInChildren<Hint>());

        int i = 0;
        foreach (var h in _hints)
        {
            h.SetHintManager(this);

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

        _solution.SetHintManager(this);
        _solution.gameObject.SetActive(GameStateHandler.Instance.GameData.UnlockedHintIDs.Contains(_solution.GUID));
    }

    public void UnlockHint(string GUID)
    {
        GameStateHandler.Instance.UseHintToken();
        GameStateHandler.Instance.GameData.UnlockedHintIDs.Add(GUID);
        GameStateHandler.Instance.SaveGame();
        int i = _hints.FindIndex(0, _hints.Count, (h) => h.GUID == GUID);
        if (i < _hints.Count - 1)
        {
            _hints[i + 1].gameObject.SetActive(true);
        }
        else
        {
            _solution.gameObject.SetActive(true);
        }
    }

    public void UnlockSolution(string GUID)
    {
        GameStateHandler.Instance.GameData.UnlockedHintIDs.Add(GUID);
        GameStateHandler.Instance.SaveGame();
    }
}
