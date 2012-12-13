using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace electrolyte
{
    class ExitDoorModel : Model
    {
        private enum DOOR_FRAMES
        {
            DOOR_1_CLOSED,
            DOOR_1_OPEN,
            DOOR_2_CLOSED,
            DOOR_2_OPEN
        }

        public enum DoorStatus
        {
            OPEN,
            CLOSED
        }

        private PlayerIndex mPlayerIndex;
        public PlayerIndex DoorPlayerIndex { get { return mPlayerIndex; } }
        private DoorStatus mDoorStatus;
        public DoorStatus ExitDoorStatus { get { return mDoorStatus; } }

        public ExitDoorModel(Game game,Pointf position, PlayerIndex pIndex)
            : base(game,new Pointf(position.X, position.Y - 64), new Size(96,64),new Velocity(),(int)pIndex)
        {
            mBlocksOnCollision = false;
        }

        protected override void Init(int param)
        {
            mDoorStatus = DoorStatus.CLOSED;
            mPlayerIndex = (PlayerIndex)param;
        }

        protected override Texture2D LoadTexture()
        {
            Texture2D rectangleTexture = new Texture2D(GameHelper.SpriteBatch.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            rectangleTexture = Game.Content.Load<Texture2D>("door");
            return rectangleTexture;
        }

        public override void Update(GameTime gameTime)
        {
            if (mPlayerIndex == (int)PlayerIndex.One)
            {
                switch (mDoorStatus)
                {
                    case DoorStatus.CLOSED:
                        mFrameIndex = (int)DOOR_FRAMES.DOOR_1_CLOSED;
                        break;
                    case DoorStatus.OPEN:
                        mFrameIndex = (int)DOOR_FRAMES.DOOR_1_OPEN;
                        break;
                }
            }
            else
            {
                switch (mDoorStatus)
                {
                    case DoorStatus.CLOSED:
                        mFrameIndex = (int)DOOR_FRAMES.DOOR_2_CLOSED;
                        break;
                    case DoorStatus.OPEN:
                        mFrameIndex = (int)DOOR_FRAMES.DOOR_2_OPEN;
                        break;
                }
            }
            
            base.Update(gameTime);
        }

        public override void Collided(Model with, Displayable.CollisionStatus status)
        {
            if (status == Displayable.CollisionStatus.TOP) return;
            if (with.GetType() != typeof(Character)) return;
            Character chara = (Character)with;
            if (mPlayerIndex != (PlayerIndex)chara.Index) return;

            mDoorStatus = DoorStatus.OPEN;
        }

        public override void NotColliding(Model with)
        {
            if (with.GetType() != typeof(Character)) return;
            Character chara = (Character)with;
            if (mPlayerIndex != (PlayerIndex)chara.Index) return;

            mDoorStatus = DoorStatus.CLOSED;
        }
        protected override void GenerateFrames()
        {
            //door 1 closed
            mFrames.Add(new Frame(new Rectangle(96, 0, 96, 64)));
            // door 1 open
            mFrames.Add(new Frame(new Rectangle(96, 128, 96, 64)));

            // door 2 closed
            mFrames.Add(new Frame(new Rectangle(288, 0, 96, 64)));
            mFrames.Add(new Frame(new Rectangle(288, 128, 96, 64)));
        }
    }
}
