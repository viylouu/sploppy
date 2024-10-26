using SimulationFramework.Desktop;

partial class sploppy {
    static void Main() {
        Simulation sim = Simulation.Create(init, rend);
        sim.Run(new DesktopPlatform());
    }
}