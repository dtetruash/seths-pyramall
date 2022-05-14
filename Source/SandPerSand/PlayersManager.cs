﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace SandPerSand
{
    public class PlayersManager : Behaviour
    {
        private static PlayersManager instance;
        internal static PlayersManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new PlayersManager();
                }
                return instance;
            }
        }

        private Dictionary<PlayerIndex, GameObject> players;
        public Dictionary<PlayerIndex, GameObject> Players
        {
            get => players;
        }

        private static List<Vector2> initialPositions;
        public List<Vector2> InitialPositions
        {
            get => initialPositions;
            private set
            {
                initialPositions = value;
            }
        }

        private static Vector2 shopEntryPosition;
        public Vector2 ShopEntryPosition {
            get => shopEntryPosition;
            set { shopEntryPosition = value; }
        }


        public PlayersManager()
        {
            instance = this;
            players = new Dictionary<PlayerIndex, GameObject>();
            initialPositions = new List<Vector2>();
            Debug.Print("player manager created");
        }

        public GameState LastGameState { get; set; }
        public GameState CurrentGameState => GameStateManager.Instance.CurrentState;
        // TODO hard coded shopTime
        public float shopTime { get; private set; } = 30f;
        public float shopTimeCounter { get; private set; } = 0;
        public int curRank { get; private set; }
        private PlayerIndex[] rankList;

        protected override void Update()
        {
            if (CurrentGameState == GameState.RoundStartCountdown)
            {
                if (LastGameState == GameState.Shop)
                {
                    // this holds because we clear initial positions each time we exit a round
                    // initial positions are round specific
                    if (InitialPositions.ToArray().Length <= 0)
                    {
                        Debug.Print("Cannot respawn Players, because the map is " +
                            "not loaded or no initial positions on map.");
                        return;
                    }
                    LastGameState = GameState.InRound;
                    foreach (PlayerIndex playerIndex in Players.Keys)
                    {
                        RespawnPlayer(playerIndex, GetRandomInitialPos());
                    }
                }
            }
            else if (CurrentGameState == GameState.Shop)
            {
                bool atLeastOneAlive = false;
                foreach (var player in Players)
                {
                    if (player.Value.GetComponentInChildren<PlayerComponent>().IsAlive == true)
                    {
                        atLeastOneAlive = true;
                    }
                }
                if (!atLeastOneAlive)
                {
                    Debug.WriteLine("No players were alive. No shop.");
                    foreach (var player in Players)
                    {
                        player.Value.GetComponentInChildren<PlayerStates>().FnishedShop = true;
                    }
                    LastGameState = GameState.Shop;
                    return;
                }
                if (LastGameState != GameState.Shop)
                {
                    // FIXME wait for shop map
                    if (ShopEntryPosition.X < 4)
                    {
                        Debug.Print("Cannot respawn Player in the shop, " +
                            "because initial position is not yet loaded or invalid.");
                        return;
                    }
                    // clear initial positions when exit a round
                    // since they are round-specific FIXME better place to do this???
                    InitialPositions = new List<Vector2>();

                    // calculate rank list
                    rankList = new PlayerIndex[Players.Count];
                    foreach (var player in Players)
                    {
                        int rank = player.Value.GetComponent<PlayerStates>().RoundRank;
                        if (rank <= 0)
                        {
                            throw new Exception("Invalid rank at end");
                        }
                        rankList[rank - 1] = player.Key;
                    }

                    // respawn players -> queue by rank list
                    // disable all players controller
                    var entryX = ShopEntryPosition.X;
                    foreach (var playerIndex in rankList)
                    {
                        if (players[playerIndex].GetComponentInChildren<PlayerComponent>().IsAlive == true)
                        {
                            RespawnPlayer(playerIndex, new Vector2(entryX--, ShopEntryPosition.Y));
                            PlayerUtils.ShieldPlayerControl(Players[playerIndex]);
                        } else
                        {
                            players[playerIndex].GetComponentInChildren<PlayerStates>().FnishedShop = true;
                        }
                    }
                    // ShopEntryPsition can be reset right after use
                    // If not reset, players will be spawned before shop map is
                    // loaded next time ... then drop
                    ShopEntryPosition = Vector2.Zero;
                    LastGameState = GameState.Shop;
                    shopTimeCounter = shopTime;
                    curRank = 0;
                }
                shopTimeCounter += Time.DeltaTime;
                if (shopTimeCounter >= shopTime || Players[rankList[curRank - 1]].GetComponent<PlayerStates>().FnishedShop)
                {
                    // Reset the shop coutner
                    shopTimeCounter = 0;

                    // If this was not the first player, set the one before to be finished with the shop
                    if(curRank>0)
                    {
                        Players[rankList[curRank - 1]].GetComponent<PlayerStates>().FnishedShop = true;
                        //Players[rankList[curRank - 1]].GetComponent<PlayerControlComponent>().IsActive = false;
                    }

                    // Activate the controller of the next player
                    if(curRank < rankList.Length) 
                        PlayerUtils.ResumePlayerControl(Players[rankList[curRank]]);
                    curRank++;
                }

            }
        }



        public GameObject GetPlayer(PlayerIndex index) {
            return players[index];
        }

        public bool DestroyPlayer(PlayerIndex index)
        {
            if (players.ContainsKey(index))
            {
                GraphicalUserInterface.Instance.destroyPlayerInfo(index);
                players[index].Destroy();
                players.Remove(index);
                return true;
            }
            else
            {
                return false;
            }
        }

        public void CreatePlayer(PlayerIndex playerIndex, Vector2 position)
        {
            GraphicalUserInterface.Instance.renderPlayerInfo(playerIndex);
            if (players.ContainsKey(playerIndex))
            {
                if (players[playerIndex] != null)
                {
                    players[playerIndex].Destroy();
                }
                players[playerIndex] = Template.MakePlayer(playerIndex, position);
            }
            else
            {
                players.Add(playerIndex, Template.MakePlayer(playerIndex, position));
            }
        }

        public void RespawnPlayer(PlayerIndex playerIndex, Vector2 position)
        {
            if (players.ContainsKey(playerIndex))
            {
                players[playerIndex].GetComponent<PlayerControlComponent>().IsActive = false;
                players[playerIndex].GetComponent<RigidBody>().LinearVelocity = Vector2.Zero;
                players[playerIndex].Transform.Position = position;
                players[playerIndex].GetComponent<Animator>().Entry();
                players[playerIndex].GetComponent<PlayerControlComponent>().IsActive = true;
                players[playerIndex].GetComponent<PlayerComponent>().IsAlive = true;
            }
            else
            {
                throw new InvalidOperationException("");
            }

        }

        public Boolean addItemToInventory(PlayerIndex player, string item, Boolean Major)
            //returns true if item was added. False if it is already full
        {
            PlayerStates state = players[player].GetComponentInChildren<PlayerStates>();
            Boolean success = state.addItemToInventory(item, Major);
            if (success)
            {
                GraphicalUserInterface.Instance.renderItem(player, item, Major);
            }
            return success;
        }

        public string useItem(PlayerIndex player, Boolean Major)
        {
            PlayerStates state = players[player].GetComponentInChildren<PlayerStates>();
            GraphicalUserInterface.Instance.removeItem(player, Major);
            return state.useItem(Major);
        }

        public void addCoins(PlayerIndex player, int coins)
        {
            PlayerStates state = players[player].GetComponentInChildren<PlayerStates>();
            int tot_coins = state.addCoins(coins);
            GraphicalUserInterface.Instance.renderCoins(player, tot_coins);
        }
        public Boolean spendCoins(PlayerIndex player, int coins)
        {
            PlayerStates state = players[player].GetComponentInChildren<PlayerStates>();
            (Boolean, int) tmp = state.spendCoins(coins);
            GraphicalUserInterface.Instance.renderCoins(player, tmp.Item2);
            return tmp.Item1;
        }
        private Vector2 GetRandomInitialPos()
        {
            Random rd = new Random();
            int totalPosNum = InitialPositions.ToArray().Length;
            if (totalPosNum <= 0)
            {
                throw new InvalidOperationException("No initial position " +
                    "registered. Please add at least one 'Entry' Tile on map and make sure " +
                    "map is loaded before Players' creation");
            }
            return InitialPositions[rd.Next(0, totalPosNum)];
        }

        public void CheckConnections()
        {
            // check for new connection / disconnection
            foreach (PlayerIndex playerIndex in Enum.GetValues(typeof(PlayerIndex)))
            {
                GamePadCapabilities capabilities = GamePad.GetCapabilities(playerIndex);
                if (capabilities.IsConnected)
                {
                    if (!players.ContainsKey(playerIndex))
                    {
                        Debug.Print("New Connected controller:" + playerIndex);
                        if (InitialPositions.ToArray().Length > 0)
                        {
                            CreatePlayer(playerIndex, GetRandomInitialPos());
                        }
                        else
                        {
                            Debug.Print("Cannot create Player" + playerIndex
                                + ", because map is not loaded or no initial positions on map.");
                        }
                    }
                }
                else
                {
                    if (DestroyPlayer(playerIndex))
                    {
                        //delete player
                        Debug.Print("Disconnected:" + playerIndex);
                    }
                }
            }

        }

        public Boolean CheckAllPrepared()
        {
            if (players.Count == 0)
            {
                return false;
            }
            var allPreparedFlag = true;
            foreach (var player in players.Values)
            {
                if (!player.GetComponent<PlayerStates>().Prepared)
                {
                    allPreparedFlag = false;
                }
            }
            return allPreparedFlag;
        }

        public Boolean CheckOneExit()
        {
            if (players.Count == 0)
            {
                return false;
            }
            foreach (var player in players.Values)
            {
                if (player.GetComponent<PlayerStates>().Exited)
                {
                    return true;
                }
            }
            return false;
        }

        public Boolean CheckAllDead()
        {
            if (players.Count == 0)
            {
                return true;
            }
            foreach (var player in players.Values)
            {
                if (player.GetComponent<PlayerComponent>().IsAlive)
                {
                    return false;
                } else {
                    player.GetComponent<PlayerStates>().RoundRank = players.Values.Count;
                }
            }
            return true;
        }

        public Boolean CheckAllDeadOrExit()
        {
            if (players.Count == 0)
            {
                return false;
            }
            var allExitedFlag = true;
            foreach (var player in players.Values)
            {
                if (!player.GetComponent<PlayerStates>().Exited && player.GetComponent<PlayerComponent>().IsAlive)
                {
                    allExitedFlag = false;
                }
            }
            return allExitedFlag;
        }

        public Boolean CheckAllFinishedShop()
        {
            if (players.Count == 0)
            {
                return true;
            }
            var allExitedFlag = true;
            foreach (var player in players.Values)
            {
                if (!player.GetComponent<PlayerStates>().FnishedShop)
                {
                    allExitedFlag = false;
                }
            }
            return allExitedFlag;
        }

        public void finalizeRanks()
        {
            int notExitedFrom = 1;
            foreach (var player in players.Values)
            {
                if (player.GetComponent<PlayerStates>().RoundRank != -1)
                {
                    notExitedFrom++;
                }
            }
            foreach (var player in players.Values)
            {
                if (player.GetComponent<PlayerStates>().RoundRank == -1)
                {
                    player.GetComponent<PlayerStates>().RoundRank = notExitedFrom++;
                }
            }
        }

        public List<GameObject> InGamePlayerGo()
        {
            List<GameObject> result = new List<GameObject>();

            foreach(GameObject player in players.Values)
            {
                if (!player.GetComponent<PlayerStates>().Exited && player.GetComponent<PlayerComponent>().IsAlive)
                {
                    result.Add(player);
                }
            }

            return result;
        }

        public void SetAllPlayerControls(bool enabled)
        {
            foreach (var player in players.Values)
            {
                player.GetComponent<PlayerControlComponent>()!.IsActive = enabled;
            }
        }
    }

    public class PlayerStates : Behaviour
    {
        public Boolean Prepared;
        public static Boolean Paused;
        public string MinorItem;
        public string MajorItem;
        public int Coins;
        public bool Exited { get; set; }
        public bool FnishedShop { get; set; }
        public int RoundRank { get; set; }
        public int Score { get; set; }
        public InputHandler InputHandler { get; set; }
        private bool PrepareButtonPressed => InputHandler.getButtonState(Buttons.A) == ButtonState.Pressed;
        public GameState LastGameState{ get; set; }
        public GameState CurrentGameState => GameStateManager.Instance.CurrentState;
        public Collider Collider { get; set; }

        public List<(string id, float timeleft, float tot_time, Vector2 pos)> activeItems { private set; get; }

        protected override void OnAwake()
        {
            Prepared = false;
            Paused = false;
            MinorItem = null;
            MajorItem = null;
            Coins = 0;
            Exited = false;
            FnishedShop = false;
            RoundRank = -1;
            activeItems = new List<(string id, float timeleft, float tot_time, Vector2 pos)>();
            Score = 0;
        }

        /// <summary>
        /// The update handles state transitions.
        /// </summary>
        protected override void Update()
        {
            //<<<<<<< Yuchen stuff
            if (PrepareButtonPressed && CurrentGameState == GameState.Prepare)
            {
                TogglePrepared();
            }
            if (LastGameState != CurrentGameState)
            {
                if(LastGameState == GameState.RoundCheck )
                {

                    FnishedShop = false;
                }else if(LastGameState == GameState.Shop)
                {
                    // reset round states
                    Exited = false;
                    RoundRank = -1;
                }
                LastGameState = CurrentGameState;
            }
            //======= End of Yuchen stuff

            //<<<<<<< Clemens stuff
            float timeDelta = Time.DeltaTime;

            List<int> remove = new List<int>();

            //bool lightning = false;

            for (int i = 0; i < activeItems.Count; i++)
            {
                Vector2 pos = activeItems[i].pos;
                float time = activeItems[i].timeleft;
                if (activeItems[i].id == "position_swap")
                {
                    Debug.Print((activeItems[i].pos - this.Transform.Position).LengthSquared().ToString());
                    if ((activeItems[i].pos - this.Transform.Position).LengthSquared() < 0.5f)
                    {
                        this.Transform.Position = activeItems[i].pos;
                        time = -1f;
                        Collider.IsActive = true;
                    }
                    else
                    {
                        Vector2 vel = (activeItems[i].pos - this.Transform.Position) / (activeItems[i].pos - this.Transform.Position).Length();
                        this.Transform.Position = (activeItems[i].pos * 0.1f + 0.9f * this.Transform.Position) + vel / 10;
                        var collider = this.Owner.GetComponent<Collider>();
                        Collider.IsActive = false;
                    }
                }
                else
                {
                    if (activeItems[i].pos.Y < 0)
                    {
                        pos = activeItems[i].pos;
                        time = activeItems[i].timeleft - timeDelta;
                    }
                    else if ((activeItems[i].pos - this.Transform.Position).LengthSquared() < 0.1f)
                    {
                        pos = -Vector2.One;
                        time = activeItems[i].timeleft - timeDelta;
                    }
                    else
                    {
                        Vector2 vel = (activeItems[i].pos - this.Transform.Position) / (activeItems[i].pos - this.Transform.Position).Length();
                        pos = activeItems[i].pos * 0.9f + 0.1f * this.Transform.Position - vel / 10;
                        time = activeItems[i].timeleft;
                    }
                }

                if (activeItems[i].timeleft < 0)
                {
                    remove.Add(i);
                }
                activeItems[i] = (activeItems[i].id, time, activeItems[i].tot_time, pos);

                //if(activeItems[i].id == "lightning")
                //{
                //    lightning = true;
                //}
            }

            //if (lightning)
            //{
            //    Collider.Owner.GetComponentInParents<PlayerComponent>().Transform.LossyScale = Vector2.One * 0.8f;
            //    Collider.IsActive = false;
            //    Collider.IsActive = true;
            //}

            for (int i = remove.Count - 1; i >= 0; i--)
            {
                if (activeItems[i].id == "lightning")
                {
                    Collider.Owner.GetComponentInParents<PlayerComponent>().Transform.LossyScale = Vector2.One;
                    Collider.Transform.LossyScale = Vector2.One * 0.8f;
                }
                activeItems.RemoveAt(remove[i]);
            }
            //======= End of Clemens stuff
        }

        public void TogglePrepared()
        {
            // Debug
            var playerIndex = InputHandler.PlayerIndex;
            if (Prepared)
            {
                Prepared = false;
                Debug.Print("Player" + playerIndex + "UnPrepared.");
            }
            else
            {
                Prepared = true;
                Debug.Print("Player" + playerIndex + "Prepared.");
            }
        }

        public static void TogglePaused()
        {
            if (Paused)
            {
                Paused = false;
            }
            else
            {
                Paused = true;
            }
        }

        public Boolean addItemToInventory(string item, Boolean Major)
        {
            //returns true if item was added
            if (Major)
            {
                if(MajorItem == null)
                {
                    MajorItem = item;
                    return true;
                }
            }
            else
            {
                if (MinorItem == null)
                {
                    MinorItem = item;
                    return true;
                }
            }
            return false;
        }

        public string useItem(Boolean Major)
            //returns null if no item
        {
            if (Major)
            {
                string tmp = MajorItem;
                MajorItem = null;
                return tmp;
            }
            else
            {
                string tmp = MinorItem;
                MinorItem=null;
                return tmp;
            }
        }

        public int addCoins(int amount)
        {
            Coins += amount;
            return Coins;
        }

        public (Boolean, int) spendCoins(int amount)
            //returns true when action was successfull
        {
            if(Coins >= amount)
            {
                Coins -= amount;
                return (true, Coins);
            }
            else
            {
                return (false, Coins);
            }
        }

        public float getJumpFactor()
        {
            float jumpfactor = 1;
            foreach(var item in activeItems)
            {
                if(item.id == "wings") 
                {
                    jumpfactor *= 1.5f;
                }
                else if(item.id == "ice_block")
                {
                    jumpfactor *= 0;
                }
                else if ((item.id == "lightning"))
                {
                    jumpfactor *= 0.8f;
                }
            }
            return jumpfactor;
        }

        public float getAccellerationFactor()
        {
            float accelleration = 1;

            foreach (var item in activeItems)
            {
                if (item.id == "speedup")
                {
                    accelleration *= 1.5f;
                }
                else if (item.id == "ice_block")
                {
                    accelleration *= 0f;
                }
                else if ((item.id == "lightning"))
                {
                    accelleration *= 0.8f;
                }
            }
            return accelleration;
        }

        public float getInvertedMovement()
        {
            float invertedMovement = 1;

            foreach (var item in activeItems)
            {
                if (item.id == "dizzy_eyes")
                {
                    invertedMovement *= -1f;
                }
            }
            return invertedMovement;
        }

        public bool gravityOn()
        {
            foreach (var item in activeItems)
            {
                if (item.id == "position_swap")
                {
                    return false;
                }
            }
            return true;
        }

        public void addActiveItem(string id, float timeleft, float tot_time, Vector2 pos)
        {
            for (int i = 0; i < activeItems.Count; i++)
            { 
                if(id == activeItems[i].id)
                {
                    activeItems[i] = (id, timeleft, tot_time, pos);
                    return;
                }
            }
            activeItems.Add((id, timeleft, tot_time, pos));
        }
    }
}
