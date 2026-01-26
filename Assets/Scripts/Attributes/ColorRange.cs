using UnityEngine;

public class ColorRange : PropertyAttribute
{
    public int MinValue;
    public int MaxValue;
    public int Breakpoint;

    public ColorRange(int min, int max, int breakpoint)
    {
        this.MinValue = min;
        this.MaxValue = max;
        this.Breakpoint = breakpoint;
    }
}