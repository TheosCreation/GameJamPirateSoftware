using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

[System.Serializable]
public class UiPage : MonoBehaviour
{
    [SerializeField] protected UiButton[] buttons;

    // Cache for MethodInfo lookups
    private readonly Dictionary<string, MethodInfo> methodCache = new Dictionary<string, MethodInfo>();
    protected IMenuManager menuManager; // Parent IMenuManager reference

    protected virtual void Awake()
    {
        // Find the IMenuManager in the parent hierarchy
        menuManager = GetComponentInParent<IMenuManager>();
    }

    protected virtual void OnEnable()
    {
        if (buttons == null || buttons.Length == 0) return;

        foreach (UiButton uiButton in buttons)
        {
            if (uiButton.button != null && !string.IsNullOrEmpty(uiButton.clickFunction))
            {
                // Attempt to cache or retrieve the MethodInfo
                if (!methodCache.TryGetValue(uiButton.clickFunction, out MethodInfo method))
                {
                    // Check UiPage for the method
                    method = GetType().GetMethod(
                        uiButton.clickFunction,
                        BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy
                    );

                    // Check IMenuManager for the method if not found in UiPage
                    if (method == null && menuManager != null)
                    {
                        method = menuManager.GetType().GetMethod(
                            uiButton.clickFunction,
                            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy
                        );
                    }

                    if (method != null)
                    {
                        methodCache[uiButton.clickFunction] = method; // Cache the result
                    }
                    else
                    {
                        Debug.LogWarning($"Method '{uiButton.clickFunction}' not found in {GetType().Name} or {menuManager?.GetType().Name}.");
                        continue; // Skip this button if no valid method is found
                    }
                }

                // Add the listener if the method was found
                try
                {
                    uiButton.button.onClick.AddListener(() =>
                    {
                        InvokeMethod(uiButton, method);
                    });
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Error setting up method '{uiButton.clickFunction}' on {nameof(UiPage)}: {ex.Message}");
                }
            }
        }
    }

    private void InvokeMethod(UiButton uiButton, MethodInfo method)
    {
        try
        {
            // Convert string parameters to the method's expected types
            ParameterInfo[] paramInfos = method.GetParameters();
            object[] methodParams = new object[paramInfos.Length];

            for (int i = 0; i < paramInfos.Length; i++)
            {
                //if (uiButton.parameters != null && uiButton.parameters.Length > i)
                //{
                //     //methodParams[i] = ConvertParameter(uiButton.parameters[i], paramInfos[i].ParameterType);
                //}
                //else
                //{
                //    methodParams[i] = paramInfos[i].HasDefaultValue
                //        ? paramInfos[i].DefaultValue
                //        : GetDefault(paramInfos[i].ParameterType); // Fallback to default
                //}
            }

            method.Invoke(method.DeclaringType == GetType() ? this : menuManager, methodParams);
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error invoking method '{method.Name}' with parameters: {ex.Message}");
        }
    }

    private object ConvertParameter(string paramValue, Type targetType)
    {
        try
        {
            // Handle special cases like Unity objects or complex types
            if (targetType == typeof(Vector3))
            {
                return StringToVector3(paramValue);
            }
            else if (targetType == typeof(UnityEngine.Object))
            {
                return Resources.Load(paramValue); // Or handle appropriately
            }
            else
            {
                return Convert.ChangeType(paramValue, targetType);
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error converting parameter '{paramValue}' to type '{targetType.Name}': {ex.Message}");
            return null;
        }
    }

    private Vector3 StringToVector3(string str)
    {
        var values = str.Split(',');
        if (values.Length == 3)
        {
            try
            {
                return new Vector3(
                    float.Parse(values[0]),
                    float.Parse(values[1]),
                    float.Parse(values[2])
                );
            }
            catch (FormatException)
            {
                Debug.LogError($"Invalid Vector3 format: {str}");
                return Vector3.zero;
            }
        }
        return Vector3.zero;
    }

    private object GetDefault(Type type)
    {
        return type.IsValueType ? Activator.CreateInstance(type) : null;
    }

    protected virtual void OnDisable()
    {
        if (buttons == null || buttons.Length == 0) return;

        foreach (UiButton uiButton in buttons)
        {
            uiButton.button?.onClick.RemoveAllListeners();
        }

        // Clear method cache to prevent memory leaks
        methodCache.Clear();
    }

    public void SetActive(bool active)
    {
        gameObject.SetActive(active);
    }
}