using DesktopApp_WPF.CustomException;
using DesktopApp_WPF.Models;
using DesktopApp_WPF.Repositories;
using System.Net;
using System.Security;
using System.Threading;
using System.Windows.Input;

namespace DesktopApp_WPF.ViewModels
{
    public class AccountViewModel : ViewModelBase
    {
        private UserAccountModel _currentUserAccount;
        private ViewModelBase _currentChildView;
        private IUserRepository userRepository;

        private string _firstname;
        private SecureString _password;
        private string _errorMessage;
        private string _addUserValid;
        private string _colorMessage;
        private UserModel _currentUser;
        private UserModel _userToModify;

        public UserModel UserToModify
        {
            get
            {
                return _userToModify;
            }

            set
            {
                _userToModify = value;
                OnPropertyChanged(nameof(UserToModify));
            }
        }


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


        public string FirstName
        {
            get
            {
                return _firstname;
            }

            set
            {
                _firstname = value;
                OnPropertyChanged(nameof(FirstName));
            }
        }


        public UserModel CurrentUser
        {
            get
            {
                return _currentUser;
            }

            set
            {
                _currentUser = value;
                OnPropertyChanged(nameof(CurrentUser));
            }
        }

        public SecureString Password
        {
            get
            {
                return _password;
            }

            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        public string ErrorMessage
        {
            get
            {
                return _errorMessage;
            }

            set
            {
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }

        public string ColorMessage
        {
            get
            {
                return _colorMessage;
            }

            set
            {
                _colorMessage = value;
                OnPropertyChanged(nameof(ColorMessage));
            }
        }


        public string AddUserValid
        {
            get
            {
                return _addUserValid;
            }

            set
            {
                _addUserValid = value;
                OnPropertyChanged(nameof(AddUserValid));
            }
        }

        //-> Commands
        public ICommand ModifyAccountCommand { get; }
        public ICommand ShowPasswordCommand { get; }
        public ICommand RememberPasswordCommand { get; }


        //Constructor
        public AccountViewModel()
        {


            userRepository = new UserRepository();

            ModifyAccountCommand = new ViewModelCommand(ExecuteModifyUserCommand);

            CurrentUser = new UserModel();

            LoadCurrentUserData();

        }


        private void ExecuteModifyUserCommand(object obj)
        {
            NetworkCredential credential = new NetworkCredential("", Password);
            try
            {

                userRepository.Edit(CurrentUser, credential);
                ErrorMessage = "User modified with success ! ";
                ColorMessage = "#06D906";
            }
            catch (DbDuplicationException ex)
            {
                ErrorMessage = ex.Message;
                ColorMessage = "red";
            }


        }

        private void LoadCurrentUserData()
        {
            var user = userRepository.GetByUsername(Thread.CurrentPrincipal.Identity.Name);
            if (user != null)
            {
                CurrentUser.Id = user.Id;
                CurrentUser.Login = user.Login;
                CurrentUser.Email = user.Email;
                CurrentUser.Password = user.Password;
            }
            else
            {
                CurrentUserAccount.DisplayName = "Invalid user, not logged in";

            }
        }




    }
}
