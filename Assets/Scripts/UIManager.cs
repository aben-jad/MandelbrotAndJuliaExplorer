using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
	[SerializeField]
	private GameObject menuIcon, menuWindow; // menu button and menu window gameobjects 
	[SerializeField]
	private Slider rSlider, gSlider, bSlider, iterSlider, sensabilitySlider; // r g b sliders and silder for iterations and sensabilty
	[SerializeField]
	private Material mainMat, secondaryMat; // main material and secandory material
	[SerializeField]
	private FractalsManager fractalsManager; // a reference for fractals manager monobehaviour in camera gameobject

	// in Awake i set the sliders values to main and secandory materials
	private void Awake()
	{
		mainMat.SetVector("_RGBA", new Vector4(rSlider.value, gSlider.value, bSlider.value, 1));
		mainMat.SetFloat("_Iter", iterSlider.value);

		secondaryMat.SetVector("_RGBA", new Vector4(rSlider.value, gSlider.value, bSlider.value, 1));

		fractalsManager.Sensability = sensabilitySlider.value;
	}

	// the two bellow methods i added them for onClick events in the inspector 
	// this method show menu window and hide menu button when the user click on menu button via Menu Button OnClick event
	public void ShowMenuWindow()
	{
		menuIcon.SetActive(false);
		menuWindow.SetActive(true);
	}

	// this method hide menu window and show menu button when the user click on "Ok" button via Ok Button OnClick event
	public void HideMenuWindow()
	{
		menuIcon.SetActive(true);
		menuWindow.SetActive(false);
	}

	// those bellow methods i added them for OnValueChanged events in the inspector 
	// this method handle r g b channels via r g b sliders OnValueChanged events 
	public void RGBChannelSlidersValueChanged()
	{
		mainMat.SetVector("_RGBA", new Vector4(rSlider.value, gSlider.value, bSlider.value, 1));

		secondaryMat.SetVector("_RGBA", new Vector4(rSlider.value, gSlider.value, bSlider.value, 1));
	}

	// this method handle sensablity value via sensablity slider OnValueChanged event
	public void SensabilitySliderValueChanged()
	{
		fractalsManager.Sensability = sensabilitySlider.value;
	}

	// this method handle iteration value via iteration slider OnValueChanged event
	public void IterationSliderValueChanged()
	{
		mainMat.SetFloat("_Iter", iterSlider.value);
	}
}
