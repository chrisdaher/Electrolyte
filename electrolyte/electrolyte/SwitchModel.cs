using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace electrolyte
{

    public delegate void SwitchEventHandler(object sender, EventArgs e);


    class SwitchModel : Model
    {
        public enum Status
        {
            OFF,
            ON            
        }
        
        public event SwitchEventHandler StateEventHandler;
        protected Status mState;
        protected int mModelIndex;        
        public Status State { get { return mState; } set { mState = value; } }


        public SwitchModel(Game game,
                    Pointf position) :
            base(game, position, new Size(10, 10), new Velocity())
        {
            mBlocksOnCollision = false;
            mState = Status.OFF;
        }

        protected virtual void OnStateChanged(EventArgs e)
        {
            if (StateEventHandler != null)
            {
                StateEventHandler(this, e);
            }
        }

        protected override Texture2D LoadTexture()
        {
            Texture2D rectangleTexture = new Texture2D(GameHelper.SpriteBatch.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            rectangleTexture = Game.Content.Load<Texture2D>("switches");
            return rectangleTexture;
        }

        public override void Collided(Model with, Displayable.CollisionStatus status)
        {
            //if (with.GetType() != typeof(Character)) return;
            mModelIndex = with.ModelId;
            mState = Status.ON;

            OnStateChanged(EventArgs.Empty);
        }

        public override void NotColliding(Model with)
        {
            //if (with.GetType() != typeof(Character)) return;
            //Character chara = (Character)with;
            if (mModelIndex != with.ModelId) return;
            mState = Status.OFF;

            OnStateChanged(EventArgs.Empty);
        }

        public override void Update(GameTime gameTime)
        {
            mFrameIndex = (int)mState;
            base.Update(gameTime);
        }


        protected override void GenerateFrames()
        {
            mFrames.Add(new Frame(new Rectangle(0, 0, 65, 65)));
            mFrames.Add(new Frame(new Rectangle(328, 0, 65, 65)));
        }
    }
}
