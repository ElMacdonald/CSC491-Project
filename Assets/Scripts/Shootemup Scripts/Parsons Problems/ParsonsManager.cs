using UnityEngine;
using TMPro;

public class ParsonsManager : MonoBehaviour
{
    public RectTransform container;
    public GameObject codeLinePrefab;

    // The correct, ordered solution
    public string[] correctLines;

    void Start()
    {
        GenerateProblem();
    }

    public void GenerateProblem()
    {
        // Clear previous children
        //foreach (Transform child in container)
            //Destroy(child.gameObject);

        // Randomize
        string[] randomized = (string[])correctLines.Clone();
        System.Random rand = new System.Random();
        for (int i = randomized.Length - 1; i > 0; i--)
        {
            int j = rand.Next(i + 1);
            (randomized[i], randomized[j]) = (randomized[j], randomized[i]);
        }

        // Spawn UI blocks
        foreach (string line in randomized)
        {
            GameObject obj = Instantiate(codeLinePrefab, container);
            obj.GetComponentInChildren<TextMeshProUGUI>().text = line;
        }
    }

    public bool IsCorrect()
    {
        for (int i = 0; i < container.childCount; i++)
        {
            string current = container.GetChild(i).GetComponentInChildren<TextMeshProUGUI>().text;

            if (current != correctLines[i])
                return false;
        }
        return true;
    }
}
