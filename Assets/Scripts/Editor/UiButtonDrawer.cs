using System;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(UiButton))]
public class UiButtonDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        // Get the UiButton fields
        var buttonProperty = property.FindPropertyRelative("button");
        var functionNameProperty = property.FindPropertyRelative("clickFunction");
        var parametersProperty = property.FindPropertyRelative("parameters");

        // Display the Button field using PropertyField (this will properly handle Unity objects)
        Rect buttonRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
        EditorGUI.PropertyField(buttonRect, buttonProperty);  // This will display the Button component

        // Get the target object (UiPage) and its parent component of type IMenuManager
        var script = property.serializedObject.targetObject as UiPage;
        var parentMenuManager = script?.GetComponentInParent<IMenuManager>();

        // Store methods for the dropdown
        var methods = Enumerable.Empty<(string Name, ParameterInfo[] Parameters)>();

        // Fetch methods from the UiPage script
        if (script != null)
        {
            methods = methods.Concat(
                script.GetType()
                    .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly)
                    .Where(m => m.ReturnType == typeof(void))
                    .Select(m => (m.Name, m.GetParameters()))
            );
        }

        // Fetch methods from the parent IMenuManager
        if (parentMenuManager != null)
        {
            methods = methods.Concat(
                parentMenuManager.GetType()
                    .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
                    .Where(m => m.ReturnType == typeof(void))
                    .Select(m => (m.Name, m.GetParameters()))
            );
        }

        // Add a "None" option and convert to array
        var methodsArray = methods
            .Prepend(("<None>", new ParameterInfo[0]))
            .ToArray();

        // Display the function dropdown
        string[] methodNames = methodsArray.Select(m => m.Item1).ToArray(); // Access the first element of the tuple
        int selectedIndex = Array.IndexOf(methodNames, functionNameProperty.stringValue);
        if (selectedIndex == -1) selectedIndex = 0;

        Rect dropdownRect = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight + 2, position.width, EditorGUIUtility.singleLineHeight);
        selectedIndex = EditorGUI.Popup(dropdownRect, "Click Function", selectedIndex, methodNames);

        // Update the property value
        functionNameProperty.stringValue = selectedIndex > 0 ? methodsArray[selectedIndex].Item1 : string.Empty;

        // If the selected method has parameters, show input fields
        var selectedMethod = methodsArray[selectedIndex];
        if (selectedMethod.Item2.Length > 0)  // Use Item2 to get the Parameters array
        {
            float yOffset = EditorGUIUtility.singleLineHeight * 2 + 4;
            for (int i = 0; i < selectedMethod.Item2.Length; i++) // Use Item2 to access parameters
            {
                var param = selectedMethod.Item2[i];  // Use Item2 to access the parameter info
                Rect paramRect = new Rect(position.x, position.y + yOffset, position.width, EditorGUIUtility.singleLineHeight);

                // Ensure the parameters array size matches the number of method parameters
                if (parametersProperty.arraySize <= i)
                {
                    parametersProperty.InsertArrayElementAtIndex(i);
                }

                SerializedProperty paramProperty = parametersProperty.GetArrayElementAtIndex(i);

                EditorGUI.BeginChangeCheck();

                // Handle different parameter types
                if (param.ParameterType == typeof(string))
                {
                    paramProperty.stringValue = EditorGUI.TextField(paramRect, $"{param.Name} ({param.ParameterType.Name})", paramProperty.stringValue);
                }
                else if (param.ParameterType == typeof(int))
                {
                    paramProperty.intValue = EditorGUI.IntField(paramRect, $"{param.Name} ({param.ParameterType.Name})", paramProperty.intValue);
                }
                else if (param.ParameterType == typeof(float))
                {
                    paramProperty.floatValue = EditorGUI.FloatField(paramRect, $"{param.Name} ({param.ParameterType.Name})", paramProperty.floatValue);
                }
                else if (param.ParameterType == typeof(bool))
                {
                    paramProperty.boolValue = EditorGUI.Toggle(paramRect, $"{param.Name} ({param.ParameterType.Name})", paramProperty.boolValue);
                }
                else if (param.ParameterType.IsSubclassOf(typeof(UnityEngine.Object)))
                {
                    // Handle Unity Object references (like UiPage or other Unity components)
                    paramProperty.objectReferenceValue = EditorGUI.ObjectField(paramRect, $"{param.Name} ({param.ParameterType.Name})", paramProperty.objectReferenceValue, param.ParameterType, true);
                }
                else
                {
                    // For unsupported types, just use the default string field
                    paramProperty.stringValue = EditorGUI.TextField(paramRect, $"{param.Name} ({param.ParameterType.Name})", paramProperty.stringValue);
                }

                yOffset += EditorGUIUtility.singleLineHeight + 2;
            }

            // Remove extra array elements
            while (parametersProperty.arraySize > selectedMethod.Item2.Length)
            {
                parametersProperty.DeleteArrayElementAtIndex(parametersProperty.arraySize - 1);
            }
        }
        else
        {
            // Clear parameters if the method takes no arguments
            parametersProperty.ClearArray();
        }

        EditorGUI.EndProperty();
    }




    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        var functionNameProperty = property.FindPropertyRelative("clickFunction");
        var parametersProperty = property.FindPropertyRelative("parameters");

        // Get the target object to check method parameters
        var script = property.serializedObject.targetObject as UiPage;
        var parentMenuManager = script?.GetComponentInParent<IMenuManager>();

        if (!string.IsNullOrEmpty(functionNameProperty.stringValue))
        {
            // Check methods in the script or the parent IMenuManager
            var method = script?.GetType()
                .GetMethod(functionNameProperty.stringValue, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly)
                ?? parentMenuManager?.GetType()
                    .GetMethod(functionNameProperty.stringValue, BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);

            if (method != null)
            {
                // Add height for parameters
                return EditorGUIUtility.singleLineHeight * (2 + method.GetParameters().Length) + 6;
            }
        }

        // Default height for button and dropdown
        return EditorGUIUtility.singleLineHeight * 2 + 4;
    }
}