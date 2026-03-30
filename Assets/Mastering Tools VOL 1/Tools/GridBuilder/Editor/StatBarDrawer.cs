using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

// ReSharper disable All

[CustomPropertyDrawer(typeof(StatBar))]
public class StatBarDrawer : PropertyDrawer
{
    public override VisualElement CreatePropertyGUI(SerializedProperty property)
    {
        StatBar attribute = this.attribute as StatBar;

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
        label.style.width = 40;

        ProgressBar progressBar = new ProgressBar()
        {
            bindingPath = property.propertyPath,
            title = $"{property.floatValue}",
            lowValue= attribute.MinValue,
            highValue = attribute.MaxValue,
            style =
            {
                flexGrow = 1,
                marginLeft = 20
            }
        };

        progressBar.RegisterValueChangedCallback(evt =>
        {
            property.floatValue = Mathf.Clamp(evt.newValue, attribute.MinValue, attribute.MaxValue);
            property.serializedObject.ApplyModifiedProperties();
            
;           progressBar.title = $"{property.floatValue}";
            Debug.Log($"Property:{property.floatValue} - Progress bar: {evt.newValue}");
        });

        Button decrease = new Button(() => { progressBar.value--; })
        {
            text = "-",
        };

        Button increase = new Button(() => { progressBar.value++; })
        {
            text = "+",
        };
        
        container.Add(label);
        container.Add(progressBar);
        container.Add(decrease);
        container.Add(increase);
        
        return container;
    }
}
