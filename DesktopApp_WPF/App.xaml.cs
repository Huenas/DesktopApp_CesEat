using DesktopApp_WPF.Views;
using System.Windows;

namespace DesktopApp_WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected void ApplicationStart(object sender, StartupEventArgs e)
        {

            var loginView = new LoginView();


            loginView.Show();

            loginView.IsVisibleChanged += (s, ev) =>
              {
                  if (loginView.IsVisible == false && loginView.IsLoaded && loginView.IsSignupVisible == true)
                  {
                      var mainView = new MainView();
                      // mainView.Show();
                      //  loginView.Close();

                  }


              };
            /*
            var mainView1 = new MainView();
            mainView1.IsVisibleChanged += (s, ev) =>
            {
                if (mainView1.IsVisible==false && mainView1.IsLoaded==false && mainView1.IsButtonLogPressed == true)
                {
                    Console.WriteLine("sdfsdf");
                    var loginView1 = new LoginView();
                    loginView1.Show();
                    loginView1.IsVisibleChanged += (s1, ev1) =>
                    {
                        if (loginView1.IsVisible == false && loginView1.IsLoaded && loginView1.IsSignupVisible == true)
                        {
                            var mainView2 = new MainView();
                            mainView2.Show();
                            
                            loginView1.Close();


                        }
                    };
                }
               
            };*/





        }
    }
}
