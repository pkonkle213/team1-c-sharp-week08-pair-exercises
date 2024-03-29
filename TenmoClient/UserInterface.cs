﻿using System;
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
        private readonly ClientUserService clientUserService = new ClientUserService();
        private readonly TransferService transferService = new TransferService();

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
                            ViewPastTransfers();
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
            Console.WriteLine("Your current account balance is: " + clientUserService.Balance(UserService.Token).ToString("C"));
        }

        private void SendBucks()
        {
            Console.WriteLine();
            Console.WriteLine("------------------------------------");
            Console.WriteLine("Users");
            Console.WriteLine("ID       Name");
            Console.WriteLine("------------------------------------");

            //Call some method to get a list users by ID and name (user model?)
            List<User> users = clientUserService.AllUsers(UserService.Token);
            List<int> ids = new List<int>();

            //foreach loop to actually write information
            foreach (User user in users)
            {
                Console.WriteLine($"{user.UserId}     {user.Username}");
                ids.Add(user.UserId);
            }

            Console.WriteLine("------------------------------------");

            //start a while loop for accurate information entered by the user
            bool quit = false;
            while (!quit)
            {
                try
                {
                    Console.WriteLine();
                    Console.Write("Enter ID of user you are sending to (0 to cancel): ");
                    string answerId = Console.ReadLine();
                    int destinationId = int.Parse(answerId);
                    //Need to check if destination ID is 0 for cancel
                    if (destinationId == 0)
                    {
                        quit = true;
                    }
                    else if (!ids.Contains(destinationId))
                    {
                        Console.WriteLine("User not found. Please enter a valid id.");
                    }
                    else
                    {
                        Console.Write("Enter amount: ");
                        string answerAmt = Console.ReadLine();
                        decimal amount = decimal.Parse(answerAmt);

                        //Need to check if amount is less than what is in their balance
                        if (amount > clientUserService.Balance(UserService.Token))
                        {
                            Console.WriteLine("Insufficient funds. Please check your balance and enter new amount.");
                        }
                        else if (amount <= 0)
                        {
                            Console.WriteLine("Please enter an amount greater than $0.");
                        }
                        else
                        {
                            //Call to another method to transfer funds
                            transferService.TransferFunds(destinationId, amount, UserService.Token);
                            quit = true;
                            Console.WriteLine("Transfer successful!!");
                        }
                    }
                }
                catch (FormatException ex)
                {
                    Console.WriteLine("Invalid input. Please try again.");
                }
            }
        }

        public void ViewPastTransfers()
        {
            int padFirst = 5;
            int padSecond = 7;
            int padThird = 15;

            Console.WriteLine();
            Console.WriteLine("------------------------------------");
            Console.WriteLine("Transfers");
            Console.WriteLine("ID     From/To              Amount");
            Console.WriteLine("------------------------------------");

            List<Transfer> transfers = transferService.AllTransfers(UserService.Token);
            List<int> ids = new List<int>();

            // THE MIDDLE
            foreach (Transfer transfer in transfers)
            {
                Console.Write(transfer.TransferId.ToString().PadRight(padFirst));
                Console.Write(transfer.Direction.PadLeft(padSecond));
                Console.Write(transfer.Username.ToString().PadRight(padThird));
                Console.WriteLine(transfer.TransferAmount.ToString("C"));
                ids.Add(transfer.TransferId);
            }

            Console.WriteLine("------------------------------------");
            bool cancel = false;
            while (!cancel)
            {
                try
                {
                    Console.WriteLine();
                    Console.Write("Please enter transfer ID to view details (0 to cancel): ");
                    string answerId = Console.ReadLine();
                    int transferId = int.Parse(answerId);
                    if (transferId == 0)
                    {
                        cancel = true;
                    }
                    //Need to check if destination ID is valid
                    else if (ids.Contains(transferId))
                    {
                        //Call to another method to transfer funds
                        Transfer transfer = transferService.SpecificTransfer(transferId, UserService.Token);
                        Console.WriteLine("--------------------------------------------");
                        Console.WriteLine("Transfer Details");
                        Console.WriteLine("--------------------------------------------");
                        Console.WriteLine($"Id: {transfer.TransferId}");
                        Console.WriteLine($"From: {transfer.SenderName}");
                        Console.WriteLine($"To: {transfer.ReceiverName}");
                        Console.WriteLine($"Type: {transfer.TransferType}");
                        Console.WriteLine($"Status: {transfer.TransferStatus}");
                        Console.WriteLine($"Amount: {transfer.TransferAmount.ToString("C")}");
                        cancel = true;
                    }
                    else
                    {
                        Console.WriteLine("Please enter a valid Transfer ID.");
                    }
                }
                catch (FormatException ex)
                {
                    Console.WriteLine("Invalid input. Please try again.");
                }
            }
        }
    }
}
