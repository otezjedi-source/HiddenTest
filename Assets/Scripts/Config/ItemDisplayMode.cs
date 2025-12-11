using System;

public static class ItemDisplayMode
{
    public enum Type
    {
        Text = 1,
        Icon = 2,
        TextIcon = Text | Icon
    }

    private static readonly Type[] order =
    {
        Type.Text,
        Type.Icon,
        Type.TextIcon
    };

    public static Type FromIndex(int index)
    {
        if (index < 0 || index >= order.Length)
            return Type.Text;
        return order[index];
    }

    public static int ToIndex(Type type)
    {
        return Array.IndexOf(order, type);
    }
}
