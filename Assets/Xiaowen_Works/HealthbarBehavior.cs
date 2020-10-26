using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthbarBehavior : MonoBehaviour
{

    public Slider Slider;
    public Vector3 Offset;
    Dictionary<float, Color> dict = new Dictionary<float, Color>();

    // Start is called before the first frame update
    void Start()
    {

        dict.Add(1.0f, Color.red);
        dict.Add(2.0f, Color.yellow);
        dict.Add(3.0f, Color.green);
        dict.Add(4.0f, Color.green);
    }

    Color getColor(float health)
    {
        if (health >= 2.5f)
        {
            return Color.green;
        }
        else if (health >= 1.5f)
        {
            return Color.yellow;
        }
        else
        {
            return Color.red;
        }
    }
    public void SetHealth(float health, float maxHealth)
    {
        Slider.gameObject.SetActive(health < maxHealth);
        Slider.value = health;
        Slider.maxValue = maxHealth;

        Slider.fillRect.GetComponentInChildren<Image>().color = getColor(health);/*Color.Lerp(Low, High, Slider.normalizedValue);*/
        //print(health + " " + maxHealth);
    }
    // Update is called once per frame
    void Update()
    {
        Slider.transform.position = Camera.main.WorldToScreenPoint(transform.parent.position + new Vector3(0, 0.5f, 1));
    }
}
