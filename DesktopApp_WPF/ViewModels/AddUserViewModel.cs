using DesktopApp_WPF.CustomException;
using DesktopApp_WPF.Models;
using DesktopApp_WPF.Repositories;
using System.Net;
using System.Security;
using System.Windows.Input;

namespace DesktopApp_WPF.ViewModels
{
    public class AddUserViewModel : ViewModelBase
    {
        //Fields

        private string _firstname;
        private SecureString _password;
        private string _errorMessage;
        private string _addUserValid;
        private string _colorMessage;
        private UserModel _userToAdd;

        private IUserRepository userRepository;

        //Properties
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

        //properties
        public UserModel UserToAdd
        {
            get
            {
                return _userToAdd;
            }

            set
            {
                _userToAdd = value;
                OnPropertyChanged(nameof(UserToAdd));
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
        public ICommand addAccountCommand { get; }
        public ICommand ShowPasswordCommand { get; }
        public ICommand RememberPasswordCommand { get; }


        //Constructor
        public AddUserViewModel()
        {
            UserToAdd = new UserModel();

            userRepository = new UserRepository();
            addAccountCommand = new ViewModelCommand(ExecuteAddUserCommand);
        }


        private void ExecuteAddUserCommand(object obj)
        {
            NetworkCredential credential = new NetworkCredential("", Password);
            try
            {
                userRepository.Add(UserToAdd, credential);
                ErrorMessage = "User added with success ! ";
                ColorMessage = "#06D906";
            }
            catch (DbDuplicationException ex)
            {
                ErrorMessage = ex.Message;
                ColorMessage = "red";
            }


        }


    }
}
