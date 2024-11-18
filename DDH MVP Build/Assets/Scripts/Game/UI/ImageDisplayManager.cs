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

            float timeElapsed = 0f;

            // wait for *blank* seconds or until the spacebar is pressed
            while (timeElapsed < 3f)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    break;
                }

                timeElapsed += Time.deltaTime;
                yield return null;
            }

            img.gameObject.SetActive(false);
        }
    }
}