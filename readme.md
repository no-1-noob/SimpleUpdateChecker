This is a small subproject to add easy UpdateChecks to my BeatSaber mods with ingame display for the players.

(Ugly early screenshot)
![20240526171136_1](https://github.com/no-1-noob/SimpleUpdateChecker/assets/91905916/d237270e-5adc-46a2-90af-3644a3eeef7f)

How to use:
### Add the project as a submodule to the mod
Execute in the main mod folder
```
git submodule add https://github.com/no-1-noob/SimpleUpdateChecker.git
git submodule update --init --recursive
```
### Add the project to your solution
Rightclick your solution -> Add existing project -> choose the .csproj

### Add the SimpleUpdateChecker to your mod using source reference
Edit the .csproj of the mod with a editor and add the following. Reloading the .sln may be needed for the files to show.
```xml
<ItemGroup>
    <Compile Include="..\SimpleUpdateChecker\**\*.cs" Link="SimpleUpdateChecker\%(RecursiveDir)%(FileName)%(Extension)" />
    <EmbeddedResource Include="..\SimpleUpdateChecker\**\*.bsml" Link="SimpleUpdateChecker\%(RecursiveDir)%(FileName)%(Extension)" />
    <Compile Remove="..\SimpleUpdateChecker\obj\**\*;..\SimpleUpdateChecker\**\obj\**\*" />
    <Compile Remove="..\SimpleUpdateChecker\**\AssemblyInfo.cs" />
  </ItemGroup>
```
### Add the SimpleUpdateChecker to the code
The main plugin has to extend SimpleUpdatePlugin. Call the CreateSimpleUpdateChecker in the Init method. In OnApplicationQuit call base.OnApplicationQuit.

<b>The IPALogger logger is assigned to 'Log' in the CreateSimpleUpdateChecker, so no need to set a new logger.</b>
```c#
namespace PauseCommander
{
    [Plugin(RuntimeOptions.SingleStartInit)]
    internal class Plugin : SimpleUpdatePlugin
    {
        internal static Plugin Instance { get; private set; }

        [Init]
        public void Init(IPALogger logger, Zenjector zenjector)
        {
            base.CreateSimpleUpdateChecker(logger, zenjector, "__UrlToTheNewestVersionStringHere__", "__UrlToNewVersionPageHere__");
            Instance = this;
            //...
        }
        //...
        [OnExit]
        new public void OnApplicationQuit()
        {
            base.OnApplicationQuit();
        }
    }
}
```
