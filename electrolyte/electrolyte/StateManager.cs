using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace electrolyte
{
    class StateManager
    {
        public enum GameState
        {
            LOADING,
            MAIN_MENU,
            SETTINGS,
            PLAY,
            PAUSE,
            RESULT_SCREEN,
            DEAD_SCREEN
        }

        private Game mGame;
        private GameState mState;
        private MainMenu mMenuState;
        private GameManager mPlayState;
        private DeadState mDeadState;
        private ResultState mResultState;
        private PauseState mPauseState;
        private SettingsState mSettingsState;
        private static StateManager mInstance;

        public static void Initialize(Game game)
        {
            mInstance = new StateManager(game);
        }

        public static StateManager GetInstance()
        {
            return mInstance;
        }

        private StateManager(Game game)
        {
            mState = GameState.MAIN_MENU;
            mMenuState = new MainMenu(game);
            mPlayState = new GameManager(game,1);
            mPauseState = new PauseState(game);
            mSettingsState = new SettingsState(game);
            mDeadState = new DeadState(game);
            mResultState = new ResultState(mGame, 0, 0, Score.AlphaScore.F, "");
            mGame = game;
        }

        public void ResetState(GameState stateToReset)
        {
            switch (stateToReset)
            {
                case GameState.MAIN_MENU:
                    mMenuState = new MainMenu(mGame);
                    break;
                case GameState.SETTINGS:
                    mSettingsState = new SettingsState(mGame);
                    break;
                case GameState.PLAY:
                    mPlayState = new GameManager(mGame, mPlayState.CurrentLevel);
                    break;
                case GameState.DEAD_SCREEN:
                    mDeadState = new DeadState(mGame);
                    break;
            }
        }

        public void IncreasePlayLevel()
        {
            mPlayState.StopMusic();
            if (mPlayState.CurrentLevel < 2)
            {
                mPlayState = new GameManager(mGame, mPlayState.CurrentLevel + 1);
                mState = GameState.PLAY;
            }
            else
            {
                ChangePlayStateLevel(1);
                SetState(GameState.MAIN_MENU);
            }
        }

        public void ShowResults(int p1score, int p2score, Score.AlphaScore alpha, string time)
        {
            mResultState = new ResultState(mGame, p1score, p2score, alpha, time);
            mState = GameState.RESULT_SCREEN;
        }

        public void ShowResults(int p1score, int p2score, string time1, string time2, Score.AlphaScore alpha1, Score.AlphaScore alpha2)
        {
            mResultState = new ResultState(mGame, p1score, p2score, alpha1, alpha2, time1, time2);
            mState = GameState.RESULT_SCREEN;
        }

        public int CurrentPlayStateLevel()
        {
            return mPlayState.CurrentLevel;
        }

        public void ChangePlayStateLevel(int level)
        {
            mPlayState = new GameManager(mGame, level);
        }

        public void Update(GameTime gameTime)
        {
            switch (mState)
            {
                case GameState.MAIN_MENU:
                    mMenuState.Update(gameTime);
                    break;
                case GameState.SETTINGS:
                    mSettingsState.Update(gameTime);
                    break;
                case GameState.PLAY:
                    mPlayState.Update(gameTime);
                    break;
                case GameState.DEAD_SCREEN:
                    mDeadState.Update(gameTime);
                    break;
                case GameState.RESULT_SCREEN:
                    mResultState.Update(gameTime);
                    break;
                case GameState.PAUSE:
                    mPauseState.Update(gameTime);
                    break;

            }
        }

        public void Draw(GameTime gameTime)
        {
            switch (mState)
            {
                case GameState.MAIN_MENU:
                    mMenuState.Draw(gameTime);
                    break;
                case GameState.SETTINGS:
                    mSettingsState.Draw(gameTime);
                    break;
                case GameState.PLAY:
                    mPlayState.Draw(gameTime);
                    break;
                case GameState.DEAD_SCREEN:
                    mDeadState.Draw(gameTime);
                    break;
                case GameState.RESULT_SCREEN:
                    mResultState.Draw(gameTime);
                    break;
                case GameState.PAUSE:
                    mPauseState.Draw(gameTime);
                    break;
            }
        }

        public void SetState(GameState state)
        {
            mState = state;
        }

        public GameState GetState()
        {
            return mState;
        }
    }
}
