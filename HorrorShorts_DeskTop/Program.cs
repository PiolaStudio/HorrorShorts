using HorrorShorts;

string s;
s = "ft".ToHexShortCommand();
s = "fc".ToHexShortCommand();
s = "bl".ToHexShortCommand();
s = "sp".ToHexShortCommand();
s = "sv".ToHexShortCommand();

using var game = new Game1();
game.Run();
