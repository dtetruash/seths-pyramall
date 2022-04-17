﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.IO;
using TiledCS;
using System.Diagnostics;
namespace Engine
{
    public class Animator : Behaviour
    {
        private Dictionary<string, Animation> animes;
        private string currentAnimeKey = null;
        private string loadFromContentPath;
        private float passedTime = 0f;
        private SpriteRenderer renderer;

        public Animation CurrentAnime
        {
            get
            {
                // TODO handle
                if (currentAnimeKey == null) {
                    return null;
                }
                return animes[currentAnimeKey];
            }
        }
        protected override void OnEnable()
        {
            animes = new Dictionary<string, Animation>();
            this.LoadFromContentPath();

        }

        protected override void Update()
        {
            base.Update();
            this.passedTime += Time.DeltaTime;
            if(passedTime >= CurrentAnime.CurrentFrame.Duration)
            {
                passedTime = 0f;
                CurrentAnime.NextFrame();
                SyncRect();
            }
        }

        public void NextAnime(string animName)
        {
            if (this.animes.ContainsKey(animName))
            {
                this.currentAnimeKey = animName;
            }
            else
            {
                throw new ArgumentException("Invalid AnimName!");
            }
        }

        private void SyncRect()
        {
            // TODO handle exceptions
            renderer.SourceRect = CurrentAnime.CurrentFrame.SourceRect;

        }

        public void LoadFromContent(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException(nameof(path));
            }

            this.loadFromContentPath = path;
            this.LoadFromContentPath();
        }

        private void LoadFromContentPath()
        {
            if (this.loadFromContentPath == null || !this.Owner.Scene.IsLoaded || !this.IsActiveInHierarchy)
            {
                return;
            }
            // load .tsx file
            //FIXME hard code path
            var tiledS = new TiledTileset($"Content/tiles/{loadFromContentPath}.tsx");
            if (tiledS == null)
            {
                throw new NullReferenceException("Load tiledS Failed");
            }

            // TODO load texture in SpriteRenderer;
            // TODO better Tiled project structure and accurate paths
            var textureAssetName = Path.GetFileNameWithoutExtension(tiledS.Image.source);
            var texture = GameEngine.Instance.Content.Load<Texture2D>(textureAssetName);
            this.renderer =  this.Owner.AddComponent<SpriteRenderer>();
            this.renderer.LoadFromContent(textureAssetName);

            // foreach tile, check if it is anime
            foreach (TiledTile tile in tiledS.Tiles)
            {
                if (tile.animation != null)
                {
                    // found a animation
                    // foreach anime calculate list of sourceRect <- (tileId, tile-w&h, texture-w)
                    // and note frame-length for each frame
                    List<Frame> frameList = new List<Frame>();
                    foreach (TiledTileAnimation tiledFrame in tile.animation)
                    {
                        // Calculate source rectangle of the frame
                        int tileId = tiledFrame.tileid;
                        Rectangle sourceRectangle = new Rectangle(
                            tileId * tiledS.TileWidth % texture.Width,
                            tileId * tiledS.TileWidth / texture.Width * tiledS.TileHeight,
                            tiledS.TileWidth,
                            tiledS.TileHeight);
                        // Calculate duration in seconds
                        float duration = tiledFrame.duration / 1000f;
                        // Create new Frame
                        frameList.Add(new Frame(sourceRectangle, duration));
                    }
                    var newAnim = new Animation(frameList.ToArray());

                    var animIsEntry = false;
                    foreach (TiledProperty p in tile.properties)
                    {
                        // TODO hard code
                        switch (p.name)
                        {
                            case "AnimName":
                                newAnim.Name = p.value;
                                break;
                            case "AnimIsLoop":
                                // TODO not used
                                newAnim.IsLoop = bool.Parse(p.value);
                                break;
                            case "AnimIsEntry":
                                animIsEntry = bool.Parse(p.value);
                                break;
                        }
                    }
                    if (newAnim.Name == null)
                    {
                        throw new ArgumentNullException(nameof(newAnim.Name));
                    }
                    if (animIsEntry)
                    {
                        if (this.currentAnimeKey == null)
                        {
                            this.currentAnimeKey = newAnim.Name;
                        }
                        else
                        {
                            throw new ArgumentException("Multiple animation entries!");
                        }
                    }
                    animes.Add(newAnim.Name, newAnim);
                }

            }
            if (this.currentAnimeKey == null)
            {
                throw new ArgumentNullException("No entry animation defined!");
            }
            SyncRect();
        }
    }

    public class Animation
    {
        // TODO regulate code style
        public Frame[] Frames { get; set; }
        public int FrameItr;
        public Frame CurrentFrame
        {
            get
            {
                return Frames[FrameItr];
            }
        }
        public bool IsLoop;
        public string Name;

        public Animation(Frame[] frameArray)
        {
            Frames = frameArray;
            FrameItr = 0;
            IsLoop = true;
            Name = null;
        }

        public void NextFrame()
        {
            FrameItr += 1;
            FrameItr %= Frames.Length;
        }

    }

    public struct Frame
    {
        public Rectangle SourceRect;
        public float Duration;

        public Frame(Rectangle sourceRectangle, float duration)
        {
            SourceRect = sourceRectangle;
            Duration = duration;
        }
    }
}
