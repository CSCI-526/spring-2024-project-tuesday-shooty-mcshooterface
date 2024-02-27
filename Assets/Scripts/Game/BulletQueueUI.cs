using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BulletQueueUI : MonoBehaviour
{
    public List<Image> bulletIndicators;

    public Color redColor;
    public Color greenColor;
    public Color blueColor;
    public Color emptyColor;

    public void UpdateQueueDisplay(Queue<BulletColor> queue)
    {
        int index = 0;
        foreach (var bullet in queue)
        {
            switch (bullet)
            {
                case BulletColor.Red:
                    bulletIndicators[index].color = redColor;
                    break;
                case BulletColor.Green:
                    bulletIndicators[index].color = greenColor;
                    break;
                case BulletColor.Blue:
                    bulletIndicators[index].color = blueColor;
                    break;
                case BulletColor.Empty:
                    bulletIndicators[index].color = emptyColor;
                    break;
            }
            index++;
        }
    }
}
