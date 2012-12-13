using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace electrolyte
{
    class Character : Model
    {
        public enum State
        {
            WalkingLeft,
            WalkingRight,
            Jumping,
            Falling,
            Idle,
            Dead
        }

        public State mPreviousJumpState = State.Idle;
        public State mCurrentJumpState = State.Idle;
        public State mMovementState = State.Idle;
        public Displayable.CollisionStatus mCollisionStatus = Displayable.CollisionStatus.NONE;
        public bool mJumpRequested = false;
        public bool JumpRequest { get { return mJumpRequested; } }
        protected PlayerIndex currentPlayer = PlayerIndex.One;
        protected Pointf jumpPosition;
        protected const int WALK_DISTANCE = 5;
        protected const int ANIMATION_FRAMES = 3;
        protected const int NUM_ANIMATIONS = 3;
        protected const int IDLE = 0;
        protected const int MOVE_LEFT = 1;
        protected const int MOVE_RIGHT = 2;
        protected int mLeftAnimationCounter = 0;
        protected int mRightAnimationCounter = 0;
        protected int deathAnimationCounter = 0;
        protected int mPlayerIndex = 0;
        public PlayerIndex CharacterPlayerIndex { get { return currentPlayer; } }
        public int Index { get { return mPlayerIndex; } }

        protected int mScore = 0;
        public int Score { get { return mScore; } }

        protected KeyboardState mOldKbState;
        public const int MAX_JUMP_HEIGHT = 80;
        public const int MAX_PLATFORM_HEIGHT = MAX_JUMP_HEIGHT - 30;
        protected int mMaxJumpDistance = MAX_JUMP_HEIGHT;
        protected int mMaxJumpHeight = MAX_JUMP_HEIGHT;
        public int JumpHeight { get { return mMaxJumpHeight; } }
        protected int mVelocitySpeed = 4;
        private float xStep = 0;
        private float yStep = 0.5f;
        private Pointf initialJumpPosition;

        protected SoundEffect mSongJump;

        public Character(Game game,
                            Pointf position,
                            Size size,
                            Velocity velocity,
                            PlayerIndex index
                        ) :
            base(game, position, size, velocity)
        {
            EnableFreeFall();

            currentPlayer = index;
            if (currentPlayer == PlayerIndex.One)
            {
                mPlayerIndex = 0;
            }
            else if (currentPlayer == PlayerIndex.Two)
            {
                mPlayerIndex = 1;
            }
            else
            {
                mPlayerIndex = 2;
            }

            mSongJump = game.Content.Load<SoundEffect>("jump4");
        }

        protected override Texture2D LoadTexture()
        {
            Texture2D rectangleTexture = new Texture2D(GameHelper.SpriteBatch.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            rectangleTexture = Game.Content.Load<Texture2D>("sprites");
            return rectangleTexture;
        }

        public void RequestJump()
        {
            mJumpRequested = true;
            jumpPosition = new Pointf(mPosition.X, mPosition.Y);
            mPreviousJumpState = mCurrentJumpState;
            mCurrentJumpState = State.Jumping;
        }

        public void MoveLeft()
        {
            mMovementState = State.WalkingLeft;
            moveLeftAnimation();
        }

        public void MoveRight()
        {
            mMovementState = State.WalkingRight;
            moveRightAnimation();
        }

        public override void Update(GameTime gameTime)
        {
            GamePadState padState;
            // here you can update your character...or handle keystates
            // to change a frame, you would set the mCurrentFrameIndex  

            // If character is dead
            if (mMovementState == State.Dead)
            {
                
                deathAnimation();
            }
            else
            {

                // If the character is idle
                if (mMovementState == State.Idle && mCurrentJumpState == State.Idle)
                {
                    idleAnimation();
                }

                // Get the player's controller state
                if (currentPlayer == PlayerIndex.One)
                {
                    padState = GamePad.GetState(PlayerIndex.One);
                }
                else
                {
                    padState = GamePad.GetState(PlayerIndex.Two);
                }

                // Check the keyboard state if the player wants to move to the right
                if ((Keyboard.GetState().IsKeyDown(Keys.D) && currentPlayer == PlayerIndex.One
                    || Keyboard.GetState().IsKeyDown(Keys.Right) && currentPlayer == PlayerIndex.Two) &&
                    mCollisionStatus != Displayable.CollisionStatus.RIGHT)
                {
                    MoveRight();
                }
                else // Check the controller state if the player wants to move to the right
                {
                    if (padState.IsConnected)
                    {
                        if (padState.ThumbSticks.Left.X >= 0.3f &&
                            mCollisionStatus != Displayable.CollisionStatus.RIGHT)
                        {
                            MoveRight();
                        }
                    }
                }

                // Check the keyboard state if the player wants to move to the left
                if ((Keyboard.GetState().IsKeyDown(Keys.A) && currentPlayer == PlayerIndex.One
                    || Keyboard.GetState().IsKeyDown(Keys.Left) && currentPlayer == PlayerIndex.Two) &&
                    mCollisionStatus != Displayable.CollisionStatus.LEFT)
                {
                    MoveLeft();
                }
                else // Check the controller state if the player wants to move to the left
                {
                    if (padState.IsConnected)
                    {
                        if (padState.ThumbSticks.Left.X <= -0.3f &&
                            mCollisionStatus != Displayable.CollisionStatus.LEFT)
                        {
                            MoveLeft();
                        }
                    }
                }

                mJumpRequested = false;

                // Initialize jump variables if the Up key was pressed
                if (((Keyboard.GetState().IsKeyDown(Keys.W) && currentPlayer == PlayerIndex.One && true)
                    || Keyboard.GetState().IsKeyDown(Keys.Up) && currentPlayer == PlayerIndex.Two)
                    && mCurrentJumpState != State.Jumping && mCurrentJumpState != State.Falling
                    && !mIsFreeFall)
                {
                    RequestJump();
                    mSongJump.Play();
                    initialJumpPosition = mPosition;
                }
                else // Initialize jump variables if the A button was pressed
                {
                    if (padState.IsConnected)
                    {
                        if (padState.Buttons.A == ButtonState.Pressed &&
                            mCurrentJumpState != State.Jumping && mCurrentJumpState != State.Falling
                            && !mIsFreeFall)
                        {
                            RequestJump();
                        }
                    }
                }

                // Check the keyboard state if the player wants to jump
                if ((Keyboard.GetState().IsKeyDown(Keys.W) && currentPlayer == PlayerIndex.One
                    || Keyboard.GetState().IsKeyDown(Keys.Up) && currentPlayer == PlayerIndex.Two)
                    && mCurrentJumpState != State.Jumping && mCurrentJumpState != State.Falling)
                {
                    mJumpRequested = true;
                }
                else // Check the controller state if the player wants to jump
                {
                    if (padState.IsConnected)
                    {
                        if (padState.Buttons.A == ButtonState.Pressed &&
                           mCurrentJumpState != State.Jumping && mCurrentJumpState != State.Falling)
                        {
                            mJumpRequested = true;
                        }
                    }
                }

                mOldKbState = Keyboard.GetState();

                // If the player's jump state is jumping or falling
                if (mCurrentJumpState == State.Jumping || mCurrentJumpState == State.Falling)
                {
                    mJumpRequested = false;
                    bool isJumping = Jump();

                    // If the player is finished jumping
                    if (!isJumping)
                    {
                        // Set the states accordingly
                        mPreviousJumpState = State.Jumping;
                        mCurrentJumpState = State.Idle;
                        mMovementState = State.Idle;
                    }
                }
                else
                {
                    // Check if the player want to move left, right, or not move at all
                    switch (mMovementState)
                    {
                        case State.WalkingLeft:
                            Velocity.Direction = new Vector2(-0.75f, 0);
                            Velocity.Speed = new Vector2(WALK_DISTANCE, WALK_DISTANCE);
                            break;
                        case State.WalkingRight:
                            Velocity.Direction = new Vector2(0.75f, 0);
                            Velocity.Speed = new Vector2(WALK_DISTANCE, WALK_DISTANCE);
                            break;
                        default:
                            Velocity.Direction = new Vector2(0, 0);
                            Velocity.Speed = new Vector2();
                            break;
                    }
                }
            }

            base.Update(gameTime);
        }

        protected override void GenerateFrames()
        {
            // here you would generate the frames for your model by adding frame object to the list.

            //Character 1
            // Idle
            mFrames.Add(new Frame(new Rectangle(0, 0, 32, 32)));
            mFrames.Add(new Frame(new Rectangle(32, 0, 32, 32)));
            mFrames.Add(new Frame(new Rectangle(64, 0, 32, 32)));

            // Left
            mFrames.Add(new Frame(new Rectangle(0, 32, 32, 32)));
            mFrames.Add(new Frame(new Rectangle(32, 32, 32, 32)));
            mFrames.Add(new Frame(new Rectangle(64, 32, 32, 32)));

            // Right
            mFrames.Add(new Frame(new Rectangle(0, 64, 32, 32)));
            mFrames.Add(new Frame(new Rectangle(32, 64, 32, 32)));
            mFrames.Add(new Frame(new Rectangle(64, 64, 32, 32)));

            //Character 2
            // Idle
            mFrames.Add(new Frame(new Rectangle(288, 0, 32, 32)));
            mFrames.Add(new Frame(new Rectangle(320, 0, 32, 32)));
            mFrames.Add(new Frame(new Rectangle(352, 0, 32, 32)));

            // Left
            mFrames.Add(new Frame(new Rectangle(288, 32, 32, 32)));
            mFrames.Add(new Frame(new Rectangle(320, 32, 32, 32)));
            mFrames.Add(new Frame(new Rectangle(352, 32, 32, 32)));

            // Right
            mFrames.Add(new Frame(new Rectangle(288, 64, 32, 32)));
            mFrames.Add(new Frame(new Rectangle(320, 64, 32, 32)));
            mFrames.Add(new Frame(new Rectangle(352, 64, 32, 32)));

            //Boss
            // Idle
            mFrames.Add(new Frame(new Rectangle(192, 0, 32, 32)));
            mFrames.Add(new Frame(new Rectangle(224, 0, 32, 32)));
            mFrames.Add(new Frame(new Rectangle(256, 0, 32, 32)));

            // Left
            mFrames.Add(new Frame(new Rectangle(192, 32, 32, 32)));
            mFrames.Add(new Frame(new Rectangle(224, 32, 32, 32)));
            mFrames.Add(new Frame(new Rectangle(256, 32, 32, 32)));

            // Right
            mFrames.Add(new Frame(new Rectangle(192, 64, 32, 32)));
            mFrames.Add(new Frame(new Rectangle(224, 64, 32, 32)));
            mFrames.Add(new Frame(new Rectangle(256, 64, 32, 32)));

            // Shock Frames
            mFrames.Add(new Frame(new Rectangle(0, 224, 32, 32)));
            mFrames.Add(new Frame(new Rectangle(32, 224, 32, 32)));
            mFrames.Add(new Frame(new Rectangle(64, 224, 32, 32)));
        }

        /*
         * Allows the player's character to jump up and down.
         * Can jump to left, right or straight up.
         */
        public bool Jump()
        {
            //xStep = 0;    // Default: Jump Straight Up
            //yStep = -0.5f;

            // Check if the jump state is idle
            if (mCurrentJumpState == State.Idle)
            {
                return false;
            }
            this.DisableFreeFall();
            // If falling and have reached the ground
            if (mCurrentJumpState == State.Falling)
            {
                //mPosition.Y = jumpPosition.Y;    // Set the y position to the original position
                //mVelocity.Direction = new Vector2(0, 0);
                //mVelocity.Speed =  new Vector2();                
                return false;
            }
            
            // Check if the player wants to jump in a direction
            switch (mMovementState)
            {
                // Set the X and Y step accordingly (Y step is positive by default)
                case State.WalkingRight:
                    xStep = 0.5f;
                    yStep = mMaxJumpHeight / ((mMaxJumpDistance / 2) / Math.Abs(xStep));
                    break;
                case State.WalkingLeft:
                    xStep = -0.5f;
                    yStep = mMaxJumpHeight / ((mMaxJumpDistance / 2) / Math.Abs(xStep));
                    break;
                default:
                    xStep = 0;
                    yStep = mMaxJumpHeight / ((mMaxJumpDistance / 2) / Math.Abs(0.5f));
                    break;
            }

            // If the player's character is jumping and has not reached max height yet, inverse the y step
            if (jumpPosition.Y - mPosition.Y < mMaxJumpHeight && mCurrentJumpState == State.Jumping)
            {
                yStep = -1 * yStep;
                //yStep *= .9f;
            }
            else
            {
                yStep /= .9f;
                mIsFreeFall = true;
                // Set the jump state to falling
                mCurrentJumpState = State.Falling;
            }
                                  
            // Set the direction and speed as needed
            Velocity.Direction = new Vector2(xStep, yStep);
            Velocity.Speed = new Vector2(mVelocitySpeed,mVelocitySpeed);
            if (ProjectHelper.IsSoundEnabled)
            {
                if (MediaPlayer.State != MediaState.Playing)
                {
                    //mSongJump.Play();
                }
            }
            return true;
        }

        // Frame to display if idle
        protected void idleAnimation()
        {
            mFrameIndex = (mPlayerIndex * NUM_ANIMATIONS * NUM_ANIMATIONS) + 1;
            mLeftAnimationCounter = 0;
            mRightAnimationCounter = 0;
        }

        // Frame to display if moving left
        protected void moveLeftAnimation()
        {
            mFrameIndex = (MOVE_LEFT * ANIMATION_FRAMES) + mLeftAnimationCounter + (mPlayerIndex * NUM_ANIMATIONS * NUM_ANIMATIONS);
            mLeftAnimationCounter = (mLeftAnimationCounter + 1) % ANIMATION_FRAMES;
            mRightAnimationCounter = 0;
        }

        // Frame to display if moving right
        protected void moveRightAnimation()
        {
            mFrameIndex = (MOVE_RIGHT * ANIMATION_FRAMES) + mRightAnimationCounter + (mPlayerIndex * NUM_ANIMATIONS * NUM_ANIMATIONS);
            mRightAnimationCounter = (mRightAnimationCounter + 1) % ANIMATION_FRAMES;
            mLeftAnimationCounter = 0;
        }

        // Frame to display if dead
        protected void deathAnimation()
        {
            mFrameIndex = 27  + deathAnimationCounter;
            deathAnimationCounter += 1;
            deathAnimationCounter %= 3;
        }

        public override void Collided(Model with, Displayable.CollisionStatus status)
        {
            //we care about collectable collisions
            if (with.GetType() != typeof(Collectable)) return;
            mScore += ((Collectable)with).GetPoints();
            with.Destroy();
            base.Collided(with, status);
        }

        public State GetState()
        {
            return mMovementState;
        }

        public void SetDeadState()
        {
            mMovementState = State.Dead;
            Velocity.Speed = new Vector2(0,0);
        }

        public void SetIdleState()
        {
            mMovementState = State.Idle;
        }
    }
}
