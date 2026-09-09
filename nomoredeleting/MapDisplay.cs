using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nomoredeleting
{
    internal class MapDisplay
    {
        private Texture2D _mapTexture;
        private Vector2 _position;
        private float _scale;
        private bool _isVisible;
        private float _animationProgress;

        private float _animationSpeed = 3f;
        private Vector2 _targetPosition;
        private Vector2 _startPosition;

        public bool isVisible => _isVisible;

        public MapDisplay(Texture2D mapTexture)
        {
            _mapTexture = mapTexture;
            _isVisible = false;
            _animationProgress = 0f;
            _scale = 20f;

            _targetPosition = Vector2.Zero;
            _startPosition = new Vector2(-_mapTexture.Width, 0);
            _position = _startPosition;
        }

        public void Update(GameTime gameTime, int screenWidth, int screenHeight)
        {
            if (_isVisible)
            {
                _animationProgress += (float)gameTime.ElapsedGameTime.TotalSeconds * _animationSpeed;
                _animationProgress = MathHelper.Clamp(_animationProgress, 0f, 1f);

                _targetPosition = new Vector2(
                    (screenWidth - _mapTexture.Width * _scale) / 2,
                    (screenHeight - _mapTexture.Height * _scale) / 2
                    );
            }
            else
            {
                _animationProgress -= (float)gameTime.ElapsedGameTime.TotalSeconds * _animationSpeed;
                _animationProgress = MathHelper.Clamp(_animationProgress, 0f, 1f);
            }
            _position = Vector2.Lerp(_startPosition, _targetPosition, _animationProgress);
        }

        public void Toggle()
        {
            _isVisible = !_isVisible;
        }

        public void Show()
        {
            _isVisible = true;
        }

        public void Hide()
        {
            _isVisible = false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (_animationProgress > 0)
            {
                var background = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
                background.SetData(new Color[] { new Color ( 0,0,0,150) });

                spriteBatch.Draw(background, new Rectangle(0, 0, spriteBatch.GraphicsDevice.Viewport.Width, spriteBatch.GraphicsDevice.Viewport.Height),Color.White);
                spriteBatch.Draw(_mapTexture, _position, null, Color.White,0f,Vector2.Zero,_scale,SpriteEffects.None,0f);
            }

        }
    }
}
