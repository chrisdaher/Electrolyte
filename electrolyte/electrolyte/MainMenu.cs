using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Input;

namespace electrolyte
{
    class MainMenu : DrawableGameComponent
    {
        private Texture2D menubg;
        private MenuButton playButton;
        private MenuButton versusButton;
        private MenuButton settingsButton;
        private MenuButton quitButton;
        private bool mIsMusicPlaying;
        private Song menuMusic;
        private Texture2D[] displayButtons;
        private int currentButtonIndex = -1;
        private GamePadState prevState;
        public MainMenu(Game game) :
            base(game)
        {
            displayButtons = new Texture2D[4];
            displayButtons[0] = game.Content.Load<Texture2D>("menu-images/start-unselected");
            displayButtons[1] = game.Content.Load<Texture2D>("menu-images/versus-unselected");
            displayButtons[2] = game.Content.Load<Texture2D>("menu-images/settings-unselected");
            displayButtons[3] = game.Content.Load<Texture2D>("menu-images/quit-unselected");

            playButton = new MenuButton(game,
                new Rectangle(Resolution.resWidth / 2 - (int)(displayButtons[0].Width / 2 * 0.7f), (int)(Resolution.resHeight * 0.75f), (int)(displayButtons[0].Width * 0.7f), (int)(displayButtons[0].Height * 0.7f)), 
                displayButtons[0], 0);
            versusButton = new MenuButton(game,
                new Rectangle(Resolution.resWidth / 2 - (int)(displayButtons[1].Width / 2 * 0.7f), (int)(Resolution.resHeight * 0.75f) + displayButtons[0].Height, (int)(displayButtons[1].Width * 0.7f), (int)(displayButtons[1].Height * 0.7f)),
                displayButtons[1], 1);
            settingsButton = new MenuButton(game,
                new Rectangle(Resolution.resWidth / 2 - (int)(displayButtons[2].Width / 2 * 0.7f), (int)(Resolution.resHeight * 0.75f) + displayButtons[0].Height + displayButtons[1].Height, (int)(displayButtons[2].Width * 0.7f), (int)(displayButtons[2].Height * 0.7f)),
                displayButtons[2], 2);
            quitButton = new MenuButton(game,
                new Rectangle(Resolution.resWidth / 2 - (int)(displayButtons[3].Width / 2 * 0.7f), (int)(Resolution.resHeight * 0.75f) + displayButtons[0].Height + displayButtons[1].Height + displayButtons[2].Height, (int)(displayButtons[3].Width * 0.7f), (int)(displayButtons[3].Height * 0.7f)),
                displayButtons[3], 3);
            prevState = GamePad.GetState(PlayerIndex.One);
            menuMusic = game.Content.Load<Song>("music/Electrolyte - Main Menu");
            menubg = game.Content.Load<Texture2D>("menu-images/mainmenubg");
        }

        public override void Update(GameTime gameTime)
        {
            GamePadState padState = GamePad.GetState(PlayerIndex.One);

            if (!mIsMusicPlaying && StateManager.GetInstance().GetState() == StateManager.GameState.MAIN_MENU)
            {
                currentButtonIndex = -1;
                mIsMusicPlaying = true;
                MediaPlayer.IsRepeating = true;
                MediaPlayer.Play(menuMusic);
            }

            // check if player is over one of the buttons.
            playButton.Update(gameTime);
            versusButton.Update(gameTime);
            settingsButton.Update(gameTime);
            quitButton.Update(gameTime);

            if (playButton.IsColliding)
            {
                currentButtonIndex = 0;
                StateManager.GetInstance().ChangePlayStateLevel(1);
                StateManager.GetInstance().SetState(StateManager.GameState.PLAY);
                mIsMusicPlaying = false;
                MediaPlayer.IsRepeating = false;
                MediaPlayer.Stop();
            }

            if (versusButton.IsColliding)
            {
                currentButtonIndex = 1;
                StateManager.GetInstance().ChangePlayStateLevel(2);
                StateManager.GetInstance().SetState(StateManager.GameState.PLAY);
                mIsMusicPlaying = false;
                MediaPlayer.IsRepeating = false;
                MediaPlayer.Stop();
            }

            if (settingsButton.IsColliding)
            {
                currentButtonIndex = 2;
                StateManager.GetInstance().SetState(StateManager.GameState.SETTINGS);
            }

            if (quitButton.IsColliding)
            {
                currentButtonIndex = 3;
                mIsMusicPlaying = false;
                MediaPlayer.IsRepeating = false;
                MediaPlayer.Stop();
                GameHelper.GetGame().Exit();
            }
            GamePadState pad = GamePad.GetState(PlayerIndex.One);
            
            if(pad.IsConnected && pad.IsButtonDown(Buttons.DPadUp) && !prevState.IsButtonDown(Buttons.DPadUp)){
                
                currentButtonIndex--;
                if (currentButtonIndex < 0)
                {
                    currentButtonIndex = displayButtons.Count() - 1;
                }
                currentButtonIndex %= displayButtons.Count(); 
            }

            if (pad.IsConnected && pad.IsButtonDown(Buttons.DPadDown) && !prevState.IsButtonDown(Buttons.DPadDown))
            {                
                currentButtonIndex++;
                currentButtonIndex %= displayButtons.Count();
            }
            prevState = pad;

            playButton.SetSelected(currentButtonIndex);
            versusButton.SetSelected(currentButtonIndex);
            settingsButton.SetSelected(currentButtonIndex);
            quitButton.SetSelected(currentButtonIndex);            

            if (padState.IsConnected && padState.IsButtonDown(Buttons.A))
            {
                if (currentButtonIndex == 0)
                {
                    StateManager.GetInstance().ChangePlayStateLevel(1);
                    StateManager.GetInstance().SetState(StateManager.GameState.PLAY);
                    mIsMusicPlaying = false;
                    MediaPlayer.IsRepeating = false;
                    MediaPlayer.Stop();
                }

                if (currentButtonIndex == 1)
                {
                    StateManager.GetInstance().ChangePlayStateLevel(2);
                    StateManager.GetInstance().SetState(StateManager.GameState.PLAY);
                    mIsMusicPlaying = false;
                    MediaPlayer.IsRepeating = false;
                    MediaPlayer.Stop();
                }

                if (currentButtonIndex == 2)
                {
                    StateManager.GetInstance().SetState(StateManager.GameState.SETTINGS);
                }

                if (currentButtonIndex == 3)
                {
                    mIsMusicPlaying = false;
                    MediaPlayer.IsRepeating = false;
                    MediaPlayer.Stop();
                    GameHelper.GetGame().Exit();
                }
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            GameHelper.SpriteBatch.Draw(menubg, new Rectangle(0,0,Resolution.resWidth,Resolution.resHeight), Color.White);
            playButton.Draw(gameTime);
            versusButton.Draw(gameTime);
            settingsButton.Draw(gameTime);
            quitButton.Draw(gameTime);
            base.Draw(gameTime);
        }
    }
}
