using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;
using System;

namespace Solar_System
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Texture2D background;

        private float time = 0; // Czas w grze

        private bool isBackgroundEnabled = true;
        private bool isGridEnabled = true;

        private BasicEffect basicEffect;
        private Matrix world, view, proj;

        private float angleX = 0.0f, angleY = 0.0f;

        private Vector3 mercuryPosition, venusPosition, earthPosition, moonPosition, marsPosition, jupiterPosition, saturnPosition, uranusPosition, neptunePosition;

        private float sunRotationSpeed = 5.0f;
        private float sunRotationAngle = 0f;

        private float mercuryRotationSpeed = 1.0f;
        private float mercuryRotationAngle = 0f;

        private float venusRotationSpeed = 0.7f;
        private float venusRotationAngle = 0f;

        private float earthRotationSpeed = 0.5f;
        private float earthRotationAngle = 0f;

        private float moonRotationSpeed = 2.0f;
        private float moonRotationAngle = 0f;

        private float marsRotationSpeed = 0.4f;
        private float marsRotationAngle = 0f;

        private float jupiterRotationSpeed = 0.2f;
        private float jupiterRotationAngle = 0f;

        private float saturnRotationSpeed = 0.15f;
        private float saturnRotationAngle = 0f;

        private float uranusRotationSpeed = 0.1f;
        private float uranusRotationAngle = 0f;

        private float neptuneRotationSpeed = 0.08f;
        private float neptuneRotationAngle = 0f;

        private float zoom = 1.0f;
        private float zoomSpeed = 0.02f;
        private KeyboardState prevKeyboardState;  // Poprzedni stan klawiatury
        private VertexPositionColor[] userPrimitives;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            this.Window.AllowUserResizing = true;
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth = 1000;
            _graphics.PreferredBackBufferHeight = 1000;
            _graphics.ApplyChanges();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            background = Content.Load<Texture2D>("stars");

            RasterizerState rs = new RasterizerState();
            rs.CullMode = CullMode.None; // Ustawienie cull mode na None
            GraphicsDevice.RasterizerState = rs;

            int gridWidth = 14;
            int gridHeight = 14;
            int gridSizeX = 80;
            int gridSizeY = 80;

            int numVertices = (gridSizeX + gridSizeY + 2) * 2;
            userPrimitives = new VertexPositionColor[numVertices];

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

            basicEffect = new BasicEffect(GraphicsDevice);
            basicEffect.VertexColorEnabled = true;
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape)) Exit();

            KeyboardState keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.B) && prevKeyboardState.IsKeyUp(Keys.B)) isBackgroundEnabled = !isBackgroundEnabled;
            if (keyboardState.IsKeyDown(Keys.X) && prevKeyboardState.IsKeyUp(Keys.X)) isGridEnabled = !isGridEnabled;

            if (keyboardState.IsKeyDown(Keys.Right)) angleY += 0.02f;
            if (keyboardState.IsKeyDown(Keys.Left)) angleY -= 0.02f;

            if (keyboardState.IsKeyDown(Keys.Up)) angleX += 0.02f;
            if (keyboardState.IsKeyDown(Keys.Down)) angleX -= 0.02f;

            if (keyboardState.IsKeyDown(Keys.Q))
            {
                zoom += zoomSpeed;
            }

            if (keyboardState.IsKeyDown(Keys.A))
            {
                zoom -= zoomSpeed;
                if (zoom < 0.1f)
                {
                    zoom = 0.1f;
                }
            }

            world = Matrix.Identity;
            view = Matrix.CreateLookAt(new Vector3(0.0f, 0.0f, 6.0f / zoom), Vector3.Zero, Vector3.Up);
            view = Matrix.CreateRotationX(angleX) * Matrix.CreateRotationY(angleY) * view;
            proj = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(50), GraphicsDevice.Viewport.AspectRatio, 0.01f, 1000.0f);

            basicEffect.World = world;
            basicEffect.View = view;
            basicEffect.Projection = proj;

            prevKeyboardState = keyboardState;


            time += (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Obliczenia dla Merkurego
            float mercuryOrbitRadius = 0.6f;
            float mercuryOrbitSpeed = 0.1f;
            float mercuryX = mercuryOrbitRadius * (float)Math.Cos(time * mercuryOrbitSpeed);
            float mercuryY = mercuryOrbitRadius * (float)Math.Sin(time * mercuryOrbitSpeed);

            // Obliczenia dla Wenus
            float venusOrbitRadius = 1.0f;
            float venusOrbitSpeed = 0.08f;
            float venusX = venusOrbitRadius * (float)Math.Cos(time * venusOrbitSpeed);
            float venusY = venusOrbitRadius * (float)Math.Sin(time * venusOrbitSpeed);

            // Obliczenia dla Ziemi
            float earthOrbitRadius = 1.5f;
            float earthOrbitSpeed = 0.05f;
            float earthX = earthOrbitRadius * (float)Math.Cos(time * earthOrbitSpeed);
            float earthY = earthOrbitRadius * (float)Math.Sin(time * earthOrbitSpeed);

            // Obliczenia dla Księżyca
            float moonOrbitRadius = 0.1f; // Wybierz odpowiedni promień orbity
            float moonOrbitSpeed = 0.2f;  // Wybierz odpowiednią prędkość obiegu
            float moonX = earthPosition.X + moonOrbitRadius * (float)Math.Cos(time * moonOrbitSpeed);
            float moonY = earthPosition.Y + moonOrbitRadius * (float)Math.Sin(time * moonOrbitSpeed);

            // Obliczenia dla Marsa
            float marsOrbitRadius = 2.0f;
            float marsOrbitSpeed = 0.04f;
            float marsX = marsOrbitRadius * (float)Math.Cos(time * marsOrbitSpeed);
            float marsY = marsOrbitRadius * (float)Math.Sin(time * marsOrbitSpeed);

            // Obliczenia dla Jowisza
            float jupiterOrbitRadius = 3.0f; // Wybierz odpowiedni promień orbity
            float jupiterOrbitSpeed = 0.01f;  // Wybierz odpowiednią prędkość obiegu
            float jupiterX = jupiterOrbitRadius * (float)Math.Cos(time * jupiterOrbitSpeed);
            float jupiterY = jupiterOrbitRadius * (float)Math.Sin(time * jupiterOrbitSpeed);

            // Obliczenia dla Saturna
            float saturnOrbitRadius = 4.0f; // Wybierz odpowiedni promień orbity
            float saturnOrbitSpeed = 0.008f;  // Wybierz odpowiednią prędkość obiegu
            float saturnX = saturnOrbitRadius * (float)Math.Cos(time * saturnOrbitSpeed);
            float saturnY = saturnOrbitRadius * (float)Math.Sin(time * saturnOrbitSpeed);

            // Obliczenia dla Urana
            float uranusOrbitRadius = 5.0f; // Wybierz odpowiedni promień orbity
            float uranusOrbitSpeed = 0.006f;  // Wybierz odpowiednią prędkość obiegu
            float uranusX = uranusOrbitRadius * (float)Math.Cos(time * uranusOrbitSpeed);
            float uranusY = uranusOrbitRadius * (float)Math.Sin(time * uranusOrbitSpeed);

            // Obliczenia dla Neptuna
            float neptuneOrbitRadius = 6.0f; // Wybierz odpowiedni promień orbity
            float neptuneOrbitSpeed = 0.005f;  // Wybierz odpowiednią prędkość obiegu
            float neptuneX = neptuneOrbitRadius * (float)Math.Cos(time * neptuneOrbitSpeed);
            float neptuneY = neptuneOrbitRadius * (float)Math.Sin(time * neptuneOrbitSpeed);

            // Przypisanie nowych pozycji planet
            mercuryPosition = new Vector3(mercuryX, mercuryY, 0f);
            venusPosition = new Vector3(venusX, venusY, 0f);
            earthPosition = new Vector3(earthX, earthY, 0f);
            moonPosition = new Vector3(moonX, moonY, 0f);
            marsPosition = new Vector3(marsX, marsY, 0f);
            jupiterPosition = new Vector3(jupiterX, jupiterY, 0f);
            saturnPosition = new Vector3(saturnX, saturnY, 0f);
            uranusPosition = new Vector3(uranusX, uranusY, 0f);
            neptunePosition = new Vector3(neptuneX, neptuneY, 0f);

            sunRotationAngle = time * sunRotationSpeed;
            mercuryRotationAngle = time * mercuryRotationSpeed;
            venusRotationAngle = time * venusRotationSpeed;
            earthRotationAngle = time * earthRotationSpeed;
            moonRotationAngle = time * moonRotationSpeed;
            marsRotationAngle = time * marsRotationSpeed;
            jupiterRotationAngle = time * jupiterRotationSpeed;
            saturnRotationAngle = time * saturnRotationSpeed;
            uranusRotationAngle = time * uranusRotationSpeed;
            neptuneRotationAngle = time * neptuneRotationSpeed;


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            if (isBackgroundEnabled)    //Włączanie i wyłączanie tła 
            {
                _spriteBatch.Begin();
                _spriteBatch.Draw(background, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);
                _spriteBatch.End();
            }

            if (isGridEnabled)      //Włączanie i wyłączanie siatki 
            {
                basicEffect.CurrentTechnique.Passes[0].Apply();
                GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineList, userPrimitives, 0, userPrimitives.Length / 2);
            }

            DrawFilledCube(0.06f, 0f, 0f, 0f, Color.Yellow, sunRotationAngle);                                             // Narysuj Słońce
            DrawFilledCube(0.015f, mercuryPosition.X, mercuryPosition.Y, 0f, Color.Gray, mercuryRotationAngle);            // Narysuj Merkurego 
            DrawFilledCube(0.018f, venusPosition.X, venusPosition.Y, 0f, Color.Orange, venusRotationAngle);                // Narysuj Wenus 
            DrawFilledCube(0.019f, earthPosition.X, earthPosition.Y, 0f, Color.Blue, earthRotationAngle);                  // Narysuj Ziemię 
            DrawFilledCube(0.01f, moonPosition.X, moonPosition.Y, 0f, Color.LightGray, moonRotationAngle);                 // Narysuj Księżyc na orbicie Ziemi
            DrawFilledCube(0.017f, marsPosition.X, marsPosition.Y, 0f, Color.Red, marsRotationAngle);                      // Narysuj Marsa 
            DrawFilledCube(0.035f, jupiterPosition.X, jupiterPosition.Y, 0f, Color.Orange, jupiterRotationAngle);          // Narysuj Jowisza
            DrawFilledCube(0.03f, saturnPosition.X, saturnPosition.Y, 0f, Color.Gold, saturnRotationAngle);                // Narysuj Saturna
            DrawFilledCube(0.02f, uranusPosition.X, uranusPosition.Y, 0f, Color.Cyan, uranusRotationAngle);                // Narysuj Urana
            DrawFilledCube(0.02f, neptunePosition.X, neptunePosition.Y, 0f, Color.Blue, neptuneRotationAngle);             // Narysuj Neptuna

            base.Draw(gameTime);
        }
        private void DrawFilledCube(float cubeSize, float x, float y, float z, Color color, float rotationAngle) //Funkcja do rysowania ciał niebieskich
        {
            Vector3 cubePosition = new Vector3(x, y, z);

            Matrix translationMatrix = Matrix.CreateTranslation(cubePosition);    // Macierz translacji
            Matrix rotationMatrix = Matrix.CreateRotationZ(rotationAngle);            // Macierz obrotu wokół własnej osi          
            Matrix cubeWorld = rotationMatrix * translationMatrix;  // Kombinuj macierze translacji i obrotu

            VertexPositionColor[] cubeVertices = new VertexPositionColor[36];

            Vector3[] cubeCorners = new Vector3[8];
            cubeCorners[0] = new Vector3(-cubeSize, -cubeSize, -cubeSize);
            cubeCorners[1] = new Vector3(cubeSize, -cubeSize, -cubeSize);
            cubeCorners[2] = new Vector3(cubeSize, cubeSize, -cubeSize);
            cubeCorners[3] = new Vector3(-cubeSize, cubeSize, -cubeSize);
            cubeCorners[4] = new Vector3(-cubeSize, -cubeSize, cubeSize);
            cubeCorners[5] = new Vector3(cubeSize, -cubeSize, cubeSize);
            cubeCorners[6] = new Vector3(cubeSize, cubeSize, cubeSize);
            cubeCorners[7] = new Vector3(-cubeSize, cubeSize, cubeSize);

            int[] cubeIndices = new int[]
            {
            0, 1, 2, 0, 2, 3, // Przednia ściana
            4, 5, 6, 4, 6, 7, // Tylna ściana
            0, 1, 5, 0, 5, 4, // Lewa ściana
            2, 3, 7, 2, 7, 6, // Prawa ściana
            0, 3, 7, 0, 7, 4, // Górna ściana
            1, 2, 6, 1, 6, 5  // Dolna ściana
            };

            for (int i = 0; i < 36; i++)
            {
                cubeVertices[i] = new VertexPositionColor(Vector3.Transform(cubeCorners[cubeIndices[i]], cubeWorld), color);
            }

            RasterizerState rs = new RasterizerState();
            rs.CullMode = CullMode.None; // Ustawienie cull mode na None
            GraphicsDevice.RasterizerState = rs;

            basicEffect.CurrentTechnique.Passes[0].Apply();
            GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, cubeVertices, 0, 12);
        }
    }
}
