using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace nomoredeleting
{
    public class WaveWaterTile
    {
        public List<Texture2D> _waterTexture;
        public Vector2 _position { get; set; }
        private float _waveTimer = 0f;
        private float _waveSpeedSin = 0.7f;
        private float _waveHeightSin = 0.5f;
        private float _frequency = 0.01f;
        private float _waveSpeedCos = 0.8f;
        private float _waveHeightCos = 0.5f;

        public WaveWaterTile(List<Texture2D> waterTexture, Vector2 position)
        {
            _waterTexture = waterTexture;
            _position = position;
        }

        public void Update(GameTime gameTime)
        {
            _waveTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //float waveOffset = PerlinNoise(_position.X * _frequency, _waveTimer * _waveSpeedSin);
            float waveOffsetSin = (float)Math.Sin(_waveTimer * _waveSpeedSin) * _waveHeightSin;//0.1
            float waveOffsetCos = (float)Math.Cos(_waveTimer * _waveSpeedCos * _waveHeightCos);//0.1
            Vector2 wavePosition = new Vector2(_position.X+ waveOffsetSin+waveOffsetCos, _position.Y + waveOffsetSin+waveOffsetCos); //+ waveOffsetCos;//
            //Vector2 wavePosition = new Vector2(_position.X, _position.Y + waveOffset);
            spriteBatch.Draw(_waterTexture[0], wavePosition, Color.White);
        }

        private float PerlinNoise(float x, float y)
        {
            return Fract((float)Math.Sin(Vector2.Dot(new Vector2(x, y), new Vector2(12.9898f, 78.223f))) * 43758.5453f);
        }

        private float Fract(float value)
        {
            return (float)(value - Math.Floor(value));
        }
    }
}
