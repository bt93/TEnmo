using System;
using System.Collections.Generic;
using TenmoClient.Data;
using TenmoServer.Models;

namespace TenmoClient
{
    class Program
    {
        private static readonly ConsoleService consoleService = new ConsoleService();
        private static readonly AuthService authService = new AuthService();
        private static ApiService apiService;

        static void Main(string[] args)
        {
            Run();
        }
        private static void Run()
        {
            while (true)
            {
                int loginRegister = -1;
                while (loginRegister != 1 && loginRegister != 2)
                {
                    Console.Clear();
                    Console.WriteLine("Welcome to TEnmo!");
                    Console.WriteLine("1: Login");
                    Console.WriteLine("2: Register");
                    Console.WriteLine("0: Exit");
                    Console.Write("Please choose an option: ");

                    if (!int.TryParse(Console.ReadLine(), out loginRegister))
                    {
                        Console.WriteLine("Invalid input. Please enter only a number.");
                    }
                    else if (loginRegister == 0)
                    {
                        Environment.Exit(0);
                    }
                    else if (loginRegister == 1)
                    {
                        while (!UserService.IsLoggedIn()) //will keep looping until user is logged in
                        {
                            LoginUser loginUser = consoleService.PromptForLogin();
                            API_User user = authService.Login(loginUser);
                            if (user != null)
                            {
                                UserService.SetLogin(user);
                                apiService = new ApiService();
                            }
                        }
                    }
                    else if (loginRegister == 2)
                    {
                        bool isRegistered = false;
                        while (!isRegistered) //will keep looping until user is registered
                        {
                            LoginUser registerUser = consoleService.PromptForLogin();
                            isRegistered = authService.Register(registerUser);
                            if (isRegistered)
                            {
                                Console.WriteLine("");
                                Console.WriteLine("Registration successful. You can now log in.");
                                loginRegister = -1; //reset outer loop to allow choice for login
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid selection.");
                    }
                }

                MenuSelection();
            }
        }

        private static void MenuSelection()
        {
            int menuSelection = -1;
            while (menuSelection != 0)
            {
                Console.Clear();
                Console.WriteLine("");
                Console.WriteLine("Welcome to TEnmo! Please make a selection: ");
                Console.WriteLine("1: View your current balance");
                Console.WriteLine("2: View your past transfers");
                Console.WriteLine("3: View your pending requests"); //Bonus
                Console.WriteLine("4: Send TE bucks");
                Console.WriteLine("5: Request TE bucks"); //Bonus
                Console.WriteLine("6: Log in as different user");
                Console.WriteLine("0: Exit");
                Console.WriteLine("---------");
                Console.Write("Please choose an option: ");

                if (!int.TryParse(Console.ReadLine(), out menuSelection))
                {
                    Console.WriteLine("Invalid input. Please enter only a number.");
                    menuSelection = -1;
                }
                else if (menuSelection == 1)
                {
                    // View your current balance
                    try
                    {
                        Account account = apiService.GetBalance();

                        Console.WriteLine($"Current balance: {account.Balance:C}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    Console.Write("Press Enter to continue.");
                    Console.ReadLine();
                }
                else if (menuSelection == 2)
                {
                    // View your past transfers
                    try
                    {
                        List<Transfer> transfers = apiService.GetTransfers();
                        foreach (Transfer transfer in transfers)
                        {
                            Console.WriteLine(transfer);
                        }
                        int transferId = consoleService.PromptForTransferID("pick a transfer");
                        Transfer singleTransfer = apiService.GetTransferById(transferId);
                        Console.WriteLine(singleTransfer.DetailsForTransfer());
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    Console.Write("Press Enter to continue.");
                    Console.ReadLine();
                }
                else if (menuSelection == 3)
                {
                    // View your pending requests
                    Console.Write("Press Enter to continue.");
                    Console.ReadLine();
                }
                else if (menuSelection == 4)
                {
                    // Send TE bucks
                    try
                    {
                        Console.Clear();
                        //Arrange
                        int toUserId = 0;
                        decimal amount = -1;
                        List<ReturnUser> users = apiService.GetAllUsers();
                        Dictionary<int, ReturnUser> userMenu = new Dictionary<int, ReturnUser>();
                        // making a dictionary for menu options and using int key for access to ReturnUsers
                        int key = 1;
                        foreach(ReturnUser user in users)
                        {
                            userMenu.Add(key, user);
                            key++;
                        }

                        foreach (var option in userMenu)
                        {
                            Console.WriteLine($"{option.Key}: {option.Value.Username}");
                        }

                        // Choose a user and amount
                        bool isValid = false;
                        while (!isValid)
                        {
                            toUserId = consoleService.PromptForUserID("transfer TE bucks to");
                            if (userMenu.ContainsKey(toUserId))
                            {
                                isValid = true;
                            }
                            else
                            {
                                Console.WriteLine("Please enter a valid user Id.");
                            }
                        }
                        
                        while (amount == -1)
                        {
                            amount = consoleService.PromtForAmount("How much would you like to transfer?");
                        }
                        if (amount == 0)
                        {
                            continue;
                        }

                        // Enter in transfer data
                        int fromUserId = UserService.GetUserId();
                        Transfer newTransfer = new Transfer(toUserId, fromUserId, amount);
                        // Send it
                        newTransfer = apiService.SendTEBucks(newTransfer);
                        Console.WriteLine(newTransfer.ToString());
                    }
                    catch (Exception x)
                    {
                        Console.WriteLine(x.Message);
                    }
                    Console.Write("Press Enter to continue.");
                    Console.ReadLine();
                }
                else if (menuSelection == 5)
                {
                    // Request TE bucks
                    try
                    {
                        Console.Clear();
                        //Arrange
                        int fromUserId = 0;
                        decimal amount = -1;
                        List<ReturnUser> users = apiService.GetAllUsers();
                        Dictionary<int, ReturnUser> userMenu = new Dictionary<int, ReturnUser>();
                        // making a dictionary for menu options and using int key for access to ReturnUsers
                        int key = 1;
                        foreach (ReturnUser user in users)
                        {
                            userMenu.Add(key, user);
                            key++;
                        }

                        foreach (var option in userMenu)
                        {
                            Console.WriteLine($"{option.Key}: {option.Value.Username}");
                        }

                        // Choose a user and amount
                        bool isValid = false;
                        while (!isValid)
                        {
                            fromUserId = consoleService.PromptForUserID("transfer TE bucks to");
                            if (userMenu.ContainsKey(fromUserId))
                            {
                                isValid = true;
                            }
                            else
                            {
                                Console.WriteLine("Please enter a valid user Id.");
                            }
                        }

                        while (amount == -1)
                        {
                            amount = consoleService.PromtForAmount("How much would you like to request?");
                        }
                        if (amount == 0)
                        {
                            continue;
                        }

                        // Enter in transfer data
                        int toUserId = UserService.GetUserId();
                        Transfer newTransfer = new Transfer(toUserId, fromUserId, amount);
                        // Send it
                        newTransfer = apiService.RequestTransfer(newTransfer);
                        Console.WriteLine(newTransfer.ToString());
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                    Console.Write("Press Enter to continue.");
                    Console.ReadLine();
                }
                else if (menuSelection == 6)
                {
                    // Log in as different user
                    Console.WriteLine("");
                    UserService.SetLogin(new API_User()); //wipe out previous login info
                    return; //return to entry point
                    Console.Write("Press Enter to continue.");
                    Console.ReadLine();
                }
                else
                {
                    Console.WriteLine("Goodbye!");
                    Environment.Exit(0);
                }
            }
        }
    }
}
