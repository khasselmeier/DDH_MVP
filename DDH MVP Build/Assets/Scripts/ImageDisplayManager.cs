using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ImageDisplayManager : MonoBehaviour
{
    public Image[] images;

    private void Start()
    {
        StartCoroutine(DisplayImages());
    }

    private IEnumerator DisplayImages()
    {
        foreach (Image img in images)
        {
            img.gameObject.SetActive(true);
            yield return new WaitForSeconds(3f); //wait for *blank* seconds
            img.gameObject.SetActive(false);
        }
    }
}