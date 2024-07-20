
namespace ModCompilKenobi
{
  public static class ModCompilKenobi {

    public const string ModCompilKenobiVersion = "v0.1.0";
    public const string BaseDirectory = "modcompilkenobi";

    public static void InitLog() {
      Util.CreateDirectory(ModCompilKenobi.BaseDirectory);
      Logger.Init(ModCompilKenobi.BaseDirectory);
    }
  }
}
