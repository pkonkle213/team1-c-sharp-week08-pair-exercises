using System;
using System.Collections.Generic;
using TenmoClient.APIClients;
using TenmoClient.Data;
using TenmoClient.Models;

namespace TenmoClient
{
    public class UserInterface
    {
        private readonly ConsoleService consoleService = new ConsoleService();
        private readonly AuthService authService = new AuthService();
        private readonly ClientUserService newService = new ClientUserService();
        private readonly NewerService newNewerService = new NewerService();

        private bool quitRequested = false;

        public void Start()
        {
            while (!quitRequested)
            {
                while (!UserService.IsLoggedIn)
                {
                    ShowLogInMenu();
                }

                // If we got here, then the user is logged in. Go ahead and show the main menu
                ShowMainMenu();
            }
        }

        private void ShowLogInMenu()
        {
            Console.WriteLine("Welcome to TEnmo!");
            Console.WriteLine("1: Login");
            Console.WriteLine("2: Register");
            Console.Write("Please choose an option: ");

            if (!int.TryParse(Console.ReadLine(), out int loginRegister))
            {
                Console.WriteLine("Invalid input. Please enter only a number.");
            }
            else if (loginRegister == 1)
            {
                HandleUserLogin();
            }
            else if (loginRegister == 2)
            {
                HandleUserRegister();
            }
            else
            {
                Console.WriteLine("Invalid selection.");
            }
        }

        private void ShowMainMenu()
        {
            int menuSelection;
            do
            {
                Console.WriteLine();
                Console.WriteLine("Welcome to TEnmo! Please make a selection: ");
                Console.WriteLine("1: View your current balance");
                Console.WriteLine("2: View your past transfers");
                Console.WriteLine("3: View your pending requests");
                Console.WriteLine("4: Send TE bucks");
                Console.WriteLine("5: Request TE bucks");
                Console.WriteLine("6: Log in as different user");
                Console.WriteLine("0: Exit");
                Console.WriteLine("---------");
                Console.Write("Please choose an option: ");

                if (!int.TryParse(Console.ReadLine(), out menuSelection))
                {
                    Console.WriteLine("Invalid input. Please enter only a number.");
                }
                else
                {
                    switch (menuSelection)
                    {
                        case 1: // View Balance
                            ViewBalance();
                            break;

                        case 2: // View Past Transfers
                            Console.WriteLine("NOT IMPLEMENTED!"); // TODO: Implement me
                            break;

                        case 3: // View Pending Requests
                            Console.WriteLine("NOT IMPLEMENTED!"); // TODO: Implement me
                            break;

                        case 4: // Send TE Bucks
                            SendBucks();
                            break;

                        case 5: // Request TE Bucks
                            Console.WriteLine("NOT IMPLEMENTED!"); // TODO: Implement me
                            break;

                        case 6: // Log in as someone else
                            Console.WriteLine();
                            UserService.ClearLoggedInUser(); //wipe out previous login info
                            return; // Leaves the menu and should return as someone else

                        case 0: // Quit
                            Console.WriteLine("Goodbye!");
                            quitRequested = true;
                            return;

                        default:
                            Console.WriteLine("That doesn't seem like a valid choice.");
                            break;
                    }
                }
            } while (menuSelection != 0);
        }

        private void HandleUserRegister()
        {
            bool isRegistered = false;

            while (!isRegistered) //will keep looping until user is registered
            {
                LoginUser registerUser = consoleService.PromptForLogin();
                isRegistered = authService.Register(registerUser);
            }

            Console.WriteLine("");
            Console.WriteLine("Registration successful. You can now log in.");
        }

        private void HandleUserLogin()
        {
            while (!UserService.IsLoggedIn) //will keep looping until user is logged in
            {
                LoginUser loginUser = consoleService.PromptForLogin();
                authService.Login(loginUser);
            }
        }

        private void ViewBalance()
        {
            Console.WriteLine();
            Console.WriteLine("Your current account balance is: " + newService.Balance(UserService.Token).ToString("C"));
        }

        private void SendBucks()
        {
            Console.WriteLine();
            Console.WriteLine("------------------------------------");
            Console.WriteLine("Users");
            Console.WriteLine("ID       Name");
            Console.WriteLine("------------------------------------");

            //Call some method to get a list users by ID and name (user model?)
            List<User> users = newService.AllUsers(UserService.Token);
            List<int> ids = new List<int>();
            //foreach loop to actually write information
            foreach (User user in users)
            {
                Console.WriteLine($"{user.UserId} {user.Username}");
                ids.Add(user.UserId);
            }

            Console.WriteLine("------------------------------------");

            //start a while loop for accurate information entered by the user
            bool sweetness = false;
            while (!sweetness)
            {
                try
                {
                    Console.WriteLine();
                    Console.Write("Enter ID of user you are sending to (0 to cancel): ");
                    string answerId = Console.ReadLine();
                    Console.Write("Enter amount: ");
                    string answerAmt = Console.ReadLine();
                    int destinationId = int.Parse(answerId);
                    decimal amount = decimal.Parse(answerAmt);

                    //Need to check if amount is less than what is in their balance
                    if (amount > newService.Balance(UserService.Token))
                    {
                        Console.WriteLine("You wish you had that much money. Please try again.");
                    }
                    else
                    {
                        //Need to check if destination ID is 0 for cancel
                        if (destinationId == 0)
                        {
                            sweetness = true;
                        }
                        //Need to check if destination ID is valid
                        else if (ids.Contains(destinationId))
                        {
                            //Call to another method to transfer funds
                            newNewerService.TransferFunds(destinationId, amount, UserService.Token);
                            sweetness = true;
                        }
                    }
                }
                catch (FormatException ex)
                {
                    Console.WriteLine("How about you try putting in some valid data for once?");
                }
            }
        }
    }
}
