using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour
{
    public List<Tile> neighbors = new List<Tile>();

    public GameObject contents;
    public GameObject unit;

    public bool hasCity = false;
    public bool occupied = false;

    public string type = "";
    public Vector2 position;

    public bool inAnimation = false;
    public bool animate = false;

    public bool addCity = false;
    public bool recruitSwordman = false;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (animate)
        {
            foreach (Tile neighbor in neighbors)
            {
                if (!neighbor.inAnimation)
                {
                    neighbor.StartCoroutine(neighbor.AnimateTileClicked());
                }
            }
            animate = false;
        }

        if (addCity && !hasCity)
        {
            GameObject city = Instantiate(Global.cityPrefab, transform.position, Quaternion.identity);
            city.transform.parent = this.transform;
            contents = city;
            addCity = false;
            hasCity = true;
        }

        if (recruitSwordman && !occupied && hasCity)
        {
            GameObject swordman = Instantiate(Global.swordmanPrefab, transform.position, Quaternion.identity);
            swordman.transform.parent = this.transform;
            unit = swordman;
            recruitSwordman = false;
            occupied = true;

        }
    }

    private IEnumerator AnimateTileClicked()
    {
        inAnimation = true;
        Vector3 originalScale = transform.localScale;
        Vector3 targetScale = originalScale * 4f;
        float animationDuration = 1f;
        float elapsedTime = 0f;
        while (elapsedTime < animationDuration)
        {
            transform.localScale = Vector3.Lerp(originalScale, targetScale, (elapsedTime / animationDuration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.localScale = targetScale;
        elapsedTime = 0f;
        while (elapsedTime < animationDuration)
        {
            transform.localScale = Vector3.Lerp(targetScale, originalScale, (elapsedTime / animationDuration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.localScale = originalScale;
        inAnimation = false;

    }
}
