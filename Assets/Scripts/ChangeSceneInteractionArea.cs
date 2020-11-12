using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(PolygonCollider2D))]
public class ChangeSceneInteractionArea : IInteractionArea
{
    [SerializeField] [HideInInspector] int _sceneToLoad;
    protected override void execute()
    {
        if(SceneManager.GetSceneByBuildIndex(_sceneToLoad) == null) Debug.LogError("TRYING TO LOAD SCENE " + _sceneToLoad + " BUT NOT IN BUILD");
        SceneManager.LoadScene(_sceneToLoad);
    }
}
