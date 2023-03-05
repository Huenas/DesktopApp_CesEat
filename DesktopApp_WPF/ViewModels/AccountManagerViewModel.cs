using DesktopApp_WPF.CustomException;
using DesktopApp_WPF.Models;
using DesktopApp_WPF.Repositories;
using HandyControl.Tools.Command;
using System.Collections.Generic;
using System.Net;
using System.Security;
using System.Windows.Input;

namespace DesktopApp_WPF.ViewModels
{
    public class AccountManagerViewModel : ViewModelBase
    {
        private UserAccountModel _currentUserAccount;

        private IUserRepository userRepository;
        private SecureString _password;
        private string _errorMessage;
        private string _addUserValid;
        private string _colorMessage;
        private UserModel _currentUser;
        private List<UserModel> _allUser;
        private UserModel _selectedAccount;


        public UserModel SelectedAccount
        {
            get
            {
                return _selectedAccount;
            }

            set
            {

                _selectedAccount = value;
                OnPropertyChanged(nameof(SelectedAccount));
            }
        }



        public List<UserModel> AllUser
        {

            get
            {

                return _allUser;
            }
            set
            {
                _allUser = value;
                OnPropertyChanged(nameof(AllUser));
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
        public ICommand addAccountCommand { get; }
        public ICommand ShowPasswordCommand { get; }
        public ICommand RememberPasswordCommand { get; }

        public ICommand EditAccountCommand { get; private set; }
        public ICommand DeleteAccountCommand { get; private set; }
        //Constructor
        public AccountManagerViewModel()
        {


            userRepository = new UserRepository();
            EditAccountCommand = new RelayCommand(item => EditAccountManager(item));
            DeleteAccountCommand = new RelayCommand(item => DeleteAccountManager(item));


            CurrentUser = new UserModel();

            LoadCurrentUserData();

        }



        private void LoadCurrentUserData()
        {
            var user = userRepository.GetByAll();
            AllUser = new List<UserModel>();
            if (user != null)
            {
                foreach (var item in user)
                {
                    AllUser.Add(new UserModel()
                    {
                        Email = item.Email,
                        FirstName = item.FirstName,
                        LastName = item.LastName,
                        Id = item.Id,
                        Login = item.Login,
                        Service = item.Service,
                        Password = item.Password

                    }); ;
                }
            }
            else
            {
                CurrentUserAccount.DisplayName = "Invalid user, not logged in";

            }
        }

        private void EditAccountManager(object parameter)
        {
            NetworkCredential credential = new NetworkCredential("", Password);
            try
            {

                userRepository.Edit(SelectedAccount, credential);
                ErrorMessage = "User added with success ! ";
                ColorMessage = "#06D906";
            }
            catch (DbDuplicationException ex)
            {
                ErrorMessage = ex.Message;
                ColorMessage = "red";
            }

        }

        private void DeleteAccountManager(object parameter)
        {
            NetworkCredential credential = new NetworkCredential("", Password);
            try
            {
                int idToDelete = SelectedAccount.Id;
                userRepository.Remove(idToDelete);
                ErrorMessage = "Removed with success! ";
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
