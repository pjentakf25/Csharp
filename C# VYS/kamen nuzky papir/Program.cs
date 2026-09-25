namespace kamen_nuzky_papir
{
    internal class Program
    {
        static void Main(string[] args)
        {
         Console.WriteLine("Vítejte ve hře Kámen, nůžky, papír!");
            Console.WriteLine("Zadejte svůj výběr (kámen, nůžky, papír):");
            string uzivateluvVyber = Console.ReadLine().ToLower();
            Random random = new Random();
            int pocitacovyVyber = random.Next(3); // 0 = kámen, 1 = nůžky, 2 = papír
            string[] moznosti = { "kámen", "nůžky", "papír" };
            Console.WriteLine($"Počítač zvolil: {moznosti[pocitacovyVyber]}");
            if (uzivateluvVyber == moznosti[pocitacovyVyber])
            {
                Console.WriteLine("Remíza!");
            }
            else if ((uzivateluvVyber == "kámen" && pocitacovyVyber == 1) ||
                     (uzivateluvVyber == "nůžky" && pocitacovyVyber == 2) ||
                     (uzivateluvVyber == "papír" && pocitacovyVyber == 0))
            {
                Console.WriteLine("Gratulujeme! Vyhrál jste!");
            }
            else
            {
                Console.WriteLine("Bohužel, prohrál jste.");
            }


        }    







}
}

