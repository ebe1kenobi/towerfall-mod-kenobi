using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TowerFall;
using ModCompilKenobi;
using TowerfallAi.Data;

namespace TowerfallAi.Core {
  /// <summary>
  /// Entry point for the modifications in the game.
  /// </summary>
  public static class AiMod {
    // Original Mods
    public const string TFModAiPythonSource = "https://github.com/ebe1kenobi/tf-mod-ai-python";
    public const string TFModAiPythonVersion = "v0.2";
    // Fork Mods
    public const string ModAiSource = "https://github.com/TowerfallAi/towerfall-ai";
    public const string ModAiVersion = "v0.1.1"; 

    private const string poolName = "default";
    private const string defaultConfigName = "config.json";

    public static bool AgentConnected = false;
    public static AgentConnection[] agents = new AgentConnection[TFGame.Players.Length];

    public static PlayerInput[] savedHumanPlayerInput = new PlayerInput[TFGame.Players.Length];
    public static int[] nbPlayerType = new int[TFGame.Players.Length];
    public static PlayerType[] currentPlayerType = new PlayerType[TFGame.Players.Length];
    public static bool isHumanPlayerTypeSaved = false;

    // If this is set to false, this mod should do no effect.
    public static bool ModAIEnabled { get; private set;}
    public static bool ModAITraining { get; private set;}

    public static readonly Random Random = new Random((int)DateTime.UtcNow.Ticks);

    public static readonly TimeSpan DefaultAgentTimeout = new TimeSpan(0, 0, 10);

    public static string ConfigPath = Util.PathCombine(ModCompilKenobi.ModCompilKenobi.BaseDirectory, defaultConfigName);

    public static MatchConfig Config { get; private set; }

    private static readonly TimeSpan ellapsedGameTime = new TimeSpan(10000000 / 60);

    private static MatchSettings matchSettings;

    public static ConnectionDispatcher ConnectionDispatcher;

    public static bool IsNoConfig { get { return true; } }
    public static bool IsFastrun { get { return true; } }
    public static bool NoGraphics { get { return false; } }

    private static object ongoingOperationLock = new object();

    private static ReconfigOperation reconfigOperation;
    private static ResetOperation resetOperation;
    private static CancellationTokenSource ctsSession = new CancellationTokenSource();

    public static Stopwatch gameTimeWatch;
    private static TimeSpan totalGameTime = new TimeSpan();
    private static long totalFrame = 0;

    private static readonly Stopwatch fpsWatch = new Stopwatch();

    public static bool loggedScreenSize;

    private static bool sessionEnded;

    static Mutex loadContentMutex = new Mutex(false, "Towerfall_loadContent");

    static bool rematch;
    public static TimeSpan logTimeInterval = new TimeSpan(0, 1, 0);
    
    public class ReconfigOperation {
      public MatchConfig Config { get; set; }
      public List<RemoteConnection> Connections { get; set; }
    }

    public class ResetOperation {
      public List<JObject> Entities { get; set; }
    }

    public static void ParseArgs(string[] args) {
      ModAIEnabled = true;
      for (int i = 0; i < args.Length; i++)
      {
        if (args[i] == "--noaimod")
        {
          ModAIEnabled = false;
        }
        if (args[i] == "--aimodtraining")
        {
          ModAITraining = true;
          ModAIEnabled = true;
        }
      }
    }

    public static void LoadConfigFromPath() {
      if (!File.Exists(ConfigPath)) {
        Logger.Info("No config in {0}. Starting game in normal mode.".Format(ConfigPath));
        Config = new MatchConfig();
        return;
      };

      Logger.Info("Loading config from {0}".Format(AiMod.ConfigPath));
      Config = JsonConvert.DeserializeObject<MatchConfig>(File.ReadAllText(ConfigPath));
    }

    /// <summary>
    /// Called before MonoGame Initialize.
    /// </summary>
    public static void PreGameInitialize() {
      loadContentMutex.WaitOne();
    }

    /// <summary>
    /// Called after MonoGame Initialize.
    /// </summary>
    public static void PostGameInitialize() {
      gameTimeWatch = Stopwatch.StartNew();


      Agents.Init();
      ctsSession = new CancellationTokenSource();

      if (!IsNoConfig) {
        LoadConfigFromPath();
      }

      Logger.Info("Waiting for game to load.");
      while (!TFGame.GameLoaded) {
        Thread.Sleep(200);
      }

      loadContentMutex.ReleaseMutex();

      ConnectionDispatcher = new ConnectionDispatcher(poolName);

      Logger.Info("Post Game Initialize.");
    }
    public static void HandleFailure(Exception ex) {
      if (ex is SocketException) {
        Logger.Info($"Connection error. Session will stop and wait for another config. Exception:\n  {ex}");
        EndSession();
      } else if (ex is JsonSerializationException || ex is JsonReaderException) {
        Logger.Info($"Serialization error. Session will stop and wait for another config. Exception:\n  {ex}");
        EndSession();
      } else if (ex is OperationCanceledException) {
        Logger.Info($"Task cancelled.\n  {ex}");
      } else if (ex is TimeoutException) {
        Logger.Info($"Task timed out.\n  {ex}");
      } else {
        Logger.Info($"Unhandled exception.\n  {ex}");
        throw ex;
      }
    }

    /// <summary>
    /// Change the config of the game. This will cause the session to restart.
    /// </summary>
    public static void Reconfig(ReconfigOperation operation) {
      lock (ongoingOperationLock) {
        Logger.Info("Cancel ongoing session.");
        ctsSession.Cancel();
        reconfigOperation = operation;
      }
    }

    /// <summary>
    /// Changes the starting configuration of entities. This is faster than a Reconfig.
    /// </summary>
    public static void Reset(ResetOperation operation) {
      lock (ongoingOperationLock) {
        resetOperation = operation;
      }
    }

    public static void Rematch() {
      rematch = true;
    }

    public static void EndSession() {
      if (sessionEnded) return;
      lock (ongoingOperationLock) {
        sessionEnded = true;
        Logger.Info("End Session");
        ctsSession.Cancel();
        Agents.DisconnectAllAgents();
      }
    }

    public static bool IsMatchRunning() {
      if (IsNoConfig) {
        if (Config == null) return false;
        if (Config.agents == null) return false;
        if (Config.agents.Count == 0) return false;
        if (Config.mode == GameModes.Sandbox && !Agents.IsReset) return false;
      }

      if (Config != null &&
          Config.agents != null &&
          Config.agents.Count > 0 &&
          !Agents.Ready) return false;

      return true;
    }

    private static void StartNewSession() {
      Logger.Info("Starting a new session.");
      CreateMatchSettings();
      Session session = new Session(matchSettings);
      session.QuestTestWave = Config.skipWaves;
      session.StartGame();
      Logger.Info("Session started.");
      sessionEnded = false;
      rematch = false;
      Agents.SessionRestarted();
    }

    public static GameTime GetGameTime()
    {
      return new GameTime(totalGameTime, ellapsedGameTime);
    }

    /// <summary>
    /// This is called before a MonoGame Update. Returns false if the frame should be skipped. This method should not ever be blocked by IO,
    /// otherwise the game window freezes.
    /// </summary>
    public static bool PreUpdate() {
      lock (ongoingOperationLock) {
        // All changes happen in the main thread to avoid race condition during Updates.
        if (ctsSession.IsCancellationRequested) {
          ctsSession = new CancellationTokenSource();
        }

        if (reconfigOperation != null) {
          // Reconfig without a new config works as a Rematch.
          if (reconfigOperation.Config != null) { 
            Config = reconfigOperation.Config;
            Agents.PrepareAgentConnections(Config.agents);
            Agents.AssignRemoteConnections(reconfigOperation.Connections, ctsSession.Token);
          }
          
          reconfigOperation = null;
          if (AiMod.ModAITraining)
          {
            StartNewSession();
          }
        } else if (rematch) {
          if (AiMod.ModAITraining)
          {
            StartNewSession();
          }
        }

        if (resetOperation != null) {
          Agents.Reset(resetOperation.Entities, ctsSession.Token);
          resetOperation = null;
        }
      }

      if (!IsMatchRunning()) {
        Sound.StopSound();
        return false;
      } else {
        Sound.ResumeSound();
      }

      totalFrame++;
      totalGameTime += ellapsedGameTime;
      return true;
    }

    public static bool IsHumanPlaying() {
      if (Config.mode == null) return true;
      if (NoGraphics) return false;

      foreach (AgentConfig agent in Config.agents) {
        if (agent.type == AgentConfig.Type.Human) {
          return true;
        }
      }

      return false;
    }

    public static void OnExit() {
      if (ConnectionDispatcher != null) {
        ConnectionDispatcher.UnregisterFromPool();
      }

      Logger.Info($"Game process ended. Pid: {Process.GetCurrentProcess().Id}");
    }

    private static LevelSystem getLevel(MatchConfig Config) {
      if (Config.mode == GameModes.LastManStanding || Config.mode == GameModes.HeadHunters
          || Config.mode == GameModes.TeamDeathmatch || Config.mode == GameModes.PlayTag)
      {
        if (Config.randomLevel)
        {
          System.Random rnd = new Random();
          return GameData.VersusTowers[rnd.Next(1, 17)].GetLevelSystem(); //16 levels
        }
        else if (Config.level != 0)
        {
          return GameData.VersusTowers[Config.level].GetLevelSystem();
        }
        else
        {
          return GameData.VersusTowers[1].GetLevelSystem();
        }
      }
      else if(Config.mode == GameModes.Quest)
      {
        if (Config.randomLevel)
        {
          System.Random rnd = new Random();
          return GameData.QuestLevels[rnd.Next(1, 13)].GetLevelSystem(); //12 levels
        }
        else if (Config.level != 0) {
          return GameData.QuestLevels[Config.level].GetLevelSystem();
        }
        else
        {
          return GameData.QuestLevels[1].GetLevelSystem();
        }
      }
      else if (Config.mode == GameModes.DarkWorld)
      {
        if (Config.randomLevel)
        {
          System.Random rnd = new Random();
          return GameData.DarkWorldTowers[rnd.Next(1, 5)].GetLevelSystem(); // 4 levels
        }
        else if (Config.level != 0)
        {
          return GameData.DarkWorldTowers[Config.level].GetLevelSystem();
        }
        else
        {
          return GameData.DarkWorldTowers[1].GetLevelSystem();
        }
      }
      else if (Config.mode == GameModes.Trials)
      {
        if (Config.randomLevel)
        {
          System.Random rnd = new Random();
          return GameData.TrialsLevels[rnd.Next(1, 17), rnd.Next(1, 4)].GetLevelSystem(); //16 levels with 3 sublevels
        }
        else if (Config.level != 0)
        {
          return GameData.TrialsLevels[Config.level, Config.subLevel].GetLevelSystem();
        }
        else
        {
          return GameData.TrialsLevels[1,1].GetLevelSystem();
        }
      }
      else //default QuestLevels
      {
          return GameData.QuestLevels[0].GetLevelSystem();
      }
    }

    private static void CreateMatchSettings() {
      if (!IsNoConfig) {
        Config = JsonConvert.DeserializeObject<MatchConfig>(File.ReadAllText(ConfigPath));
      }
      MatchSettings.MatchLengths matchLength;
      if (Config.matchLengths == "Instant") {
        matchLength = MatchSettings.MatchLengths.Instant;
      }
      else if (Config.matchLengths == "Quick")
      {
        matchLength = MatchSettings.MatchLengths.Quick;
      }
      else if (Config.matchLengths == "Epic")
      {
        matchLength = MatchSettings.MatchLengths.Epic;
      }
      else {
        matchLength = MatchSettings.MatchLengths.Standard;

      }
      LevelSystem levelSystem = getLevel(Config);
      
      if (Config.mode == GameModes.HeadHunters)
      {
        Logger.Info("Configuring HeadHunters mode.");
        matchSettings = new MatchSettings(levelSystem, Modes.HeadHunters, matchLength);
        matchSettings.Variants.TournamentRules();
      }
      else if (Config.mode == GameModes.TeamDeathmatch)
      {
        Logger.Info("Configuring TeamDeathmatch mode.");
        matchSettings = new MatchSettings(levelSystem, Modes.TeamDeathmatch, matchLength);
        matchSettings.Variants.TournamentRules();
      }
      else if (Config.mode == GameModes.LastManStanding)
      {
        Logger.Info("Configuring LastManStanding mode.");
        matchSettings = new MatchSettings(levelSystem, Modes.LastManStanding, matchLength);
        matchSettings.Variants.TournamentRules();
      }
      else if (Config.mode == GameModes.PlayTag)
      {
        Logger.Info("Configuring PlayTag mode.");
        matchSettings = new MatchSettings(levelSystem, Modes.PlayTag, matchLength);
        matchSettings.Variants.TournamentRules();
      }
      else if (Config.mode == GameModes.Quest)
      {
        Logger.Info("Configuring Quest mode.");
        matchSettings = new MatchSettings(levelSystem, Modes.Quest, matchLength);
        if (Config.difficulty == "Hardcore")
        {
          matchSettings.QuestHardcoreMode = true; 
        }
        else
        {
          matchSettings.QuestHardcoreMode = false;
        }
      }
      else if (Config.mode == GameModes.DarkWorld)
      {
        Logger.Info("Configuring DarkWorld mode.");
        matchSettings = new MatchSettings(levelSystem, Modes.DarkWorld, matchLength);

        if (Config.difficulty == "Legendary") {
          matchSettings.DarkWorldDifficulty = DarkWorldDifficulties.Legendary;
        }
        else if (Config.difficulty == "Hardcore") {
          matchSettings.DarkWorldDifficulty = DarkWorldDifficulties.Hardcore;
        }
        else {
          matchSettings.DarkWorldDifficulty = DarkWorldDifficulties.Normal;
        }
      }
      else if (Config.mode == GameModes.Trials)
      {
        Logger.Info("Configuring Trials mode.");
        matchSettings = new MatchSettings(levelSystem, Modes.Trials, matchLength);
      }
      else if (Config.mode == GameModes.Sandbox)
      {
        Logger.Info("Configuring Sandbox mode.");
        matchSettings = MatchSettings.GetDefaultTrials();
        matchSettings.Mode = Modes.LevelTest;
        matchSettings.LevelSystem = new SandboxLevelSystem(GameData.QuestLevels[Config.level], Config.solids);
      }
      else
      {
        throw new Exception("Game mode not supported: {0}".Format(Config.mode));
      }

      int indexHuman = 0;
      int indexRemote = Agents.CountHumanConnections(Config.agents);
      int indexForTeam = 0;
      for (int i = 0; i < Config.agents.Count; i++) {
        var agent = Config.agents[i];
        // when human in agent config, the distribution is erronous when Teams are involved !
        // because the human joystick are always at the beginning and the remote at the end
        // if human, we need to calculate the right index Like in TFGame.PlayerInput
        if (agent.type == "human")
        {
          indexForTeam = indexHuman;
          indexHuman++;
        }
        else
        {
          indexForTeam = indexRemote;
          indexRemote++;
        }

        TFGame.Players[indexForTeam] = true;
        TFGame.Characters[indexForTeam] = agent.GetArcherIndex();
        TFGame.AltSelect[indexForTeam] = agent.GetArcherType();

        matchSettings.Teams[indexForTeam] = agent.GetTeam();
      }
    }

    public static void ValidateConfig(MatchConfig config) {
      
      if (config.mode == null && IsNoConfig) {
        throw new ConfigException("Game mode need to be specified in config request.");
      }
      if (config.mode == null && IsFastrun)
      {
        throw new ConfigException("Fastrun can only be enabled when game mode is selected.");
      }

      switch (config.mode) {
        case "sandbox":
          if (config.agents == null || config.agents.Count <= 0)
          {
            throw new ConfigException("No agent in config, starting normal game.");
          }
          if (config.level <= 0)
          {
            throw new ConfigException("Invalid level {0}.");
          }
          break;
        case "LastManStanding":
        case "HeadHunters":
        case "TeamDeathmatch":
        case "Quest":
        case "DarkWorld":
        case "Trials":
        case "PlayTag":
          //TODO
          //skipWaves
          //solids

          if (config.agentTimeout == null)
          {
            Logger.Info($"Agent timeout not specified. Using default {DefaultAgentTimeout}.");
            config.agentTimeout = DefaultAgentTimeout;
          }

          if (config.agents.Count > 8)
          {
            throw new ConfigException("Too many agents. Only 8 players are supported.");
          }
          if (TF8PlayerMod.TF8PlayerMod.Mod8PEnabled && AiMod.ModAITraining && config.agents.Count > 6 && config.mode == "TeamDeathmatch")
          {
            throw new ConfigException("Too many agents. Only 6 players are supported for TeamDeathmatch.");
          }

          //If not training, the matchsettings will be set in the game interface
          if (!AiMod.ModAITraining)
          {
            break;
          }
          if (!config.randomLevel)
          {
            if (config.level <= 0)
            {
              throw new ConfigException("Invalid level {0}.".Format(config.level));
            }

            if (config.mode == "Trials" && config.subLevel <= 0) {
              throw new ConfigException("Invalid subLevel {0}.".Format(config.subLevel));
            }
          }

          if ((config.mode == "LastManStanding" || config.mode == "HeadHunters"
            || config.mode == "TeamDeathmatch" || config.mode == "PlayTag") &&
                config.matchLengths != "Instant" && config.matchLengths != "Quick" &&
                config.matchLengths != "Standard" && config.matchLengths != "Epic") {
            throw new ConfigException("matchLengths invalid.");
          }

          if (config.mode == "DarkWorld" &&
              (config.difficulty != "Normal" && config.difficulty != "Hardcore" && config.difficulty != "Legendary"))
          {
            throw new ConfigException("difficulty invalid.");
          }
          if (config.mode == "Quest" && config.agents.Count > 4)
          {
            throw new ConfigException("DarkWorld mode is for 1-4 player.");
          }

          if (config.mode == "Quest" &&
              (config.difficulty != "Normal" && config.difficulty != "Hardcore"))
          {
            throw new ConfigException("difficulty invalid.");
          }
          if (config.mode == "Quest" && config.agents.Count > 2)
          {
            throw new ConfigException("Quest mode is for 1-2 player.");
          }

          if (config.mode == "Trial" && config.agents.Count > 1)
          {
            throw new ConfigException("Trials mode is for 1 player.");
          }
          break;
        default:
        throw new ConfigException("Mode value unknown.");
      }
    }

    public static void LogGameTime() {
      Logger.Info("{0}s, {1} frames".Format((long)GetGameTime().TotalGameTime.TotalSeconds, totalFrame));
    }


  }
}
