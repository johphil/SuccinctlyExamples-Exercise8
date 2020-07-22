using System;
using System.Collections.Generic;
using System.Data.Entity;
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
            Menu.Add("Extract People by Age");
            Menu.Add("Update Person Info");
            Menu.Add("Delete Person");
            Menu.Add("Exit");

            DisplayMenu("Select:", Menu, out Selected);

            NewScreen(Menu[Selected - 1]);
            switch (Selected)
            {
                case 1:
                    {
                        GetPeople();
                        break;
                    }
                case 2:
                    {
                        GetPeopleByGender();
                        break;
                    }
                case 3:
                    {
                        GetPersonByLastName();
                        break;
                    }
                case 4:
                    {
                        GetPeopleByAge();
                        break;
                    }
                case 5:
                    {
                        UpdatePersonInfo();
                        break;
                    }
                case 6:
                    {
                        DeletePerson();
                        break;
                    }
                case 7:
                    {
                        Environment.Exit(0);
                        break;
                    }
                default:
                    {
                        break;
                    }
            }

            Console.WriteLine("Press any key to return.");
        }

        //List Extract All People
        private static void GetPeople()
        {
            using (var context = new SuccinctlyExamplesEntities())
            {
                List<Person> people = context.People.ToList();
                DisplayPersonTable(people);
            }
        }
        //IQueryable Extract People by Gender
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
        //Get Person By Last Name
        private static void GetPersonByLastName()
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
        //Get People by Age
        private static void GetPeopleByAge()
        {
            string[] ops = new string[5]
            {
                "=",//is equal
                ">",//greater than
                "<",//less than
                ">=",//greater than or equal
                "<="//less than or equal
            };

            string[] words;
            do
            {
                words = null;
                PromptInputMessage(" Age is ");
                string input = GetStringInput();
                words = input.Split(' ');
            } while (words.Length != 2);

            bool isValidOp = false;

            foreach (string o in ops)
            {
                if (words.First() == o)
                    isValidOp = true;
            }


            if (isValidOp)
            {
                int Age;
                bool isValidAge = Int32.TryParse(words.Last(), out Age);

                if (isValidAge)
                {
                    using (var context = new SuccinctlyExamplesEntities())
                    {
                        List<Person> people = null;
                        switch (words.First())
                        {
                            case "=": //is equal
                                {
                                    people = context.People.IsEqual(Age).ToList();
                                    break;
                                }
                            case ">": //is greater than
                                {
                                    people = context.People.IsGreaterThan(Age).ToList();
                                    break;
                                }
                            case "<": //is less than
                                {
                                    people = context.People.IsLessThan(Age).ToList();
                                    break;
                                }
                            case ">=": //is greater than or equal
                                {
                                    people = context.People.IsGreaterThanOrEqual(Age).ToList();
                                    break;
                                }
                            case "<=": //is less than or equal
                                {
                                    people = context.People.IsLessThanOrEqual(Age).ToList();
                                    break;
                                }
                            default: 
                                {
                                    break;
                                }
                        }
                        
                        if (people != null)
                            DisplayPersonTable(people);
                    }
                }
            }
        }
        //Update Person Information
        private static void UpdatePersonInfo()
        {
            GetPeople();
            int PersonID;
            GetIntegerInput(" Enter ID: ", out PersonID);

            using (var context = new SuccinctlyExamplesEntities())
            {
                Console.Clear();
                Console.WriteLine("Enter new data for this person below.");

                Person P = context.People.FirstOrDefault(p => p.ID == PersonID);
                if (P != null)
                {
                    DisplayPersonTable(null, P);

                    P.ID = PersonID;

                    PromptInputMessage("Enter Last Name: ");
                    P.LastName = GetStringInput();
                    do
                    {
                        PromptInputMessage("Enter First Name: ");
                        P.FirstName = GetStringInput();

                        if (string.IsNullOrWhiteSpace(P.FirstName))
                            DisplayError("First Name cannot be empty.");

                    } while (string.IsNullOrWhiteSpace(P.FirstName));


                    PromptInputMessage("Enter Middle Name: ");
                    P.MiddleName = GetStringInput();

                    PromptInputMessage("Enter Date of Birth: ");
                    DateTime DOB;
                    bool isValidDOB = DateTime.TryParse(Console.ReadLine(), out DOB);
                    if (isValidDOB)
                        P.DateOfBirth = DOB;
                    else
                    {
                        DisplayError("Invalid Date. It will be set to null.");
                        P.DateOfBirth = null;
                    }

                    PromptInputMessage("Enter Gender (0 = Unknown, 1 = Male, 2 = Female): ");
                    int GenderID;
                    bool isValidGender = int.TryParse(Console.ReadLine(), out GenderID);
                    if (isValidGender)
                        isValidGender = GenderID >= 0 && GenderID < 3 ? true : false;
                    if (isValidGender)
                        P.GenderID = GenderID;
                    else
                    {
                        DisplayError("Invalid Gender. It will be set to null.");
                        P.GenderID = null;
                    }

                    if (context.SaveChanges() > 0)
                        Console.WriteLine("Update Success");
                    else
                        DisplayError("Error!");
                }
                else
                    DisplayError("Person does not exist.");
            }
        }
        //Delete Person 
        private static void DeletePerson()
        {
            GetPeople();
            int PersonID;
            GetIntegerInput(" Enter ID: ", out PersonID);

            using (var context = new SuccinctlyExamplesEntities())
            {
                Person P = context.People.FirstOrDefault(p => p.ID == PersonID);

                if (P != null)
                {
                    context.People.Remove(P);

                    if (context.SaveChanges() > 0)
                        Console.WriteLine("Delete Success");
                    else
                        DisplayError("Error!");
                }
                else
                    DisplayError("Person does not exist.");
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
            Console.WriteLine(" | ID    | Last Name          | First Name         | Middle Name        | Date of Birth          |  Age  | Gender     |");
            Console.WriteLine(" ----------------------------------------------------------------------------------------------------------------------");

            if (lPersons != null)
            {
                foreach (Person p in lPersons)
                {
                    Console.WriteLine(" | {0} | {1} | {2} | {3} | {4} | {5} | {6} |",
                        p.ID.ToString().PadRight(5, ' '),
                        p.LastName.Length > nMaxLength ? p.LastName.Substring(0, nMaxLength) + strOverflow : p.LastName.PadRight(nPaddingRight, ' '),
                        p.FirstName.Length > nMaxLength ? p.FirstName.Substring(0, nMaxLength) + strOverflow : p.FirstName.PadRight(nPaddingRight, ' '),
                        p.MiddleName.Length > nMaxLength ? p.MiddleName.Substring(0, nMaxLength) + strOverflow : p.MiddleName.PadRight(nPaddingRight, ' '),
                        p.DateOfBirth.Equals(DateTime.MinValue) ? ("").PadRight(22, ' ') : p.DateOfBirth.ToString().PadRight(22, ' '),
                        p.Age.ToString().PadRight(5, ' '),
                        p.Gender.Description.PadRight(10, ' '));
                }
            }
            else
            {
                Console.WriteLine(" | {0} | {1} | {2} | {3} | {4} | {5} | {6} |",
                        person.ID.ToString().PadRight(5, ' '),
                        person.LastName.Length > nMaxLength ? person.LastName.Substring(0, nMaxLength) + strOverflow : person.LastName.PadRight(nPaddingRight, ' '),
                        person.FirstName.Length > nMaxLength ? person.FirstName.Substring(0, nMaxLength) + strOverflow : person.FirstName.PadRight(nPaddingRight, ' '),
                        person.MiddleName.Length > nMaxLength ? person.MiddleName.Substring(0, nMaxLength) + strOverflow : person.MiddleName.PadRight(nPaddingRight, ' '),
                        person.DateOfBirth.Equals(DateTime.MinValue) ? ("").PadRight(22, ' ') : person.DateOfBirth.ToString().PadRight(22, ' '),
                        person.Age.ToString().PadRight(5, ' '),
                        person.Gender.Description.PadRight(10, ' '));
            }
            Console.WriteLine(" ----------------------------------------------------------------------------------------------------------------------");
        }
        //ReadLine String
        private static string GetStringInput()
        {
            return Console.ReadLine();
        }
        //Get Integer Input
        private static void GetIntegerInput(string msg, out int input)
        {
            do
            {
                PromptInputMessage(msg);
            } while (Int32.TryParse(Console.ReadLine(), out input) == false);
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
            Console.WriteLine(error);
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
            Console.WriteLine(stitle);
            Console.WriteLine();
        }
    }
}
