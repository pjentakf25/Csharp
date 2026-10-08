namespace kostka
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine ("hrací kostka vam vybere číslo od 1 do 6");
            Console.WriteLine("stiskněte space pro hod kostkou");
            Random kostka = new Random();
            int hod = kostka.Next(3);

            if (Console.ReadKey().Key == ConsoleKey.Spacebar)
            {
                Console.WriteLine("hod kostkou");
            }


            if (hod == 0)
            {
                Console.WriteLine("padlo číslo 1");
            }
            else if (hod == 1)
            {
                Console.WriteLine("padlo číslo 2");
            }
            else if (hod == 2)
            {
                Console.WriteLine("padlo číslo 3");
            }
            else if (hod == 3)
            {
                Console.WriteLine("padlo číslo 4");
            }
            else if (hod == 4)
            {
                Console.WriteLine("padlo číslo 5");
            }
            else if (hod == 5)
            {
                Console.WriteLine("padlo číslo 6");
                


        }
    }
}
