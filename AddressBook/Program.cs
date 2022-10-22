using AddressBook.Services;

IMenuManager menu = new MenuManager();

do
{

    Console.Clear();
    menu.MainMenu();

    Console.ReadKey();
} while (true);
