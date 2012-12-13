using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace electrolyte
{
    static class MapHelper
    {
        private static float OFFSET = 15;
        private static float DEFAULT_PLATFORM_WIDTH = 0;
        private static float DEFAULT_PLATFORM_HEIGHT = -1;
        private static MapModel mMap = null;

        public static float GetOffset()
        {
            return OFFSET;
        }

        public static void SetPlayerInfo(float playerJump, Size playerSize)
        {
            DEFAULT_PLATFORM_WIDTH = playerSize.Width * 1.5f;
            DEFAULT_PLATFORM_HEIGHT = playerJump/1.5f;
        }

        public static void SetMap(ref MapModel map)
        {
            if (map == null) throw new Exception("Null MapModel passed to MapHelper.SetMap");
            mMap = map;
        }

        private static MapModel GetMap()
        {
            if (mMap == null) throw new Exception("MapModel object not passed to helper methods and MapHelper.SetMap was not called before! Call MapHelper.SetMap to assign the map.");
            return mMap;
        }

        public static int GetLastColumn(ref MapModel map)
        {
            if (DEFAULT_PLATFORM_HEIGHT == -1) throw new Exception("Cannot use MapHelper.GetLastColumn before SetPlayerInfo is called");
            return (int)(map.mSize.Width / DEFAULT_PLATFORM_WIDTH) +1;
        }

        public static int GetLastColumn()
        {
            MapModel temp = GetMap();
            return GetLastColumn(ref temp);
        }

        public static int GetLastLevel(ref MapModel map)
        {
            if (DEFAULT_PLATFORM_HEIGHT == -1) throw new Exception("Cannot use MapHelper.GetLastLevel before SetPlayerInfo is called");
            return (int)(map.mSize.Height / DEFAULT_PLATFORM_HEIGHT) +1;
        }

        public static int GetLastLevel()
        {
            MapModel temp = GetMap();
            return GetLastLevel(ref temp);
        }

        public static void BuildMapBorders(ref MapModel map)
        {
            // top wall
            Wall wall = new Wall(map.Game, new Pointf(0, 0), new Size(map.mSize.Width, OFFSET), Wall.Colors.WHITE);
            map.AddComponent(wall);

            // left wall
            wall = new Wall(map.Game, new Pointf(0, 0), new Size(OFFSET, map.mSize.Height), Wall.Colors.WHITE);
            map.AddComponent(wall);

            // bottom wall
            wall = new Wall(map.Game, new Pointf(0, map.mSize.Height - OFFSET), new Size(map.mSize.Width, OFFSET), Wall.Colors.WHITE);
            map.AddComponent(wall);

            // right wall
            wall = new Wall(map.Game, new Pointf(map.mSize.Width - OFFSET, 0), new Size(OFFSET, map.mSize.Height), Wall.Colors.WHITE);
            map.AddComponent(wall);
        }

        public static void BuildMapBorders()
        {
            MapModel temp = GetMap();
            BuildMapBorders(ref temp);
        }

        public static float GetPlatformYAtLevel(float level, ref MapModel map)
        {
            if (DEFAULT_PLATFORM_HEIGHT == -1) throw new Exception("Cannot use MapHelper.GetPlatformYAtLevel before SetPlayerInfo is called");
            return (map.mSize.Height - OFFSET) - (DEFAULT_PLATFORM_HEIGHT * level);
        }

        public static float GetPlatformYAtLevel(float level)
        {
            MapModel temp = GetMap();
            return GetPlatformYAtLevel(level, ref temp);
        }

        public static float GetColumnXAt(float column, ref MapModel map)
        {
            if (DEFAULT_PLATFORM_HEIGHT == -1) throw new Exception("Cannot use MapHelper.GetColumnXAt before SetPlayerInfo is called");
            float temp = OFFSET + column * DEFAULT_PLATFORM_WIDTH;
            if (temp >= map.Right)
            {
                temp = map.Right - map.Origin.X - OFFSET;
            }

            return temp;
        }

        public static float GetColumnXAt(float column)
        {
            MapModel temp = GetMap();
            return GetColumnXAt(column, ref temp);
        }

        public static Pointf GetPointForColumnAndLevel(float column, float level, ref MapModel map)
        {
            return new Pointf(GetColumnXAt(column, ref map), GetPlatformYAtLevel(level, ref map));
        }

        public static Pointf GetPointForColumnAndLevel(float column, float level)
        {
            MapModel temp = GetMap();
            return GetPointForColumnAndLevel(column, level, ref temp);
        }

        public static Wall BuildWall(Pointf wallPosition, Size wallSize, Wall.Colors wallColor, ref MapModel map)
        {
            Wall wall = new Wall(map.Game, new Pointf(wallPosition.X, wallPosition.Y), new Size(wallSize.Width, wallSize.Height), wallColor);
            map.AddComponent(wall);
            return wall;
        }

        public static Wall BuildWall(Pointf wallPosition, Size wallSize, Wall.Colors wallColor)
        {
            var temp = GetMap();
            return BuildWall(wallPosition, wallSize, wallColor, ref temp);
        }

        public static Wall BuildWallAtX(float posX, float platformLevel, Size wallSize, Wall.Colors wallColor, ref MapModel map)
        {
            return BuildWall(new Pointf(posX, GetPlatformYAtLevel(platformLevel, ref map)), wallSize, wallColor, ref map);
        }

        public static Wall BuildWallAtX(float posX, float platformLevel, Size wallSize, Wall.Colors wallColor)
        {
            var temp = GetMap();
            return BuildWallAtX(posX, platformLevel, wallSize, wallColor, ref temp);
        }

        public static Wall BuildWall(float column, float platformLevel, Size wallSize, Wall.Colors wallColor, ref MapModel map)
        {
            return BuildWall(GetPointForColumnAndLevel(column, platformLevel, ref map), wallSize, wallColor, ref map);
        }

        public static Wall BuildWall(float column, float platformLevel, Size wallSize, Wall.Colors wallColor)
        {
            var temp = GetMap();
            return BuildWall(column, platformLevel, wallSize, wallColor, ref temp);
        }

        public static Wall BuildPlatform(float column, float platformLevel, float columnSpan, Wall.Colors color, ref MapModel map)
        {
            return BuildWall(GetPointForColumnAndLevel(column, platformLevel, ref map), new Size(columnSpan * DEFAULT_PLATFORM_WIDTH, OFFSET), color, ref map);
        }

        public static Wall BuildPlatform(float column, float platformLevel, float columnSpan, Wall.Colors color)
        {
            var temp = GetMap();
            return BuildPlatform(column, platformLevel, columnSpan, color, ref temp);
        }

        public static SwitchModel BuildSwitch(Pointf switchPosition, ref MapModel map)
        {
            SwitchModel toAdd = new SwitchModel(map.Game, switchPosition);
            map.AddComponent(toAdd);
            return toAdd;
        }

        public static SwitchModel BuildSwitch(Pointf switchPosition)
        {
            var temp = GetMap();
            return BuildSwitch(switchPosition, ref temp);
        }

        public static SwitchModel BuildSwitchAtX(float posX, float platformLevel, ref MapModel map)
        {
            float yLevel = GetPlatformYAtLevel(platformLevel, ref map) - OFFSET * 3;
            return BuildSwitch(new Pointf(posX, yLevel), ref map);
        }

        public static SwitchModel BuildSwitchAtX(float posX, float platformLevel)
        {
            var temp = GetMap();
            return BuildSwitchAtX(posX, platformLevel, ref temp);
        }

        public static SwitchModel BuildSwitch(float column, float platformLevel, ref MapModel map)
        {
            Pointf switchPos = GetPointForColumnAndLevel(column, platformLevel, ref map);
            switchPos.Y -= OFFSET/2;
            return BuildSwitch(switchPos, ref map);
        }

        public static SwitchModel BuildSwitch(float column, float platformLevel)
        {
            var temp = GetMap();
            return BuildSwitch(column, platformLevel, ref temp);
        }

        public static MoveableBox BuildMoveableBox(Pointf boxPosition, Size boxSize, ref MapModel map)
        {
            MoveableBox box = new MoveableBox(map.Game, boxPosition, boxSize);
            map.AddComponent(box);
            return box;
        }

        public static MoveableBox BuildMoveableBox(Pointf boxPosition, Size boxSize)
        {
            var temp = GetMap();
            return BuildMoveableBox(boxPosition, boxSize, ref temp);
        }

        public static MoveableBox BuildMoveableBoxAtX(float posX, float platformLevel, Size boxSize, ref MapModel map)
        {
            return BuildMoveableBox(new Pointf(posX, GetPlatformYAtLevel(platformLevel, ref map)), boxSize, ref map);
        }

        public static MoveableBox BuildMoveableBoxAtX(float posX, float platformLevel, Size boxSize)
        {
            var temp = GetMap();
            return BuildMoveableBoxAtX(posX, platformLevel, boxSize, ref temp);
        }

        public static MoveableBox BuildMoveableBox(float column, float platformLevel, Size boxSize, ref MapModel map)
        {
            return BuildMoveableBox(GetPointForColumnAndLevel(column, platformLevel, ref map), boxSize, ref map);
        }

        public static MoveableBox BuildMoveableBox(float column, float platformLevel, Size boxSize)
        {
            var temp = GetMap();
            return BuildMoveableBox(column, platformLevel, boxSize, ref temp);
        }

        public static ExitDoorModel BuildExitDoor(Pointf doorPosition, PlayerIndex playerIndex, ref MapModel map)
        {
            ExitDoorModel exit = new ExitDoorModel(map.Game, doorPosition, playerIndex);
            map.AddComponent(exit);
            return exit;
        }

        public static ExitDoorModel BuildExitDoor(Pointf doorPosition, PlayerIndex playerIndex)
        {
            var temp = GetMap();
            return BuildExitDoor(doorPosition, playerIndex, ref temp);
        }

        public static ExitDoorModel BuildExitDoorAtX(float posX, float platformLevel, PlayerIndex playerIndex, ref MapModel map)
        {
            return BuildExitDoor(new Pointf(posX, GetPlatformYAtLevel(platformLevel, ref map)), playerIndex, ref map);
        }

        public static ExitDoorModel BuildExitDoorAtX(float posX, float platformLevel, PlayerIndex playerIndex)
        {
            var temp = GetMap();
            return BuildExitDoorAtX(posX, platformLevel, playerIndex, ref temp);
        }

        public static ExitDoorModel BuildExitDoor(float column, float platformLevel, PlayerIndex playerIndex, ref MapModel map)
        {
            return BuildExitDoor(GetPointForColumnAndLevel(column, platformLevel, ref map), playerIndex, ref map);
        }

        public static ExitDoorModel BuildExitDoor(float column, float platformLevel, PlayerIndex playerIndex)
        {
            var temp = GetMap();
            return BuildExitDoor(column, platformLevel, playerIndex, ref temp);
        }

        public static ElevatorModel BuildElevator(Pointf elevatorPosition, float elevatorWidth, float maxMovementHeight, ref MapModel map)
        {
            ElevatorModel elev = new ElevatorModel(map.Game, elevatorWidth, maxMovementHeight, elevatorPosition);
            map.AddComponent(elev);
            return elev;
        }

        public static ElevatorModel BuildElevator(Pointf elevatorPosition, float elevatorWidth, float maxMovementHeight)
        {
            var temp = GetMap();
            return BuildElevator(elevatorPosition, elevatorWidth, maxMovementHeight, ref temp);
        }

        public static int GetDefaultElevatorColumnSpan()
        {
            return 1;
        }

        public static int GetDefaultElevatorLevelSplan()
        {
            return 2;
        }

        public static ElevatorModel BuildElevator(Pointf elevatorPosition, ref MapModel map)
        {
            return BuildElevator(elevatorPosition, GetDefaultElevatorColumnSpan() * DEFAULT_PLATFORM_WIDTH, GetDefaultElevatorLevelSplan() * DEFAULT_PLATFORM_HEIGHT, ref map);
        }

        public static ElevatorModel BuildElevator(Pointf elevatorPosition)
        {
            var temp = GetMap();
            return BuildElevator(elevatorPosition, ref temp);
        }

        public static ElevatorModel BuildElevator(float column, float platformLevel, ref MapModel map)
        {
            return BuildElevator(GetPointForColumnAndLevel(column, platformLevel, ref map), ref map);
        }

        public static ElevatorModel BuildElevator(float column, float platformLevel)
        {
            var temp = GetMap();
            return BuildElevator(column, platformLevel, ref temp);
        }

        public static ElevatorModel BuildElevatorAtX(float column, float platformLevel, float elevatorWidth, float maxMovementHeight, ref MapModel map)
        {
            return BuildElevator(GetPointForColumnAndLevel(column, platformLevel, ref map), elevatorWidth, maxMovementHeight, ref map);
        }

        public static ElevatorModel BuildElevatorAtX(float column, float platformLevel, float elevatorWidth, float maxMovementHeight)
        {
            var temp = GetMap();
            return BuildElevatorAtX(column, platformLevel, elevatorWidth, maxMovementHeight, ref temp);
        }

        public static ElevatorModel BuildElevator(float column, float platformLevel, float columnSpan, float levelSpan, ref MapModel map)
        {
            return BuildElevator(GetPointForColumnAndLevel(column, platformLevel, ref map), columnSpan * DEFAULT_PLATFORM_WIDTH, levelSpan * DEFAULT_PLATFORM_HEIGHT, ref map);
        }

        public static ElevatorModel BuildElevator(float column, float platformLevel, float columnSpan, float levelSpan)
        {
            var temp = GetMap();
            return BuildElevator(column, platformLevel, columnSpan, levelSpan, ref temp);
        }

        public static void RegisterSwitchToElevator(ref SwitchModel sw, ref ElevatorModel elev)
        {
            elev.RegisterSwitch(ref sw);
        }

        public static Collectable BuildCollectable(Pointf position, ref MapModel map, int points)
        {
            Collectable c = new Collectable(map.Game, position, points);
            map.AddComponent(c);
            return c;
        }

        public static Collectable BuildCollectable(Pointf position, int points)
        {
            var temp = GetMap();
            return BuildCollectable(position, ref temp, points);
        }

        public static Collectable BuildCollectable(float column, float platformLevel, ref MapModel map, int points)
        {
            return BuildCollectable(GetPointForColumnAndLevel(column, platformLevel), ref map, points);
        }

        public static Collectable BuildCollectable(float column, float platformLevel, int points)
        {
            var temp = GetMap();
            return BuildCollectable(column, platformLevel, ref temp, points);
        }

        public static Resistor BuildResistor(Pointf position, Resistor.Type index, ref MapModel map)
        {
            Resistor res = new Resistor(map.Game, position, index);
            map.AddComponent(res);
            return res;
        }

        public static Resistor BuildResistor(Pointf position, Resistor.Type index)
        {
            var temp = GetMap();
            return BuildResistor(position, index, ref temp);
        }

        public static Resistor BuildResistor(float column, float platformLevel, Resistor.Type index, ref MapModel map)
        {
            platformLevel--;
            platformLevel += 0.05f;
            return BuildResistor(GetPointForColumnAndLevel(column, platformLevel), index, ref map);
        }

        public static Resistor BuildResistor(float column, float platformLevel, Resistor.Type index)
        {
            var temp = GetMap();
            return BuildResistor(column, platformLevel, index, ref temp);
        }
    }
}
