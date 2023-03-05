using System;

namespace DesktopApp_WPF.CustomException
{
    public class DbDuplicationException : Exception
    {
        public DbDuplicationException() : base("user already exists")
        {

        }
    }

}
