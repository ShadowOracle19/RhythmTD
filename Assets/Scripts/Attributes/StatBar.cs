using UnityEngine;

public class StatBar : PropertyAttribute
{
    public float MinValue;
    public float MaxValue;
    public Color Color;

    public StatBar(float min, float max, float r = 0, float g = 0, float b = 255)
    {
        this.MinValue = min;
        this.MaxValue = max;
        this.Color = new Color(r / 255f, g / 255f, b / 255f);
    }
}