using HorrorShorts_Game;

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
