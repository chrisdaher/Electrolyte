using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace electrolyte
{
    class Resistor : Model
    {
        private PlayerIndex mPlayerIndex;
        private const int interval = 100;
        private float currentTime=0;
        public enum Type
        {
            PLAYER_ONE,
            PLAYER_TWO,
            NEUTRAL
        }

        public Resistor(Game game, Pointf position, Type type) :
            base(game, position, new Size(45, 20), new Velocity(), (int)type)
        {
            DisableFreeFall();
            mBlocksOnCollision = false;
        }

        protected override void Init(int param)
        {
            mPlayerIndex = (PlayerIndex)param;
        }

        protected override Texture2D LoadTexture()
        {
            Texture2D rectangleTexture = new Texture2D(GameHelper.SpriteBatch.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            switch (mPlayerIndex)
            {
                case PlayerIndex.One:
                    rectangleTexture = Game.Content.Load<Texture2D>("BlueRes");
                    break;
                case PlayerIndex.Two:
                    rectangleTexture = Game.Content.Load<Texture2D>("RedRes");
                    break;
                case PlayerIndex.Three: //neutral
                    rectangleTexture = Game.Content.Load<Texture2D>("NeutralRes");
                    break;
            }
            return rectangleTexture;
        }
        public override void Update(GameTime gameTime)
        {
          currentTime += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
          if (currentTime > interval)
          {
            mFrameIndex++;
            mFrameIndex %= 8;
            currentTime = 0;
          }
          base.Update(gameTime);
        }
        protected override void GenerateFrames()
        {
            mFrames.Add(new Frame(new Rectangle(5, 5, 50, 17)));
            mFrames.Add(new Frame(new Rectangle(57, 5, 50, 17)));
            mFrames.Add(new Frame(new Rectangle(57+52, 5, 50, 17)));
            mFrames.Add(new Frame(new Rectangle(57+ 52 + 52, 5, 50, 17)));
            mFrames.Add(new Frame(new Rectangle(57 + 52 +52 + 52, 5, 50, 17)));
            
            mFrames.Add(new Frame(new Rectangle(57 + 52 + 52, 5, 50, 17)));
            mFrames.Add(new Frame(new Rectangle(57 + 52, 5, 50, 17)));
            mFrames.Add(new Frame(new Rectangle(57, 5, 50, 17)));
        }
        


        public override void Collided(Model with, Displayable.CollisionStatus status)
        {
            if (ProjectHelper.IsDebugNoKill) return;
            if (status != Displayable.CollisionStatus.BOTTOM) return;
            if (with.IsDestroyed) return;
            if (with.GetType() != typeof(Character)) return;
            Character chara = (Character)with;
            if (chara.CharacterPlayerIndex == mPlayerIndex || mPlayerIndex == PlayerIndex.Three)
            {
                with.Destroy();
            }
        }
    }
}
