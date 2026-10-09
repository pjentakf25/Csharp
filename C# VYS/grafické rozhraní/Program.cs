
using _grafické_rozhraní;


namespace _grafické_rozhraní
{
    internal class program
    {
     static void Main(string[] args)
        {
            using (var game = new mojehra())
                game.Run();
        }
    }
}
