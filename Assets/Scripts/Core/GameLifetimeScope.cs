using UnityEngine;
using VContainer;
using VContainer.Unity;

public class GameLifetimeScope : LifetimeScope
{
    [SerializeField] private GameConfig gameConfig;
    
    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterInstance(gameConfig);

        builder.Register<GamePlay>(Lifetime.Singleton);
        builder.Register<GameState>(Lifetime.Singleton);
        builder.Register<GameTimer>(Lifetime.Singleton);
        builder.Register<ItemsManager>(Lifetime.Singleton);
        builder.Register<LevelLoader>(Lifetime.Singleton);
        builder.Register<Settings>(Lifetime.Singleton);
        builder.Register<ISaveManager, PlayerPrefsSaveManager>(Lifetime.Singleton);

        builder.RegisterComponentInHierarchy<GameScene>();

        builder.RegisterComponentInHierarchy<UI>();
        builder.RegisterComponentInHierarchy<MainMenuScreen>();
        builder.RegisterComponentInHierarchy<GameScreen>();
        builder.RegisterComponentInHierarchy<VictoryScreen>();
        builder.RegisterComponentInHierarchy<DefeatScreen>();
        builder.RegisterComponentInHierarchy<SettingsPanel>();

        builder.RegisterEntryPoint<Startup>();
    }
}
