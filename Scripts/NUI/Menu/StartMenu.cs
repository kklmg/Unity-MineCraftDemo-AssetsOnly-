
namespace Assets.Scripts.NUI
{
    class StartMenu : MenuBase
    {
        public SwitchMenuButton StartButton;
        public ExitButton ExitButton;

        public MenuBase WorldMenu;

        protected override void Init()
        {
            StartButton.SetDestMenu(WorldMenu);
        }
    }
}
