using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace electrolyte
{
    static class MapBuilder
    {
        public static MapModel BuildLevel1(Game game, Size mapSize, ref Character aChar)
        {
            MapModel map = new MapModel(game, mapSize, Score.GenerateDefaultScore(), "bg");
            MapHelper.SetPlayerInfo(aChar.JumpHeight, aChar.mSize);
            MapHelper.SetMap(ref map);

            MapHelper.BuildMapBorders();

            MapHelper.BuildResistor(3, 1, Resistor.Type.PLAYER_TWO);
            MapHelper.BuildResistor(2, 1, Resistor.Type.PLAYER_TWO);

            MapHelper.BuildPlatform(2, 1, 2, Wall.Colors.BLUE);

            MapHelper.BuildCollectable(1.9f, 1.8f, 1);
            MapHelper.BuildCollectable(3f, 1.8f, 1);
            MapHelper.BuildResistor(3, 2, Resistor.Type.PLAYER_ONE);
            MapHelper.BuildResistor(2, 2, Resistor.Type.PLAYER_ONE);

            // 2nd platform level 1
            MapHelper.BuildPlatform(7, 1, 2, Wall.Colors.RED);
            MapHelper.BuildResistor(7, 1, Resistor.Type.PLAYER_ONE);
            MapHelper.BuildResistor(8, 1, Resistor.Type.PLAYER_ONE);

            MapHelper.BuildCollectable(6.9f, 1.8f, 1);
            MapHelper.BuildCollectable(8f, 1.8f, 1);
            MapHelper.BuildResistor(7, 2, Resistor.Type.PLAYER_TWO);
            MapHelper.BuildResistor(8, 2, Resistor.Type.PLAYER_TWO);

            MapHelper.BuildWall(MapHelper.GetLastColumn() - 2,
                                1,
                                new Size(map.mSize.Width - MapHelper.GetColumnXAt(MapHelper.GetLastColumn() - 2) - MapHelper.GetOffset(),
                                         map.mSize.Height - MapHelper.GetPlatformYAtLevel(1) - MapHelper.GetOffset()),
                                Wall.Colors.WHITE);

            MapHelper.BuildPlatform(0, 2, MapHelper.GetLastColumn()-2, Wall.Colors.BLUE);

            MapHelper.BuildMoveableBox(MapHelper.GetLastColumn() - 4, 3f, new Size(40, 20));
            MapHelper.BuildCollectable(MapHelper.GetLastColumn() - 6, 3, 1);
            MapHelper.BuildCollectable(MapHelper.GetLastColumn() - 7, 3, 1);
            MapHelper.BuildResistor(MapHelper.GetLastColumn() - 6, 3.1f, Resistor.Type.PLAYER_TWO);
            MapHelper.BuildResistor(MapHelper.GetLastColumn() - 7, 3.1f, Resistor.Type.PLAYER_TWO);

            MapHelper.BuildCollectable(MapHelper.GetLastColumn() - 10, 3, 1);
            MapHelper.BuildCollectable(MapHelper.GetLastColumn() - 11, 3, 1);
            MapHelper.BuildResistor(MapHelper.GetLastColumn() - 10, 3.1f, Resistor.Type.PLAYER_ONE);
            MapHelper.BuildResistor(MapHelper.GetLastColumn() - 11, 3.1f, Resistor.Type.PLAYER_ONE);

            ElevatorModel elev1 = MapHelper.BuildElevator(0, 4);
            SwitchModel switch1 = MapHelper.BuildSwitch(3, 2.2f);
            MapHelper.RegisterSwitchToElevator(ref switch1, ref elev1);

            MapHelper.BuildPlatform(1, 4, MapHelper.GetLastColumn() - 2, Wall.Colors.BLUE);
            MapHelper.BuildPlatform(2, 5, 2, Wall.Colors.BLUE);

            SwitchModel switch12 = MapHelper.BuildSwitch(5, 4.05f);
            MapHelper.RegisterSwitchToElevator(ref switch12, ref elev1);

            MapHelper.BuildResistor(2, 5, Resistor.Type.NEUTRAL);
            MapHelper.BuildResistor(3, 5, Resistor.Type.NEUTRAL);

            MapHelper.BuildCollectable(7, 5, 1);
            MapHelper.BuildCollectable(8, 5, 1);

            MapHelper.BuildPlatform(11, 5, 2, Wall.Colors.BLUE);

            MapHelper.BuildResistor(11, 5, Resistor.Type.NEUTRAL);
            MapHelper.BuildResistor(12, 5, Resistor.Type.NEUTRAL);

            MapHelper.BuildWall(MapHelper.GetLastColumn() - 2.5f,
                                4.5f,
                                new Size(map.mSize.Width - MapHelper.GetColumnXAt(MapHelper.GetLastColumn() - 2.5f) - MapHelper.GetOffset(),
                                         map.mSize.Height - MapHelper.GetPlatformYAtLevel(0.5f) - MapHelper.GetOffset()),
                                Wall.Colors.WHITE);

            MapHelper.BuildWall(MapHelper.GetLastColumn() - 1.8f,
                                5f,
                                new Size(map.mSize.Width - MapHelper.GetColumnXAt(MapHelper.GetLastColumn() - 1.8f) - MapHelper.GetOffset(),
                                         map.mSize.Height - MapHelper.GetPlatformYAtLevel(0.6f) - MapHelper.GetOffset()),
                                Wall.Colors.WHITE);

            MapHelper.BuildPlatform(0, 6, MapHelper.GetLastColumn()-2, Wall.Colors.BLUE);

            MapHelper.BuildPlatform((MapHelper.GetLastColumn()/2)-3, 7, 4, Wall.Colors.BLUE);

            MapHelper.BuildPlatform(0, 7, 1, Wall.Colors.BLUE);
            MapHelper.BuildCollectable(0, 8, 1);
            MapHelper.BuildPlatform(MapHelper.GetLastColumn()-2, 7, 1, Wall.Colors.BLUE);
            MapHelper.BuildCollectable(MapHelper.GetLastColumn()-2, 8, 1);

            ExitDoorModel door1 = MapHelper.BuildExitDoor((MapHelper.GetLastColumn()/2)-1, 7, PlayerIndex.One);
            ExitDoorModel door2 = MapHelper.BuildExitDoor((MapHelper.GetLastColumn() / 2) - 3f, 7, PlayerIndex.Two);

            map.AssignExitDoors(ref door1, ref door2);

            return map;
        }

        public static MapModel BuildSolo(Game game, Size mapSize, ref Character aChar)
        {
            MapModel map = new MapModel(game, mapSize, Score.GenerateDefaultScore(), "bg");
            MapHelper.SetPlayerInfo(aChar.JumpHeight, aChar.mSize);
            MapHelper.SetMap(ref map);

            MapHelper.BuildMapBorders();

            MapHelper.BuildResistor(2, 1, Resistor.Type.NEUTRAL);
            MapHelper.BuildResistor(2.9f, 1, Resistor.Type.NEUTRAL);
            MapHelper.BuildResistor(3.7f, 1, Resistor.Type.NEUTRAL);
            MapHelper.BuildResistor(4.5f, 1, Resistor.Type.NEUTRAL);
            MapHelper.BuildResistor(5.3f, 1, Resistor.Type.NEUTRAL);

            MapHelper.BuildResistor(MapHelper.GetLastColumn() - 4, 1, Resistor.Type.NEUTRAL);
            MapHelper.BuildResistor(MapHelper.GetLastColumn() - 4.9f, 1, Resistor.Type.NEUTRAL);
            MapHelper.BuildResistor(MapHelper.GetLastColumn() - 5.7f, 1, Resistor.Type.NEUTRAL);
            MapHelper.BuildResistor(MapHelper.GetLastColumn() - 6.5f, 1, Resistor.Type.NEUTRAL);
            MapHelper.BuildResistor(MapHelper.GetLastColumn() - 7.3f, 1, Resistor.Type.NEUTRAL);


            MapHelper.BuildCollectable((MapHelper.GetLastColumn() / 2) + 0.5f , 1, 1);
            MapHelper.BuildCollectable((MapHelper.GetLastColumn() / 2)-0.7f, 1, 1);
            MapHelper.BuildCollectable((MapHelper.GetLastColumn() / 2) - 1.8f, 1, 1);

            MapHelper.BuildPlatform(1, 1, 1, Wall.Colors.BLUE);
            MapHelper.BuildCollectable(1,2,1);
            MapHelper.BuildPlatform(0, 2, 1, Wall.Colors.BLUE);
            MapHelper.BuildCollectable(0, 3, 1);
            MapHelper.BuildPlatform(1, 3, 1, Wall.Colors.BLUE);
            MapHelper.BuildCollectable(1, 4, 1);
            MapHelper.BuildPlatform(0, 4, 1, Wall.Colors.BLUE);
            MapHelper.BuildCollectable(0, 5, 1);
            MapHelper.BuildPlatform(1, 5, 1, Wall.Colors.BLUE);
            MapHelper.BuildCollectable(1, 6, 1);
            MapHelper.BuildPlatform(0, 6, 1, Wall.Colors.BLUE);
            MapHelper.BuildCollectable(0, 7, 1);
            MapHelper.BuildResistor(0, 7, Resistor.Type.PLAYER_ONE);

            MapHelper.BuildPlatform(MapHelper.GetLastColumn() - 3, 1, 1, Wall.Colors.BLUE);
            MapHelper.BuildCollectable(MapHelper.GetLastColumn() - 3, 2, 1);
            MapHelper.BuildPlatform(MapHelper.GetLastColumn() - 2, 2, 1, Wall.Colors.BLUE);
            MapHelper.BuildCollectable(MapHelper.GetLastColumn() - 2, 3, 1);
            MapHelper.BuildPlatform(MapHelper.GetLastColumn() - 3, 3, 1, Wall.Colors.BLUE);
            MapHelper.BuildCollectable(MapHelper.GetLastColumn() - 3, 4, 1);
            MapHelper.BuildPlatform(MapHelper.GetLastColumn() - 2, 4, 1, Wall.Colors.BLUE);
            MapHelper.BuildCollectable(MapHelper.GetLastColumn() - 2, 5, 1);
            MapHelper.BuildPlatform(MapHelper.GetLastColumn() - 3, 5, 1, Wall.Colors.BLUE);
            MapHelper.BuildCollectable(MapHelper.GetLastColumn() - 3, 6, 1);
            MapHelper.BuildPlatform(MapHelper.GetLastColumn() - 2, 6, 1, Wall.Colors.BLUE);
            MapHelper.BuildCollectable(MapHelper.GetLastColumn() - 2, 7, 1);
            MapHelper.BuildResistor(MapHelper.GetLastColumn()-2, 7, Resistor.Type.PLAYER_TWO);


            MapHelper.BuildPlatform(3, 5, MapHelper.GetLastColumn() - 7, Wall.Colors.BLUE);
            MapHelper.BuildResistor(5, 6, Resistor.Type.PLAYER_ONE);
            MapHelper.BuildResistor(5.8f, 6, Resistor.Type.PLAYER_ONE);
            MapHelper.BuildResistor(MapHelper.GetLastColumn() - 7, 6,Resistor.Type.PLAYER_TWO);
            MapHelper.BuildResistor(MapHelper.GetLastColumn() - 7.8f, 6, Resistor.Type.PLAYER_TWO);

            MapHelper.BuildMoveableBox(4, 5.5f, new Size(40, 20));
            MapHelper.BuildMoveableBox(MapHelper.GetLastColumn()-6, 5.5f, new Size(40, 20));

            MapHelper.BuildPlatform(1, 7, MapHelper.GetLastColumn() - 3, Wall.Colors.BLUE);
            ElevatorModel e1 = MapHelper.BuildElevator(3.3f, 8.5f,1,0.8f);
            ElevatorModel e2 = MapHelper.BuildElevator(MapHelper.GetLastColumn() - 5.7f, 8.5f, 1, 0.8f);
            MapHelper.BuildResistor(3, 8, Resistor.Type.PLAYER_TWO);
            MapHelper.BuildResistor(3.8f, 8, Resistor.Type.PLAYER_TWO);
            MapHelper.BuildResistor(MapHelper.GetLastColumn() - 6, 8, Resistor.Type.PLAYER_ONE);
            MapHelper.BuildResistor(MapHelper.GetLastColumn() - 5.2f, 8, Resistor.Type.PLAYER_ONE);
            SwitchModel s1 = MapHelper.BuildSwitch(3.5f, 5);
            SwitchModel s2 = MapHelper.BuildSwitch(MapHelper.GetLastColumn()-4.5f, 5);
            MapHelper.RegisterSwitchToElevator(ref s1, ref e1);
            MapHelper.RegisterSwitchToElevator(ref s2, ref e2);

            MapHelper.BuildPlatform(0, 8, 2, Wall.Colors.BLUE);
            MapHelper.BuildPlatform(MapHelper.GetLastColumn() - 3, 8, 2, Wall.Colors.BLUE);
            ExitDoorModel door1 = MapHelper.BuildExitDoor(0, 8, PlayerIndex.One);
            ExitDoorModel door2 = MapHelper.BuildExitDoor(MapHelper.GetLastColumn()-3, 8, PlayerIndex.Two);

            map.AssignExitDoors(ref door1, ref door2);

            return map;
        }

        public static MapModel BuildBossLevel(Game game, Size mapSize, ref Character aChar)
        {
            MapModel map = new MapModel(game, mapSize, Score.GenerateDefaultScore(), "bg");
            MapHelper.SetPlayerInfo(aChar.JumpHeight, aChar.mSize);
            MapHelper.SetMap(ref map);

            MapHelper.BuildMapBorders();

            ExitDoorModel door1 = MapHelper.BuildExitDoor(0, 8, PlayerIndex.One);
            ExitDoorModel door2 = MapHelper.BuildExitDoor(MapHelper.GetLastColumn() - 3, 8, PlayerIndex.Two);

            map.AssignExitDoors(ref door1, ref door2);

            return map;
        }

        public static MapModel BuildLevel3(Game game, 
                                           Size mapSize,
                                           ref Character aChar)
        {
            MapModel map = new MapModel(game, mapSize, Score.GenerateDefaultScore(), "bg");
            MapHelper.SetPlayerInfo(aChar.JumpHeight, aChar.mSize);
            MapHelper.SetMap(ref map);

            MapHelper.BuildMapBorders();
            MapHelper.BuildPlatform(0, 1, 6, Wall.Colors.WHITE);

            MapHelper.BuildCollectable(0, 1,1);
            MapHelper.BuildCollectable(1, 1, 1);
            MapHelper.BuildCollectable(2, 1, 1);
            MapHelper.BuildCollectable(3, 1, 1);
            
            MapHelper.BuildPlatform(MapHelper.GetLastColumn() - 2, 1, 2, Wall.Colors.WHITE);
            MapHelper.BuildPlatform(0, 2, MapHelper.GetLastColumn() - 2, Wall.Colors.WHITE);
            SwitchModel switch1 = MapHelper.BuildSwitch(5, 2);
            
            ElevatorModel elev1 = MapHelper.BuildElevator(0, 4f);

            MapHelper.RegisterSwitchToElevator(ref switch1, ref elev1);

            MapHelper.BuildPlatform(MapHelper.GetDefaultElevatorColumnSpan(), 4, MapHelper.GetLastColumn() - MapHelper.GetDefaultElevatorColumnSpan(), Wall.Colors.WHITE);
            SwitchModel switch12 = MapHelper.BuildSwitch(5, 4);

            MapHelper.RegisterSwitchToElevator(ref switch12, ref elev1);

            MapHelper.BuildPlatform(MapHelper.GetLastColumn() - 2, 5, 2, Wall.Colors.WHITE);

            MapHelper.BuildPlatform(0, 6, MapHelper.GetLastColumn() - 2, Wall.Colors.WHITE);

            MapHelper.BuildPlatform(5, 7, 7, Wall.Colors.WHITE);

            ExitDoorModel door1 = MapHelper.BuildExitDoor(6, 7, PlayerIndex.One);
            ExitDoorModel door2 = MapHelper.BuildExitDoor(8, 7, PlayerIndex.Two);

            map.AssignExitDoors(ref door1, ref door2);

            MapHelper.BuildMoveableBox(7, 6, new Size(30,20));

            MapHelper.BuildResistor(8, 1, Resistor.Type.PLAYER_ONE);

            return map;
        }

        public static MapModel BuildLevel2(Game game,
                                          Size mapSize,
                                          ref Character aChar)
        {
            MapModel map = new MapModel(game, mapSize, Score.GenerateDefaultScore(), "bg");
            MapHelper.SetPlayerInfo(aChar.JumpHeight, aChar.mSize);
            MapHelper.SetMap(ref map);

            MapHelper.BuildMapBorders();

            MapHelper.BuildPlatform(0, 1.2f, 5, Wall.Colors.BLUE);
            MapHelper.BuildWall(MapHelper.GetLastColumn() - 3, 
                                1, 
                                new Size(map.mSize.Width - MapHelper.GetColumnXAt(MapHelper.GetLastColumn()-3) - MapHelper.GetOffset(), 
                                         map.mSize.Height - MapHelper.GetPlatformYAtLevel(1) - MapHelper.GetOffset()), 
                                Wall.Colors.WHITE);

            MapHelper.BuildPlatform(7, 2f, 7, Wall.Colors.BLUE);
            MapHelper.BuildWall(7,2,new Size(MapHelper.GetOffset(), MapHelper.GetOffset()), Wall.Colors.BLUE);
            MapHelper.BuildPlatform(0, 3f, 7, Wall.Colors.BLUE);

            SwitchModel switch1 = MapHelper.BuildSwitch(5, 3);
            ElevatorModel elev1 = MapHelper.BuildElevator(0, 5.5f,1.5f,2.7f);

            MapHelper.RegisterSwitchToElevator(ref switch1, ref elev1);

            ExitDoorModel door1 = MapHelper.BuildExitDoor(6, 7, PlayerIndex.One);
            ExitDoorModel door2 = MapHelper.BuildExitDoor(8, 7, PlayerIndex.Two);

            map.AssignExitDoors(ref door1, ref door2);

            return map;
        }
    }
}
