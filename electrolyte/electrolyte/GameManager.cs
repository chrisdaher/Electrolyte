using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace electrolyte
{
    class GameManager
    {
        private Character mPlayer1;
        private Character mPlayer2;
        private MapModel mMap;
        private Game mGame;
        private SoundEffect mGangnamSound;
        private SoundEffectInstance mGangnamInstance;
        private Song mBackgroundSong;
        private bool mIsPlayingSound;
        private bool mIsPlayingMusic;
        private bool mIsWinner = false;
        private Hud mHud;
        private int mWinAnimationCntr = 0;
        private bool mWinAnimationLeft = true;
        private int mAnimCntrCurrentInterval = 700;
        private int ANIM_INTERVAL_MIN = 200;
        private int mSongCntr = 0;
        private int mLevel;
        public int CurrentLevel { get { return mLevel; } }
        private Character mBoss;
        private int mBossCntr = 0;
        private double mTimeFinishedAt;
        private double mP1Time = -1;
        private double mP2Time = -1;
        private float deadAnimationTimer = 0f;
        private float deadAnimationTimeLimit = 2000f;
        
        public GameManager(Game game, int level)
        {
            mGame = game;
            
            mPlayer1 = new Character(game, new Pointf(400, 200), new Size(32, 32), new Velocity(), PlayerIndex.One);
            mPlayer2 = new Character(game, new Pointf(350, 200), new Size(32, 32), new Velocity(), PlayerIndex.Two);
            mBoss = null;
            mLevel = level;
            switch (level)
            {
                case 1:
                    mMap = MapBuilder.BuildLevel1(mGame, new Size(game.Window.ClientBounds.Width, game.Window.ClientBounds.Height - Hud.HUD_HEIGHT), ref mPlayer1);
                    mPlayer1.mPosition = new Pointf(MapHelper.GetPointForColumnAndLevel(0.5f, 1));
                    mPlayer2.mPosition = new Pointf(MapHelper.GetPointForColumnAndLevel(0.5f, 1));
                    mBackgroundSong = mGame.Content.Load<Song>("music/Electrolyte - New Mission");
                    break;
                case 2:
                    mMap = MapBuilder.BuildSolo(mGame, new Size(game.Window.ClientBounds.Width, game.Window.ClientBounds.Height - Hud.HUD_HEIGHT), ref mPlayer1);
                    mPlayer1.mPosition = new Pointf(MapHelper.GetPointForColumnAndLevel(0.5f, 1));
                    mPlayer2.mPosition = new Pointf(MapHelper.GetPointForColumnAndLevel(MapHelper.GetLastColumn()-1.5f, 1));
                    mBackgroundSong = mGame.Content.Load<Song>("music/Electrolyte - Plus vs Minus");
                    break;
                case 3:
                    mMap = MapBuilder.BuildBossLevel(mGame, new Size(game.Window.ClientBounds.Width, game.Window.ClientBounds.Height - Hud.HUD_HEIGHT), ref mPlayer1);
                    mBoss = new Character(game, new Pointf(350, MapHelper.GetPlatformYAtLevel(1.5f)), new Size(64, 64), new Velocity(), PlayerIndex.Three);
                    mBackgroundSong = mGame.Content.Load<Song>("music/Electrolyte - Volt");
                    break;
            }


            mHud = new Hud(new Pointf(0, game.Window.ClientBounds.Height - Hud.HUD_HEIGHT), mGame, level);

            mIsPlayingSound = false;
            mGangnamSound = game.Content.Load<SoundEffect>("newgangnam");
            mGangnamInstance = mGangnamSound.CreateInstance();
        }

        public void HandleCollision(List<Model.Collision> collisions, ref Character chara)
        {
            chara.mCollisionStatus = Displayable.CollisionStatus.NONE;
            
            chara.EnableFreeFall();
            foreach (Model.Collision collision in collisions)
            {
                if (!collision.BlocksMovement) continue;
                switch (collision.Status)
                {
                    case Displayable.CollisionStatus.BOTTOM:
                        chara.DisableFreeFall();
                        if (!chara.JumpRequest)
                        {
                            chara.mMovementState = Character.State.Idle;
                            chara.mCurrentJumpState = Character.State.Idle;
                        }
                        break;
                    case Displayable.CollisionStatus.LEFT:
                        chara.mCollisionStatus = Displayable.CollisionStatus.LEFT;
                        chara.mMovementState = Character.State.Idle;
                        break;
                    case Displayable.CollisionStatus.RIGHT:
                        chara.mCollisionStatus = Displayable.CollisionStatus.RIGHT;
                        chara.mMovementState = Character.State.Idle;
                        break;
                    case Displayable.CollisionStatus.TOP:
                        if (chara.mCurrentJumpState == Character.State.Jumping)
                        {
                            chara.mCurrentJumpState = Character.State.Falling;
                        }
                        chara.EnableFreeFall();
                        break;
                    case Displayable.CollisionStatus.NONE:
                        chara.EnableFreeFall();
                        break;
                }
            }
        }

        public void CheckCollision()
        {
            List<Model.Collision> collisions = mMap.IsColliding(mPlayer1);
            HandleCollision(collisions, ref mPlayer1);
            collisions = mMap.IsColliding(mPlayer2);
            HandleCollision(collisions, ref mPlayer2);
            if (mBoss != null)
            {
                collisions = mMap.IsColliding(mBoss);
                HandleCollision(collisions, ref mBoss);
            }
        }

        public bool IsDone()
        {
            return mMap.IsDone();
        }

        public void StopMusic()
        {
            mGangnamInstance.Stop();
        }

        public void Update(GameTime gameTime)
        {
            // pause state
            if (Keyboard.GetState().IsKeyDown(Keys.P) || (GamePad.GetState(PlayerIndex.One).IsConnected && GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.Back)))
            {
                StateManager.GetInstance().SetState(StateManager.GameState.PAUSE);
                return;
            }

            if (!mIsPlayingMusic && !mIsPlayingSound)
            {
                mIsPlayingMusic = true;
                MediaPlayer.IsRepeating = true;
                MediaPlayer.Play(mBackgroundSong);
            }

            CheckCollision();

            mPlayer1.Update(gameTime);
            mPlayer2.Update(gameTime);

            if (mBoss != null) mBoss.Update(gameTime);
            mMap.Update(gameTime);

            if (mBoss != null)
            {
                mBossCntr += gameTime.ElapsedGameTime.Milliseconds;
                if (mBossCntr >= 600)
                {
                    //mBoss.RequestJump();
                    mBossCntr = 0;
                }
            }

            if (mP1Time == -1 && mMap.IsPlayerDone(PlayerIndex.One))
            {
                mP1Time = mHud.CurrentTime;
            }
            if (mP2Time == -1 && mMap.IsPlayerDone(PlayerIndex.Two))
            {
                mP2Time = mHud.CurrentTime;
            }

            if (IsDone())
            {
                mIsWinner = true;
                Console.WriteLine(mMap.GetAlphaScore(mHud.CurrentTime).ToString());

                if (!mIsPlayingSound)
                {
                    mIsPlayingSound = true;
                    mIsPlayingMusic = false;
                    MediaPlayer.IsRepeating = false;
                    MediaPlayer.Stop();
                    mGangnamInstance.Play();
                    mHud.GameWin(mLevel, PlayerIndex.Four);

                    mTimeFinishedAt = mHud.CurrentTime;
                }

            }
            if (mIsWinner)
            {
                mSongCntr += gameTime.ElapsedGameTime.Milliseconds;
                mWinAnimationCntr += gameTime.ElapsedGameTime.Milliseconds;
                if (mWinAnimationCntr >= mAnimCntrCurrentInterval && !mPlayer1.JumpRequest &&
                    (mSongCntr < 56300 || mSongCntr > 59600))
                {
                    if (mWinAnimationLeft)
                    {
                        mPlayer1.MoveLeft();
                        mPlayer2.MoveRight();
                    }
                    else
                    {
                        mPlayer1.MoveRight();
                        mPlayer2.MoveLeft();
                    }
                    mPlayer1.RequestJump();
                    mPlayer2.RequestJump();
                    mWinAnimationCntr = 0;
                    mWinAnimationLeft = !mWinAnimationLeft;

                    if (mAnimCntrCurrentInterval > ANIM_INTERVAL_MIN)
                    {
                        mAnimCntrCurrentInterval -= 15;
                    }
                }
                else if (mSongCntr >= 5000 && mLevel != 3)
                {
                    if (mLevel == 1)
                    {
                        TimeSpan time = TimeSpan.FromMilliseconds(mTimeFinishedAt);
                        StateManager.GetInstance().ShowResults(mPlayer1.Score, mPlayer2.Score, mMap.GetAlphaScore(mTimeFinishedAt), time.Minutes.ToString("D2") + ":" + time.Seconds.ToString("D2"));
                    }
                    else
                    {
                        TimeSpan time1 = TimeSpan.FromMilliseconds(mP1Time);
                        TimeSpan time2 = TimeSpan.FromMilliseconds(mP2Time);
                        StateManager.GetInstance().ShowResults(mPlayer1.Score, mPlayer2.Score, time1.Minutes.ToString("D2") + ":" + time1.Seconds.ToString("D2"), time2.Minutes.ToString("D2") + ":" + time2.Seconds.ToString("D2"),
                                                                mMap.GetAlphaScore(mP1Time), mMap.GetAlphaScore(mP2Time));
                    }
                }
            }

            mHud.UpdatePlayerScore(mPlayer1.Score, mPlayer2.Score);
            mHud.Update(gameTime);

            CheckCollision();

            if (!ProjectHelper.IsDebugNoKill)
            {
                if (mPlayer1.IsDestroyed)
                {
                    deadAnimationTimer += gameTime.ElapsedGameTime.Milliseconds;
                    mPlayer1.SetDeadState();
                    if (deadAnimationTimer > deadAnimationTimeLimit)
                    {
                        Console.WriteLine("Player1 died");
                        if (mLevel == 2)
                        {
                            mPlayer1 = new Character(mGame, new Pointf(400, 200), new Size(32, 32), new Velocity(), PlayerIndex.One);
                            mPlayer1.mPosition = new Pointf(MapHelper.GetPointForColumnAndLevel(0.5f, 1));
                            mPlayer1.SetIdleState();
                        }
                        else
                        {
                            StateManager.GetInstance().SetState(StateManager.GameState.DEAD_SCREEN);
                            mPlayer1.SetIdleState();
                        }
                        deadAnimationTimer = 0f;
                    }
                }
                else if (mPlayer2.IsDestroyed)
                {
                    deadAnimationTimer += gameTime.ElapsedGameTime.Milliseconds;
                    mPlayer2.SetDeadState();
                    if (deadAnimationTimer > deadAnimationTimeLimit)
                    {
                        Console.WriteLine("Player 2 died");
                        if (mLevel == 2)
                        {
                            mPlayer2 = new Character(mGame, new Pointf(350, 200), new Size(32, 32), new Velocity(), PlayerIndex.Two);
                            mPlayer2.mPosition = new Pointf(MapHelper.GetPointForColumnAndLevel(MapHelper.GetLastColumn() - 1.5f, 1));
                            mPlayer2.SetIdleState();
                        }
                        else
                        {
                            StateManager.GetInstance().SetState(StateManager.GameState.DEAD_SCREEN);
                            mPlayer2.SetIdleState();
                        }
                        deadAnimationTimer = 0f;
                    }
                }
                else if (mBoss != null && mBoss.IsDestroyed)
                {
                    Console.WriteLine("Boss died");
                }
            }
            GamePadState pad1 = GamePad.GetState(PlayerIndex.One);
            GamePadState pad2 = GamePad.GetState(PlayerIndex.Two);
            if (Keyboard.GetState().IsKeyDown(Keys.R) ||
                (pad1.IsConnected && pad1.IsButtonDown(Buttons.Start) &&
                pad2.IsConnected && pad2.IsButtonDown(Buttons.Start)))
            {
                StateManager.GetInstance().ResetState(StateManager.GameState.PLAY);
            }
        }

        public bool IsWinner()
        {
            return mIsWinner;
        }

        public void Draw(GameTime gameTime)
        {
            mMap.Draw(gameTime);
            mPlayer1.Draw(gameTime);
            mPlayer2.Draw(gameTime);
            if (mBoss != null) mBoss.Draw(gameTime);
            
            if (mPlayer1.GetState() == Character.State.Dead && mPlayer2.GetState() == Character.State.Dead)
            {
                Hud.SetPlayerDeathIndex(0);
            }
            else
            {
                if (mPlayer1.GetState() == Character.State.Dead)
                {
                    Hud.SetPlayerDeathIndex(1);
                }
                if (mPlayer2.GetState() == Character.State.Dead)
                {
                    Hud.SetPlayerDeathIndex(2);
                }
            }
            mHud.Draw(gameTime);
        }
    }
}
