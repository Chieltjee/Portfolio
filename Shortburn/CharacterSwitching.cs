using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSwitching : MonoBehaviour
{
    [SerializeField] private GameObject[] _characters;
    [SerializeField] private CinemachineVirtualCamera[] _cameras;

    [SerializeField] private GameObject _activeCharacter;
    [SerializeField] private CinemachineVirtualCamera _activeCamera;

    [SerializeField] private bool _areEnabled = true;
    public bool unlockedAllCharacters;
    private void Start()
    {
        _activeCharacter = _characters[0];
        _activeCamera = _cameras[0];
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            if (unlockedAllCharacters)
                SwitchCharacter();
        }
    }
    /// <summary>
    /// Handles the switching characters with camera and controller
    /// </summary>
    public void SwitchCharacter()
    {
        //Check the current character
        int l_currentCharacter = 0;
        for (int i = 0; i < _characters.Length; i++)
        {
            //if the active character is equal to i
            if (_activeCharacter == _characters[i])
            {
                //if i is equal to the length minus 1
                if (i == _characters.Length - 1)
                    //set current character to index - 1
                    l_currentCharacter = i - 1;
                else
                    //set currencharater to index + 1
                    l_currentCharacter = i + 1;
            }
        }
        //Disable cam and controller for previous character
        _activeCamera.enabled = false;
        _activeCharacter.GetComponent<SC_FPSController>().canMove = false;
        _activeCharacter = _characters[l_currentCharacter];
        //Enable cam and controller for current character
        _activeCamera = _cameras[l_currentCharacter];
        _activeCamera.enabled = true;
        _activeCharacter.GetComponent<SC_FPSController>().canMove = true;
    }
}
