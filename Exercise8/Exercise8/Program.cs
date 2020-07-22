using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercise8
{
    class Program
    {
        static void Main(string[] args)
        {
            do
            {
                MainMenu();
                Console.WriteLine("Press any key to return.");
                Console.ReadKey();
            } while (true);
        }

        static void MainMenu()
        {
            Console.Clear();

            int Selected;
            List<string> Menu = new List<string>();
            Menu.Add("Extract People");
            Menu.Add("Extract People by Gender");
            Menu.Add("Extract Person by Last Name");
            Menu.Add("Exit");
            DisplayMenu("Select:", Menu, out Selected);

            switch (Selected)
            {
                case 1:
                    {
                        NewScreen(Menu[Selected - 1]);
                        GetPeople();
                        break;
                    }
                case 2:
                    {
                        NewScreen(Menu[Selected - 1]);
                        GetPeopleByGender();
                        break;
                    }
                case 3:
                    {
                        NewScreen(Menu[Selected - 1]);
                        GetSinglePerson();
                        break;
                    }
                case 4:
                    {
                        Environment.Exit(0);
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }

        //List
        private static void GetPeople()
        {
            Console.WriteLine("Getting All Person");
            using (var context = new SuccinctlyExamplesEntities())
            {
                List<Person> people = context.People.ToList();
                DisplayPersonTable(people);
            }
        }
        //IQueryable
        private static void GetPeopleByGender()
        {
            int Gender;
            List<string> Menu = new List<string> { "MALE" , "FEMALE" };
            DisplayMenu("Select Gender", Menu, out Gender);
            
            using (var context = new SuccinctlyExamplesEntities())
            {
                // Don't access the database just yet...
                IQueryable<Person> query = context.People
                .Where(p => p.GenderID == Gender)
                .OrderBy(p => p.LastName);
                // Access the database.

                List<Person> people = query.ToList();
                DisplayPersonTable(people);
            }
        }
        //Get Person
        private static void GetSinglePerson()
        {
            PromptInputMessage("Enter Last Name: ");
            string LastName = GetStringInput();

            using (var context = new SuccinctlyExamplesEntities())
            {
                Person person = context.People.SingleOrDefault(p => p.LastName == LastName);

                if (person != null)
                    DisplayPersonTable(null, person);
                else
                    DisplayError("This person does not exist!");
            }
        }
        //Display Output
        private static void DisplayPersonTable(List<Person> lPersons = null, Person person = null)
        {
            int nMaxLength = 15;
            string strOverflow = "...";
            int nPaddingRight = nMaxLength + strOverflow.Length;
            Console.WriteLine();
            Console.WriteLine(" ----------------------------------------------------------------------------------------------------------------------");
            Console.WriteLine(" | ID    | Last Name          | First Name         | Middle Name        | Date of Birth                  | Gender     |");
            Console.WriteLine(" ----------------------------------------------------------------------------------------------------------------------");

            if (lPersons != null)
            {
                foreach (Person p in lPersons)
                {
                    Console.WriteLine(" | {0} | {1} | {2} | {3} | {4} | {5} |",
                        p.ID.ToString().PadRight(5, ' '),
                        p.LastName.Length > nMaxLength ? p.LastName.Substring(0, nMaxLength) + strOverflow : p.LastName.PadRight(nPaddingRight, ' '),
                        p.FirstName.Length > nMaxLength ? p.FirstName.Substring(0, nMaxLength) + strOverflow : p.FirstName.PadRight(nPaddingRight, ' '),
                        p.MiddleName.Length > nMaxLength ? p.MiddleName.Substring(0, nMaxLength) + strOverflow : p.MiddleName.PadRight(nPaddingRight, ' '),
                        p.DateOfBirth.Equals(DateTime.MinValue) ? ("").PadRight(30, ' ') : p.DateOfBirth.ToString().PadRight(30, ' '),
                        p.Gender.Description.PadRight(10, ' '));
                }
            }
            else
            {
                Console.WriteLine(" | {0} | {1} | {2} | {3} | {4} | {5} |",
                        person.ID.ToString().PadRight(5, ' '),
                        person.LastName.Length > nMaxLength ? person.LastName.Substring(0, nMaxLength) + strOverflow : person.LastName.PadRight(nPaddingRight, ' '),
                        person.FirstName.Length > nMaxLength ? person.FirstName.Substring(0, nMaxLength) + strOverflow : person.FirstName.PadRight(nPaddingRight, ' '),
                        person.MiddleName.Length > nMaxLength ? person.MiddleName.Substring(0, nMaxLength) + strOverflow : person.MiddleName.PadRight(nPaddingRight, ' '),
                        person.DateOfBirth.Equals(DateTime.MinValue) ? ("").PadRight(30, ' ') : person.DateOfBirth.ToString().PadRight(30, ' '),
                        person.Gender.Description.PadRight(10, ' '));
            }
            Console.WriteLine(" ----------------------------------------------------------------------------------------------------------------------");
        }
        //ReadLine String
        private static string GetStringInput()
        {
            return Console.ReadLine();
        }
        //Prompt Input Message
        private static void PromptInputMessage(string msg)
        {
            Console.Write(msg);
        }
        //Display Error Message
        private static void DisplayError(string err)
        {
            string error = string.Format("###### {0} ######", err);
            Console.WriteLine(error.PadLeft(80));
        }
        //Display Menu
        private static void DisplayMenu(string Title, List<string> MenuList, out int Selected)
        {
            Selected = 0;
            if (MenuList.Count > 0)
            {
                string MenuItem;
                Console.WriteLine(Title);

                for (int i = 0; i < MenuList.Count; i++)
                {
                    MenuItem = string.Format("[{0}] {1}", i+1, MenuList[i]);
                    Console.WriteLine(MenuItem);
                }

                bool isValidSelected;

                do
                {
                    Console.Write("Enter selected item: ");
                    isValidSelected = Int32.TryParse(Console.ReadLine(), out Selected);
                } while (!isValidSelected || Selected <= 0 || Selected > MenuList.Count);
            }
        }
        //Clear Screen
        private static void NewScreen(string Title)
        {
            string stitle = string.Format("_____{0}_____", Title);
            Console.Clear();
            Console.WriteLine(stitle.PadLeft(80));
            Console.WriteLine();
        }
    }
}
