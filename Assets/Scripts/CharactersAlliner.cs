using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharactersAligner : MonoBehaviour
{
    [SerializeField] private GameObject[] characters;
    [SerializeField] private float maxX = 3f;
    [SerializeField] private float maxZ = 2f;
    [SerializeField] private GameFSM gameFsm;

    private Camera _mainCamera;
    private Vector3 _axis = new Vector3(1, 0, 1);
    private bool _firstAlign = true;
    

    private void Awake()
    {
        _mainCamera = Camera.main;
    }

    public void RealignCharacters(List<Vector3> positions)
    {
        int plusValue;
        
        if (positions.Count > characters.Length)
        {
            plusValue = positions.Count / characters.Length;
        }
        else
        {
            plusValue = 1;
        }

        int i = 0;
        int maxI;
        int currentI = 0;
        bool ch;
        if (characters.Length >= positions.Count)
        {
            maxI = characters.Length / positions.Count;
            ch = true;
        }
        else
        {
            maxI = Mathf.CeilToInt(positions.Count / characters.Length); //asdasdfasd
            ch = false;
        }
        foreach (var character in characters)
        {
            Vector3 pos = new();
            if (ch)
            {
                if (currentI % maxI != 0)
                {
                    pos = _mainCamera.ScreenToWorldPoint(positions[i]);
                    for (int iter = 1; iter < maxI; iter++)
                    {
                        pos += _mainCamera.ScreenToWorldPoint(positions[i + iter]);
                    }

                    pos /= maxI;
                }
                else
                {
                    pos = _mainCamera.ScreenToWorldPoint(positions[i]);
                }
                
                var positionTemp = pos;
                positionTemp = transform.InverseTransformPoint(positionTemp);
                var newPlace = new Vector3(positionTemp.x, characters[i].transform.localPosition.y, positionTemp.y - 7);
                newPlace.z = Mathf.Clamp(newPlace.z, -maxZ, maxZ);
                newPlace.x = Mathf.Clamp(newPlace.x, -maxX, maxX);
                character.transform.localPosition = newPlace;
                if (currentI != maxI)
                {
                    currentI++;
                }
                else
                {
                    i++;
                    currentI = 0;
                }
                i = Mathf.Clamp(i, 0, positions.Count - 1);
            }
            else
            {
                pos = _mainCamera.ScreenToWorldPoint(positions[i]);
                var positionTemp = pos;
                positionTemp = transform.InverseTransformPoint(positionTemp);
                var newPlace = new Vector3(positionTemp.x, character.transform.localPosition.y, positionTemp.y - 5.5f);
                newPlace.z = Mathf.Clamp(newPlace.z, -maxZ, maxZ);
                newPlace.x = Mathf.Clamp(newPlace.x, -maxX, maxX);
                character.transform.localPosition = newPlace;
                i += maxI;
                i = Mathf.Clamp(i, 0, positions.Count - 1);
            }
        }
        // for (int i = 0; i < characters.Length; i += plusValue)
        // {
        //     if ((characters.Length == positions.Count || positions.Count > characters.Length) && positions.Count > i)
        //     {
        //         Debug.Log("HERE");
        //         var positionTemp = _mainCamera.ScreenToWorldPoint(positions[i]);
        //         positionTemp = transform.InverseTransformPoint(positionTemp);
        //         var newPlace = new Vector3(positionTemp.x, characters[i].transform.localPosition.y, positionTemp.y - 7);
        //         newPlace.z = Mathf.Clamp(newPlace.z, -maxZ, maxZ);
        //         newPlace.x = Mathf.Clamp(newPlace.x, -maxX, maxX);
        //         characters[i].transform.localPosition = newPlace;
        //     }
        // }

        if (_firstAlign)
        {
            gameFsm.ChangeState(gameFsm.playState);
        }
    }
}
