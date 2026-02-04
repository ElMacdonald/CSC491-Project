using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlanetCarouselUI : MonoBehaviour
{
    [Header("UI Planet Objects (in order)")]
    public List<RectTransform> planets;

    [Header("Spacing between planets in UI pixels")]
    public float spacing = 400f;

    [Header("Slide speed")]
    public float slideSpeed = 8f;

    [Header("3D Depth Settings")]
    public float centerScale = 1.2f;        // Size of the center planet
    public float sideScale = 0.7f;         // Size of far planets
    public float depthOffset = 200f;       // How far back side planets go (fake depth)
    public float fadeAmount = 0.4f;        // Alpha fade for non-center planets

    private int currentIndex = 0;
    private bool isSliding = false;

    public Image leftButton;
    public Image rightButton;
    public Image nameDisplay;

    public Sprite[] planetNames;
    public Sprite[] buttonImages;

    public GameObject[] planetLevelPanels;



    void HandleImages()
    {
        switch(currentIndex)
        {
            case 0:
                leftButton.sprite = buttonImages[2];
                rightButton.sprite = buttonImages[1];
                nameDisplay.sprite = planetNames[0];
                break;
            case 1:
                leftButton.sprite = buttonImages[0];
                rightButton.sprite = buttonImages[1];
                nameDisplay.sprite = planetNames[1];
                break;
            case 2:
                nameDisplay.sprite = planetNames[2];
                break;
            case 3:
                nameDisplay.sprite = planetNames[3];
                break;
            case 4:
                leftButton.sprite = buttonImages[0];
                rightButton.sprite = buttonImages[1];
                nameDisplay.sprite = planetNames[4];
                break;
            case 5:
                leftButton.sprite = buttonImages[0];
                rightButton.sprite = buttonImages[3];
                nameDisplay.sprite = planetNames[5];
                break;
        }
    }
    void Start()
    {
        SetPositionsInstant();
        ApplyDepthInstant();
    }

    public void ShiftRight()
    {
        if (isSliding) return;
        if (currentIndex < planets.Count - 1)
        {
            currentIndex++;
            HandleImages();
            StartCoroutine(SlideToNewPositions());
        }
    }

    public void ShiftLeft()
    {
        if (isSliding) return;
        if (currentIndex > 0)
        {
            currentIndex--;
            HandleImages();
            StartCoroutine(SlideToNewPositions());
        }
        
    }

    public void EnterPlanet()
    {
        Debug.Log("Selected planet: " + planets[currentIndex].name);
        planetLevelPanels[currentIndex].SetActive(true);
    }

    IEnumerator SlideToNewPositions()
    {
        isSliding = true;

        Vector2[] startPos = new Vector2[planets.Count];
        Vector2[] targetPos = new Vector2[planets.Count];
        Vector3[] startScale = new Vector3[planets.Count];
        Vector3[] targetScale = new Vector3[planets.Count];
        float[] startZ = new float[planets.Count];
        float[] targetZ = new float[planets.Count];

        for (int i = 0; i < planets.Count; i++)
        {
            int offset = i - currentIndex;

            // Start values
            startPos[i] = planets[i].anchoredPosition;
            startScale[i] = planets[i].localScale;
            startZ[i] = planets[i].localPosition.z;

            // Target positions
            targetPos[i] = new Vector2(offset * spacing, 0);

            // Depth scale
            float depthT = Mathf.Abs(offset); // 0 = center, 1 = nearby, 2 = far
            float scale = Mathf.Lerp(centerScale, sideScale, depthT);
            targetScale[i] = new Vector3(scale, scale, 1);

            // Z-depth
            targetZ[i] = -depthT * depthOffset;
        }

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * slideSpeed;

            for (int i = 0; i < planets.Count; i++)
            {
                planets[i].anchoredPosition =
                    Vector2.Lerp(startPos[i], targetPos[i], t);

                planets[i].localScale =
                    Vector3.Lerp(startScale[i], targetScale[i], t);

                Vector3 pos3D = planets[i].localPosition;
                pos3D.z = Mathf.Lerp(startZ[i], targetZ[i], t);
                planets[i].localPosition = pos3D;

                // Fade alpha
                Image img = planets[i].GetComponent<Image>();
                if (img != null)
                {
                    float a = Mathf.Lerp(
                        (i == currentIndex ? 1f : fadeAmount),
                        (i == currentIndex ? 1f : fadeAmount),
                        t
                    );
                    img.color = new Color(img.color.r, img.color.g, img.color.b, a);
                }
            }

            yield return null;
        }

        isSliding = false;
    }

    void SetPositionsInstant()
    {
        for (int i = 0; i < planets.Count; i++)
        {
            int offset = i - currentIndex;
            planets[i].anchoredPosition = new Vector2(offset * spacing, 0);
        }
    }

    void ApplyDepthInstant()
    {
        for (int i = 0; i < planets.Count; i++)
        {
            int offset = i - currentIndex;
            float depthT = Mathf.Abs(offset);

            float scale = Mathf.Lerp(centerScale, sideScale, depthT);
            planets[i].localScale = new Vector3(scale, scale, 1);

            Vector3 pos3D = planets[i].localPosition;
            pos3D.z = -depthT * depthOffset;
            planets[i].localPosition = pos3D;

            Image img = planets[i].GetComponent<Image>();
            if (img != null)
            {
                img.color = new Color(img.color.r, img.color.g, img.color.b,
                    (i == currentIndex ? 1f : fadeAmount));
            }
        }
    }
}
