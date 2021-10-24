using System;

namespace Procedural
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new TestProcedural())
                game.Run();
        }
    }
}
