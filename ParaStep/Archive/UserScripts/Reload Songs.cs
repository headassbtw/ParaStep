using System;
using ParaStep;
using ParaStep.Menus.Levels;
using ParaStep.Simfile;

LevelSelectMenu lsm = StateManager.Get<LevelSelectMenu>() as LevelSelectMenu;
lsm.Simfiles.Clear();
lsm.LoadSongs();
