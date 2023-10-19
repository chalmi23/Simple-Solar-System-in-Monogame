using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.Direct3D9;
using System;

namespace Solar_System
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Texture2D background;       //Tło

        private bool isBackgroundEnabled = true;

        private VertexPositionColor[] userPrimitives;
        private BasicEffect BasicEffect;
        private Matrix world, view, proj;
        private bool isGridEnabled = true;

        private float angleX = 0.0f;
        private float angleY = 0.0f;

        private float zoom = 1.0f; // Wartość przybliżenia
        private float zoomSpeed = 0.02f; // Prędkość przybliżania

        private int numVertices = 0; // Liczba wierzchołków (poziome + pionowe)
        private KeyboardState prevKeyboardState;  // Poprzedni stan klawiatury
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            this.Window.AllowUserResizing = true;       //Możliwość zmieniania wielkości okna
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth = 1000; // Szerokość ekranu
            _graphics.PreferredBackBufferHeight = 1000; // Wysokość ekranu
            _graphics.ApplyChanges();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);



            RasterizerState rs = new RasterizerState();
            rs.CullMode = CullMode.None;
            GraphicsDevice.RasterizerState = rs;

            int gridWidth = 4; // Szerokość siatki
            int gridHeight = 4; // Wysokość siatki

            int gridSizeX = 50; // Liczba kratek w poziomie
            int gridSizeY = 50; // Liczba kratek w pionie

            numVertices = (gridSizeX + gridSizeY + 2) * 2; // Liczba wierzchołków (poziome + pionowe)

            userPrimitives = new VertexPositionColor[numVertices]; // Inicjalizacja userPrimitives

            background = Content.Load<Texture2D>("stars");

            float spacingX = (float)gridWidth / gridSizeX;
            float spacingY = (float)gridHeight / gridSizeY;

            int vertexIndex = 0;

            // Poziome linie
            for (int i = 0; i <= gridSizeX; i++)
            {
                float x = -gridWidth / 2 + i * spacingX;
                float yStart = -gridHeight / 2;
                float yEnd = gridHeight / 2;

                userPrimitives[vertexIndex] = new VertexPositionColor(new Vector3(x, yStart, 0), Color.White);
                vertexIndex++;
                userPrimitives[vertexIndex] = new VertexPositionColor(new Vector3(x, yEnd, 0), Color.White);
                vertexIndex++;
            }

            // Pionowe linie
            for (int i = 0; i <= gridSizeY; i++)
            {
                float y = -gridHeight / 2 + i * spacingY;
                float xStart = -gridWidth / 2;
                float xEnd = gridWidth / 2;

                userPrimitives[vertexIndex] = new VertexPositionColor(new Vector3(xStart, y, 0), Color.White);
                vertexIndex++;
                userPrimitives[vertexIndex] = new VertexPositionColor(new Vector3(xEnd, y, 0), Color.White);
                vertexIndex++;
            }

            BasicEffect = new BasicEffect(GraphicsDevice);
            BasicEffect.VertexColorEnabled = true;
        }
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape)) Exit();

            KeyboardState keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.B) && prevKeyboardState.IsKeyUp(Keys.B))
            {
                isBackgroundEnabled = !isBackgroundEnabled;
            }

            if (keyboardState.IsKeyDown(Keys.X) && prevKeyboardState.IsKeyUp(Keys.X))
            {
                isGridEnabled = !isGridEnabled;
            }

            if (keyboardState.IsKeyDown(Keys.Right)) angleY += 0.02f;
            if (keyboardState.IsKeyDown(Keys.Left)) angleY -= 0.02f;

            if (keyboardState.IsKeyDown(Keys.Up)) angleX += 0.02f;
            if (keyboardState.IsKeyDown(Keys.Down)) angleX -= 0.02f;

            // Przybliżanie (Q)
            if (keyboardState.IsKeyDown(Keys.Q))
            {
                zoom += zoomSpeed;
            }

            // Oddalanie (A)
            if (keyboardState.IsKeyDown(Keys.A))
            {
                zoom -= zoomSpeed;
                if (zoom < 0.1f) // Ograniczenie minimalnego przybliżenia
                {
                    zoom = 0.1f;
                }
            }

            world = Matrix.Identity;
            view = Matrix.CreateLookAt(new Vector3(0.0f, 0.0f, 4.0f/zoom), Vector3.Zero, Vector3.Up);

            view = Matrix.CreateRotationX(angleX) * Matrix.CreateRotationY(angleY) * view;
            proj = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(50), GraphicsDevice.Viewport.AspectRatio, 0.01f, 1000.0f);

            BasicEffect.World = world;
            BasicEffect.View = view;
            BasicEffect.Projection = proj;

            prevKeyboardState = keyboardState;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            if (isBackgroundEnabled)
            {
                _spriteBatch.Begin();
                _spriteBatch.Draw(background, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);
                _spriteBatch.End();
            }

            if (isGridEnabled)
            {
                BasicEffect.CurrentTechnique.Passes[0].Apply();
                GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(Microsoft.Xna.Framework.Graphics.PrimitiveType.LineList, userPrimitives, 0, numVertices / 2);
            }

            base.Draw(gameTime);
        }
    }

}