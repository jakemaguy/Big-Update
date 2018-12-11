using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Outlook;

namespace BigUpdate
{
    struct Entry
    {
        public readonly string text;
        public readonly string type;

        public Entry(string text, string type)
        {
            this.text = text;
            this.type = type;
        }
    }
    class Email_Operations
    {
        readonly static List<Entry> entries = new List<Entry>();
        public static void DisplayEmailBody()
        {
            Menu.ClearOtherHalfScreen();
            int DisplayWidth = 60;
            int DisplayHeight = 2;
            Console.SetCursorPosition(DisplayWidth, DisplayHeight);
            Console.Write("             Current Email Body");
            DisplayHeight++;
            Console.SetCursorPosition(DisplayWidth, DisplayHeight);
            Console.Write("=========================================");
            DisplayHeight++;

            // Loop over all entries
            foreach(Entry s in entries)
            {
                Console.SetCursorPosition(DisplayWidth, DisplayHeight);
                if (s.type.Equals("task"))
                    Console.Write(s.text);
                else
                    Console.Write("\t" + s.text);
                DisplayHeight++;
            }
        }
        private static void CreateEmailItem(string subjectEmail,
            string toEmail, string bodyEmail)
        {
            Application app = new Application();
            MailItem eMail = (MailItem)
                app.CreateItem(OlItemType.olMailItem);
            eMail.Subject = subjectEmail;
            //eMail.Body = bodyEmail;
            eMail.To = toEmail;
            eMail.HTMLBody = bodyEmail;
           // eMail.Importance = OlImportance.olImportanceLow;
            ((_MailItem)eMail).Send();
        }

        public static string GenerateEmailBody()
        {
            StringBuilder body = new StringBuilder();
            body.Append("<h2>Tasks Completed Today</h2>");
            body.Append("<ul>");
            for (int i = 0; i < entries.Count; i++)
            {
                Entry current = entries[i];
                body.Append("<li>" + current.text);

                if (i < (entries.Count - 1))
                {
                    Entry nextItem = entries[i + 1];
                    if (nextItem.type.Equals("detail") && current.type.Equals("task"))
                    {
                        body.Append("<ul>");
                    }
                    else if (nextItem.type.Equals("task") && current.type.Equals("detail"))
                    {
                        body.Append("</ul>");
                    }
                    else
                        body.Append("</li>");
                }
                else
                {
                    if (current.type.Equals("detail"))
                        body.Append("</li></ul></li></ul>");
                    else
                        body.Append("</li></ul>");
                }

            }
            return body.ToString();
        }

        public static void ComposeEmail()
        {
            Menu.NonSelectedItemColors();
            Menu.ClearHalfScreen();
            Console.SetCursorPosition(Menu.MENU_WIDTH, (int)MenuHeight.Item1);
            Console.Write("Enter The IDs of the Emails To Send To");
            Console.SetCursorPosition(Menu.MENU_WIDTH, (int)MenuHeight.Item1 + 1);
            Console.Write("Use Comma Seperated Values (e.g. 1, 2, 3, 4 etc)");
            Console.SetCursorPosition(Menu.MENU_WIDTH, (int)MenuHeight.Item1 + 2);

            Console.ForegroundColor = ConsoleColor.White;

            List<int> choices = Console.ReadLine().Split(',').Select(int.Parse).ToList();
            List<Contact> arr = Sqlite.LoadContacts();
            StringBuilder formatStr = new StringBuilder();

            foreach(int i in choices)
            {
                formatStr.Append(arr[(i - 1)].Email);
                formatStr.Append("; ");
            }
            
            CreateEmailItem(GenerateSubject(), formatStr.ToString().TrimEnd(new char[] { ';' }) , GenerateEmailBody());
        }

        static bool SBIsEmpty(ref StringBuilder sb)
        {
            return sb.Length > 0;
        }

        public static void AddTask()
        {
            Menu.NonSelectedItemColors(); 
            Menu.ClearHalfScreen();
            Console.SetCursorPosition(Menu.MENU_WIDTH, (int)MenuHeight.Item1);
            Console.Write("Enter Task: ");
            Console.SetCursorPosition(Menu.MENU_WIDTH, (int)MenuHeight.Item2);

            // Chose this particular string as buffer becuase it is the longest
            string buffer = "Enter Task Detail: ";
            Console.Write(buffer);

            Console.SetCursorPosition(Menu.MENU_WIDTH, (int)MenuHeight.Item3);
            Console.Write("Go Back To Previous Menu");

            Console.SetCursorPosition(Menu.MENU_WIDTH + buffer.Length, (int)MenuHeight.Item1);
            Console.CursorVisible = true;
            Console.ForegroundColor = ConsoleColor.White;
            // Implement Console Read Functionality
            StringBuilder sbTask = new StringBuilder();
            StringBuilder sbDetail = new StringBuilder();
            ref StringBuilder sb = ref sbTask;
            short currentItem = 1;
            while (true)
            {
                if (currentItem == 1)  sb = sbTask;
                else sb = sbDetail;
                // Key is used to get keyinfo for Shift + key functionality
                ConsoleKeyInfo key = Console.ReadKey();

                // key string is ASCII representation of the key that was pressed
                string keystring = key.Key.ToString();
                switch (keystring)
                {
                    case "Backspace":
                        if (currentItem != 3)
                        {
                            if (SBIsEmpty(ref sb))
                            {
                                sb.Remove(sb.Length - 1, 1);
                                Console.Write(" ");
                                Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                            }
                            else Console.SetCursorPosition(buffer.Length + Menu.MENU_WIDTH, Console.CursorTop);
                        }
                        break;
                    /* TODO IMPLEMENT ARROW FUNCTIONALITY
                    case "LeftArrow":
                        Console.SetCursorPosition(Console.CursorLeft - 1, (int)MenuHeight.Item1);
                        break;
                    case "RightArrow":
                        break;
                    */
                    case "Enter":
                        if (currentItem != 3)
                        {
                            if (currentItem == 1)
                            {
                                Console.SetCursorPosition(buffer.Length + Menu.MENU_WIDTH, (int)MenuHeight.Item1);
                                entries.Add(new Entry(sb.ToString(), "task"));
                            }
                            else
                            {
                                Console.SetCursorPosition(buffer.Length + Menu.MENU_WIDTH, (int)MenuHeight.Item2);
                                entries.Add(new Entry(sb.ToString(), "detail"));
                            }
                            DisplayEmailBody();

                            if (currentItem == 1)
                                Console.SetCursorPosition(buffer.Length + Menu.MENU_WIDTH, (int)MenuHeight.Item1);
                            else
                                Console.SetCursorPosition(buffer.Length + Menu.MENU_WIDTH, (int)MenuHeight.Item2);

                            for (int i = 0; i < sb.Length; i++)
                            {
                                Console.Write(" ");
                            }
                            if (currentItem == 1)
                                Console.SetCursorPosition(buffer.Length + Menu.MENU_WIDTH, (int)MenuHeight.Item1);
                            else
                                Console.SetCursorPosition(buffer.Length + Menu.MENU_WIDTH, (int)MenuHeight.Item2);
                            sb.Clear();
                            break;
                        }
                        else
                        {
                            Menu.NonSelectedItemColors();
                            return;
                        }
                    case "Tab":
                        if (currentItem == 3)
                        {
                            sb.Clear();
                            Menu.NonSelectedItemColors();
                            Console.CursorVisible = true;
                            Console.SetCursorPosition(Menu.MENU_WIDTH, Console.CursorTop);
                            
                            Console.Write("Go Back To Previous Menu");
                            for (int i =0; i < buffer.Length; i++)
                            {
                                Console.Write(" ");
                            }
                            Console.ForegroundColor = ConsoleColor.White;
                            /*
                            Console.SetCursorPosition(buffer.Length + Menu.MENU_WIDTH, (int)MenuHeight.Item1);
                            for (int i =0; i< sb.Length; i++)
                            {
                                Console.Write(" ");
                            }
                            */
                            Console.SetCursorPosition(buffer.Length + Menu.MENU_WIDTH, (int)MenuHeight.Item1);
                            Console.Write(sb.ToString());
                            Console.SetCursorPosition(buffer.Length + Menu.MENU_WIDTH, (int)MenuHeight.Item1);
                            currentItem = 1;
                        }
                        else
                        {
                            sb.Clear();
                            Console.SetCursorPosition(buffer.Length + Menu.MENU_WIDTH, Console.CursorTop);
                            for (int i = 0; i < sb.Length; i++)
                            {
                                Console.Write(" ");
                            }
                            Console.SetCursorPosition(buffer.Length + Menu.MENU_WIDTH, Console.CursorTop);
                            Console.Write(sb.ToString());
                            Console.SetCursorPosition(buffer.Length + Menu.MENU_WIDTH, Console.CursorTop+2);
                            currentItem++;
                            if (currentItem == 3)
                            {
                                Console.SetCursorPosition(Menu.MENU_WIDTH, Console.CursorTop);
                                Console.CursorVisible = false;
                                Menu.CurrentSelectionColors();
                                Console.Write("Go Back To Previous Menu");
                            }
                        }
                      
                        break;
                    default:
                        if (currentItem != 3)
                        {
                            if (keystring.Equals("Spacebar")) sb.Append(" ");
                            else
                            {
                                if ((key.Modifiers & ConsoleModifiers.Shift) != 0) sb.Append(keystring);
                                else if (char.IsDigit(key.KeyChar) || char.IsPunctuation(key.KeyChar))
                                    sb.Append(key.KeyChar.ToString());
                                else sb.Append(keystring.ToLower());
                            }
                        }
                        //Console.SetCursorPosition(Console.CursorLeft, (int)MenuHeight.Item1);
                        break;
                }
            }
        }

        public static string GenerateSubject() { return "Daily Update (" + DateTime.Today.ToString("MM/dd/yyyy") + ")"; }
    }
}
