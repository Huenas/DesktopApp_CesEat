using DesktopApp_WPF.Models;
using DesktopApp_WPF.Repositories;
using FontAwesome.Sharp;
using System.Threading;
using System.Windows.Input;

namespace DesktopApp_WPF.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        //Fields
        private UserAccountModel _currentUserAccount;
        private ViewModelBase _currentChildView;
        private string _caption;
        private string _visibilityService = "Collapsed";
        private string _visibilityCommercialService = "Collapsed";
        private string _visibilityTechniqueService = "Collapsed";
        private IconChar _icon;
        private bool _isViewVisible = false;
        private bool _isButtonPressed = false;

        private IUserRepository userRepository;


        public UserAccountModel CurrentUserAccount
        {
            get
            {
                return _currentUserAccount;
            }

            set
            {
                _currentUserAccount = value;
                OnPropertyChanged(nameof(CurrentUserAccount));
            }
        }

        public ViewModelBase CurrentChildView
        {
            get
            {
                return _currentChildView;
            }
            set
            {
                _currentChildView = value;
                OnPropertyChanged(nameof(CurrentChildView));
            }
        }
        public string Caption
        {
            get
            {
                return _caption;
            }
            set
            {
                _caption = value;
                OnPropertyChanged(nameof(Caption));
            }
        }

        public bool IsButtonPressed
        {
            get
            {
                return _isButtonPressed;
            }

            set
            {
                _isButtonPressed = value;
                OnPropertyChanged(nameof(IsButtonPressed));
            }
        }

        public bool IsViewVisible
        {
            get
            {
                return _isViewVisible;
            }

            set
            {
                _isViewVisible = value;
                OnPropertyChanged(nameof(IsViewVisible));
            }
        }


        public string VisibilityCommercialService
        {
            get
            {
                return _visibilityCommercialService;
            }
            set
            {
                _visibilityCommercialService = value;
                OnPropertyChanged(nameof(_visibilityCommercialService));
            }
        }

        public string VisibilityTechniqueService
        {
            get
            {
                return _visibilityTechniqueService;
            }
            set
            {
                _visibilityTechniqueService = value;
                OnPropertyChanged(nameof(VisibilityTechniqueService));
            }
        }

        public string VisibilityService
        {
            get
            {
                return _visibilityService;
            }
            set
            {
                _visibilityService = value;
                OnPropertyChanged(nameof(VisibilityService));
            }
        }

        public IconChar Icon
        {
            get
            {
                return _icon;
            }

            set
            {
                _icon = value;
                OnPropertyChanged(nameof(Icon));
            }
        }

        public ICommand LogoutCommand { get; }

        // -> Commands
        public ICommand ShowHomeViewCommand { get; }
        public ICommand ShowAccountViewCommand { get; }
        public ICommand ShowAccountManagerViewCommand { get; }
        public ICommand ShowDashboardViewCommand { get; }
        public ICommand ShowStatNpmPackageViewCommand { get; }
        public ICommand ShowRestaurantViewCommand { get; }
        public ICommand ShowLivreurViewCommand { get; }
        public ICommand ShowCommStatViewCommand { get; }
        public ICommand ShowStatisticsViewCommand { get; }
        public ICommand ShowCommercialStatisticsViewCommand { get; }
        public ICommand ShowAddUserViewCommand { get; }


        public MainViewModel()
        {
            userRepository = new UserRepository();
            CurrentUserAccount = new UserAccountModel();

            //Initialize commands
            ShowHomeViewCommand = new ViewModelCommand(ExecuteShowHomeViewCommand);
            ShowAccountViewCommand = new ViewModelCommand(ExecuteShowAccountViewCommand);
            ShowAccountManagerViewCommand = new ViewModelCommand(ExecuteShowAccountManagerViewCommand);
            ShowDashboardViewCommand = new ViewModelCommand(ExecuteShowDashboardViewCommand);
            ShowLivreurViewCommand = new ViewModelCommand(ExecuteShowLivreurViewCommand);
            ShowRestaurantViewCommand = new ViewModelCommand(ExecuteShowRestaurantViewCommand);
            ShowStatNpmPackageViewCommand = new ViewModelCommand(ExecuteShowStatNpmPackageViewCommand);
            ShowStatisticsViewCommand = new ViewModelCommand(ExecuteShowStatisticsViewCommand);
            ShowCommStatViewCommand = new ViewModelCommand(ExecuteShowCommStatsViewCommand);
            ShowCommercialStatisticsViewCommand = new ViewModelCommand(ExecuteShowCommercialStatisticsViewCommand);
            ShowAddUserViewCommand = new ViewModelCommand(ExecuteShowAddUserViewCommand);
            LogoutCommand = new ViewModelCommand(ExecuteLogoutCommand);

            //default view
            ExecuteShowHomeViewCommand(null);
            VisibilityAddUserButton();
            VisibilityCommercialButton();
            VisibilityTechniqueButton();


            LoadCurrentUserData();
        }

        private void ExecuteLogoutCommand(object obj)
        {

            IsButtonPressed = true;
            IsViewVisible = true;

        }

        private void VisibilityAddUserButton()
        {
            var user = userRepository.GetByUsername(Thread.CurrentPrincipal.Identity.Name);
            if (user != null && (user.Service == "2"))
            {
                VisibilityService = "Visible";
                VisibilityTechniqueService = "Visible";
                VisibilityCommercialService = "Visible";
            }

        }
        private void VisibilityCommercialButton()
        {
            var user = userRepository.GetByUsername(Thread.CurrentPrincipal.Identity.Name);
            if (user != null && (user.Service == "1" | user.Service == "2"))
            {
                VisibilityCommercialService = "Visible";

            }

        }
        private void VisibilityTechniqueButton()
        {
            var user = userRepository.GetByUsername(Thread.CurrentPrincipal.Identity.Name);
            if (user != null && (user.Service == "3" | user.Service == "2"))
            {
                VisibilityTechniqueService = "Visible";
            }

        }

        private void ExecuteShowAddUserViewCommand(object obj)
        {
            CurrentChildView = new AddUserViewModel();
            Caption = "Add User";
            Icon = IconChar.UserPlus;
        }


        private void ExecuteShowStatisticsViewCommand(object obj)
        {
            CurrentChildView = new ContainerViewModel();
            Caption = "Container Statistics";
            Icon = IconChar.ChartArea;
        }

        private void ExecuteShowCommStatsViewCommand(object obj)
        {
            CurrentChildView = new CommStatViewModel();
            Caption = "Commercial Statistics";
            Icon = IconChar.ChartArea;
        }

        private void ExecuteShowStatNpmPackageViewCommand(object obj)
        {
            CurrentChildView = new StatNpmPackageViewModel();
            Caption = "Npm Package Statistics";
            Icon = IconChar.ChartArea;
        }


        private void ExecuteShowCommercialStatisticsViewCommand(object obj)
        {
            CurrentChildView = new CommercialStatisticsViewModel();
            Caption = "Commercial Statistics";
            Icon = IconChar.ChartArea;
        }


        private void ExecuteShowDashboardViewCommand(object obj)
        {
            CurrentChildView = new DashboardViewModel();
            Caption = "Client";
            Icon = IconChar.GripHorizontal;
        }

        private void ExecuteShowLivreurViewCommand(object obj)
        {
            CurrentChildView = new LivreurViewModel();
            Caption = "Livreur";
            Icon = IconChar.GripHorizontal;
        }

        private void ExecuteShowRestaurantViewCommand(object obj)
        {
            CurrentChildView = new RestaurantViewModel();
            Caption = "Restaurant";
            Icon = IconChar.GripHorizontal;
        }

        private void ExecuteShowAccountViewCommand(object obj)
        {
            CurrentChildView = new AccountViewModel();
            Caption = "Accounts";
            Icon = IconChar.UserGroup;
        }

        private void ExecuteShowAccountManagerViewCommand(object obj)
        {
            CurrentChildView = new AccountManagerViewModel();
            Caption = "Account Manager";
            Icon = IconChar.UserGroup;
        }


        private void ExecuteShowHomeViewCommand(object obj)
        {
            CurrentChildView = new HomeViewModel();
            Caption = "Home";
            Icon = IconChar.Home;
        }

        private void LoadCurrentUserData()
        {
            var user = userRepository.GetByUsername(Thread.CurrentPrincipal.Identity.Name);
            if (user != null)
            {
                CurrentUserAccount.Username = user.FirstName;
                CurrentUserAccount.DisplayName = $"{user.FirstName} {user.LastName}";
                CurrentUserAccount.ProfilePicture = null;
            }
            else
            {
                CurrentUserAccount.DisplayName = "Invalid user, not logged in";
            }
        }
    }
}
