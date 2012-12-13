using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace electrolyte
{
    class MapModel : Model
    {
        private List<Model> mMapComponents;
        public List<Model> MapComponents { get { return mMapComponents; } }
        private List<ExitDoorModel> mExitDoors;
        private Score mScore;
        private Texture2D background;
        private Rectangle bgFrame;
        private int mCollisionCntr = 0;

        public MapModel(Game game, Size size, Score score, string backgroundName) :
            base(game, new Pointf(), size, new Velocity())
        {
            mScore = score;
            mMapComponents = new List<Model>();
            
            background = game.Content.Load<Texture2D>(backgroundName);
            bgFrame = new Rectangle(0, 0, game.GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
        }

        public void AddComponent(Model comp)
        {
            mMapComponents.Add(comp);
        }
       
        public new List<Collision> IsColliding(Model other)
        {
            List<Collision> toRet = new List<Collision>();
            foreach (Model model in mMapComponents)
            {
                Collision coll = model.IsColliding(other);
                if (coll.Status != Displayable.CollisionStatus.NONE)
                    toRet.Add(coll);
            }
            return toRet;
        }

        public void AssignExitDoors(ref ExitDoorModel door1, ref ExitDoorModel door2)
        {
            mExitDoors = new List<ExitDoorModel>();
            mExitDoors.Add(door1);
            mExitDoors.Add(door2);
        }

        public bool IsDone()
        {
            if (mExitDoors == null) throw new Exception("ExitDoors not set for MapModel. Assign exit doors with MapModel.AssignExitDoors");
            foreach (ExitDoorModel door in mExitDoors)
            {
                if (door.ExitDoorStatus == ExitDoorModel.DoorStatus.CLOSED) return false;
            }
            return true;
        }

        public bool IsPlayerDone(PlayerIndex pIndex)
        {
            if (mExitDoors == null) throw new Exception("ExitDoors not set for MapModel. Assign exit doors with MapModel.AssignExitDoors");
            foreach (ExitDoorModel door in mExitDoors)
            {
                if (door.DoorPlayerIndex == pIndex && door.ExitDoorStatus == ExitDoorModel.DoorStatus.OPEN) return true;
            }
            return false;
        }

        protected void checkComponentsCollision()
        {
            int max = mMapComponents.Count;
            for (int i=0;i<max;i++)
            {
                if (!mMapComponents[i].BlocksOnCollision && !mMapComponents[i].DoesFreeFall()) 
                    continue;
                for (int k = i + 1; k < max; k++)
                {
                    if (mMapComponents[i].GetType() == typeof(Wall) && mMapComponents[k].GetType() != typeof(MoveableBox)) continue;
                    if (mMapComponents[i] != mMapComponents[k])
                    {
                        mMapComponents[i].IsColliding(mMapComponents[k]);
                    }
                }
            }
        }

        public Score.AlphaScore GetAlphaScore(double ms)
        {
            return mScore.GetAlphaFromTimeMs(ms);
        }

#region OVERIDES - DO NOT DRAW ME

        protected override Microsoft.Xna.Framework.Graphics.Texture2D LoadTexture()
        {
            // no texture...
            return null;
        }

        protected override void GenerateFrames()
        {
            // no frames to generate!
        }

        public override void Update(GameTime gameTime)
        {
            // remove destroyed
            List<Model> destroyed = new List<Model>();

            foreach (Model model in mMapComponents)
            {
                if (!model.IsDestroyed)
                {
                    model.Update(gameTime);
                }
                else
                {
                    destroyed.Add(model);
                }
            }
            foreach (Model model in destroyed)
            {
                mMapComponents.Remove(model);
            }

            mCollisionCntr += gameTime.ElapsedGameTime.Milliseconds;
            if (mCollisionCntr >= 10)
            {
                checkComponentsCollision();
                mCollisionCntr = 0;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            GameHelper.SpriteBatch.Draw(background, bgFrame, Color.White);
            foreach (Model model in mMapComponents) model.Draw(gameTime);
        }
#endregion

    }
}
