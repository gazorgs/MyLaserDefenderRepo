using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    // Parameters
    [SerializeField] int breakableBlocks; // Serialize for debugging purposes

    // Cached reference
    SceneLoader sceneloader; // so Unity doesn't have to look every time this script with method BlockDestroyed is called

    // Start
    private void Start()
    {
        sceneloader = FindObjectOfType<SceneLoader>();
    }

    // Everytime this Level.class script is called from a Block.class script the CountBlocks method below adds 1 to breakableBlocks
    // So if there are 20 blocks breakableBlocks will equal 20 as it is called 20 times
    public void CountBlocks()
    {
        breakableBlocks++;
    }

    public void BlockDestroyed()
    {
        breakableBlocks--;
        if (breakableBlocks <= 0)
        {
            sceneloader.LoadNextScene(); // call LoadNextScene method in SceneLoader.cs
        }
    }
}
