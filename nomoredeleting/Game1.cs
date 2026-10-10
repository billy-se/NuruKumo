using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using SharpDX.Direct2D1.Effects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace nomoredeleting
{
    public class Game1 : Game
    {
        float popupScreenTiming = 0f;
        Texture2D textureMainBackgroundSettings;
        Rectangle rectMainBackgroundSettings;

        Texture2D textureSettings;
        Rectangle rectSettings;
        bool SettingsClick = false;

        Vector2 PositionDeploy;
        bool AvailDeploy = false;
        List<Vector2> _listOfWater;
        
        private MapDisplay _mapDisplay;
        private Texture2D _mapTexture;

        private List<Cloud> _clouds;
        private Texture2D _cloudTexture;

        //creating LSystem TREE
        private List<LSystemTree> _lSystemTrees;
        private Texture2D _pixel;
        private string _currentTypeTree = "willow";


        //for waving water effect
       
        public List<WaveWaterTile> _listWaveWTile;
        private List<Texture2D> _listWaveTexture;

        //text fade out effects
        private float _displayTimer = 0f;
        private float _fadeAlpha = 1f;

        //const tiles
        private const int sizeTiles = 32;

        //trees
        private List<Tree> _trees;
        private float scale = 3f;

        //mouse position and movement coordination
        MouseState _mouseState;
        string _mousePositionDisplay, _deltaDisplay;
        private int mouseX, mouseY, screenCenteredX, screenCenteredY, deltaX, deltaY;

        //export music
        Song song;
        private bool _musicLoaded = false;

        //input keyboard
        private KeyboardState _previousKeyboardState;

        //text from screen
        private string xyPlayerPosition = "", botCount = "", showStats = "";
        private bool shiftWasPressedFirst = false, _showStats = false;

        //dialog setup
        private BotManager _botManager = new BotManager();
        private SpriteFont _font;
        private string _currentDialogue = "";

        Random random = new Random();

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        //Bot setup
        private Dictionary<string, AutoMovingSprite> _movingSprite = new Dictionary<string, AutoMovingSprite>();
        private List<MovingBot> movingArmies = new List<MovingBot>();

        //main character
        Player _player;
        Texture2D _playerTexture;
        Texture2D swimFrames;
        Texture2D airFrames;
        private List<Whale> massiveWhale = new List<Whale>();

        //ground setup
        //displaying ground
        public bool[,] _waterTiles;
        public Vector2 thisIsWaterTiles;
        public int rows = 64;
        public int cols = 64;
        List<Tile> tiles = new List<Tile>();
        Texture2D rockTile, waterTile, dirtTile, grassTile;

        //3d

        private Vector3 cameraPosition = new Vector3(0, 1.5f, 0);
        private float yaw = 0f;
        private float pitch = 0f;

        private Matrix viewMatrix;
        private Matrix projectionMatrix;

        private MouseState _previousMouseState;

        private bool gameHasStarted = true;
        private bool isTransitioning = false;
        private float fadeTimer = 0f;

        private enum GameState
        {
            Random2DSimulation,
            RestaurantObserver
        }

        private BasicEffect _basicEffect;

        private GameState currentGameState = GameState.Random2DSimulation;
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _waterTiles = new bool[rows, cols];
            _listWaveWTile = new List<WaveWaterTile>();
            MediaPlayer.IsRepeating = true;

            _graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            // TODO: Add your initialization logic here
            _graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            _graphics.ApplyChanges();

            _graphics.IsFullScreen = true;


            _lSystemTrees = new List<LSystemTree>();//INITIALIZE LSYSTEMTREE
            _clouds = new List<Cloud>();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            textureMainBackgroundSettings = Content.Load<Texture2D>("mainBackgroundSettngs");
            rectMainBackgroundSettings =new Rectangle(550,250, textureMainBackgroundSettings.Width, textureMainBackgroundSettings.Height);

            textureSettings = Content.Load<Texture2D>("settings_");
            rectSettings = new Rectangle(1880, 15, textureSettings.Width, textureSettings.Height);

            _listOfWater = new List<Vector2>();
            

            _mapTexture = Content.Load<Texture2D>("malicious_map");
            _mapDisplay = new MapDisplay(_mapTexture);

            _cloudTexture = Content.Load<Texture2D>("cloud");

            _clouds.Add(new Cloud(_cloudTexture, new Vector2(100, 100)) { Speed = 2, Scale = 4f });
            _clouds.Add(new Cloud(_cloudTexture, new Vector2(400, 150)) { Speed = 0.5f, Scale = 1.0f });
            _clouds.Add(new Cloud(_cloudTexture, new Vector2(1080, 500)) { Speed = 2f, Scale = 10f });
            _clouds.Add(new Cloud(_cloudTexture, new Vector2(0, 1920)) { Speed = 0.5f, Scale = 69f });
            _pixel = new Texture2D(GraphicsDevice, 1, 4);
            _pixel.SetData(new Color[] { Color.Blue, Color.Green, Color.White, Color.Violet });

            //CreateSampleTrees();

            _listWaveTexture = new List<Texture2D>()
            {
                Content.Load<Texture2D>("waterWaves1"),
                Content.Load<Texture2D>("waterWaves2"),
                Content.Load<Texture2D>("waterWaves3"),
                Content.Load<Texture2D>("waterWaves4")
            };

            screenCenteredX = _graphics.PreferredBackBufferWidth / 2;
            screenCenteredY = _graphics.PreferredBackBufferHeight / 2;

            song = Content.Load<Song>("alc_");
            _musicLoaded = true;

            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _font = Content.Load<SpriteFont>("DebugFont");

            Texture2D botTexture = Content.Load<Texture2D>("llama");

            //movingArmies.Add(new MovingBot(new Vector2(100,100), botTexture));

            //it must something calculated within bounds
            //setting up bot
            Texture2D _botPlayerTexture0 = Content.Load<Texture2D>("SpriteBot1(nharry)");
            _movingSprite["harry"] = (new AutoMovingSprite(_botPlayerTexture0, new Vector2(960, 540), new Vector2(1920, 1080), 1f, "harry"));

            Texture2D _botPlayerTexture1 = Content.Load<Texture2D>("SpriteBot1(nharry[back])");
            _movingSprite["harry2"] = (new AutoMovingSprite(_botPlayerTexture1, new Vector2(290, 205), new Vector2(290, 1000), 1f, "harry2"));

            Texture2D _botPlayerTexture2 = Content.Load<Texture2D>("kura");
            _movingSprite["kura"] = (new AutoMovingSprite(_botPlayerTexture2, new Vector2(500, 500), new Vector2(700, 700), 1f, "kura"));

            Texture2D _botPlayerTexture3 = Content.Load<Texture2D>("omae");
            _movingSprite["omae"] = (new AutoMovingSprite(_botPlayerTexture3, new Vector2(700, 700), new Vector2(900, 900), 1f, "omae"));
            //_botManager.AddBot(new AutoMovingSprite(_botPlayerTexture,new Vector2(100,100),new Vector2(100,500),1f,"harry"));

            //adding their dialog and verify 
            _botManager.AddBot(_movingSprite["harry"]);
            _botManager.AddBot(_movingSprite["harry2"]);
            _botManager.AddBot(_movingSprite["kura"]);
            _botManager.AddBot(_movingSprite["omae"]);

            //ground loading
            grassTile = Content.Load<Texture2D>("Gras");
            waterTile = Content.Load<Texture2D>("water_break");
            dirtTile = Content.Load<Texture2D>("Dirtorpath");
            rockTile = Content.Load<Texture2D>("rock");

            List<Texture2D> swimFrames = new List<Texture2D>
            {
                Content.Load<Texture2D>("whaleFin"),
                Content.Load<Texture2D>("whaleSwim")
            };

            List<Texture2D> airFrames = new List<Texture2D>
            {
                Content.Load<Texture2D>("ikam"),
                Content.Load<Texture2D>("ikam2"),
                Content.Load<Texture2D>("ikam3")
            };

            //coordinate of trees deploy
            Texture2D textureTree = Content.Load<Texture2D>("trees");
            _trees = new List<Tree>()
            {
                new Tree(){Position = new Vector2(20,5), Texture=textureTree },
                new Tree(){Position = new Vector2(10,10), Texture=textureTree }
            };


            for (int y = 0; y < rows; y++)
            {
                for (int x = 0; x < cols; x++)
                {
                    double lowChanceWater = random.NextDouble();
                    int SelectTiles = random.Next(0, 5);
                    if (SelectTiles == 1 && lowChanceWater <= 0.7)
                    {
                        
                        SpreadWaterRecursive(x, y, 2);
                        
                        //Vector2 position = new Vector2(x * sizeTiles, y * sizeTiles);
                        //_listWaveWTile.Add(new WaveWaterTile(_listWaveTexture, position));
                        //SpreadWater(x, y, 10);

                    }
                    else
                    {
                        Texture2D selectedTiles = SelectTiles switch
                        {
                            0 => grassTile,
                            2 => waterTile,
                            3 => dirtTile,
                            _ => rockTile
                        };
                        tiles.Add(new Tile(selectedTiles, new Vector2(x * sizeTiles, y * sizeTiles)));
                        _waterTiles[x, y] = false;
                        AvailDeploy = true ;
                        PositionDeploy = new Vector2(x*sizeTiles, y*sizeTiles);
                    }
                }
            }
            _listOfWater = _listWaveWTile.Select(t => t._position).ToList();//sus
            var deployable = tiles.Where(t => !_listWaveWTile.Any(w => w._position == t.position)).ToList();
            var deployableForWhale = tiles.Where(t => _listWaveWTile.Any(w => w._position == t.position)).ToList();
            var land = deployable[random.Next(deployable.Count)];//sus
            // TODO: use this.Content to load your game content here
            _playerTexture = Content.Load<Texture2D>("LUM");//Player characterto
            if (AvailDeploy )
            {
                _player = new Player(_playerTexture, new Vector2(land.position.X, land.position.Y));//setting up the player position
                AvailDeploy = false;
            }
            for (int i=0;i<10;i++)
            {
                if(deployable.Count > 0)
                {
                    var randomLand = deployable[random.Next(deployable.Count)];
                    movingArmies.Add(new MovingBot(new Vector2(randomLand.position.X, randomLand.position.Y), botTexture));
                }
                var randomWater = deployableForWhale[random.Next(deployableForWhale.Count)];
                massiveWhale.Add(new Whale(swimFrames, airFrames, new Vector2(randomWater.position.X, randomWater.position.Y), _lSystemTrees));
            }      
        }

        void SpreadWater(int startX, int startY, int radius)
        {
            for (int y = startY - radius; y <= startY + radius; y++)
            {
                for (int x = startX - radius; x <= startX + radius; x++)
                {
                    if (x >= 0 && y >= 0 && x < rows && y < cols)
                    {
                        if (random.NextDouble() < 0.9)
                        {
                            Vector2 position = new Vector2(x * sizeTiles, y * sizeTiles);
                            _listWaveWTile.Add(new WaveWaterTile(_listWaveTexture, position));
                        }
                    }
                }
            }
        }

        bool TileIsWater(int x, int y)
        {
            if (x < 0 || y < 0 || x >= rows || y >= cols) return false;
            return _waterTiles[x, y];
        }

        void SpreadWaterRecursive(int startX, int startY, int maxDepth)
        {
            Queue<(int x, int y, int depth)> _waterQueue = new Queue<(int, int, int)>();//init
            _waterQueue.Enqueue((startX, startY, 0));//1,1,0

            while (_waterQueue.Count > 0)//1 > 0
            {
                var (x, y, depth) = _waterQueue.Dequeue();

                if (depth >= maxDepth) continue;//0 >= 15/maxDepth
                if (x < 0 || y < 0 || x >= rows || y >= cols) continue;//if 1<0||1<0||1>=row../skip if out of bounds
                if (TileIsWater(x, y)) continue;//stop when its already fill

                Vector2 position = new Vector2(x * sizeTiles, y * sizeTiles);



                _waterTiles[x, y] = true;
                var index = tiles.FindIndex(t => t.position == position);
                if (index != -1)
                {

                    tiles.RemoveAt(index);
                }
                _listWaveWTile.Add(new WaveWaterTile(_listWaveTexture, position));
                thisIsWaterTiles = new Vector2(position.X , position.Y);

                Probability(_waterQueue, x - 1, y, depth + 1);
                Probability(_waterQueue, x + 1, y, depth + 1);
                Probability(_waterQueue, x, y - 1, depth + 1);
                Probability(_waterQueue, x, y + 1, depth + 1);

                Probability(_waterQueue, x - 1, y - 1, depth + 1);
                Probability(_waterQueue, x - 1, y + 1, depth + 1);
                Probability(_waterQueue, x + 1, y - 1, depth + 1);
                Probability(_waterQueue, x + 1, y + 1, depth + 1);
            }
        }

        void Probability(Queue<(int x, int y, int depth)> _queue, int x, int y, int depth)
        {
            if (random.NextDouble() < 0.6)
            {
                _queue.Enqueue((x, y, depth));
            }

        }

        private int frameCount = 0;
        private double elapsedTimer = 0;
        private int currentFPS = 0;

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                    Exit();
            if (currentGameState == GameState.Random2DSimulation)
            {
                //initialize mouse and delta coor
                _mouseState = Mouse.GetState();
                if (SettingsClick)
                {
                    popupScreenTiming += 5f * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    popupScreenTiming = MathHelper.Clamp(popupScreenTiming, 0f, 1f);
                }
                else
                {
                    popupScreenTiming -= 5f * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    popupScreenTiming = MathHelper.Clamp(popupScreenTiming, 0f, 1f);
                }

                if (rectSettings.Contains(_mouseState.Position) && _mouseState.LeftButton == ButtonState.Pressed){
                    SettingsClick = !SettingsClick;
                }

                mouseX = _mouseState.X;
                mouseY = _mouseState.Y;
                deltaX = mouseX - screenCenteredX;
                deltaY = mouseY - screenCenteredY;
                MouseState testMouse = Mouse.GetState();
                Debug.WriteLine($"Mouse RAW: {testMouse.X}, {testMouse.Y}");
            
                KeyboardState currentKeyboardState = Keyboard.GetState();
                //display waterMove
                if (currentKeyboardState.IsKeyDown(Keys.M) && _previousKeyboardState.IsKeyDown(Keys.M))
                {
                    _mapDisplay.Toggle();
                }

                foreach (var waterTile in _listWaveWTile)
                {
                    waterTile.Update(gameTime);
                }



                _mapDisplay.Update(gameTime, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
                foreach (var cloud in _clouds)
                {
                    cloud.Update(_graphics.PreferredBackBufferWidth);
                }

                //fading text structure
                if (!string.IsNullOrEmpty(_currentDialogue))
                {
                    _displayTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;

                    if (_displayTimer <= 1f)
                    {
                        _fadeAlpha = _displayTimer;
                    }

                    if (_displayTimer <= 0f)
                    {
                        _currentDialogue = "";
                        _fadeAlpha = 1f;
                    }
                }

                //repeating music
                if (_musicLoaded && MediaPlayer.State != MediaState.Playing)
                {
                    MediaPlayer.IsRepeating = true;
                    MediaPlayer.Play(song);
                }

                //updatingclasses
                
                _player.Update(gameTime,GraphicsDevice,_listOfWater);
                _botManager.Update(gameTime);

                foreach (var bot in _movingSprite)
                {
                    bot.Value.Update(gameTime);
                }

                //for pressing T
                if (currentKeyboardState.IsKeyDown(Keys.T) && _previousKeyboardState.IsKeyUp(Keys.T))
                {

                    _currentDialogue = _botManager.TalkToClosestBot(_player.position, 30f);
                    _displayTimer = 2f;
                    _fadeAlpha = 1f;
                }
                
                if (currentKeyboardState.IsKeyDown(Keys.LeftShift) && currentKeyboardState.IsKeyDown(Keys.RightShift) || currentKeyboardState.IsKeyDown(Keys.T)) {
                    Random random = new Random();
                    _player.position = new Vector2(random.Next(100, 1900), random.Next(100,1000));
                }

                HandleInput(gameTime, currentKeyboardState);

                _showStats = (currentKeyboardState.IsKeyDown(Keys.LeftShift) || currentKeyboardState.IsKeyDown(Keys.RightShift));

                foreach (var bot in movingArmies)
                {
                    bot.Update(gameTime, _listOfWater);
                }

                bool allWhalesDead = false;

                foreach (var whale in massiveWhale.ToList()) 
                {
                    whale.Update(gameTime, GraphicsDevice, _listOfWater);

                    if (!whale.IsAlive)
                    {
                        massiveWhale.Remove(whale);
                    }
                }

                if (massiveWhale.Count == 0 && gameHasStarted && !isTransitioning) {
                    isTransitioning = true;
                }

                if (isTransitioning)
                {
                    fadeTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

                    if (fadeTimer >= 3f)
                    {
                        LoadRestaurantScene();
                    }
                }
                _previousKeyboardState = currentKeyboardState;
                // TODO: Add your update logic here
            }else if (currentGameState == GameState.RestaurantObserver)
            {
                UpdateRestaurantCamera(gameTime);
            }

            elapsedTimer += gameTime.ElapsedGameTime.TotalMilliseconds;
            frameCount++;

            if (elapsedTimer >= 1000.0)
            {
                currentFPS = frameCount;

                long totalMemory = GC.GetTotalMemory(false);
                double memoryInMB = totalMemory / (1024.0 * 1024.0);
                int gen0 = GC.CollectionCount(0);
                int gen1 = GC.CollectionCount(1);
                int gen2 = GC.CollectionCount(2);

                Console.WriteLine($"FPS: {currentFPS} | Memory: {memoryInMB:F2} MB | GC Counts -> Gen0: {gen0}, Gen1: {gen1}, Gen2: {gen2}");

                frameCount = 0;
                elapsedTimer = 0;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            if (currentGameState == GameState.Random2DSimulation) 
            {
                GraphicsDevice.Clear(Color.CornflowerBlue);
                _mousePositionDisplay = $"Mouse Position - X: {mouseX}, Y: {mouseY}";
                _deltaDisplay = $"Delta Position - X: {deltaX}, Y: {deltaY}";
                _spriteBatch.Begin();

                //displaying
                foreach (var tile in tiles)
                {
                    tile.Draw(_spriteBatch);
                }
                foreach (var waterTile in _listWaveWTile)
                {
                    waterTile.Draw(_spriteBatch);
                }
                foreach (var bot in _movingSprite)
                {
                    bot.Value.Draw(_spriteBatch);
                }
                foreach (Tree tree in _trees) //watch
                {
                    Vector2 pixelPosition = new Vector2(tree.Position.X * sizeTiles, tree.Position.Y * sizeTiles);

                    _spriteBatch.Draw(tree.Texture, pixelPosition, null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
                }
                _player.Draw(_spriteBatch);
                _botManager.Draw(_spriteBatch);
                foreach (var tree in _lSystemTrees)
                {
                    tree.Draw(_spriteBatch);
                }
                //displaying text 

                xyPlayerPosition = $"Player - X: {_player.position.X:F2}, Y: {_player.position.Y:F2}";
                botCount = $"Total Bots: {_botManager._bots.Count.ToString()}";

                foreach (var bot in movingArmies)
                {
                    bot.Draw(_spriteBatch);
                }
                foreach (var whale in massiveWhale)
                {
                    whale.Draw(_spriteBatch);
                }
                foreach (var cloud in _clouds)
                {
                    cloud.Draw(_spriteBatch);
                }

                if (!string.IsNullOrEmpty(_currentDialogue))
                {
                    _spriteBatch.DrawString(_font, _currentDialogue, new Vector2(800, 990), Color.Yellow * _fadeAlpha);
                }
                _spriteBatch.DrawString(_font, xyPlayerPosition, new Vector2(10, 1045), Color.Red);
                _spriteBatch.DrawString(_font, botCount, new Vector2(10, 1010), Color.Red);
                _spriteBatch.DrawString(_font, _mousePositionDisplay, new Vector2(10, 975), Color.Red);
                _spriteBatch.DrawString(_font, _deltaDisplay, new Vector2(10, 940), Color.Red);
                _spriteBatch.DrawString(_font, IsActive.ToString(), new Vector2(10, 900), Color.White);
                if (_showStats) _spriteBatch.DrawString(_font, "Show Stats", new Vector2(500, 500), Color.Red);
                _spriteBatch.Draw(textureSettings, rectSettings, Color.White);
                if (popupScreenTiming > 0f)
                {
                    _spriteBatch.Draw(textureMainBackgroundSettings, rectMainBackgroundSettings, Color.White*popupScreenTiming);
                }
                _mapDisplay.Draw(_spriteBatch);

                _spriteBatch.End();
                // TODO: Add your drawing code here
            } else if (currentGameState == GameState.RestaurantObserver)
            {
                DrawRestaurant3D();
            }
            base.Draw(gameTime);
        }

        public class Tree
        {
            public Vector2 Position;
            public Texture2D Texture;
            public int Width = 10;
            public int Height = 10;
        }

        private void CreateSampleTrees()
        {

            _lSystemTrees.Clear();


            var tree1 = new LSystemTree(_pixel)
            {
                Position = new Vector2(800, 500),
                BranchLength = 70f,
                Iterations = 2,
                BranchColor = Color.SaddleBrown,
                LeafColor = Color.ForestGreen
            };

            tree1.SetTreeType("bushy");
            _lSystemTrees.Add(tree1);

            var tree2 = new LSystemTree(_pixel)
            {
                Position = new Vector2(400, 500),
                BranchLength = 100f,
                Iterations = 5,
                BranchColor = Color.SaddleBrown,
                LeafColor = Color.LimeGreen
            };
            tree2.SetTreeType("tall");
            _lSystemTrees.Add(tree2);

            var tree3 = new LSystemTree(_pixel)
            {
                Position = new Vector2(600, 500),
                BranchLength = 60f,
                Iterations = 2,
                BranchColor = Color.Peru,
                LeafColor = Color.DarkGreen
            };
            tree3.SetTreeType("willow");
            _lSystemTrees.Add(tree3);
        }

        double holdSeconds = 0;
        bool trigger = false;
        public void HandleInput(GameTime gameTime, KeyboardState keyboard)
        {

            /*if (keyboard.IsKeyDown(Keys.Space)) {
                foreach (var tree in _lSystemTrees) {
                    tree.GenerateTree();
                }
            }*/

            if (keyboard.IsKeyDown(Keys.D1)) { _currentTypeTree = "bushy"; UpdateAllThreeTypes(); }
            ;
            if (keyboard.IsKeyDown(Keys.D2)) { _currentTypeTree = "tall"; UpdateAllThreeTypes(); }
            ;
            if (keyboard.IsKeyDown(Keys.D3)) { _currentTypeTree = "willow"; UpdateAllThreeTypes(); }
            ;
            if (keyboard.IsKeyDown(Keys.D4)) { _currentTypeTree = "palm"; UpdateAllThreeTypes(); }
            ;

            MouseState mouse = Mouse.GetState();
            double deltaTime = gameTime.ElapsedGameTime.TotalSeconds;

            if (mouse.LeftButton == ButtonState.Pressed)
            {
                holdSeconds += deltaTime;

                if (holdSeconds >= 1 && !trigger)
                {
                    trigger = true;
                    var newTree = new LSystemTree(_pixel)
                    {
                        Position = new Vector2(mouse.X, mouse.Y),
                        BranchLength = 60f + (float) random.NextDouble() * 40f,
                        Iterations = 3 + random.Next(3)
                    };
                    newTree.SetTreeType(_currentTypeTree);
                    _lSystemTrees.Add(newTree);
                }
            }
            else
            {
                holdSeconds = 0;
                trigger = false;
            }
        }

        private void UpdateAllThreeTypes()
        {
            foreach (var tree in _lSystemTrees)
            {
                tree.SetTreeType(_currentTypeTree);
            }
        }

        private void LoadRestaurantScene()
        {
            currentGameState = GameState.RestaurantObserver;

            MediaPlayer.Stop();
            _lSystemTrees?.Clear();
            _clouds?.Clear();
            _listWaveWTile?.Clear();

            _basicEffect = new BasicEffect(GraphicsDevice)
            {
                VertexColorEnabled = true
            };

            cameraPosition = new Vector3(0f, 1.2f, -1.5f);
            yaw = 0f;
            pitch = 0f;
            
            fadeTimer = 0f;
            _previousMouseState = Mouse.GetState();
        }

        private void UpdateRestaurantCamera(GameTime gameTime)
        {
            MouseState currentMouseState = Mouse.GetState();

            float deltaX = currentMouseState.X - _previousMouseState.X;
            float deltaY = currentMouseState.Y - _previousMouseState.Y;

            float sensitivity = 0.002f;
            yaw -= deltaX * sensitivity;
            pitch -= deltaY * sensitivity;

            pitch = MathHelper.Clamp(pitch, -MathHelper.PiOver4, MathHelper.PiOver4);

            _previousMouseState = currentMouseState;

            Vector3 direction = new Vector3(
                (float)(Math.Cos(pitch) * Math.Sin(yaw)),
                (float)Math.Sin(pitch),
                (float)(Math.Cos(pitch) * Math.Cos(yaw))
            );

            Vector3 target = cameraPosition + direction;

            viewMatrix = Matrix.CreateLookAt(cameraPosition, target, Vector3.Up);
            
            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.ToRadians(60f),
                GraphicsDevice.Viewport.AspectRatio, 
                0.1f, 
                100f
            );
        }

        private void DrawRestaurant3D()
        {
            GraphicsDevice.Clear(new Color(12, 20, 35));

            GraphicsDevice.RasterizerState = RasterizerState.CullNone;

            _basicEffect.View = viewMatrix;
            _basicEffect.Projection = projectionMatrix;
            _basicEffect.World = Matrix.Identity;

            DepthStencilState depthState = new DepthStencilState() { DepthBufferEnable = true };
            GraphicsDevice.DepthStencilState = depthState;

            foreach (var pass in _basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();

            VertexPositionColor[] leftWallFront = new VertexPositionColor[]
            {
                new VertexPositionColor(new Vector3(-7f, 0f,  4f), new Color(55, 60, 68)),
                new VertexPositionColor(new Vector3(-7f, 0f,  1f), new Color(55, 60, 68)),
                new VertexPositionColor(new Vector3(-7f, 4.5f, 1f), new Color(40, 45, 52)),

                new VertexPositionColor(new Vector3(-7f, 0f,  4f), new Color(55, 60, 68)),
                new VertexPositionColor(new Vector3(-7f, 4.5f, 1f), new Color(40, 45, 52)),
                new VertexPositionColor(new Vector3(-7f, 4.5f, 4f), new Color(40, 45, 52)),
            };
            GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, leftWallFront, 0, 2);

            VertexPositionColor[] leftWallBack = new VertexPositionColor[]
            {
                new VertexPositionColor(new Vector3(-7f, 0f, -1f), new Color(55, 60, 68)),
                new VertexPositionColor(new Vector3(-7f, 0f, -7f), new Color(55, 60, 68)),
                new VertexPositionColor(new Vector3(-7f, 4.5f, -7f), new Color(40, 45, 52)),

                new VertexPositionColor(new Vector3(-7f, 0f, -1f), new Color(55, 60, 68)),
                new VertexPositionColor(new Vector3(-7f, 4.5f, -7f), new Color(40, 45, 52)),
                new VertexPositionColor(new Vector3(-7f, 4.5f, -1f), new Color(40, 45, 52)),
            };
            GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, leftWallBack, 0, 2);

            VertexPositionColor[] leftWallTop = new VertexPositionColor[]
            {
                new VertexPositionColor(new Vector3(-7f, 3.0f,  1f), new Color(55, 60, 68)),
                new VertexPositionColor(new Vector3(-7f, 3.0f, -1f), new Color(55, 60, 68)),
                new VertexPositionColor(new Vector3(-7f, 4.5f, -1f), new Color(40, 45, 52)),

                new VertexPositionColor(new Vector3(-7f, 3.0f,  1f), new Color(55, 60, 68)),
                new VertexPositionColor(new Vector3(-7f, 4.5f, -1f), new Color(40, 45, 52)),
                new VertexPositionColor(new Vector3(-7f, 4.5f,  1f), new Color(40, 45, 52)),
            };
            GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, leftWallTop, 0, 2);

                VertexPositionColor[] ceilingVertices = new VertexPositionColor[]
                {
                    new VertexPositionColor(new Vector3(-7f, 4.5f, -7f), new Color(20, 15, 12)),
                    new VertexPositionColor(new Vector3( 7f, 4.5f, -7f), new Color(20, 15, 12)),
                    new VertexPositionColor(new Vector3( 7f, 4.5f,  4f), new Color(20, 15, 12)),

                    new VertexPositionColor(new Vector3(-7f, 4.5f, -7f), new Color(20, 15, 12)),
                    new VertexPositionColor(new Vector3( 7f, 4.5f,  4f), new Color(20, 15, 12)),
                    new VertexPositionColor(new Vector3(-7f, 4.5f,  4f), new Color(20, 15, 12)),
                };
                GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, ceilingVertices, 0, 2);

                VertexPositionColor[] leftWallVertices = new VertexPositionColor[]
                {
                    new VertexPositionColor(new Vector3(-7f, 0f,  4f), new Color(55, 60, 68)),
                    new VertexPositionColor(new Vector3(-7f, 0f, -7f), new Color(55, 60, 68)),
                    new VertexPositionColor(new Vector3(-7f, 4.5f, -7f), new Color(40, 45, 52)),

                    new VertexPositionColor(new Vector3(-7f, 0f,  4f), new Color(55, 60, 68)),
                    new VertexPositionColor(new Vector3(-7f, 4.5f, -7f), new Color(40, 45, 52)),
                    new VertexPositionColor(new Vector3(-7f, 4.5f,  4f), new Color(40, 45, 52)),
                };
                GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, leftWallVertices, 0, 2);

                VertexPositionColor[] rightWallVertices = new VertexPositionColor[]
                {
                    new VertexPositionColor(new Vector3(7f, 0f, -7f), new Color(55, 60, 68)),
                    new VertexPositionColor(new Vector3(7f, 0f,  4f), new Color(55, 60, 68)),
                    new VertexPositionColor(new Vector3(7f, 4.5f,  4f), new Color(40, 45, 52)),

                    new VertexPositionColor(new Vector3(7f, 0f, -7f), new Color(55, 60, 68)),
                    new VertexPositionColor(new Vector3(7f, 4.5f,  4f), new Color(40, 45, 52)),
                    new VertexPositionColor(new Vector3(7f, 4.5f, -7f), new Color(40, 45, 52)),
                };
                GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, rightWallVertices, 0, 2);

                VertexPositionColor[] backWallLeft = new VertexPositionColor[]
                {
                    new VertexPositionColor(new Vector3(-7f, 0f, -7f), new Color(65, 50, 40)),
                    new VertexPositionColor(new Vector3(-2.5f, 0f, -7f), new Color(65, 50, 40)),
                    new VertexPositionColor(new Vector3(-2.5f, 4.5f, -7f), new Color(50, 38, 30)),

                    new VertexPositionColor(new Vector3(-7f, 0f, -7f), new Color(65, 50, 40)),
                    new VertexPositionColor(new Vector3(-2.5f, 4.5f, -7f), new Color(50, 38, 30)),
                    new VertexPositionColor(new Vector3(-7f, 4.5f, -7f), new Color(50, 38, 30)),
                };
                GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, backWallLeft, 0, 2);

                VertexPositionColor[] backWallRight = new VertexPositionColor[]
                {
                    new VertexPositionColor(new Vector3(2.5f, 0f, -7f), new Color(65, 50, 40)),
                    new VertexPositionColor(new Vector3(7f, 0f, -7f), new Color(65, 50, 40)),
                    new VertexPositionColor(new Vector3(7f, 4.5f, -7f), new Color(50, 38, 30)),

                    new VertexPositionColor(new Vector3(2.5f, 0f, -7f), new Color(65, 50, 40)),
                    new VertexPositionColor(new Vector3(7f, 4.5f, -7f), new Color(50, 38, 30)),
                    new VertexPositionColor(new Vector3(2.5f, 4.5f, -7f), new Color(50, 38, 30)),
                };
                GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, backWallRight, 0, 2);

                VertexPositionColor[] backWallTop = new VertexPositionColor[]
                {
                    new VertexPositionColor(new Vector3(-2.5f, 3.0f, -7f), new Color(65, 50, 40)),
                    new VertexPositionColor(new Vector3( 2.5f, 3.0f, -7f), new Color(65, 50, 40)),
                    new VertexPositionColor(new Vector3( 2.5f, 4.5f, -7f), new Color(50, 38, 30)),

                    new VertexPositionColor(new Vector3(-2.5f, 3.0f, -7f), new Color(65, 50, 40)),
                    new VertexPositionColor(new Vector3( 2.5f, 4.5f, -7f), new Color(50, 38, 30)),
                    new VertexPositionColor(new Vector3(-2.5f, 4.5f, -7f), new Color(50, 38, 30)),
                };
                GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, backWallTop, 0, 2);

                VertexPositionColor[] backWallBottom = new VertexPositionColor[]
                {
                    new VertexPositionColor(new Vector3(-2.5f, 0.0f, -7f), new Color(65, 50, 40)),
                    new VertexPositionColor(new Vector3( 2.5f, 0.0f, -7f), new Color(65, 50, 40)),
                    new VertexPositionColor(new Vector3( 2.5f, 0.8f, -7f), new Color(55, 42, 33)),

                    new VertexPositionColor(new Vector3(-2.5f, 0.0f, -7f), new Color(65, 50, 40)),
                    new VertexPositionColor(new Vector3( 2.5f, 0.8f, -7f), new Color(55, 42, 33)),
                    new VertexPositionColor(new Vector3(-2.5f, 0.8f, -7f), new Color(55, 42, 33)),
                };
                GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, backWallBottom, 0, 2);

                VertexPositionColor[] tableTopVertices = new VertexPositionColor[]
                {
                    new VertexPositionColor(new Vector3(-1.1f, 0.5f, -0.8f), new Color(90, 56, 32)),
                    new VertexPositionColor(new Vector3( 1.1f, 0.5f, -0.8f), new Color(90, 56, 32)),
                    new VertexPositionColor(new Vector3( 1.1f, 0.5f, -2.2f), new Color(75, 45, 25)),

                    new VertexPositionColor(new Vector3(-1.1f, 0.5f, -0.8f), new Color(90, 56, 32)),
                    new VertexPositionColor(new Vector3( 1.1f, 0.5f, -2.2f), new Color(75, 45, 25)),
                    new VertexPositionColor(new Vector3(-1.1f, 0.5f, -2.2f), new Color(75, 45, 25)),
                };
                GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, tableTopVertices, 0, 2);

                VertexPositionColor[] tableStandVertices = new VertexPositionColor[]
                {
                    new VertexPositionColor(new Vector3(-0.15f, 0.0f, -1.4f), new Color(40, 25, 15)),
                    new VertexPositionColor(new Vector3( 0.15f, 0.0f, -1.4f), new Color(40, 25, 15)),
                    new VertexPositionColor(new Vector3( 0.15f, 0.5f, -1.4f), new Color(60, 38, 22)),

                    new VertexPositionColor(new Vector3(-0.15f, 0.0f, -1.4f), new Color(40, 25, 15)),
                    new VertexPositionColor(new Vector3( 0.15f, 0.5f, -1.4f), new Color(60, 38, 22)),
                    new VertexPositionColor(new Vector3(-0.15f, 0.5f, -1.4f), new Color(60, 38, 22)),
                };
                GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, tableStandVertices, 0, 2);

                VertexPositionColor[] cupVertices = new VertexPositionColor[]
                {
                    new VertexPositionColor(new Vector3(-0.12f, 0.5f, -1.4f), Color.Gold),
                    new VertexPositionColor(new Vector3( 0.12f, 0.5f, -1.4f), Color.Gold),
                    new VertexPositionColor(new Vector3( 0.12f, 0.85f, -1.4f), Color.Orange),

                    new VertexPositionColor(new Vector3(-0.12f, 0.5f, -1.4f), Color.Gold),
                    new VertexPositionColor(new Vector3( 0.12f, 0.85f, -1.4f), Color.Orange),
                    new VertexPositionColor(new Vector3(-0.12f, 0.85f, -1.4f), Color.Orange),
                };
                GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, cupVertices, 0, 2);
            }
        }
    }
}
