﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Engine
{
    public sealed class Scene
    {
        private readonly List<GameObject> allObjects = new List<GameObject>();

        public string Name { get; set; }

        public bool IsLoaded => SceneManager.OpenScenes.Contains(this);

        public bool IsActive => SceneManager.ActiveScene == this;

        public IReadOnlyList<GameObject> Objects => this.allObjects.AsReadOnly();

        public IEnumerable<GameObject> RootObjects => this.allObjects.Where(go => go.Transform.Parent == null);

        public Scene()
        {
            this.Name = "New Scene";
        }

        internal void OnLoad()
        {
            if (!SceneManager.IsReady)
            {
                return;
            }

            // Use for instead of foreach to support game object creation in OnAwake
            for (var i = 0; i < this.allObjects.Count; i++)
            {
                var go = this.allObjects[i];

                if (go.State == GameObject.GameObjectState.Created)
                {
                    go.OnAwakeInternal();
                }
            }

            // Same for OnEnable
            for (var i = 0; i < this.allObjects.Count; i++)
            {
                var go = this.allObjects[i];
                if (go.IsEnabledInHierarchy && go.State == GameObject.GameObjectState.Disabled)
                {
                    go.OnEnableInternal();
                }
            }
        }

        internal void OnUnload()
        {
            if (!SceneManager.IsReady)
            {
                return;
            }

            for (var i = 0; i < this.allObjects.Count; i++)
            {
                var go = this.allObjects[i];
                if (go.IsEnabledInHierarchy)
                {
                    go.OnDisableInternal();
                }
            }

            for (var i = 0; i < this.allObjects.Count; i++)
            {
                var go = this.allObjects[i];
                go.OnDestroyInternal();
            }
        }

        internal void DoUpdate()
        {
            for (var i = 0; i < this.allObjects.Count; i++)
            {
                var go = this.allObjects[i];
                go.DoUpdate();
            }
        }

        internal void AddGameObject(GameObject go)
        {
            if (go == null)
            {
                throw new ArgumentNullException(nameof(go));
            }

            Debug.Assert(!this.allObjects.Contains(go));

            go.Scene = this;
            this.allObjects.Add(go);
        }

        internal void RemoveGameObject(GameObject go)
        {
            if (go == null)
            {
                throw new ArgumentNullException(nameof(go));
            }

            Debug.Assert(this.allObjects.Contains(go));

            this.allObjects.RemoveSwapBack(go);
            go.Scene = null;
        }
    }
}
