﻿using Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SandPerSand
{
    internal class PlayerControlComponent : Behaviour
    {
        /// <summary>
        /// This component is responsible for interpreting GamePad input and updating the player accordingly.
        /// </summary>
        private InputHandler inputHandler;

        private RigidBody playerRB;
        private Vector2[] dubugPlayerColliderOutline;

        private const float JumpForce = 10.0f;
        private const float WalkForce = 20.0f;

        public PlayerIndex PlayerIndex
        {
            get;
            set;
        }


        public PlayerControlComponent()
        {
            /*Empty component constructor*/
        }

        protected override void OnAwake()
        {
            inputHandler = new InputHandler(this.PlayerIndex);
            playerRB = this.Owner.GetComponent<RigidBody>();
            dubugPlayerColliderOutline = this.Owner.GetComponent<PolygonCollider>().Outline;
        }

        protected override void Update()
        {
            //impulse up if A is pressed
            switch (inputHandler.getButtonState(Buttons.A))
            {
                case ButtonState.Pressed:
                    playerRB.ApplyLinearImpulse(Vector2.UnitY * JumpForce);
                    break;
            }

            //force in the stick direction
            Vector2 stickDir = inputHandler.getLeftThumbstickDirX();
            //System.Diagnostics.Debug.Write($"Stick Dir X: {stickDir}\n");
            playerRB.ApplyForce(stickDir * WalkForce * Time.DeltaTime);


            // Update the input handler's state
            inputHandler.UpdateState();

            // draw collider for debug purposes
            var p0 = this.Transform.TransformPoint(dubugPlayerColliderOutline[0]);
            var pCur = p0;

            for (var i = 0; i < dubugPlayerColliderOutline.Length - 1; i++)
            {
                var pNext = this.Transform.TransformPoint(dubugPlayerColliderOutline[i + 1]);
                Gizmos.DrawLine(pCur, pNext, Color.Red);
                pCur = pNext;
            }

            Gizmos.DrawLine(pCur, p0, Color.Red);
        }


    }
}
