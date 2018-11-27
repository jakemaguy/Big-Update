using System;
using System.IO;
using System.Threading;

namespace BigUpdate
{
    class Program
    {
        static void PrintInstruction(string text)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(text);
            Console.ForegroundColor = ConsoleColor.White;
        }

        static void Main(string[] args)
        {
            // Initilizing Important Aspects At the start
            Console.Title = "Big Update";
            Console.CursorVisible = false;
            bool UserQuit = false;

            // Constructs MainMenu
            Menu menu = new Menu();
            menu.RenderMainMenu();
            while (!UserQuit)
            {
                string choice = menu.DetectKeyPress();
                switch (menu.MenuName)
                {
                    case "Main Menu":
                        switch (choice)
                        {
                            case "Database Operations":
                                menu.RenderDatabaseMenu();
                                menu.DisplayDbContacts();
                                break;
                            case "Enter Tasks For The Day":
                                menu.RenderTaskMenu();
                                Email_Operations.DisplayEmailBody();
                                break;
                            case "Quit and Move On with your life":
                                UserQuit = true;
                                break;
                            default:
                                break;
                        }
                        break;
                    
                    case "Database Menu":
                        switch (choice)
                        {
                            case "Go Back To Main menu":
                                menu.RenderMainMenu();
                                break;
                            case "Add Contact To Database":
                                menu.DrawContactAddMenu();
                                break;
                            case "Remove Contact From Database":
                                menu.DrawContactRemoveMenu();
                                break;
                            default:
                                break;
                        }
                        break;

                    case "Task Menu":
                        switch(choice)
                        {
                            case "Enter A Task":
                                Email_Operations.AddTask();
                                menu.RenderTaskMenu();
                                Email_Operations.DisplayEmailBody();
                                break;
                            case "Compose Email":
                                menu.DisplayDbContacts();
                                Email_Operations.ComposeEmail();
                                menu.RenderTaskMenu();
                                Email_Operations.DisplayEmailBody();
                                break;
                            case "Go Back To Main menu":
                                menu.RenderMainMenu();
                                break;
                            default:
                                break;
                        }
                        break;
                }
            }
        }
    }
}
