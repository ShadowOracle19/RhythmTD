using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
// ReSharper disable All

[CustomPropertyDrawer(typeof(LineAttribute))]
public class LineAttributeDrawer : DecoratorDrawer
{
    public override VisualElement CreatePropertyGUI()
    {
        LineAttribute _attribute = this.attribute as LineAttribute;

        VisualElement container = new VisualElement()
        {
            style =
            {
                height = _attribute.Height,
                backgroundColor = _attribute.LineColor,
                marginBottom = 10
            }
        };

        return container;
    }
}
