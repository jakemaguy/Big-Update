using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BigUpdate
{
    public enum MenuHeight { Item1 = 2, Item2 = 4, Item3 = 6, Item4 = 8 }

    struct MenuItem
    {
        public MenuItem(string name, MenuHeight height, ref List<MenuItem> Items) : this()
        {
            Name = name;
            Height = height;
            Items.Add(this);
        }

        public string Name { get; set; }
        public MenuHeight Height { get; set; }
    }

    class Menu 
    {
        public static short NumOptions;
        public string MenuName;
        public int Current_Item;

        // Defines the width to where menu options are rendered
        public static int MENU_WIDTH = 5;
        public static int RIGHT_MENU_WIDTH = 60;

        public List<MenuItem> Items = new List<MenuItem>(NumOptions);

        public void CurrentSelection(MenuItem item, MenuHeight height)
        {
            Current_Item = Items.IndexOf(item);
            Console.SetCursorPosition(MENU_WIDTH, (int)height);

            CurrentSelectionColors();
            Console.Write(item.Name);

            // Makes the Rest of the Items Default Color
            NonSelectedItemColors();
        }

        protected void ChangeSelection(MenuItem item, ref string key)
        {
            switch (key)
            {
                case "DownArrow":
                    if (Current_Item <= (NumOptions - 1) && Current_Item != (NumOptions-1))
                    {
                        // Step 1 Recolor previous option 
                        NonSelectedItemColors();
                        Console.SetCursorPosition(MENU_WIDTH, (int)item.Height);
                        Console.Write(item.Name);

                        // step 2 HightLight New option
                        MenuItem item2 = Items[Current_Item + 1];
                        CurrentSelectionColors();
                        Console.SetCursorPosition(MENU_WIDTH, (int)item2.Height);
                        Console.Write(item2.Name);

                        // Makes the Rest of the Items Default Color
                        NonSelectedItemColors();
                        Current_Item++;
                    }
                    else
                        Console.Beep();
                    break;

                case "UpArrow":
                    if ((NumOptions-1) >= Current_Item && Current_Item != 0)
                    {
                        // Step 1 Recolor previous option 
                        NonSelectedItemColors();
                        Console.SetCursorPosition(MENU_WIDTH, (int)item.Height);
                        Console.Write(item.Name);

                        // step 2 HightLight New option
                        MenuItem item2 = Items[Current_Item - 1];
                        CurrentSelectionColors();
                        Console.SetCursorPosition(MENU_WIDTH, (int)item2.Height);
                        Console.Write(item2.Name);

                        // Makes the Rest of the Items Default Color
                        NonSelectedItemColors();
                        Current_Item--;
                    }
                    else
                        Console.Beep();
                    break;
            }
        }

        public string DetectKeyPress()
        {
            while (true)
            {
                string selection = Console.ReadKey().Key.ToString();
                switch (selection)
                {
                    case "UpArrow":
                        ChangeSelection(Items[Current_Item], ref selection);
                        break;
                    case "DownArrow":
                        ChangeSelection(Items[Current_Item], ref selection);
                        break;
                    case "Enter":
                        return Items[Current_Item].Name;
                    default:
                        break;
                }
            }
            
        }
        public static void CurrentSelectionColors()
        {
            Console.BackgroundColor = ConsoleColor.Yellow;
            Console.ForegroundColor = ConsoleColor.Black;
        }

        public static void NonSelectedItemColors()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.BackgroundColor = ConsoleColor.Black;
        }

        public Menu() { }

        ~Menu() => Console.Clear();

        public void RenderMainMenu()
        {
            Items.Clear();
            Console.Clear();
            MenuItem option1 = new MenuItem("Database Operations", MenuHeight.Item1, ref Items);
            CurrentSelection(option1, option1.Height);
            Current_Item = Items.IndexOf(option1);

            Console.SetCursorPosition(MENU_WIDTH, 4);
            MenuItem option2 = new MenuItem("Enter Personal Information ;)", MenuHeight.Item2, ref Items);
            Console.Write(option2.Name);

            Console.SetCursorPosition(MENU_WIDTH, 6);
            MenuItem option3 = new MenuItem("Enter Tasks For The Day", MenuHeight.Item3, ref Items);
            Console.Write(option3.Name);

            Console.SetCursorPosition(MENU_WIDTH, 8);
            MenuItem option4 = new MenuItem("Quit and Move On with your life", MenuHeight.Item4, ref Items);
            Console.Write(option4.Name);

            Console.ForegroundColor = ConsoleColor.White;
            NumOptions = 4;
            MenuName = "Main Menu";
        }

        public void RenderDatabaseMenu()
        {
            Items.Clear();
            Console.Clear();

            MenuItem option1 = new MenuItem("Add Contact To Database", MenuHeight.Item1, ref Items);
            CurrentSelection(option1, option1.Height);
            Current_Item = Items.IndexOf(option1);

            Console.SetCursorPosition(MENU_WIDTH, 4);
            MenuItem option2 = new MenuItem("Remove Contact From Database", MenuHeight.Item2, ref Items);
            Console.Write(option2.Name);

            Console.SetCursorPosition(MENU_WIDTH, 6);
            MenuItem option3 = new MenuItem("Go Back To Main menu", MenuHeight.Item3, ref Items);
            Console.Write(option3.Name);

            Console.ForegroundColor = ConsoleColor.White;
            NumOptions = 3;
            MenuName = "Database Menu";
        }

        public void RenderTaskMenu()
        {
            Items.Clear();
            Console.Clear();

            MenuItem option1 = new MenuItem("Enter A Task", MenuHeight.Item1, ref Items);
            CurrentSelection(option1, option1.Height);
            Current_Item = Items.IndexOf(option1);

            Console.SetCursorPosition(MENU_WIDTH, 4);
            MenuItem option2 = new MenuItem("Compose Email", MenuHeight.Item2, ref Items);
            Console.Write(option2.Name);

            Console.SetCursorPosition(MENU_WIDTH, 6);
            MenuItem option3 = new MenuItem("Go Back To Main menu", MenuHeight.Item3, ref Items);
            Console.Write(option3.Name);

            Console.ForegroundColor = ConsoleColor.White;
            NumOptions = 3;
            MenuName = "Task Menu";
        }

        public void DisplayDbContacts()
        {
            ClearOtherHalfScreen();
            Console.ForegroundColor = ConsoleColor.White;
            List<Contact> contacts = Sqlite.LoadContacts();
            Console.SetCursorPosition(60, 2);
            Console.Write("           Contact Infomation");
            Console.SetCursorPosition(60, 3);
            Console.Write("=========================================");

            short heightpos = 4;
            foreach (Contact s in contacts)
            {
                Console.SetCursorPosition(60, heightpos);
                Console.Write("Name: {0}, Email: {1}", s.Name, s.Email);
                heightpos++;
            }
        }

        public static void ClearHalfScreen()
        {
            for (int i = 2; i <= NumOptions*2; i++)
            {
                Console.SetCursorPosition(MENU_WIDTH, i);
                Console.Write("                                   ");
            }
        }

        public static void ClearOtherHalfScreen()
        {
            for (int i = 2; i <= NumOptions * 8; i++)
            {
                Console.SetCursorPosition(RIGHT_MENU_WIDTH, i);
                Console.Write("                                   ");
            }
        }

        public void DrawContactAddMenu()
        {
            ClearHalfScreen();
            Console.SetCursorPosition(MENU_WIDTH, (int)MenuHeight.Item1);
            NonSelectedItemColors();
            Console.Write("Enter Name: ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.CursorVisible = true;
            string name = Console.ReadLine();

            NonSelectedItemColors();
            Console.SetCursorPosition(MENU_WIDTH, (int)MenuHeight.Item2);
            Console.Write("Enter Email: ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.CursorVisible = true;
            string email = Console.ReadLine();

            Console.CursorVisible = false;
            Contact person = new Contact
            {
                Name = name,
                Email = email
            };

            Sqlite.AddContact(person);
            ClearHalfScreen();
            RenderDatabaseMenu();
            DisplayDbContacts();
        }

        public void DrawContactRemoveMenu()
        {
            ClearHalfScreen();
            Console.SetCursorPosition(MENU_WIDTH, (int)MenuHeight.Item1);
            NonSelectedItemColors();
            Console.Write("Enter ID to remove: ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.CursorVisible = true;
            int ID = Int32.Parse(Console.ReadLine());
            Console.CursorVisible = false;

            List<Contact> contacts = Sqlite.LoadContacts();
            Sqlite.RemoveContact(contacts[(ID-1)]);

            ClearHalfScreen();
            RenderDatabaseMenu();
            DisplayDbContacts();
        }


    }
}
