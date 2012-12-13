using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace electrolyte
{
    class ElevatorModel : Model
    {

        private enum State
        {
            IDLE,
            UP,
            DOWN
        }

        private List<SwitchModel> mSwitches;
        private State mCurrentState;
        private State mPreviousState;
        private Pointf mInitialPosition;
        private float mMaxMovementHeight;
        private bool mIsColliding;
        private int mCurrentTopCollisionId = -1;

        public ElevatorModel(Game game,
                             float width,
                             float maxMovementHeight,
                             Pointf position) :
            base(game, position, new Size(width, 10), new Velocity())
        {
            mMaxMovementHeight = maxMovementHeight;
            mSwitches = new List<SwitchModel>();
            mCurrentState = State.IDLE;
            mPreviousState = State.IDLE;
            mInitialPosition = new Pointf(mPosition);
        }


        public void RegisterSwitch(ref SwitchModel sw)
        {
            mSwitches.Add(sw);
        }

        public override void Collided(Model with, Displayable.CollisionStatus status)
        {
            if (with.GetType() != typeof(Character)) return;
            mIsColliding = true;
            if (status == Displayable.CollisionStatus.TOP)
            {
                mCurrentState = State.IDLE;
                mCurrentTopCollisionId = with.ModelId;
            }
        }

        public override void NotColliding(Model with)
        {
            if (mCurrentTopCollisionId == with.ModelId)
            {
                mCurrentTopCollisionId = -1;
            }
            if (with.GetType() != typeof(Character)) return;
            mIsColliding = false;
        }

        void sw_StateEventHandler(object sender, EventArgs e)
        {
            // this code is not used for now...

            SwitchModel sw = (SwitchModel)sender;

            switch (sw.State)
            {
                case (SwitchModel.Status.ON):
                                        this.Velocity.Direction = new Vector2(0, 1);
                    this.Velocity.Speed = new Vector2(1.5f, 1.5f);
                    break;
                case (SwitchModel.Status.OFF):
                    this.Velocity.Direction = new Vector2(0, -1);
                    this.Velocity.Speed = new Vector2(1.5f, 1.5f);
                    break;
            }
        }


        public override void Update(GameTime gameTime)
        {
            mPreviousState = mCurrentState;
            SwitchModel.Status switchStatus = SwitchModel.Status.OFF;
            if (!mIsColliding)
            {
                foreach (SwitchModel sw in mSwitches)
                {
                    if (sw.State == SwitchModel.Status.ON)
                    {
                        switchStatus = SwitchModel.Status.ON;
                        if (this.mPosition.Y - this.mInitialPosition.Y < mMaxMovementHeight)
                        {
                            mPreviousState = mCurrentState;
                            mCurrentState = State.DOWN;

                        }
                    }
                }
            }
            if (switchStatus == SwitchModel.Status.OFF &&
                this.mPosition.Y > this.mInitialPosition.Y)
            {
                mCurrentState = State.UP;
            }

            if (mCurrentTopCollisionId != -1)
            {
                mCurrentState = State.IDLE;
            }

            switch (mCurrentState)
            {
                case State.UP:
                    if (this.mPosition.Y <= this.mInitialPosition.Y)
                    {
                        mCurrentState = State.IDLE;
                    }
                    else
                    {
                        this.Velocity = new Velocity(new Vector2(0, -1), new Vector2(1.5f,1.5f));
                    }
                    break;
                case State.DOWN:
                    if (this.mPosition.Y - this.mInitialPosition.Y >= mMaxMovementHeight)
                    {
                        mCurrentState = State.IDLE;
                    }
                    else
                    {
                        this.Velocity = new Velocity(new Vector2(0, 1), new Vector2(1.5f, 1.5f));
                    }
                    break;
                case State.IDLE:
                    this.Velocity = new Velocity();
                    if (this.mPosition.Y <= this.mInitialPosition.Y) this.mPosition.Y = this.mInitialPosition.Y;
                    break;
            }
            base.Update(gameTime);
        }

        protected override Texture2D LoadTexture()
        {
          Texture2D rectangleTexture = new Texture2D(GameHelper.SpriteBatch.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
          rectangleTexture = Game.Content.Load<Texture2D>("elevator");
            return rectangleTexture;
        }        

        protected override void GenerateFrames()
        {
            mFrames.Add(new Frame(new Rectangle(0, 0, 48, 10)));
        }
    }
}
