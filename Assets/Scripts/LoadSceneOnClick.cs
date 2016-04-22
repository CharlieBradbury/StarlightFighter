using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadSceneOnClick : MonoBehaviour {

	// Declare a game object for the loading image
	public GameObject goLoadingImage;
	
	// Shows the loading image while loading a scene
	public void LoadLevel(int iLevel)
	{
		goLoadingImage.SetActive(true);
		SceneManager.LoadScene(iLevel);
	}
}
