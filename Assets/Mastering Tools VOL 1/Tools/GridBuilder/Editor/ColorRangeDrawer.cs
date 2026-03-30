using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
// ReSharper disable All

[CustomPropertyDrawer(typeof(ColorRange))]
public class ColorRangeDrawer : PropertyDrawer
{
    public override VisualElement CreatePropertyGUI(SerializedProperty property)
    {
        ColorRange attribute = this.attribute as ColorRange;

        VisualElement container = new VisualElement()
        {
            style =
            {
                flexDirection = FlexDirection.Row,
                alignItems = Align.Center,
                paddingLeft = 2
            }
        };
        
        Label label = new Label(property.displayName);

        SliderInt slider = new SliderInt(attribute.MinValue, attribute.MaxValue)
        {
            bindingPath = property.propertyPath,
            
            style =
            {
                flexGrow = 1,
                marginRight = 20,
                marginLeft = 20
            }
        };

        IntegerField valueField = new IntegerField()
        {
            bindingPath = property.propertyPath,
            
            style =
            {
                width = 40,
                paddingRight = 6,
            }
        };

        slider.RegisterValueChangedCallback(evt =>
        {
            label.style.backgroundColor = evt.newValue < attribute.Breakpoint ? Color.red : Color.green;
            Debug.Log($"Value: {property.intValue} - New Value: {evt.newValue}");
        });
        
        container.Add(label);
        container.Add(slider);
        container.Add(valueField);

        return container;
    }
}
