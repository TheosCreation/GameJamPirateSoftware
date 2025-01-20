using System.Collections.Generic;
using System.Linq;
using System.Reflection;

public class MainMenuManager : Singleton<MainMenuManager>, IMenuManager
{
    private readonly Stack<UiPage> navigationHistory = new Stack<UiPage>();

    public StartMenu startMenu;
    public OptionsMenu optionsPage;

    private UiPage[] allUiPages;

    protected override void Awake()
    {
        base.Awake();

        //get all UiPage attributes and add them into a array
        allUiPages = GetAllUiPages();
    }

    void Start()
    {
        OpenPage(startMenu);
    }

    public void OpenPage(UiPage page)
    {
        navigationHistory.Push(page);
        ActivatePage(page);
    }

    private UiPage[] GetAllUiPages()
    {
        // Use reflection to find all public fields of type UiPage
        var fields = GetType()
            .GetFields(BindingFlags.Public | BindingFlags.Instance)
            .Where(field => typeof(UiPage).IsAssignableFrom(field.FieldType));

        return fields
            .Select(field => field.GetValue(this) as UiPage)
            .Where(page => page != null) // Filter out any null entries
            .ToArray();
    }

    private void ActivatePage(UiPage uiPageToActivate)
    {
        // Deactivate all pages first
        foreach (UiPage uiPage in allUiPages)
        {
            uiPage.SetActive(false);   
        }

        // Finally, activate the selected page
        uiPageToActivate.SetActive(true);
    }

    public void Back()
    {
        if (navigationHistory.Count > 1) // Ensure there's a page to go back to
        {
            // Pop the current page and activate the previous one
            navigationHistory.Pop();
            var previousPage = navigationHistory.Peek();
            ActivatePage(previousPage);
        }
    }
}
