using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ImageTracker : MonoBehaviour
{
    private ARTrackedImageManager trackedImages;
    public GameObject[] ARPrefabs;

    List<GameObject> ARObjects = new List<GameObject>();

    private void Awake()
    {
        trackedImages = GetComponent<ARTrackedImageManager>();
    }

    private void OnEnable()
    {
        trackedImages.trackedImagesChanged += OnTrackedImagesChanged;
    }

    private void OnDisable()
    {
        trackedImages.trackedImagesChanged -= OnTrackedImagesChanged;
    }

    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (var trackedImage in eventArgs.added)
        {
            foreach (var ARPrefab in ARPrefabs)
            {
                if (trackedImage.referenceImage.name == ARPrefab.name)
                {
                    var newPrefab = Instantiate(ARPrefab, trackedImage.transform);
                    ARObjects.Add(newPrefab);
                }
            }
        }

        foreach (var trackedImage in eventArgs.updated)
        {
            foreach (var gameObject in ARObjects)
            {
                if (gameObject.name == trackedImage.name)
                {
                    gameObject.SetActive(trackedImage.trackingState == UnityEngine.XR.ARSubsystems.TrackingState.Tracking);
                }
            }
        }
    }
}
