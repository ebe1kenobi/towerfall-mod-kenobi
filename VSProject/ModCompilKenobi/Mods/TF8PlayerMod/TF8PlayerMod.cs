namespace TF8PlayerMod
{
  public static class TF8PlayerMod
  {
    public const string TF8PlayerModVersion = "v1.3";
    public const string TF8PlayerModSource = "https://github.com/Jonesey13/TF-8-Player";

    public static bool Mod8PEnabled { get; private set; }

    public static void ParseArgs(string[] args)
    {
      Mod8PEnabled = true;
      for (int i = 0; i < args.Length; i++)
      {
        if (args[i] == "--no8pmod")
        {
          Mod8PEnabled = false;
        }
      }
    }

    public static int GetPlayerCount()
    {
      return Mod8PEnabled ? 8 : 4;
    }
  }
}
