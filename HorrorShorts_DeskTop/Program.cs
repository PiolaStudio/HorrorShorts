using HorrorShorts_Game;
using System;


Logger.InternException += (s, e) => Core.Game?.Exit();
Logger.Start();

try
{

    #if DEBUG
    string testType = string.Empty;
    if (args.Length > 0)
        testType = args[0];

    using var game = new Game1(testType);
    game.Run();
#else
    using var game = new Game1();
    game.Run();
#endif

    Logger.Advice("Game close");
}
catch (Exception ex)
{
    Logger.Fatal(ex);
    Logger.Finish();
	throw;
}

Logger.Finish();
