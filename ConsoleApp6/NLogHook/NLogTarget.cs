using NLog.Targets;
[Target(nameof(NLogTarget))] 
public sealed class NLogTarget : TargetWithLayout
{ 
    public NLogTarget()
    {
        StaticLoader.StaticLoad();
    }
}