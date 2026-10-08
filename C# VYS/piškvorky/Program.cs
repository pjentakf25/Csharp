namespace piškvorky
{
    internal class Program
    {
        static void Main(string[] args)
        {
            char[,] pole = new char[3, 3]
            {
                { ' ', ' ', ' ' },
                { ' ', ' ', ' ' },
                { ' ', ' ', ' ' }
            };

            char hrac = 'X';
            int pocetTahu = 0;

            while (true)
            {
                // Vypsání hracího pole
                Console.WriteLine();

                for (int i = 0; i < 3; i++)
                {
                    Console.WriteLine($"{pole[i, 0]} | {pole[i, 1]} | {pole[i, 2]}");

                    if (i < 2)
                    {
                        Console.WriteLine("--+---+--");
                    }
                }

                // Zadání tahu
                Console.WriteLine();
                Console.WriteLine("Hraje hráč " + hrac);

                Console.Write("Zadej řádek (1-3): ");
                if (!int.TryParse(Console.ReadLine(), out int radek))
                {
                    Console.WriteLine("Zadej prosím číslo!");
                    continue;
                }

                Console.Write("Zadej sloupec (1-3): ");
                if (!int.TryParse(Console.ReadLine(), out int sloupec))
                {
                    Console.WriteLine("Zadej prosím číslo!");
                    continue;
                }

                radek--;
                sloupec--;

                // Kontrola rozsahu
                if (radek < 0 || radek > 2 || sloupec < 0 || sloupec > 2)
                {
                    Console.WriteLine("Čísla musí být v rozsahu 1-3!");
                    continue;
                }

                // Kontrola, jestli je políčko volné
                if (pole[radek, sloupec] != ' ')
                {
                    Console.WriteLine("Toto políčko je obsazené!");
                    continue;
                }

                // Zapíšeme X nebo O
                pole[radek, sloupec] = hrac;
                pocetTahu++;

                // Kontrola výhry
                bool vyhral = false;

                // Řádky
                for (int i = 0; i < 3; i++)
                {
                    if (pole[i, 0] == hrac &&
                        pole[i, 1] == hrac &&
                        pole[i, 2] == hrac)
                    {
                        vyhral = true;
                    }
                }

                // Sloupce
                for (int i = 0; i < 3; i++)
                {
                    if (pole[0, i] == hrac &&
                        pole[1, i] == hrac &&
                        pole[2, i] == hrac)
                    {
                        vyhral = true;
                    }
                }

                // První diagonála
                if (pole[0, 0] == hrac &&
                    pole[1, 1] == hrac &&
                    pole[2, 2] == hrac)
                {
                    vyhral = true;
                }

                // Druhá diagonála
                if (pole[0, 2] == hrac &&
                    pole[1, 1] == hrac &&
                    pole[2, 0] == hrac)
                {
                    vyhral = true;
                }

                // Konec hry - výhra
                if (vyhral)
                {
                    VypisPole(pole);
                    Console.WriteLine();
                    Console.WriteLine("Vyhrál hráč " + hrac + "!");
                    break;
                }

                // Konec hry - remíza
                if (pocetTahu == 9)
                {
                    VypisPole(pole);
                    Console.WriteLine();
                    Console.WriteLine("Remíza!");
                    break;
                }

                // Změna hráče
                if (hrac == 'X')
                {
                    hrac = 'O';
                }
                else
                {
                    hrac = 'X';
                }
            }
        }

        static void VypisPole(char[,] pole)
        {
            Console.WriteLine();

            for (int i = 0; i < 3; i++)
            {
                Console.WriteLine($"{pole[i, 0]} | {pole[i, 1]} | {pole[i, 2]}");

                if (i < 2)
                {
                    Console.WriteLine("--+---+--");
                }
            }
        }
    }
}