namespace TextQuestGame
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            using (var appController = new Presenter.ApplicationController())
            {
                appController.Start();
            }
        }
    }
}