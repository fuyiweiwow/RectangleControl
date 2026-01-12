namespace RectangleControl.Enums
{
    [Flags]
    public enum ResizeDirection
    {
        Top = 1,
        Bottom = 2,
        Left = 4,
        Center = 8,
        Right = 16,
        TopLeft = Top | Left,
        TopRight = Top | Right,
        BottomLeft = Bottom | Left,
        BottomRight = Bottom | Right,
    }
}