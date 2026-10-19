namespace week7
{
    public class Employee
    {
        public int EmployeeID { get; set; }
        public string LoginName { get; set; } = "";
        public string Password { get; set; } = "";

        // Definisikan Constructor
        public Employee() { }

        public Employee(string loginName, string password)
        {
            LoginName = loginName;
            Password = password;
        }

        public bool Login()
        {
            if (LoginName == "Jono" && Password == "wkkw")
            {
                EmployeeID = 1;
                return true;
            }
            else if (LoginName == "Jones" && Password == "haha")
            {
                EmployeeID = 2;
                return true;
            }
            else
            {
                return false;
            }
        }

        // Tetap ada karena btnLogin_Click di modul memanggil Login(LoginName, Password)
        public bool Login(string loginName, string password)
        {
            LoginName = loginName;
            Password = password;
            return Login();
        }
    }
}
