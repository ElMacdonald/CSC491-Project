using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlanetCarouselUI : MonoBehaviour
{
    [Header("Planets (in order)")]
    public List<RectTransform> planets;

    [Header("Layout")]
    public float spacing   = 400f;
    public float slideSpeed = 8f;

    [Header("Depth")]
    public float centerScale = 1.2f;
    public float sideScale   = 0.7f;
    public float depthOffset = 200f;
    public float fadeAmount  = 0.4f;

    private int  currentIndex = 0;
    private bool isSliding    = false;

    public Image    leftButton;
    public Image    rightButton;
    public Image    nameDisplay;
    public Sprite[] planetNames;
    public Sprite[] buttonImages;
    public GameObject[] planetLevelPanels;

    void HandleImages()
    {
        switch (currentIndex)
        {
            case 0:
                leftButton.sprite  = buttonImages[2];
                rightButton.sprite = buttonImages[1];
                nameDisplay.sprite = planetNames[0];
                break;
            case 1:
                leftButton.sprite  = buttonImages[0];
                rightButton.sprite = buttonImages[1];
                nameDisplay.sprite = planetNames[1];
                break;
            case 2: nameDisplay.sprite = planetNames[2]; break;
            case 3: nameDisplay.sprite = planetNames[3]; break;
            case 4:
                leftButton.sprite  = buttonImages[0];
                rightButton.sprite = buttonImages[1];
                nameDisplay.sprite = planetNames[4];
                break;
            case 5:
                leftButton.sprite  = buttonImages[0];
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
        if (isSliding || currentIndex >= planets.Count - 1) return;
        currentIndex++;
        HandleImages();
        StartCoroutine(SlideToNewPositions());
    }

    public void ShiftLeft()
    {
        if (isSliding || currentIndex <= 0) return;
        currentIndex--;
        HandleImages();
        StartCoroutine(SlideToNewPositions());
    }

    public void EnterPlanet()
    {
        planetLevelPanels[currentIndex].SetActive(true);
        RefreshPanel(planetLevelPanels[currentIndex]);
    }

    // Called by LevelManager when Level Select loads
    public void RefreshAllPanels()
    {
        foreach (GameObject panel in planetLevelPanels)
        {
            if (panel == null) continue;
            LevelSelectUI ui = panel.GetComponentInChildren<LevelSelectUI>(includeInactive: true);
            if (ui != null) ui.RefreshAllButtons();
        }
    }

    private void RefreshPanel(GameObject panel)
    {
        LevelSelectUI ui = panel.GetComponentInChildren<LevelSelectUI>(includeInactive: true);
        if (ui != null) ui.RefreshAllButtons();
    }

    IEnumerator SlideToNewPositions()
    {
        isSliding = true;

        var startPos   = new Vector2[planets.Count];
        var targetPos  = new Vector2[planets.Count];
        var startScale = new Vector3[planets.Count];
        var targetScale= new Vector3[planets.Count];
        var startZ     = new float[planets.Count];
        var targetZ    = new float[planets.Count];

        for (int i = 0; i < planets.Count; i++)
        {
            int   offset = i - currentIndex;
            float depthT = Mathf.Abs(offset);
            float scale  = Mathf.Lerp(centerScale, sideScale, depthT);

            startPos[i]    = planets[i].anchoredPosition;
            startScale[i]  = planets[i].localScale;
            startZ[i]      = planets[i].localPosition.z;
            targetPos[i]   = new Vector2(offset * spacing, 0);
            targetScale[i] = new Vector3(scale, scale, 1);
            targetZ[i]     = -depthT * depthOffset;
        }

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * slideSpeed;
            for (int i = 0; i < planets.Count; i++)
            {
                planets[i].anchoredPosition = Vector2.Lerp(startPos[i], targetPos[i], t);
                planets[i].localScale       = Vector3.Lerp(startScale[i], targetScale[i], t);

                Vector3 pos3D = planets[i].localPosition;
                pos3D.z = Mathf.Lerp(startZ[i], targetZ[i], t);
                planets[i].localPosition = pos3D;

                Image img = planets[i].GetComponent<Image>();
                if (img != null)
                {
                    float a = i == currentIndex ? 1f : fadeAmount;
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
            planets[i].anchoredPosition = new Vector2((i - currentIndex) * spacing, 0);
    }

    void ApplyDepthInstant()
    {
        for (int i = 0; i < planets.Count; i++)
        {
            float depthT = Mathf.Abs(i - currentIndex);
            float scale  = Mathf.Lerp(centerScale, sideScale, depthT);

            planets[i].localScale = new Vector3(scale, scale, 1);

            Vector3 pos3D = planets[i].localPosition;
            pos3D.z = -depthT * depthOffset;
            planets[i].localPosition = pos3D;

            Image img = planets[i].GetComponent<Image>();
            if (img != null)
                img.color = new Color(img.color.r, img.color.g, img.color.b, i == currentIndex ? 1f : fadeAmount);
        }
    }
}
