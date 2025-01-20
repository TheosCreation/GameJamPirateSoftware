public class OptionsMenu : UiPage
{
    public void Back()
    {
        IMenuManager menuManager = GetComponentInParent<IMenuManager>();
        menuManager.Back();
    }
}