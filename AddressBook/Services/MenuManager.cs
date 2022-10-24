using AddressBook.Models;
using Microsoft.VisualBasic.FileIO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

/*
 * =================================================================== 
 *                      Adress Book
 *  ------------------------------------------------------------------
 *  TOC:
 *  01. IMenuManager => är ett interface. Här står ett antal krav för ett antal metoder som klassen MenuManager behöver implementera.
 *  02. MenuManager:IMenuManager => är en klass. Alla metoder och funktioner i nedan ligger inom den här klass.  Här ligger också sökvägen till filen. 
 *  03. MainMenu => är en metod.visar och hanterar huvudmenyn.
 *  04. ListAllContacts => är en metod. Ska visa och lista upp alla kontakter som finns i listan.
 *  05. AddContact => är en metod. Den här funktionen möjliggör att du kunna lägga till en ny kontaktperson.
 *  06. SearchForContact  => är en metod. Den här funktionen möjliggör att du kunna söka efter en kontaktperson.
 *  07. ContactDetails => är en metod.Den här funktionen möjliggör att du kunna se detaljerad information om en specifik kontaktperson.
 *  08. UpdateContact => är en metod. Den här funktionen möjliggör att du kunna uppdatera en kontaktperson med hjälp av en Guid ID.
 *  09. DeleteContact => är en metod. Den här funktionen möjliggör att du kunna Radera en kontaktperson med hjälp av en Guid ID.
 *
 * =================================================================== 
 */
namespace AddressBook.Services
{
    /** 
    * ===================================================================
    *                    01.  Här står ett antal krav 
    * ------------------------------------------------------------------- 
    */
    internal interface IMenuManager
    {
        // Här står ett antal krav för ett antal metoder som klassen IMenuManager behöver genomföra.
        public void MainMenu();
        public void ListAllContacts();
        public void AddContact();
        public void SearchForContact();
        public void ContactDetails(Guid id);
        public void UpdateContact(Contact contact);
        public void DeleteContact(Guid id);
    }
    internal class MenuManager : IMenuManager
    {
        /** 
        * ===================================================================
        *        02. Här ligger också sökvägen till file och listor
        * ------------------------------------------------------------------- 
        */
        private List<Contact> _contacts = new();
        private IFileManager _fileManager = new FileManager();


        // Här är sökvägen till där filen ska sparas med namnet ContactPath.Json 
        private string _filePath = $@"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\ContactPath.Json";



        /** 
        * ===================================================================
        *                       03. Huvudmenyn
        * ------------------------------------------------------------------- 
        */
        public void MainMenu()
        {
            _contacts = JsonConvert.DeserializeObject<List<Contact>>(_fileManager.Read(_filePath))!;

            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("|||||| Address Book ||||||");
            Console.ResetColor();
            Console.WriteLine("1. List all Contacts");
            Console.WriteLine("2. Add a new Contact");
            Console.WriteLine("3. Search");
            Console.WriteLine("E. Exit");

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("_________________________");
            Console.ResetColor();
            Console.Write("Please, Choose one option: ");



            var option = Console.ReadLine();
            Console.WriteLine();

            switch (option)
            {
                case "1":
                    ListAllContacts();
                    break;

                case "2":
                    AddContact();
                    break;

                case "3":
                    SearchForContact();
                    break;

                case "E":
                case "e":
                    Environment.Exit(0);
                    break;

                default:
                    Console.WriteLine("\u001b[33;1mError! Enter a valid option\u001b[0m");
                    break;
            }
        }

        /** 
        * ===================================================================
        *                  04. Visa alla kontakter
        * ------------------------------------------------------------------- 
        */

        public void ListAllContacts()
        {
            try
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine("----- All Contacts -----");
                Console.ResetColor();
                foreach (var contact in _contacts)
                {
                    Console.WriteLine($" {_contacts.IndexOf(contact) + 1} - {contact.FirstName} {contact.LastName} \nID - {contact.Id} \n");
                }
                if (_contacts.Count != 0)
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine("_________________________________________________________");
                    Console.ResetColor();

                    Console.WriteLine();
                    Console.Write("Press \u001b[1m\u001b[32mY\u001b[0m to see details or Press \u001b[1m\u001b[34;1mEnter\u001b[0m to return to the main menu: ");

                    var option = Console.ReadLine();
                    if (option?.ToLower() == "y")
                    {
                        Console.Write("Enter Contact Id: ");
                        var id = Guid.Parse(Console.ReadLine()!);

                        if (id != Guid.Empty)
                        {
                            ContactDetails(id);
                        }
                        else
                        {
                            Console.WriteLine("\u001b[33;1mError!\u001b[0m");
                        }
                    }
                    else
                    {
                        Console.Clear();
                        MainMenu();
                    }

                }
            }
            catch
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error!");
                Console.ResetColor();
            }

        }


        /** 
        * ===================================================================
        *             05. Lägga till en ny kontaktperson.
        * ------------------------------------------------------------------- 
        */
        public void AddContact()
        {
            Contact contact = new Contact();

            Console.Clear();
            Console.WriteLine("----- Add New Contact -----");

            Console.Write("Enter contact first name: ");
            contact.FirstName = Console.ReadLine()!;

            Console.Write("Enter contact last name: ");
            contact.LastName = Console.ReadLine()!;

            Console.Write("Enter email: ");
            contact.EmailAddress = Console.ReadLine()!;

            Console.Write("Enter Street name: ");
            contact.StreetName = Console.ReadLine()!;

            Console.Write("Enter postal code: ");
            contact.PostalCode = Console.ReadLine()!;

            Console.Write("Enter city: ");
            contact.City = Console.ReadLine()!;

            _contacts.Add(contact);

            _fileManager.Save(_filePath, JsonConvert.SerializeObject(_contacts, Formatting.Indented));

            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("The new contact has been successfully added to the address book");
            Console.ResetColor();
            Console.Write("Press Enter to continue: ");
        }

        /** 
        * ===================================================================
        *                 06. Sök efter en kontaktperson
        * ------------------------------------------------------------------- 
        */
        public void SearchForContact()
        {
            Console.Clear();
            Console.Write("Enter the first name, last name or email address of the desired contact you're looking for: ");
            string searchKey = Console.ReadLine()!.ToLower();
            Console.Clear();

            var contact = _contacts.FirstOrDefault(x => x.FirstName == searchKey);


            if (searchKey == string.Empty)
            {
                MainMenu();
            }
            if (searchKey != string.Empty)
            {
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine("===== Results =====");
                Console.ResetColor();
                foreach (Contact SearchResults in _contacts)
                {
                    if (SearchResults.FirstName.ToLower() == searchKey || SearchResults.LastName.ToLower() == searchKey || SearchResults.EmailAddress.ToLower() == searchKey)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkCyan;
                        Console.WriteLine("-----------------------------");
                        Console.ResetColor();
                        Console.WriteLine();
                        Console.WriteLine($"Full name: {SearchResults!.FirstName} {SearchResults.LastName}");
                        Console.WriteLine($"Email: {SearchResults.EmailAddress}");
                        Console.WriteLine($"Address: {SearchResults.StreetName}, {SearchResults.PostalCode},  {SearchResults.City}");
                        Console.WriteLine($"ID: {SearchResults.Id} \n");
                        //var id = Scontact.Id;
                        //ContactDetails(id);
                    }
                }
                Console.Write("Press Enter to continue: ");
                return;
            }
            else
            {
                Console.Clear();
                Console.Write("Contact not found ");
            }
        }

        /** 
        * ===================================================================
        *      07. Visa detaljerad info om en specifik kontaktperson.
        * ------------------------------------------------------------------- 
        */
        public void ContactDetails(Guid id)
        {
            var contact = _contacts.FirstOrDefault(x => x.Id == id);

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("===== Details =====");
            Console.ResetColor();
            Console.WriteLine();
            Console.WriteLine($"Full name: {contact!.FirstName} {contact.LastName}");
            Console.WriteLine($"Email: {contact.EmailAddress}");
            Console.WriteLine($"Address: {contact.StreetName}, {contact.PostalCode},  {contact.City}");
            Console.WriteLine($"ID: {contact.Id} \n");
            Console.WriteLine("------------------------------------------------------");
            Console.WriteLine("To edit contact press \u001b[34;1mE\u001b[0m \nTo delete contact press \u001b[31;1mD\u001b[0m \nor Press \u001b[34;1mEnter\u001b[0m to return to the main menu.");

            Console.Write("Please, Choose one option: ");
            var option = Console.ReadLine();

            switch (option)
            {
                case "e":
                case "E":
                    UpdateContact(contact);
                    break;

                case "d":
                case "D":
                    DeleteContact(contact.Id);
                    break;

                default:
                    Console.Clear();
                    MainMenu();
                    break;
            }
        }

        /** 
        * ===================================================================
        *                   08. Uppdatera en kontaktperson
        * ------------------------------------------------------------------- 
        */
        public void UpdateContact(Contact contact)
        {
            var index = _contacts.IndexOf(contact);
            Console.Write("\u001b[35;1mOBS: If you want to keep the same valid, Press Enter, otherwise input the new valid.\u001b[0m");
            Console.ReadLine();

            Console.Write("Enter first name:  ");
            string firstName = Console.ReadLine()!;
            if (firstName != string.Empty)
                contact.FirstName = firstName;

            Console.Write("Input last name: ");
            string lastName = Console.ReadLine()!;
            if (lastName != string.Empty)
                contact.LastName = lastName;

            Console.Write("Input email: ");
            string email = Console.ReadLine()!;
            if (email != string.Empty)
                contact.EmailAddress = email;

            Console.Write("Input street Name: ");
            string streetName = Console.ReadLine()!;
            if (streetName != string.Empty)
                contact.StreetName = streetName;

            Console.Write("Input postal code: ");
            string postalCode = Console.ReadLine()!;
            if (postalCode != string.Empty)
                contact.PostalCode = postalCode;

            Console.Write("Input city: ");
            string city = Console.ReadLine()!;
            if (city != string.Empty)
                contact.City = city;


            Console.Write("\nDone. ");
            _fileManager.Save(_filePath, JsonConvert.SerializeObject(_contacts, Formatting.Indented));
        }

        /** 
        * ===================================================================
        *                  09. Radera en kontaktperson 
        * ------------------------------------------------------------------- 
        */
        public void DeleteContact(Guid id)
        {
            _contacts = _contacts.Where(x => x.Id != id).ToList();
            _fileManager.Save(_filePath, JsonConvert.SerializeObject(_contacts, Formatting.Indented));
            Console.Write("\nDone. ");
        }
    }
}
