using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using nomoredeleting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace nomoredeleting
{
    internal class BotManager
    {
        public List<AutoMovingSprite> _bots = new List<AutoMovingSprite>();
        private DialogSystem _dialogSystem = new DialogSystem();

        public void AddBot(AutoMovingSprite bot)
        {
            _bots.Add(bot);
        }

        public string TalkToClosestBot(Vector2 playerPosition, float talkRange = 50f)
        {
            AutoMovingSprite closestBot = null;
            float closestDistance = float.MaxValue;

            System.Diagnostics.Debug.WriteLine($"=== LOOKING FOR BOTS ===");
            System.Diagnostics.Debug.WriteLine($"Player position: {playerPosition}");
            System.Diagnostics.Debug.WriteLine($"Number of bots: {_bots.Count}");

            foreach (var bot in _bots)
            {
                if (!bot.canTalk) continue;


                float distance = Vector2.Distance(playerPosition, bot.currentPosition);

                System.Diagnostics.Debug.WriteLine($"Bot: {bot.Name} at {bot.currentPosition}, Distance: {distance}");

                if (distance < talkRange && distance < closestDistance)
                {
                    closestDistance = distance;
                    closestBot = bot;
                }
            }
            if (closestBot != null)
            {
                string dialog = _dialogSystem.TalkToBot(closestBot.Name);
                System.Diagnostics.Debug.WriteLine($"RETURNING DIALOGUE: {dialog}");
                return dialog;
            }
            System.Diagnostics.Debug.WriteLine("NO BOTS IN RANGE");
            return "No Nearby Character  to Talk";
        }

        public void Update(GameTime gameTime)
        {
            foreach (var bot in _bots)
            {
                bot.Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var bot in _bots)
            {
                bot.Draw(spriteBatch);
            }
        }
    }
}
