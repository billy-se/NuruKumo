using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using nomoredeleting.Models.Environment.Restaurant;

namespace nomoredeleting.Renderers
{
    public class RestaurantRenderer
    {
        private BasicEffect _basicEffect;

        public RestaurantRenderer(GraphicsDevice graphics)
        {
            _basicEffect = new BasicEffect(graphics)
            {
                VertexColorEnabled = true
            };
        }

        public void Draw(GraphicsDevice graphics, Matrix viewMatrix, Matrix projectionMatrix, IEnumerable<TableInfo> tables)
        {
            graphics.Clear(new Color(15, 18, 25));

            graphics.RasterizerState = RasterizerState.CullNone;

            _basicEffect.View = viewMatrix;
            _basicEffect.Projection = projectionMatrix;
            _basicEffect.LightingEnabled = false; 

            DepthStencilState depthState = new DepthStencilState() { DepthBufferEnable = true };
            graphics.DepthStencilState = depthState;

            foreach (var pass in _basicEffect.CurrentTechnique.Passes)
            {
                _basicEffect.World = Matrix.Identity;
                pass.Apply();

                VertexPositionColor[] floorVertices = new VertexPositionColor[]
                {
                    new VertexPositionColor(new Vector3(-7f, 0f, -7f), new Color(30, 25, 25)),
                    new VertexPositionColor(new Vector3( 7f, 0f, -7f), new Color(30, 25, 25)),
                    new VertexPositionColor(new Vector3( 7f, 0f,  6f), new Color(20, 15, 15)),

                    new VertexPositionColor(new Vector3(-7f, 0f, -7f), new Color(30, 25, 25)),
                    new VertexPositionColor(new Vector3( 7f, 0f,  6f), new Color(20, 15, 15)),
                    new VertexPositionColor(new Vector3(-7f, 0f,  6f), new Color(20, 15, 15)),
                };
                graphics.DrawUserPrimitives(PrimitiveType.TriangleList, floorVertices, 0, 2);

                VertexPositionColor[] ceilingVertices = new VertexPositionColor[]
                {
                    new VertexPositionColor(new Vector3(-7f, 4.5f, -7f), new Color(15, 15, 20)),
                    new VertexPositionColor(new Vector3( 7f, 4.5f, -7f), new Color(15, 15, 20)),
                    new VertexPositionColor(new Vector3( 7f, 4.5f,  6f), new Color(15, 15, 20)),

                    new VertexPositionColor(new Vector3(-7f, 4.5f, -7f), new Color(15, 15, 20)),
                    new VertexPositionColor(new Vector3( 7f, 4.5f,  6f), new Color(15, 15, 20)),
                    new VertexPositionColor(new Vector3(-7f, 4.5f,  6f), new Color(15, 15, 20)),
                };
                graphics.DrawUserPrimitives(PrimitiveType.TriangleList, ceilingVertices, 0, 2);

                VertexPositionColor[] leftWall = new VertexPositionColor[]
                {
                    new VertexPositionColor(new Vector3(-7f, 0f,  6f), new Color(35, 40, 50)),
                    new VertexPositionColor(new Vector3(-7f, 0f, -7f), new Color(35, 40, 50)),
                    new VertexPositionColor(new Vector3(-7f, 4.5f, -7f), new Color(20, 25, 32)),

                    new VertexPositionColor(new Vector3(-7f, 0f,  6f), new Color(35, 40, 50)),
                    new VertexPositionColor(new Vector3(-7f, 4.5f, -7f), new Color(20, 25, 32)),
                    new VertexPositionColor(new Vector3(-7f, 4.5f,  6f), new Color(20, 25, 32)),
                };
                graphics.DrawUserPrimitives(PrimitiveType.TriangleList, leftWall, 0, 2);

                VertexPositionColor[] rightWall = new VertexPositionColor[]
                {
                    new VertexPositionColor(new Vector3(7f, 0f, -7f), new Color(35, 40, 50)),
                    new VertexPositionColor(new Vector3(7f, 0f,  6f), new Color(35, 40, 50)),
                    new VertexPositionColor(new Vector3(7f, 4.5f,  6f), new Color(20, 25, 32)),

                    new VertexPositionColor(new Vector3(7f, 0f, -7f), new Color(35, 40, 50)),
                    new VertexPositionColor(new Vector3(7f, 4.5f,  6f), new Color(20, 25, 32)),
                    new VertexPositionColor(new Vector3(7f, 4.5f, -7f), new Color(20, 25, 32)),
                };
                graphics.DrawUserPrimitives(PrimitiveType.TriangleList, rightWall, 0, 2);

                foreach (var table in tables)
                {
                    _basicEffect.World = Matrix.CreateTranslation(table.Position);
                    pass.Apply();

                    VertexPositionColor[] tableTop = new VertexPositionColor[]
                    {
                        new VertexPositionColor(new Vector3(-1.1f, 0.55f, -0.8f), new Color(90, 60, 35)),
                        new VertexPositionColor(new Vector3( 1.1f, 0.55f, -0.8f), new Color(90, 60, 35)),
                        new VertexPositionColor(new Vector3( 1.1f, 0.55f, -1.9f), new Color(60, 38, 20)),
                        new VertexPositionColor(new Vector3(-1.1f, 0.55f, -0.8f), new Color(90, 60, 35)),
                        new VertexPositionColor(new Vector3( 1.1f, 0.55f, -1.9f), new Color(60, 38, 20)),
                        new VertexPositionColor(new Vector3(-1.1f, 0.55f, -1.9f), new Color(60, 38, 20)),

                        new VertexPositionColor(new Vector3(-1.1f, 0.43f, -1.9f), new Color(40, 24, 12)),
                        new VertexPositionColor(new Vector3( 1.1f, 0.43f, -1.9f), new Color(40, 24, 12)),
                        new VertexPositionColor(new Vector3( 1.1f, 0.43f, -0.8f), new Color(40, 24, 12)),
                        new VertexPositionColor(new Vector3(-1.1f, 0.43f, -1.9f), new Color(40, 24, 12)),
                        new VertexPositionColor(new Vector3( 1.1f, 0.43f, -0.8f), new Color(40, 24, 12)),
                        new VertexPositionColor(new Vector3(-1.1f, 0.43f, -0.8f), new Color(40, 24, 12)),

                        new VertexPositionColor(new Vector3(-1.1f, 0.43f, -0.8f), new Color(70, 45, 25)),
                        new VertexPositionColor(new Vector3( 1.1f, 0.43f, -0.8f), new Color(70, 45, 25)),
                        new VertexPositionColor(new Vector3( 1.1f, 0.55f, -0.8f), new Color(90, 60, 35)),
                        new VertexPositionColor(new Vector3(-1.1f, 0.43f, -0.8f), new Color(70, 45, 25)),
                        new VertexPositionColor(new Vector3( 1.1f, 0.55f, -0.8f), new Color(90, 60, 35)),
                        new VertexPositionColor(new Vector3(-1.1f, 0.55f, -0.8f), new Color(90, 60, 35)),

                        new VertexPositionColor(new Vector3( 1.1f, 0.43f, -1.9f), new Color(50, 32, 18)),
                        new VertexPositionColor(new Vector3(-1.1f, 0.43f, -1.9f), new Color(50, 32, 18)),
                        new VertexPositionColor(new Vector3(-1.1f, 0.55f, -1.9f), new Color(60, 38, 20)),
                        new VertexPositionColor(new Vector3( 1.1f, 0.43f, -1.9f), new Color(50, 32, 18)),
                        new VertexPositionColor(new Vector3(-1.1f, 0.55f, -1.9f), new Color(60, 38, 20)),
                        new VertexPositionColor(new Vector3( 1.1f, 0.55f, -1.9f), new Color(60, 38, 20)),

                        new VertexPositionColor(new Vector3(-1.1f, 0.43f, -1.9f), new Color(55, 35, 20)),
                        new VertexPositionColor(new Vector3(-1.1f, 0.43f, -0.8f), new Color(70, 45, 25)),
                        new VertexPositionColor(new Vector3(-1.1f, 0.55f, -0.8f), new Color(90, 60, 35)),
                        new VertexPositionColor(new Vector3(-1.1f, 0.43f, -1.9f), new Color(55, 35, 20)),
                        new VertexPositionColor(new Vector3(-1.1f, 0.55f, -0.8f), new Color(90, 60, 35)),
                        new VertexPositionColor(new Vector3(-1.1f, 0.55f, -1.9f), new Color(60, 38, 20)),

                        new VertexPositionColor(new Vector3( 1.1f, 0.43f, -0.8f), new Color(70, 45, 25)),
                        new VertexPositionColor(new Vector3( 1.1f, 0.43f, -1.9f), new Color(55, 35, 20)),
                        new VertexPositionColor(new Vector3( 1.1f, 0.55f, -1.9f), new Color(60, 38, 20)),
                        new VertexPositionColor(new Vector3( 1.1f, 0.43f, -0.8f), new Color(70, 45, 25)),
                        new VertexPositionColor(new Vector3( 1.1f, 0.55f, -1.9f), new Color(60, 38, 20)),
                        new VertexPositionColor(new Vector3( 1.1f, 0.55f, -0.8f), new Color(90, 60, 35)),
                    };
                    graphics.DrawUserPrimitives(PrimitiveType.TriangleList, tableTop, 0, 12);

                    int legSegments = 12;
                    List<VertexPositionColor> tableLegVertices = new List<VertexPositionColor>();
                    float legRadius = 0.22f;
                    float legCenterX = 0.0f;
                    float legCenterZ = -1.35f;
                    float legBottomY = 0.0f;
                    float legTopY = 0.43f;

                    for (int i = 0; i < legSegments; i++)
                    {
                        float angle1 = (i / (float)legSegments) * MathHelper.TwoPi;
                        float angle2 = ((i + 1) / (float)legSegments) * MathHelper.TwoPi;

                        float x1 = legCenterX + (float)Math.Cos(angle1) * legRadius;
                        float z1 = legCenterZ + (float)Math.Sin(angle1) * legRadius;
                        float x2 = legCenterX + (float)Math.Cos(angle2) * legRadius;
                        float z2 = legCenterZ + (float)Math.Sin(angle2) * legRadius;

                        tableLegVertices.Add(new VertexPositionColor(new Vector3(x1, legBottomY, z1), new Color(30, 20, 12)));
                        tableLegVertices.Add(new VertexPositionColor(new Vector3(x1, legTopY, z1), new Color(50, 35, 20)));
                        tableLegVertices.Add(new VertexPositionColor(new Vector3(x2, legBottomY, z2), new Color(30, 20, 12)));

                        tableLegVertices.Add(new VertexPositionColor(new Vector3(x1, legTopY, z1), new Color(50, 35, 20)));
                        tableLegVertices.Add(new VertexPositionColor(new Vector3(x2, legTopY, z2), new Color(50, 35, 20)));
                        tableLegVertices.Add(new VertexPositionColor(new Vector3(x2, legBottomY, z2), new Color(30, 20, 12)));

                        tableLegVertices.Add(new VertexPositionColor(new Vector3(legCenterX, legBottomY, legCenterZ), new Color(20, 15, 10)));
                        tableLegVertices.Add(new VertexPositionColor(new Vector3(x2, legBottomY, z2), new Color(30, 20, 12)));
                        tableLegVertices.Add(new VertexPositionColor(new Vector3(x1, legBottomY, z1), new Color(30, 20, 12)));
                    }
                    graphics.DrawUserPrimitives(PrimitiveType.TriangleList, tableLegVertices.ToArray(), 0, tableLegVertices.Count / 3);

                    VertexPositionColor[] chairOne = new VertexPositionColor[]
                    {
                        new VertexPositionColor(new Vector3(-0.7f, 0.42f, -0.2f), new Color(70, 20, 25)),
                        new VertexPositionColor(new Vector3(-0.3f, 0.42f, -0.2f), new Color(70, 20, 25)),
                        new VertexPositionColor(new Vector3(-0.3f, 0.42f, -0.6f), new Color(50, 15, 20)),
                        new VertexPositionColor(new Vector3(-0.7f, 0.42f, -0.2f), new Color(70, 20, 25)),
                        new VertexPositionColor(new Vector3(-0.3f, 0.42f, -0.6f), new Color(50, 15, 20)),
                        new VertexPositionColor(new Vector3(-0.7f, 0.42f, -0.6f), new Color(50, 15, 20)),

                        new VertexPositionColor(new Vector3(-0.7f, 0.38f, -0.6f), new Color(40, 10, 15)),
                        new VertexPositionColor(new Vector3(-0.3f, 0.38f, -0.6f), new Color(40, 10, 15)),
                        new VertexPositionColor(new Vector3(-0.3f, 0.38f, -0.2f), new Color(40, 10, 15)),
                        new VertexPositionColor(new Vector3(-0.7f, 0.38f, -0.6f), new Color(40, 10, 15)),
                        new VertexPositionColor(new Vector3(-0.3f, 0.38f, -0.2f), new Color(40, 10, 15)),
                        new VertexPositionColor(new Vector3(-0.7f, 0.38f, -0.2f), new Color(40, 10, 15)),

                        new VertexPositionColor(new Vector3(-0.7f, 0.38f, -0.2f), new Color(50, 15, 20)),
                        new VertexPositionColor(new Vector3(-0.3f, 0.38f, -0.2f), new Color(50, 15, 20)),
                        new VertexPositionColor(new Vector3(-0.3f, 1.1f, -0.2f), new Color(30, 10, 15)),
                        new VertexPositionColor(new Vector3(-0.7f, 0.38f, -0.2f), new Color(50, 15, 20)),
                        new VertexPositionColor(new Vector3(-0.3f, 1.1f, -0.2f), new Color(30, 10, 15)),
                        new VertexPositionColor(new Vector3(-0.7f, 1.1f, -0.2f), new Color(30, 10, 15)),

                        new VertexPositionColor(new Vector3(-0.68f, 0.0f, -0.58f), new Color(30, 12, 15)),
                        new VertexPositionColor(new Vector3(-0.64f, 0.0f, -0.58f), new Color(30, 12, 15)),
                        new VertexPositionColor(new Vector3(-0.64f, 0.38f, -0.58f), new Color(50, 15, 20)),
                        new VertexPositionColor(new Vector3(-0.68f, 0.0f, -0.58f), new Color(30, 12, 15)),
                        new VertexPositionColor(new Vector3(-0.64f, 0.38f, -0.58f), new Color(50, 15, 20)),
                        new VertexPositionColor(new Vector3(-0.68f, 0.38f, -0.58f), new Color(50, 15, 20)),

                        new VertexPositionColor(new Vector3(-0.36f, 0.0f, -0.58f), new Color(30, 12, 15)),
                        new VertexPositionColor(new Vector3(-0.32f, 0.0f, -0.58f), new Color(30, 12, 15)),
                        new VertexPositionColor(new Vector3(-0.32f, 0.38f, -0.58f), new Color(50, 15, 20)),
                        new VertexPositionColor(new Vector3(-0.36f, 0.0f, -0.58f), new Color(30, 12, 15)),
                        new VertexPositionColor(new Vector3(-0.32f, 0.38f, -0.58f), new Color(50, 15, 20)),
                        new VertexPositionColor(new Vector3(-0.36f, 0.38f, -0.58f), new Color(50, 15, 20)),

                        new VertexPositionColor(new Vector3(-0.68f, 0.0f, -0.22f), new Color(30, 12, 15)),
                        new VertexPositionColor(new Vector3(-0.64f, 0.0f, -0.22f), new Color(30, 12, 15)),
                        new VertexPositionColor(new Vector3(-0.64f, 0.38f, -0.22f), new Color(50, 15, 20)),
                        new VertexPositionColor(new Vector3(-0.68f, 0.0f, -0.22f), new Color(30, 12, 15)),
                        new VertexPositionColor(new Vector3(-0.64f, 0.38f, -0.22f), new Color(50, 15, 20)),
                        new VertexPositionColor(new Vector3(-0.68f, 0.38f, -0.22f), new Color(50, 15, 20)),

                        new VertexPositionColor(new Vector3(-0.36f, 0.0f, -0.22f), new Color(30, 12, 15)),
                        new VertexPositionColor(new Vector3(-0.32f, 0.0f, -0.22f), new Color(30, 12, 15)),
                        new VertexPositionColor(new Vector3(-0.32f, 0.38f, -0.22f), new Color(50, 15, 20)),
                        new VertexPositionColor(new Vector3(-0.36f, 0.0f, -0.22f), new Color(30, 12, 15)),
                        new VertexPositionColor(new Vector3(-0.32f, 0.38f, -0.22f), new Color(50, 15, 20)),
                        new VertexPositionColor(new Vector3(-0.36f, 0.38f, -0.22f), new Color(50, 15, 20)),
                    };
                    graphics.DrawUserPrimitives(PrimitiveType.TriangleList, chairOne, 0, 14);

                    VertexPositionColor[] chairTwo = new VertexPositionColor[]
                    {
                        new VertexPositionColor(new Vector3(0.3f, 0.42f, -0.2f), new Color(70, 20, 25)),
                        new VertexPositionColor(new Vector3(0.7f, 0.42f, -0.2f), new Color(70, 20, 25)),
                        new VertexPositionColor(new Vector3(0.7f, 0.42f, -0.6f), new Color(50, 15, 20)),
                        new VertexPositionColor(new Vector3(0.3f, 0.42f, -0.2f), new Color(70, 20, 25)),
                        new VertexPositionColor(new Vector3(0.7f, 0.42f, -0.6f), new Color(50, 15, 20)),
                        new VertexPositionColor(new Vector3(0.3f, 0.42f, -0.6f), new Color(50, 15, 20)),

                        new VertexPositionColor(new Vector3(0.3f, 0.38f, -0.6f), new Color(40, 10, 15)),
                        new VertexPositionColor(new Vector3(0.7f, 0.38f, -0.6f), new Color(40, 10, 15)),
                        new VertexPositionColor(new Vector3(0.7f, 0.38f, -0.2f), new Color(40, 10, 15)),
                        new VertexPositionColor(new Vector3(0.3f, 0.38f, -0.6f), new Color(40, 10, 15)),
                        new VertexPositionColor(new Vector3(0.7f, 0.38f, -0.2f), new Color(40, 10, 15)),
                        new VertexPositionColor(new Vector3(0.3f, 0.38f, -0.2f), new Color(40, 10, 15)),

                        new VertexPositionColor(new Vector3(0.3f, 0.38f, -0.2f), new Color(50, 15, 20)),
                        new VertexPositionColor(new Vector3(0.7f, 0.38f, -0.2f), new Color(50, 15, 20)),
                        new VertexPositionColor(new Vector3(0.7f, 1.1f, -0.2f), new Color(30, 10, 15)),
                        new VertexPositionColor(new Vector3(0.3f, 0.38f, -0.2f), new Color(50, 15, 20)),
                        new VertexPositionColor(new Vector3(0.7f, 1.1f, -0.2f), new Color(30, 10, 15)),
                        new VertexPositionColor(new Vector3(0.3f, 1.1f, -0.2f), new Color(30, 10, 15)),

                        new VertexPositionColor(new Vector3(0.32f, 0.0f, -0.58f), new Color(30, 12, 15)),
                        new VertexPositionColor(new Vector3(0.36f, 0.0f, -0.58f), new Color(30, 12, 15)),
                        new VertexPositionColor(new Vector3(0.36f, 0.38f, -0.58f), new Color(50, 15, 20)),
                        new VertexPositionColor(new Vector3(0.32f, 0.0f, -0.58f), new Color(30, 12, 15)),
                        new VertexPositionColor(new Vector3(0.36f, 0.38f, -0.58f), new Color(50, 15, 20)),
                        new VertexPositionColor(new Vector3(0.32f, 0.38f, -0.58f), new Color(50, 15, 20)),

                        new VertexPositionColor(new Vector3(0.64f, 0.0f, -0.58f), new Color(30, 12, 15)),
                        new VertexPositionColor(new Vector3(0.68f, 0.0f, -0.58f), new Color(30, 12, 15)),
                        new VertexPositionColor(new Vector3(0.68f, 0.38f, -0.58f), new Color(50, 15, 20)),
                        new VertexPositionColor(new Vector3(0.64f, 0.0f, -0.58f), new Color(30, 12, 15)),
                        new VertexPositionColor(new Vector3(0.68f, 0.38f, -0.58f), new Color(50, 15, 20)),
                        new VertexPositionColor(new Vector3(0.64f, 0.38f, -0.58f), new Color(50, 15, 20)),

                        new VertexPositionColor(new Vector3(0.32f, 0.0f, -0.22f), new Color(30, 12, 15)),
                        new VertexPositionColor(new Vector3(0.36f, 0.0f, -0.22f), new Color(30, 12, 15)),
                        new VertexPositionColor(new Vector3(0.36f, 0.38f, -0.22f), new Color(50, 15, 20)),
                        new VertexPositionColor(new Vector3(0.32f, 0.0f, -0.22f), new Color(30, 12, 15)),
                        new VertexPositionColor(new Vector3(0.36f, 0.38f, -0.22f), new Color(50, 15, 20)),
                        new VertexPositionColor(new Vector3(0.32f, 0.38f, -0.22f), new Color(50, 15, 20)),

                        new VertexPositionColor(new Vector3(0.64f, 0.0f, -0.22f), new Color(30, 12, 15)),
                        new VertexPositionColor(new Vector3(0.68f, 0.0f, -0.22f), new Color(30, 12, 15)),
                        new VertexPositionColor(new Vector3(0.68f, 0.38f, -0.22f), new Color(50, 15, 20)),
                        new VertexPositionColor(new Vector3(0.64f, 0.0f, -0.22f), new Color(30, 12, 15)),
                        new VertexPositionColor(new Vector3(0.68f, 0.38f, -0.22f), new Color(50, 15, 20)),
                        new VertexPositionColor(new Vector3(0.64f, 0.38f, -0.22f), new Color(50, 15, 20)),
                    };
                    graphics.DrawUserPrimitives(PrimitiveType.TriangleList, chairTwo, 0, 14);

                    if (table.HasPlate)
                    {
                        int segments = 24;
                        List<VertexPositionColor> plateVertices = new List<VertexPositionColor>();
                        float outerRadius = 0.32f;
                        float innerRadius = 0.20f;
                        float centerX = 0.0f;
                        float centerZ = -1.4f;
                        float rimY = 0.62f;
                        float baseY = 0.59f;
                        float bottomY = 0.57f;

                        for (int i = 0; i < segments; i++)
                        {
                            float angle1 = (i / (float)segments) * MathHelper.TwoPi;
                            float angle2 = ((i + 1) / (float)segments) * MathHelper.TwoPi;

                            float ox1 = centerX + (float)Math.Cos(angle1) * outerRadius;
                            float oz1 = centerZ + (float)Math.Sin(angle1) * outerRadius;
                            float ox2 = centerX + (float)Math.Cos(angle2) * outerRadius;
                            float oz2 = centerZ + (float)Math.Sin(angle2) * outerRadius;

                            float ix1 = centerX + (float)Math.Cos(angle1) * innerRadius;
                            float iz1 = centerZ + (float)Math.Sin(angle1) * innerRadius;
                            float ix2 = centerX + (float)Math.Cos(angle2) * innerRadius;
                            float iz2 = centerZ + (float)Math.Sin(angle2) * innerRadius;

                            plateVertices.Add(new VertexPositionColor(new Vector3(centerX, baseY, centerZ), new Color(235, 235, 225)));
                            plateVertices.Add(new VertexPositionColor(new Vector3(ix1, baseY, iz1), new Color(235, 235, 225)));
                            plateVertices.Add(new VertexPositionColor(new Vector3(ix2, baseY, iz2), new Color(235, 235, 225)));

                            plateVertices.Add(new VertexPositionColor(new Vector3(ix1, baseY, iz1), new Color(225, 225, 215)));
                            plateVertices.Add(new VertexPositionColor(new Vector3(ox1, rimY, oz1), new Color(245, 245, 235)));
                            plateVertices.Add(new VertexPositionColor(new Vector3(ix2, baseY, iz2), new Color(225, 225, 215)));

                            plateVertices.Add(new VertexPositionColor(new Vector3(ix2, baseY, iz2), new Color(225, 225, 215)));
                            plateVertices.Add(new VertexPositionColor(new Vector3(ox1, rimY, oz1), new Color(245, 245, 235)));
                            plateVertices.Add(new VertexPositionColor(new Vector3(ox2, rimY, oz2), new Color(245, 245, 235)));

                            plateVertices.Add(new VertexPositionColor(new Vector3(ox1, rimY, oz1), new Color(200, 200, 190)));
                            plateVertices.Add(new VertexPositionColor(new Vector3(ox1, bottomY, oz1), new Color(170, 170, 160)));
                            plateVertices.Add(new VertexPositionColor(new Vector3(ox2, rimY, oz2), new Color(200, 200, 190)));

                            plateVertices.Add(new VertexPositionColor(new Vector3(ox1, bottomY, oz1), new Color(170, 170, 160)));
                            plateVertices.Add(new VertexPositionColor(new Vector3(ox2, bottomY, oz2), new Color(170, 170, 160)));
                            plateVertices.Add(new VertexPositionColor(new Vector3(ox2, rimY, oz2), new Color(200, 200, 190)));
                        }
                        graphics.DrawUserPrimitives(PrimitiveType.TriangleList, plateVertices.ToArray(), 0, plateVertices.Count / 3);

                        int foodSegments = 12;
                        List<VertexPositionColor> foodVertices = new List<VertexPositionColor>();
                        float foodRadius = 0.11f;
                        float foodBaseY = 0.60f;
                        float foodTopY = 0.65f;

                        for (int i = 0; i < foodSegments; i++)
                        {
                            float angle1 = (i / (float)foodSegments) * MathHelper.TwoPi;
                            float angle2 = ((i + 1) / (float)foodSegments) * MathHelper.TwoPi;

                            float fx1 = centerX + (float)Math.Cos(angle1) * foodRadius;
                            float fz1 = centerZ + (float)Math.Sin(angle1) * foodRadius;
                            float fx2 = centerX + (float)Math.Cos(angle2) * foodRadius;
                            float fz2 = centerZ + (float)Math.Sin(angle2) * foodRadius;

                            foodVertices.Add(new VertexPositionColor(new Vector3(fx1, foodBaseY, fz1), new Color(120, 60, 20)));
                            foodVertices.Add(new VertexPositionColor(new Vector3(centerX, foodTopY, centerZ), new Color(180, 100, 40)));
                            foodVertices.Add(new VertexPositionColor(new Vector3(fx2, foodBaseY, fz2), new Color(120, 60, 20)));
                        }
                        graphics.DrawUserPrimitives(PrimitiveType.TriangleList, foodVertices.ToArray(), 0, foodVertices.Count / 3);
                    }

                    if (table.HasMug)
                    {
                        int segments = 16;
                        List<VertexPositionColor> mugVertices = new List<VertexPositionColor>();
                        List<VertexPositionColor> coffeeVertices = new List<VertexPositionColor>();
                        float radius = 0.12f;
                        float centerX = 0.5f;
                        float centerZ = -1.3f;
                        float bottomY = 0.57f;
                        float topY = 0.77f;
                        float coffeeY = 0.75f;

                        for (int i = 0; i < segments; i++)
                        {
                            float angle1 = (i / (float)segments) * MathHelper.TwoPi;
                            float angle2 = ((i + 1) / (float)segments) * MathHelper.TwoPi;

                            float x1 = centerX + (float)Math.Cos(angle1) * radius;
                            float z1 = centerZ + (float)Math.Sin(angle1) * radius;
                            float x2 = centerX + (float)Math.Cos(angle2) * radius;
                            float z2 = centerZ + (float)Math.Sin(angle2) * radius;

                            mugVertices.Add(new VertexPositionColor(new Vector3(x1, bottomY, z1), new Color(160, 160, 150)));
                            mugVertices.Add(new VertexPositionColor(new Vector3(x1, topY, z1), new Color(220, 220, 210)));
                            mugVertices.Add(new VertexPositionColor(new Vector3(x2, bottomY, z2), new Color(160, 160, 150)));

                            mugVertices.Add(new VertexPositionColor(new Vector3(x1, topY, z1), new Color(220, 220, 210)));
                            mugVertices.Add(new VertexPositionColor(new Vector3(x2, topY, z2), new Color(220, 220, 210)));
                            mugVertices.Add(new VertexPositionColor(new Vector3(x2, bottomY, z2), new Color(160, 160, 150)));

                            mugVertices.Add(new VertexPositionColor(new Vector3(centerX, bottomY, centerZ), new Color(140, 140, 130)));
                            mugVertices.Add(new VertexPositionColor(new Vector3(x2, bottomY, z2), new Color(160, 160, 150)));
                            mugVertices.Add(new VertexPositionColor(new Vector3(x1, bottomY, z1), new Color(160, 160, 150)));

                            coffeeVertices.Add(new VertexPositionColor(new Vector3(centerX, coffeeY, centerZ), new Color(45, 22, 10)));
                            coffeeVertices.Add(new VertexPositionColor(new Vector3(x1, coffeeY, z1), new Color(45, 22, 10)));
                            coffeeVertices.Add(new VertexPositionColor(new Vector3(x2, coffeeY, z2), new Color(45, 22, 10)));
                        }

                        graphics.DrawUserPrimitives(PrimitiveType.TriangleList, mugVertices.ToArray(), 0, mugVertices.Count / 3);
                        graphics.DrawUserPrimitives(PrimitiveType.TriangleList, coffeeVertices.ToArray(), 0, coffeeVertices.Count / 3);
                    }

                    if (table.HasCutlery)
                    {
                        VertexPositionColor[] forkAndKnife = new VertexPositionColor[]
                        {
                            new VertexPositionColor(new Vector3(-0.55f, 0.58f, -1.6f), new Color(200, 200, 210)),
                            new VertexPositionColor(new Vector3(-0.52f, 0.58f, -1.6f), new Color(200, 200, 210)),
                            new VertexPositionColor(new Vector3(-0.535f, 0.58f, -1.25f), new Color(170, 170, 180)),

                            new VertexPositionColor(new Vector3(-0.54f, 0.58f, -1.25f), new Color(170, 170, 180)),
                            new VertexPositionColor(new Vector3(-0.53f, 0.58f, -1.25f), new Color(170, 170, 180)),
                            new VertexPositionColor(new Vector3(-0.535f, 0.58f, -1.15f), new Color(220, 220, 230)),

                            new VertexPositionColor(new Vector3(-0.56f, 0.58f, -1.15f), new Color(200, 200, 210)),
                            new VertexPositionColor(new Vector3(-0.51f, 0.58f, -1.15f), new Color(200, 200, 210)),
                            new VertexPositionColor(new Vector3(-0.535f, 0.58f, -1.10f), new Color(200, 200, 210)),

                            new VertexPositionColor(new Vector3(-0.45f, 0.58f, -1.6f), new Color(190, 190, 200)),
                            new VertexPositionColor(new Vector3(-0.42f, 0.58f, -1.6f), new Color(190, 190, 200)),
                            new VertexPositionColor(new Vector3(-0.435f, 0.58f, -1.20f), new Color(160, 160, 170)),

                            new VertexPositionColor(new Vector3(-0.435f, 0.58f, -1.20f), new Color(160, 160, 170)),
                            new VertexPositionColor(new Vector3(-0.435f, 0.58f, -1.12f), new Color(210, 210, 220)),
                            new VertexPositionColor(new Vector3(-0.46f, 0.58f, -1.15f), new Color(180, 180, 190)),
                        };
                        graphics.DrawUserPrimitives(PrimitiveType.TriangleList, forkAndKnife, 0, 5);
                    }
                }
            }
        }
    }
}