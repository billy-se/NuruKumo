using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.DirectWrite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace nomoredeleting
{
    internal class LSystemTree
    {
        private List<CachedBranch> _cachedBranches = new List<CachedBranch>();
        private List<CachedLeaf> _cachedLeaves = new List<CachedLeaf>();
        private bool _needRegeneration = true;
        public Vector2 Position { get; set; }
        public float BranchLength { get; set; }
        public int Iterations { get; set; }
        public Color BranchColor { get; set; }
        public Color LeafColor { get; set; }

        private string _treeString;
        private List<Vector2> _leaves;
        private Texture2D _pixel;

        private Dictionary<char, string> _rules;
        private string _axiom;

        private struct CachedBranch
        {
            public Vector2 Start;
            public Vector2 End;
            public Color Color;
            public int Thickness;
        }

        private struct CachedLeaf
        {
            public Vector2 Position;
            public Color Color;
        }
        private struct TreeState
        {
            public Vector2 Position;
            public float Angle;
            public float CurrentBranchLength;
            public int Depth;

            public TreeState(Vector2 pos, float angle, float branchLength, int depth)
            {
                Position = pos;
                Angle = angle;
                CurrentBranchLength = branchLength;
                Depth = depth;
            }
        }

        public LSystemTree(Texture2D pixelTexture)
        {
            _pixel = pixelTexture;
            _leaves = new List<Vector2>();

            Position = Vector2.Zero;
            BranchLength = 70f;
            Iterations = 2;
            BranchColor = Color.Brown;
            LeafColor = Color.Green;

            _axiom = "F";
            _rules = new Dictionary<char, string>
            {
                {'F', "F[+F][-F]" }
            };
        }

        public void GenerateTree()
        {
            _treeString = GenerateSystemString(_axiom, _rules, Iterations);
            _needRegeneration = true;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (_needRegeneration)
            {
                GenerateTree();
                CacheTreeGeometry();
                _needRegeneration = false;
            }
            DrawCachedTree(spriteBatch);
        }

        private void DrawCachedTree(SpriteBatch spriteBatch)
        {
            foreach (var branch in _cachedBranches)
            {
                DrawBranch(spriteBatch, branch.Start, branch.End, branch.Color, branch.Thickness);
            }

            foreach (var leaf in _cachedLeaves)
            {
                DrawLeaf(spriteBatch,leaf.Position);
            }
        }

        public void SetTreeType(string treeType)
        {
            (_axiom, _rules) = treeType.ToLower() switch
            {
                "bushy" => ("F", new Dictionary<char, string> { { 'F', "F[+F+F][-F-F]" } }),
                "tall" => ("F", new Dictionary<char, string> { { 'F', "F[+F+F][-F-F][+F][-F][+F][-F]" } }),
                "willow" => ("F", new Dictionary<char, string> { { 'F', "F[+F][-F]" } }),
                "palm" => ("F", new Dictionary<char, string> { { 'F', "F[-F][+F]" } })
            };
            GenerateTree();
            _needRegeneration = true;
        }

        public void SetCustomRules(string axiom, Dictionary<char, string> rules)
        {
            _axiom = axiom;
            _rules = rules;
            GenerateTree();
        }

        private string GenerateSystemString(string axiom, Dictionary<char, string> rules, int iterations)
        {
            string current = axiom;

            for (int i = 0; i < iterations; i++)
            {
                StringBuilder next = new StringBuilder();
                foreach (char c in current)
                {
                    if (rules.ContainsKey(c))
                    {
                        next.Append(rules[c]);
                    }
                    else
                    {
                        next.Append(c);
                    }
                }
                current = next.ToString();
            }
            return current;
        }

        private void CacheTreeGeometry()
        {
            _cachedBranches.Clear();
            _cachedLeaves.Clear();
            System.Diagnostics.Debug.WriteLine($"Tree drawing at: X={Position.X}, Y={Position.Y}");
            System.Diagnostics.Debug.WriteLine($"Tree string length: {_treeString?.Length}");
            Stack<TreeState> stateStack = new Stack<TreeState>();
            TreeState currentState = new TreeState(Position, -90f, BranchLength, 0);
            float angleChange = 25f;

            foreach (char c in _treeString)
            {
                switch (c)
                {
                    case 'F':
                        Vector2 endPos = currentState.Position + AngleToVector(currentState.Angle) * currentState.CurrentBranchLength;
                        int thickness = 5 - currentState.Depth;
                        if (thickness < 1) thickness = 1;

                        //DrawBranch(spriteBatch, currentState.Position, endPos, BranchColor, thickness);
                        _cachedBranches.Add(new CachedBranch{ 
                            Start = currentState.Position,
                            End = endPos,
                            Color = BranchColor,
                            Thickness = thickness
                        });
                        currentState.Position = endPos;
                        break;

                    case '+':
                        currentState.Angle += angleChange;
                        break;
                    case '-':
                        currentState.Angle -= angleChange;
                        break;
                    case '[':
                        stateStack.Push(currentState);
                        currentState = new TreeState(
                            currentState.Position,
                            currentState.Angle,
                            currentState.CurrentBranchLength * 0.7f,
                            currentState.Depth + 1
                            );
                        break;

                    case ']':
                        //DrawLeaf(spriteBatch, currentState.Position);
                        _cachedLeaves.Add(new CachedLeaf
                        {
                            Position = currentState.Position,
                            Color = LeafColor
                        });
                        currentState = stateStack.Pop();
                        break;
                }
            }
        }

        private Vector2 AngleToVector(float angleDegrees) {
            float angleRadians = MathHelper.ToRadians(angleDegrees);
            return new Vector2((float)Math.Cos(angleRadians), (float)Math.Sin(angleRadians));
        }

        private void DrawBranch(SpriteBatch spriteBatch, Vector2 start, Vector2 end, Color color,int thickness)
        {
            float length = Vector2.Distance(start, end);
            float rotation = (float)Math.Atan2(end.Y - start.Y, end.X - start.X);

            spriteBatch.Draw(_pixel, start, null, color, rotation, Vector2.Zero, new Vector2(length, thickness), SpriteEffects.None, 0f);
        }

        private void DrawLeaf(SpriteBatch spriteBatch, Vector2 position)
        {
            const int leafSize = 16;
            spriteBatch.Draw(_pixel, new Rectangle((int)position.X - leafSize / 2, (int)position.Y - leafSize/2, leafSize, leafSize ),LeafColor);
        }
    }
}
