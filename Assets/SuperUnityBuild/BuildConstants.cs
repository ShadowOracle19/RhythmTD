using System;

// This file is auto-generated. Do not modify or move this file.

namespace SuperUnityBuild.Generated
{
    public enum ReleaseType
    {
        None,
        Beta,
    }

    public enum Platform
    {
        None,
        Windows,
        WebGL,
    }

    public enum ScriptingBackend
    {
        None,
        Mono,
        IL2CPP,
    }

    public enum Target
    {
        None,
        Player,
    }

    public enum Distribution
    {
        None,
        itch_io,
    }

    public static class BuildConstants
    {
        public static readonly DateTime buildDate = new DateTime(638723735961761465);
        public const string version = "1.0.0.1";
        public const int buildCounter = 1;
        public const ReleaseType releaseType = ReleaseType.Beta;
        public const Platform platform = Platform.WebGL;
        public const ScriptingBackend scriptingBackend = ScriptingBackend.IL2CPP;
        public const Target target = Target.Player;
        public const Distribution distribution = Distribution.itch_io;
    }
}

