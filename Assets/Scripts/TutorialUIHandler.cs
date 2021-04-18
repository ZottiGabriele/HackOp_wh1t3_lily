using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialUIHandler : MonoBehaviour
{
    [SerializeField] Vector3 _offset;
    [SerializeField] Transform _firstClickTarget;
    [SerializeField] GameObject _btnOne;
    [SerializeField] GameObject _btnTwo;
    [SerializeField] GameObject _firstClickPrompt;

    Transform _target;

    private void LateUpdate()
    {
        gameObject.SetActive(!GameStateHandler.Instance.GameData.TutorialCompleted);

        if ((transform.position - _firstClickTarget.position).magnitude < 2)
        {
            _btnOne.SetActive(false);
            _btnTwo.SetActive(false);
            _target = _firstClickTarget;
            _offset = new Vector3(0, 1);
            _firstClickPrompt.SetActive(true);
        }

        transform.position = _target.position + _offset;

        GameStateHandler.Instance.GameData.TutorialCompleted = GameStateHandler.Instance.GameData.FirstTokenFound;
    }

    // Start is called before the first frame update
    void Start()
    {
        _target = PlayerController.Instance.transform;
    }
}
