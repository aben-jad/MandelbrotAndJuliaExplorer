using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FractalsManager : MonoBehaviour
{
	
	[SerializeField]
	private Material mainMat, secondaryMat; // mainMat for the whole screen , secondaryMat for The smallscreen

	private float sensability; // sensability of camera mouvement
	private float zoom; // zoom of camera
	private float aspect;

	public float Sensability //Sensability property
	{
		get => sensability;
		set
		{
			sensability = value / 100;
		}
	}

	// here is the whole work , in this post processing event function i just get the image that will renderer in the screen and i aplly to it my mainMat
	// notice that in this post processing i ignore ui elements and the small screen 
	private void OnRenderImage(RenderTexture src, RenderTexture dst) => Graphics.Blit(src, dst, mainMat);

	//on Awake method i set those properties values in mainMat and secondaryMat
	//i enable also enable toggle in mainMat to draw mandelbrot fractal , and i disable it in secondaryMat to draw julia fractal
	private void Awake()
	{
		zoom = 4f;
		aspect = Camera.main.aspect;

		mainMat.SetVector("_Area", new Vector4(0, 0, 0, 0));
		mainMat.SetFloat("_Zoom", zoom);
		mainMat.SetFloat("_Aspect", aspect);

		secondaryMat.SetVector("_Area", new Vector4(0, 0, 0, 0));
		secondaryMat.SetFloat("_Zoom", zoom);

		mainMat.EnableKeyword("_ENABLE_ON");
		secondaryMat.DisableKeyword("_ENABLE_ON");
	}

	// update method
	private void Update()
	{
		if (aspect != Camera.main.aspect)
		{
			aspect = Camera.main.aspect;
			mainMat.SetFloat("_Aspect", aspect);
		}
		
		HandleInpute();
	}

	//here i handle input to move camera and modify zoom variable and also switch beetwen mandelbrot and julia fractals to select what fractal will draww in whole screen
	private void HandleInpute()
	{
		MoveCamera();

		ZoomCamera();

		if (Input.GetKeyDown(KeyCode.Space))
			SwitchFractals();
	}

	// this method for switchinf what fractal will draw in the whole screen
	private void SwitchFractals()
	{
		if (mainMat.IsKeywordEnabled("_ENABLE_ON")) 
		{
			mainMat.DisableKeyword("_ENABLE_ON");
			secondaryMat.EnableKeyword("_ENABLE_ON");
		}
		else
		{
			secondaryMat.DisableKeyword("_ENABLE_ON");
			mainMat.EnableKeyword("_ENABLE_ON");
		}
	}

	// set zoom for the camera
	private void ZoomCamera()
	{
		if (Input.GetKey(KeyCode.X))
		{
			zoom *= 1.01f;
			mainMat.SetFloat("_Zoom", zoom);
			secondaryMat.SetFloat("_Zoom", zoom);
		}
		else if (Input.GetKey(KeyCode.C))
		{
			zoom *= 0.99f;
			mainMat.SetFloat("_Zoom", zoom);
			secondaryMat.SetFloat("_Zoom", zoom);
		}

	}

	// move the camera via changing the _Area x and y variables
	private void MoveCamera()
	{
		Vector2 dir = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

		if (dir == Vector2.zero)
			return;

		Vector2 area = mainMat.GetVector("_Area");
		area = area + dir * sensability * zoom;
		mainMat.SetVector("_Area", area);
		secondaryMat.SetVector("_Area", area);
	}
}
