namespace papoušek_projekt
{
    internal class Program
    {
        static void Main(string[] args)
        {
            while (true)

            {


                Console.WriteLine(" jsem papoušek Zadej nějaký text a já ti ho vypíšu zpět: ");


                string coUzivatelZadal = Console.ReadLine();

                Console.WriteLine(coUzivatelZadal);

                {
                    if (coUzivatelZadal == "dělaš si ze mě srandu?")


                        Console.WriteLine("Ne, já jsem papoušek a neumím si dělat srandu.");

                }
            }

            }
        }
    }
}
