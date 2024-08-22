using Microsoft.MixedReality.Toolkit.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScalePointer : MonoBehaviour
{
    public PinchSlider pinchSlider;
    public GameObject targetObject;
    public float minScale = 0.0f;
    public float maxScale = 1.0f;

    void Start()
    {
        // Ensure pinchSlider is assigned in the Inspector
        if (pinchSlider == null)
        {
            Debug.LogError("PinchSlider is not assigned!");
            return;
        }

        // Subscribe to the OnValueUpdated event of the PinchSlider
        pinchSlider.OnValueUpdated.AddListener(OnPinchSliderUpdated);
    }

    void OnPinchSliderUpdated(SliderEventData eventData)
    {
        // Retrieve the slider value
        float sliderValue = eventData.NewValue;

        // Map the slider value to the desired scale range
        float scaleFactor = Mathf.Lerp(minScale, maxScale, sliderValue);

        // Apply the scale to the target object
        targetObject.transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
    }
}


